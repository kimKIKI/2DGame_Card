using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class CardEff_Result : MonoBehaviour {


    public Transform PanelTop;    //Top의 화면 
    public GameObject card;       //승리한 카드
    public Transform EndEffect;    //패배한 카드가 동전으로 변하기전 이동할 위치
    public Transform gridGroup;    //패배한 카드요소
    public Transform VicgridGroup; //승리한 카드의 위치
    public Transform spec_Text;    //특별기능 TEXT
    int vic;                        //승리한 편


    public Transform title;       //카드의 이름
    public GameObject arrowLevel;  //card amount 증가
    public GameObject arrowGold;   //골드 증가
    public GameObject arrowJew;    //보석 증가
    public GameObject arrow;       //화살표의 위로올라가는 애니
    public GameObject cardNum;     //상자가 열렸을때  몇개의 상품항목이 있는지 보여줄 것
    public GameObject experience;  //경험치 오브젝트패널

    Text txGold;
    Text txJew;
    Text txLevel;
    Text itemNum;                   //상자내 표시 카운트 
    Text txExperienceLevel;         //경험치 레벨표시
    Text txExperienceCount;         //경험치 레벨카운트
    Text[] titleNames;
    Text txSpecText;                //특수한 기능이 있음을 알린다.

    int spawnNum = 5;                //스폰될 량

    //Dictionary<int, IList<Card>> vicCard;
    //Dictionary<int, IList<Card>> defCard;

    //델리게이트는 이벤트 발생부분에서 실행
    public delegate void UnitEventHandler();
    public static event UnitEventHandler onUnitySpawn;
    public static event UnitEventHandler onUnityDestroy;

    public delegate void ItemDelegate(int a);
    public static event ItemDelegate itemEvent;

    int[] playerLevel = { 500, 1000, 2000, 3000, 4000, 6000, 8000, 10000, 20000, 30000, 40000, 50000 };

    public enum eDEFECTCARD
    {
        NONE,
        START,
        VICTORYCARD, //승리한 카드의 정보와 패배한 카드의 숫자를 얻어온다.
        DEFECTCARD,  //패배한 카드의 아이템 애니를 실행한다.
        DELAY,       //패배한 카드의 특수한 능력을 확인하기 위해서 잠시 대기 시킨다.
        RESTART,     //패배한 카드의 애니를 계속 진행시킨다.
    }

    private void Awake()
    {
        //보이는 위치가 아니므로 gradGroup와 같이 한다.

        txGold = arrowGold.GetComponentInChildren<Text>();
        txJew = arrowJew.GetComponentInChildren<Text>();
        txLevel = arrowLevel.GetComponentInChildren<Text>();
        txExperienceLevel = experience.transform.Find("Text_LevelNum").GetComponent<Text>();        //경험치 레벨표시
        txExperienceCount = experience.transform.Find("Text_Value").GetComponent<Text>(); ;         //경험치 레벨카운트
        //titleName[0] Title [1] Kinds  [2] Level
        titleNames = title.GetComponentsInChildren<Text>();
        txSpecText = spec_Text.GetComponentInChildren<Text>();
    }

    private void Start()
    {
        itemEvent += OnCoinAnime;

        if (GameData.Instance.players.ContainsKey(1))
        {   //현재의 캐릭터의 정보를 받아온다.
            txExperienceLevel.text = GameData.Instance.players[1].exp.ToString();
            string count           = GameData.Instance.players[1].expCount.ToString();
            string defaultExp      = playerLevel[GameData.Instance.players[1].exp].ToString();
            txExperienceCount.text = string.Format("{0} / {1}", count, defaultExp);

            txGold.text            = string.Format("{0}", GameData.Instance.players[1].coin);
            txJew.text             = string.Format("{0}", GameData.Instance.players[1].jew);
            //시작과동시에 다시 자신의 골드를 프로퍼티로 사용학 위해서  데이터에 저장한다.
            //titleName[0] Title [1] Kinds  [2] Level
            titleNames[0].text     = string.Format("{0}", "");
            titleNames[1].text     = string.Format("{0}", "");
            titleNames[2].text     = string.Format("{0}", "");

        }
        StartCoroutine(Auto_Step());
    }


    //private void OnEnable()
    //{
    //    //팡파레 처럼 시작음
    //    AppSound.instance.SE_RESULT_START.Play();
    //    //승리한 정보를 받아야 한다.

      
    //    if (vic == 3)
    //    { //승리
    //        vicCard = GameData.Instance.playerVic;
    //        defCard = GameData.Instance.comVic;
    //    }
    //    else if (vic == 1)
    //    { //패배
    //        vicCard = GameData.Instance.comVic;
    //        defCard = GameData.Instance.playerVic;
    //    }
    //    //AppUIEffect.
    //}

    //delegate로  AppUIEffect에서 이펙트 오브젝트를 받기위해서 이벤트등록
    public static void newEffCreated()
    {
        if (onUnitySpawn != null)
        {
            onUnitySpawn();
        }

    }

    public static void EffDead()
    {
        if (onUnityDestroy != null)
            onUnityDestroy();
    }


    IEnumerator Auto_Step()
    {
        yield return new WaitForSeconds(0.1f);
        FormationCard();
        yield return new WaitForSeconds(1f);
        StartCoroutine(VictoryCardAnimation());
        yield return new WaitForSeconds(5f);
        StartCoroutine(DefectCardAnimation());
    }

    //승리한 데이터를 바탕으로 오브젝트를 생성한다.
    void FormationCard()
    {
        // vicCard.ContainsKey(i).Equals(vicCard.Values);
        //딕셔너리를 돌면서 id - card,card,card 를 배열시켜야 한다.
        //vicCard에 정보가 있는가 확인할것
        foreach (KeyValuePair<int, IList<Card>> id in GameData.Instance.playerVic)
        {
            //승리한 카드 
            GameObject newCard0 = Instantiate(card, VicgridGroup);
            Card vCardVic = new Card();
    
            vCardVic.ID         = id.Key;
            vCardVic.IconName   = GameData.Instance.UnityDatas[id.Key].Name;
            newCard0.GetComponent<VictoryCard>().SetCardInfo(vCardVic);
            //패배한 카드를 담을 리스트를 여기서 생성시킨다.
            // newCard0.GetComponent<VictoryCard>().vicCards = new List<Card>();
            newCard0.transform.localPosition = new Vector3(0, 0, 0);


            //승리한 카드가 패배시킨 카드들
            IList<Card> cards = new List<Card>();
            if (GameData.Instance.playerVic.TryGetValue(id.Key, out cards))
            {
                for (int d = cards.Count - 1; d >= 0; d--)
                {
                    GameObject newCard = Instantiate(card, gridGroup);
                    Card vCard         = new Card();
                    vCard.ID           = cards[d].ID;
                    vCard.IconName     = GameData.Instance.UnityDatas[cards[d].ID].Name;
                    newCard.GetComponent<VictoryCard>().SetCardInfo(vCard);
                    newCard.transform.localPosition = new Vector3(0, 0, 0);
                    newCard0.GetComponent<VictoryCard>().vicCards.Add(vCard);

                }
            }
        }
    } //FormationCard END

    //패배한 카드의 특성이나 승리한 카드의 특성을 추가할수 있는가?
    //높은 레벨의 카드를 패배시키면 더 많은 보상을 줄수 있는가?
    //승리한 카드 를 보여준다.
    IEnumerator VictoryCardAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        //group에서 카드의 정보를 받아서 검사한다.
        int vicCardNum = VicgridGroup.transform.childCount;
        //gridGroup에서의 순서를 위해서 
        //int sumNum = 0;
        float sideAmount = 400f;

        for (int i = 0; i < vicCardNum; i++)
        {

            //승리한 카드의 데이터를 가져올수 있게 한다.
            VicgridGroup.transform.GetChild(i);

            Vector3 pos = new Vector3(2400 + sideAmount, 50, 0);
            //titleName[0] Title [1] Kinds  [2] Level

            titleNames[0].text = VicgridGroup.transform.GetChild(i).transform.GetComponent<VictoryCard>().iconName.text;
            titleNames[1].text = string.Format("{0}", "");
            titleNames[2] = VicgridGroup.transform.GetChild(i).transform.GetComponent<VictoryCard>().txLevelNum;
            iTween.MoveTo(VicgridGroup.transform.GetChild(i).gameObject, iTween.Hash("islocal", true, "position", pos, "time", 0.6f,
                                                "onstart", "VicCardStart", "onstarttarget", gameObject, "easetype", "easeOutQuart"));
            //위치를정렬시킨다.
            //Grid를 코드로 구현한다.
            sideAmount -= 100;
            yield return new WaitForSeconds(1f);


            #region 연속애니실행시
            //int listCards = VicgridGroup.transform.GetChild(i).GetComponent<VictoryCard>().vicCards.Count;

            //    for (int j = 0; j < listCards; j++)
            //    {

            //        gridGroup.transform.GetChild(j + sumNum).gameObject.transform.localPosition = pos;
            //        //순서가 보장되는가?foreach에서 순서대로넣어서 가능할지 모름
            //        Vector3 pos2 = new Vector3(2400, 1000, 0);
            //        iTween.MoveTo(gridGroup.transform.GetChild(j+sumNum).gameObject, iTween.Hash("islocal", true, "position", pos2, "time", 0.5f,
            //                                            "easetype", "easeOutQuart"));

            //        yield return new WaitForSeconds(0.5f);
            //    }
            //sumNum += listCards;
            #endregion
        }

        yield return new WaitForSeconds(0.7f);


        VicgridGroup.GetComponent<GridLayoutGroup>().enabled = true;
        float sellX    = VicgridGroup.GetComponent<GridLayoutGroup>().cellSize.x;
        float spacingX = VicgridGroup.GetComponent<GridLayoutGroup>().spacing.x;
        float widthX   = vicCardNum * sellX + (vicCardNum - 1) * spacingX;
        float startX   = widthX * 0.5f * -1f;
        VicgridGroup.GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0);
        VicgridGroup.GetComponent<RectTransform>().localPosition = new Vector3(startX, 140, 0);

    }

    void VicCardStart()
    {
        AppSound.instance.SE_CARD_APPERMOVE.Play();
        
    }

    eDEFECTCARD eDefectState = eDEFECTCARD.NONE;


   
    IEnumerator DefectCardAnimation()
    {
        //승리한 카드의 전체 숫자
        int vicCardNum = VicgridGroup.transform.childCount;
        //패널그룹에서의 인덱스를 계산하기 위해서 사용
        int sumNum = 0;
        int sumNumCount = 0;
        //패배한 카드의 숫자를 넣는다.
        int cardCount = 0;
        int cardCountMax = 0;
        //승리한 카드의 카운트 시작
        int vicCount = 0;

        while (true)
        {
            if (eDefectState == eDEFECTCARD.NONE)
            {
                yield return new WaitForSeconds(0.1f);
                eDefectState = eDEFECTCARD.START;
            }
            else if (eDefectState == eDEFECTCARD.START)
            {
                eDefectState = eDEFECTCARD.VICTORYCARD;
            }
            else if (eDefectState == eDEFECTCARD.VICTORYCARD)
            {
                //전체승리한 카드의 숫자
                if (vicCardNum > 0)
                {
                    //승리한 카드의 턴 ERROR 접근에러 확률높음
                    //Vector3 cursize     = VicgridGroup.transform.GetChild(vicCount).GetComponent<RectTransform>().sizeDelta;
                    //해당 승리한 카드가 가지고 있는 패배한 카드의 수량을 기록한다.
                    int DefectcardsIndex = VicgridGroup.transform.GetChild(vicCount).GetComponent<VictoryCard>().vicCards.Count;
                    cardCountMax = DefectcardsIndex;

                    eDefectState = eDEFECTCARD.DEFECTCARD;
                    //승한 카드가 없습니다.
                   
                }
                else if (vicCardNum <= 0)
                {
                    Debug.Log("승리한 카드가 없습니다.");

                    //TODO: 메인씬으로이동한다.
                    //TODO:  아니면 좀더 다른 애니를 실행시킨다.
                    //일단 씬전환
                    SceneManager.LoadScene("3_Main_Scene");
                }

                //TODO: 마지막 카드가 사라지면 특별한 이벤트가 발생해야 한다.
                //카드가 움직임이 발생하면 특정한 이벤트발생

                yield return new WaitForSeconds(0.1f);
            }
            else if (eDefectState == eDEFECTCARD.DEFECTCARD)
            {
                //TODO : spawnNum의 양을 미리 입력해서 특수한 능력을 줘야 한다.

                int sp = gridGroup.transform.GetChild(cardCount + sumNum).gameObject.GetComponent<VictoryCard>().CardID;
                switch (sp)
                {

                    case 0:

                        Debug.Log("아이디 0이0가?");
                        break;

                    case 1:
                        //패배한 적의 ID 가 1이면 골들의 양을 100로 준다.
                        spawnNum = 100;
                        StartCoroutine(CoReturnDefaultGold(1));
                        Debug.Log("아이디 1이 없는건가?");
                        break;
                }
                //-------------------------------------------------------------------

                Vector3 pos = VicgridGroup.GetChild(vicCount).transform.position;
                VicgridGroup.transform.GetChild(vicCount).GetComponent<VictoryCard>().AnimeClick();
                //ERROR 발생함 원인 모름
                //gridGroup.transform.GetChild(j + sumNum).GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(cursize.x, cursize.y);
                //움직임을 시작할 시작위치
                gridGroup.transform.GetChild(cardCount + sumNum).gameObject.transform.position = pos;

                //마무리되는 위치 
                Vector3 pos2 = EndEffect.gameObject.GetComponent<RectTransform>().localPosition;
                //원하는 위치로 이동후 item이 스폰시 갯수 

                iTween.MoveTo(gridGroup.transform.GetChild(cardCount + sumNum).gameObject,
                              iTween.Hash("islocal", true, "position", pos2, "time", 0.8f,
                                         "onstart", "DefectCardStart", "onstarttarget", gameObject,
                                          "easetype", "easeOutQuart", "oncomplete", "OnCoinAnime",
                                          "oncompletetarget", gameObject, "oncompleteparams", spawnNum));

                yield return new WaitForSeconds(1f);
                //만약 특별한 기능이 없다면 바로 넘어가고 뭔가 있다면  여기서 대기 시킨다.
                eDefectState = eDEFECTCARD.DELAY;
                //진행된 카드의 숫자를 계산한다.

            }
            else if (eDefectState == eDEFECTCARD.DELAY)
            {   //패배한 카드가 어떤특수 능력이 있나 데이터를 확인한다.

                //특수한 기능을 추가 할수 있다----------------------------------
                //Show 적용된 특수능력이 적용됐다고 보여줘야 할부분
                int sp = gridGroup.transform.GetChild(cardCount + sumNum).gameObject.GetComponent<VictoryCard>().CardID;
                switch (sp)
                {

                    case 0:
                        Debug.Log("아이디 0이0가show?");
                        break;

                    case 1:
                        StartCoroutine(coAnimationSpec(.3f));
                        Debug.Log("아이디 1이 없는건가? 그림이 안보이는건가?");
                        break;
                }

                //----------------------------------------------------------
                //카드의 이동이 완료됐다면 안보이게 가린다.
                gridGroup.transform.GetChild(cardCount + sumNum).gameObject.GetComponent<VictoryCard>().hideCard.gameObject.SetActive(false);
                cardCount++;
                sumNumCount++;

                eDefectState = eDEFECTCARD.RESTART;
            }
            else if (eDefectState == eDEFECTCARD.RESTART)
            {   //끝낼것인지 다시 리턴시킬지 결정한다.

                //승리한 카드의 숫자와 카운터 인덱스가 같은지 판단한다.
                if (cardCountMax > cardCount)
                {
                    eDefectState = eDEFECTCARD.DEFECTCARD;

                }                                     //2              0,1,
                else if (cardCountMax <= cardCount && vicCardNum - 1 > vicCount)
                {                   //2true
                    //승리한 카드의 인덱스를 증가 시키고 패배카드의 인덱스를 0으로 리세팅한다.
                    vicCount++;
                    cardCount    = 0;
                    sumNum       = sumNumCount;
                    yield return new WaitForSeconds(1f);
                    eDefectState = eDEFECTCARD.VICTORYCARD;
                }
                else if (vicCardNum - 1 <= vicCount)
                {
                    //모든 회전을 다했으로 종료 시킨다.
                    StartCoroutine(ChangeSecne(3f));
                    yield break;
                }
            }
        }
    }
    //카드의 이동이 완료했을때 실행되어야 할것
    void OnCoinAnime(int amount)
    {
        //여기서 만약 특별한 기능이 있다면 잠시 대기 시켜야 한다.

        CollectingEffectController._instance.CollectItem(amount);
        AppUIEffect.instance.InstanceVFX(eEFFECT_NAME.GOLD);
        //코인애니메이션실행

    }

    IEnumerator CoReturnDefaultGold(float t)
    {
        yield return new WaitForSeconds(t);
        spawnNum = 5;
    }

    IEnumerator coAnimationSpec(float t)
    {
        yield return new WaitForSeconds(t);
        spec_Text.gameObject.SetActive(true);
        txSpecText.text = string.Format("GOLD 추가 획득 {0}", 100);

        yield return new WaitForSeconds(1.6f);
        spec_Text.gameObject.SetActive(false);
    }
    IEnumerator ChangeSecne(float t)
    {
        //만약 터치가 있다면 더 빨리 체인지 시킨다.
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("2_Main_Scene");
    }
    void DefectCardStart()
    {
        AppSound.instance.SE_CARD_DEFECTSHOOT.Play();
    }



     // TEST   Dictionary<int, IList<Card>> vicCard;
     //private void Update()
     // {
     //   if (Input.GetKeyDown(KeyCode.A))
     //   {
     //       OnCoinAnime(5);
     //   }
     //   #region AATemp
     //   if (Input.GetKeyDown(KeyCode.L))
     //   {

     //       //임시로 카드를 넣고 테스트 하기 위해서 작성
     //       int[] temp = { 2, 5, 4, 7 }; //일단 같은 카드가 없도록 한다.                     
     //       int[] temp2 = { 9, 8 };
     //       int[] temp3 = { 1, 6 };
     //       int[] victemp = { 3, 4, 1 };    //최종 승리한 카드 

     //       vicCard = new Dictionary<int, IList<Card>>();


     //       IList<Card> newList = new List<Card>();

     //       for (int i = 0; i < temp.Length; i++)
     //       {
     //           Card newCard = new Card();
     //           newCard.ID = GameData.Instance.UnityDatas[temp[i]].Id;
     //           newList.Add(newCard);
     //       }
     //       vicCard.Add(victemp[0], newList);

     //       IList<Card> newList2 = new List<Card>();
     //       for (int i = 0; i < temp2.Length; i++)
     //       {
     //           Card newCard = new Card();
     //           newCard.ID = GameData.Instance.UnityDatas[temp2[i]].Id;
     //           newList2.Add(newCard);
     //       }
     //       vicCard.Add(victemp[1], newList2);

     //       IList<Card> newList3 = new List<Card>();
     //       for (int i = 0; i < temp3.Length; i++)
     //       {
     //           Card newCard = new Card();
     //           newCard.ID = GameData.Instance.UnityDatas[temp3[i]].Id;
     //           newList3.Add(newCard);
     //       }
     //       vicCard.Add(victemp[2], newList3);

     //       if (Input.GetKeyDown(KeyCode.K))
     //       {
     //           FormationCard();
     //       }

     //       if (Input.GetKeyDown(KeyCode.S))
     //       {
     //           //StartCoroutine(ResultAnimation());
     //       }
     //       #endregion

     //       if (Input.GetKeyDown(KeyCode.P))
     //       {

     //           // public Dictionary<int, IList<Card>> playerVic;
     //           // public Dictionary<int, IList<Card>> comVic;
     //           //public Dictionary<int, IList<Card>> defectPlayer;
     //           // GameData.Instance.playerVic

     //       }
     //     }
     //    }//UPdate END
     }



    




