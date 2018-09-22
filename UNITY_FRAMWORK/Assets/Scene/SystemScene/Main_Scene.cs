using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using LitJson;
using System.IO;
using System.Linq;
using System;




public class Main_Scene : MonoBehaviour, IPointerClickHandler
{

    public RectTransform[] panelItems;   // UI의 bottom메뉴  Item고유ID설정
    public GameObject cardObj;           //선택되어 tab1의 위치로 이동하기 전에 보여줘야 할 오브젝트 
    public GameObject itemSample1;       //카드가 선택되서 복제될 오브젝트 ,하단의선택 Tab
    public GameObject itemSample2;       //선택되어 보여지는 버튼
    public GameObject[] CaseItem;        //상품이 선택됐을때 열리게 하기
    public GameObject fadeOut;           //fadeOut 를 복제하기 위함

    public Transform marketTop;          //market Top메뉴
    public Transform marketSpecial;      //daily 
    public Transform marketNor;          //상단 특별상자
    public Transform marketRoyal;        //보석으로 살수 있는 상자


    public Transform TopLabel_experience; //메인상단의 경험치 라벨
    //public Transform TopLabel_Coins;      //플레이어 보유 전체금액
    public Transform TopLabel_Jews;       //플레이어 보유 전체 보석

    //public Transform  buttonPanel;      //복사되어 배치될 위치 앞면
    //public Transform  buttonPanelBack;  //복사되어 배치될 버튼중 뒤

    public Transform switchPanel;         //선택시 보여질 버튼이 보여질 
    public Transform switchMove;          //switch가 교환되기 위해서 위치해야할  root
    public Transform unfindCards;         //찾지못한 카드를 배치하게 할 위치 
    public Transform temp;                //찾은 카드를 배치하게 할 위치 
                                          //public Transform temp2;             //찾은 카드를 배치할 위치
    public int playerindex = 1;           //플레이어의 데이터 ID;


    //public Transform switchTemp;        //switch의 바뀌는 카드가 임시로 위치할 좌표
    public Text colectCard;               //찾은 카드의 숫자를 표시해준다.
    public Text buttonCollectionLabel;    //카드 정렬이 바뀌었을때 정렬이름을 정해준다.

    public static string nextScene;
    public UnityEvent FADEBUTTON;
    public PlayerSaveJsonData saveJsonData;

    List<DataClass>      list         = new List<DataClass>();                   //?
    IList<RectTransform> lsSlots      = new List<RectTransform>();       //slot의 위치 
    IList<RectTransform> lsSlotsBack  = new List<RectTransform>();       //slotBack

    IList<RectTransform> lsSwtichSlots = new List<RectTransform>();      //swtichSlots

    List<UnityCard> lsCards           = new List<UnityCard>();           //찾은 카드를 선택별로 정렬하기위해 담는다.
    List<UnityCard> lsSwitchCards     = new List<UnityCard>();           //찾은 카드를 선택별로 정렬하기위해 담는다.

    List<UnityCard> tempCards         = new List<UnityCard>();            //tab 버튼이 실행됐을때 생성해 주는 unityCards
    List<UnityCard> tempCards2        = new List<UnityCard>();            //tab 버튼이 실행됐을때 생성해 주는 unityCards

    public Dictionary<int, Vector2> switchGrid = new Dictionary<int, Vector2>();
                                                                    //switch의 anchor의 좌표를 순서대로 기록한다.
    List<int> InDexID = new List<int>();                            //항상 바뀔수 있는 정렬 이므로 이벤트 발생때마다 생성정렬
                                                                    //public으로 등록된 DailySale스크립트를 리스트화 한다.


    int ArrayNum = 0;                                               // 0 :기본시작정렬 1:엘릭서  -1:엘릭서+ratial
    int arrayPos = -1;                                              //gridGroup에 순차적으로 들어갈 순서
    int curTabButton = 1;
    int levelCount;                                                 //레벨업까지 필요한 카드의 숫자를 계산한다.

    [SerializeField]
    Slider slider;
    string jumpSceneName;
    //FadeOut fadeOut;
    public delegate void    ButtonClick();
    public static   event   ButtonClick OnButtonHandler;
    public UnityEvent ARRAYCARDS;

    //ScrollViewController stroll;

    //Tab버튼 3개
    int[] cur           = new int[3];
    //tab에 선택되어지는 갯수 
    int[] tem           = new int[4];
    IList<int> hasTem   = new List<int>();  //hasCards에서 int 만 리스트화 
    IList<int> hasTemp  = new List<int>();  //tob1의 카드를 빼기 위해서 임시로 생성

    GameObject MoveSelect;                  //Tab로 이동하게 하기 위해서 생성된 객체 

    bool bCanvasClick;
    bool isRed;
    bool isSwitch;                          //Tab와 교환하기 위해서 선택되어있을경우 
                                            //tab에 이미선택되어 있는지판단
    bool isTabIN;
    Text[] experinceText;
    Text coinText;
    Text jewText;
    int curGold;                          //현재의Gold 를 기록하고 증가시킬때  From의 역할을 한다.
    int addGold;                          //증가시키너가 감소시킬 값
                                          //Level 단계 TODO: LEVEL 데이터화 필요 카드 경험치
    float yPos = -400f;                   //카드가 교체되기 위해서 중심으로 이동하는 y좌표

    int[] playerLevel = { 500, 1000, 2000, 3000, 4000, 6000, 8000, 10000, 20000, 30000, 40000, 50000 };



    public bool canvasClick
    {
        get { return bCanvasClick; }
        set { value = bCanvasClick; }
    }


    void Awake()
    {
        for (int i = 0; i < panelItems.Length; i++)
        {
            panelItems[i].GetComponentInChildren<UI_Icon>().PanelItem = i + 1;
        }

        //fadeOut = GameObject.Find("FadeOut").GetComponent<FadeOut>();

        experinceText = TopLabel_experience.GetComponentsInChildren<Text>();
        //coinText    = TopLabel_Coins.GetComponentInChildren<Text>();
        jewText       = TopLabel_Jews.GetComponentInChildren<Text>();

     
        //DontDestroyOnLoad(this.gameObject);
        
    }

    void Start() {
        // !!! 가비지 컬렉션 강제 실행 !!!
         System.GC.Collect();
        // !!!!!!!!!!!!!!!!!!!!!
        //stroll = GetComponent<ScrollViewController>();
        //시작과 동시에 item 에 필요한 json데이터를파싱한다.

        // StartCoroutine(FADEOUT_FLASH());
        // slot 리스트 좌표임
        //lsSlots       = buttonPanel.GetComponent<UI_GridGroup>().lsrcTransforms;
        //lsSlotsBack   = buttonPanelBack.GetComponent<UI_GridGroup>().lsrcTransforms;
        StrollVertical.eveVerticalMove += DeleteCopyUnity;
        lsSwtichSlots = switchPanel.GetComponent<UI_GridGroup>().lsrcTransforms;
        //시작시 1부터 시작
        GameData.Instance.PanelItem = 1;

        //플레이어의 키를 확인 플레이어1의 데이터를 가지고 온다.
        if (GameData.Instance.players.ContainsKey(playerindex))
        {

            string exp = GameData.Instance.players[playerindex].exp.ToString();
            string Num = GameData.Instance.players[playerindex].expCount.ToString();
            string defaultExp = playerLevel[GameData.Instance.players[playerindex].exp].ToString();
            experinceText[0].text = string.Format("{0}/{1}", Num, defaultExp);
            experinceText[1].text = string.Format("{0}", GameData.Instance.players[playerindex].exp);
            //메인에서 직접 출력해 줄 경우 
            //coinText.text = string.Format("{0}", GameData.Instance.players[1].coin);
            curGold = GameData.Instance.players[playerindex].coin;
            jewText.text = string.Format("{0}", GameData.Instance.players[playerindex].jew);
            //시작과동시에 다시 자신의 골드를 프로퍼티로 사용학 위해서  데이터에 저장한다.
        }

        //FADEBUTTON.Invoke();
        //StartCoroutine(FADEOUT_FLASH());
        //카드의 Icon Imag를 바꿔주기 위해서 시작시 로드한다.

        Create_FADEOUT(marketRoyal.parent.parent, 0.8f);
        //----------
        //Tab 에 위치하게될 카드
        //isSelectDeck();
        UnFindCardSet();
        //Market 생성
        MarketSetDaily();
        MarketSetRoyle();
        MarketSetNor();
    }
    //카드가 이동이 완료되기전에 턴이 발생시 원래데로 돌려놓기 초기화 하기 위해서 삭제함
    void DeleteCopyUnity()
    {
        //ERROR요인 아님..ㅠ.ㅠ
        // if (switchMove.childCount > 1)
        //{
        //    GameObject a = switchMove.transform.Find("ItemCard(Clone)").gameObject;
        //    Destroy(a);
        //}
    }
    //void Button_Play(MenuObject_Button button)
    //   {
    //	SaveData.continuePlay 				= false;
    //	PlayerController.initParam 			= true;
    //	PlayerController.checkPointEnabled 	= false;

    //	zFoxFadeFilter.instance.FadeOut (Color.white, 1.0f);
    //	AppSound.instance.SE_MENU_OK.Play ();
    //	jumpSceneName = "StageA";
    //	Invoke ("SceneJump",1.2f);
    //}
    //void Button_Continue(MenuObject_Button button)
    //   {
    //	SaveData.continuePlay 				= true;
    //	PlayerController.initParam 			= false;

    //	zFoxFadeFilter.instance.FadeOut (Color.white, 1.0f);
    //	AppSound.instance.SE_MENU_OK.Play ();
    //	jumpSceneName = SaveData.LoadContinueSceneName();
    //	Invoke ("SceneJump",1.2f);
    //}
    //void Button_HiScore(MenuObject_Button button)
    //   {
    //	zFoxFadeFilter.instance.FadeOut (Color.black, 0.5f);
    //	AppSound.instance.SE_MENU_OK.Play ();
    //	jumpSceneName = "Menu_HiScore";
    //	Invoke ("SceneJump",1.2f);
    //}
    //void Button_Option(MenuObject_Button button)
    //   {
    //	zFoxFadeFilter.instance.FadeOut (Color.black, 0.5f);
    //	AppSound.instance.SE_MENU_OK.Play ();
    //	jumpSceneName = "Menu_Option";
    //	Invoke ("SceneJump",1.2f);
    //}

    void OnEnable()
    {
        CardEff_Open.eff += FadeOutA;
       //StrollVertical.eveVerticalMove += DeleteCopyUnity;

    }

    //void OnDisable()
    //{
    //    StrollVertical.eveVerticalMove -= DeleteCopyUnity;
    //}

    void FadeOutA()
    {
        Create_FADEOUT(marketRoyal.parent.parent,0.5f);
    }

    void SceneJump()
    {
        Debug.Log(string.Format("Start Game : {0}", jumpSceneName));
        SceneManager.LoadScene(jumpSceneName);
    }

    //계속 반복되는 코드 발생:Main_Scene,Game_Secne .
    //따로 쓰는 이유는 버튼OnClick에서 스크립트명과 오브젝트명이 같아야 되는 문제 때문임
    public void SceneChange()
    {
        SceneManager.LoadScene("3_Game_Scene");
        //nextScene = "2_Main_Scene";
    }

    public void SceneChangeTitel()
    {
        SceneManager.LoadScene("0_Title_Scene");
        //nextScene = "2_Main_Scene";
    }

    //  //순간적으로 빨리 켜지고 꺼지는 효과 
    //  IEnumerator FADEOUT_FLASH()
    //  {
    //      fadeOut.isFadeIN = true;
    //      fadeOut.StartFadeAnim();
    //      yield return new WaitForSeconds(0.1f);
    //  }

    //public void  FADEOUT_FLASHClick()
    //  {
    //      fadeOut.isFadeIN = true;
    //      fadeOut.StartFadeAnim();

    //  }

    void Create_FADEOUT(Transform t,float delay)
    {
        GameObject fadeObj = Instantiate(fadeOut,t);
        fadeObj.GetComponent<FadeOut>().animTime = delay;
    }

    #region NotNeedCode1

    //public void cardMade()
    //{
    //    #region ex)
    //    //카드의 정보를 읽어와서 만들 메소드 --예시-------------------------------
    //    //List<int> listHasCard = new List<int>(GameData.Instance.hasCard.Keys);
    //    //foreach (var ke in listHasCard)
    //    //{
    //    //}-----------------------------------------------------------------------
    //    #endregion

    //    int switchIndex = 0;
    //    //기본적으로 설정된 전체 카드 정보
    //    int units = GameData.Instance.UnityDatas.Count;
    //    //시작시에 tab1,tab2,tab3중에 활성화 된것의 id를 얻는다.
    //    int curTab = GameData.Instance.curTab;

    //    //유닛 카드를 하나씩 전부 생성한다.

    //    foreach (KeyValuePair<int, int[]> pair in GameData.Instance.playerSelectDecks)
    //    {
    //        //정렬시킬카드에서 curTab에 선택된것을 빼기 위해서 
    //        if (pair.Key == curTab)
    //        {
    //            tem = pair.Value;
    //            //pair .1,4,8,2
    //        }
    //    }

    //    foreach (KeyValuePair<int, Card> pair in GameData.Instance.hasCard)
    //    {       //3,2,4,5,7,1,8

    //         hasTem.Add(pair.Value.ID);
    //         hasTemp.Add(pair.Value.ID);
    //    }

    //    #region TEST1


    //     for (int i = 0; i < GameData.Instance.hasCard.Count; i++)
    //     {
    //             #region ES

    //             // GameObject temp    = Instantiate(cardObj);

    //             //찾지못한 카드의 위치
    //             // temp.transform.parent = unfindCards;
    //             //부모에 따라서 상대위치가 달라지므로 다시 스케일 세팅해야됨
    //             //  temp.transform.localScale = new Vector3(1, 1, 1);
    //             //ugui에서의 z값이 없기 때문에 쓰레기 값이 들어 갈수 있으므로 z -->0 으로 세팅
    //             // x,y는 영향을 주지 못함
    //             // temp.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

    //             //카드의 세팅
    //             // string cardName = GameData.Instance.UnityDatas[i].Name;
    //             // int elixir      = GameData.Instance.UnityDatas[i].Elixir;

    //             //slot
    //             //|_panel moving  able
    //             // temp.GetComponent<UI_Panel_Item>().ID = GameData.Instance.UnityDatas[i].Id;
    //             // temp.GetComponent<UI_Panel_Item>().Elixir = GameData.Instance.UnityDatas[i].Elixir;
    //             //temp.GetComponent<UI_Panel_Item>().kinds = GameData.Instance.UnityDatas[i].Kinds;

    //             //카드이름
    //             //temp.GetComponent<UnityCard>().Icon_name = GameData.Instance.UnityDatas[i].Name;
    //             ////엘릭서 
    //             //temp.GetComponent<UnityCard>().Elixer    = GameData.Instance.UnityDatas[i].Elixir;
    //             ////enum 으로 리스트를 위한 number을 얻는다.
    //             //temp.GetComponent<UnityCard>().Kinds     = GameData.Instance.UnityDatas[i].Kinds;
    //             //temp.GetComponent<UnityCard>().ID        = GameData.Instance.UnityDatas[i].Id;

    //             //TODO: 리소스를 불러와서 교체하는 구간임 kind타입에 따라 아이콘의 모양이 달라지게 한다.
    //             //if (temp.GetComponent<UnityCard>().kindNum >= 1 && temp.GetComponent<UnityCard>().kindNum < 3)
    //             //{
    //             //    temp.GetComponent<UnityCard>().mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", "Sample_UI_1");
    //             //    //전체 테두리
    //             //    temp.GetComponent<Image>().sprite = SpriteManager.GetSpriteByName("Sprite", "Sample_UI_1");
    //             //}
    //             //float width  = buttonPanel.GetComponent<GridLayoutGroup>().cellSize.x;
    //             //float height = buttonPanel.GetComponent<GridLayoutGroup>().cellSize.y;
    //             //float spaceX = buttonPanel.GetComponent<GridLayoutGroup>().spacing.x;
    //             //float spaceY = buttonPanel.GetComponent<GridLayoutGroup>().spacing.y;

    //             //tab에 있는 id를 제외하고 id 0 ,4,8,2,
    //             //카드 정보와 player가 가지고 있고 tab1에 선택되어진 카드가 아닐때
    //             //Containkey, 인덱스가 일치한다.따라서 비교가능하다.즉 i 가 id와 같음
    //             #endregion
    //             for (int j = 0; j < tem.Length; j++)
    //             {
    //                 if (hasTem[i] == tem[j])
    //                 {
    //                 hasTemp.RemoveAt(i);
    //                 hasTemp.Insert(i,0);

    //                 break;

    //                 }
    //                 else
    //                 {
    //                        continue;
    //                 }

    //             }


    //      } //forEND

    //     for (int i = 0; i < hasTemp.Count; i++)
    //     {
    //         if (hasTemp[i] > 0)
    //         {

    //                 //false일때 생성
    //                 GameObject sample1 = Instantiate(itemSample1); //panel_button에 위치
    //                 GameObject sample2 = Instantiate(itemSample2); //panel_switch 위치

    //                 sample1.transform.parent = lsSlots[switchIndex].transform;
    //                 sample1.GetComponent<UnityCard>().rootIndex = switchIndex;
    //                 //sloat의 크기를 입력해준다.
    //                 sample1.GetComponent<UnityCard>().width = 260f;
    //                 sample1.GetComponent<UnityCard>().height = 400f;
    //                 sample1.GetComponent<UnityCard>().ID = hasTemp[i]; //키와 같다면 id를 입력한다.
    //                 sample1.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);//부모의 위치 세팅
    //                 sample1.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

    //                 sample2.transform.parent = lsSwtichSlots[switchIndex].transform;
    //                 sample2.GetComponent<UnityCard>().rootIndex = switchIndex;
    //                 //sloat의 크기를 입력해준다.
    //                 sample2.GetComponent<UnityCard>().width = 260f;
    //                 sample2.GetComponent<UnityCard>().height = 400f;
    //                 sample2.GetComponent<UnityCard>().ID = hasTemp[i]; //키와 같다면 id를 입력한다.
    //                 sample2.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);//부모의 위치 세팅
    //                 sample2.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

    //                 #region haveCard Setting

    //                 //int cardNum = GameData.Instance.hasCard[i].hasCard;
    //                 //int cardhavLevel = GameData.Instance.hasCard[i].level;
    //                 ////카드에anchor 좌표를 그리드에맞게 저장해 주어야 한다.
    //                 ////grid에 밑에 놓이게 되면 외부에서 anchor에 접근할수 없기 때문이다.
    //                 ////Level
    //                 //for (int index = 0; index < cardhavLevel; index++)
    //                 //{
    //                 //    LevelSum += Level[index]; //현재 까지의 레벨에 필요한 카드 갯수
    //                 //    visualCardNum = cardNum - LevelSum;
    //                 //    //남은 숫자가 레벨*숫자 보다 클경우 
    //                 //    temp.GetComponent<UnityCard>().RemainCardNum = visualCardNum;
    //                 //}

    //                 ////업데이트후의 남은 카드
    //                 //temp.GetComponent<UnityCard>().LevelCards        = Level[cardhavLevel];
    //                 //temp.GetComponent<UnityCard>().LevelNum.text     = string.Format("레벨 " + cardhavLevel);
    //                 //temp.GetComponent<UnityCard>().tempLevelNum.text = string.Format("카드 :" + cardNum); //확인위한전체카드
    //                 //temp.GetComponent<UnityCard>().ownCards.text     = string.Format(visualCardNum + "/" + cardhavLevel);
    //                 ////현재의 레벨을 받아서 UnityCard에저장
    //                 //temp.GetComponent<UnityCard>().Level = GameData.Instance.hasCard[i].level;

    //                 //if (temp.GetComponent<UnityCard>().slider != null)
    //                 //{ //Slider 설정
    //                 //    temp.GetComponent<UnityCard>().slider.maxValue = cardhavLevel;
    //                 //    temp.GetComponent<UnityCard>().slider.value    = visualCardNum;
    //                 //}

    //                 //버튼 생성시 발생시킬 특수한 기능을 리스너로 추가 시킴 
    //                 //itemSample2 2번째복제된것에 위치값을 전달한다.
    //                 //--------------버튼 정렬이 바뀌면 순서도 같이 바뀌어야 함.--------
    //                 //data - GameObject,panel의 좌표,
    //                 //sample1.GetComponentInChildren<UnityCard>().ID = i;
    //                 //float ax = width * 0.5f + arrayPos % 4 * (width + spaceX);
    //                 //float ay = 0 - (height * 0.5f + arrayPos / 4 * (height + spaceY));
    //                 //생성될 스크립트에 그리드의 좌표를 전달,anchor 를 수정한다.
    //                 //sample1.GetComponentInChildren<UnityCard>().VcGridPos = new Vector2(ax,ay);

    //                 //sample1.transform.parent = switchPanel;
    //                 ////새로 생성된 카드를 ID에 맞게 세팅한다.
    //                 //sample1.GetComponent<UnityCard>().ReSeting();
    //                 //Vector2 min = new Vector2(0, 1);
    //                 //Vector2 max = new Vector2(0, 1);
    //                 //Vector2 pivot = new Vector2(0.5f, 0.5f);
    //                 //sample1.GetComponent<RectTransform>().anchorMin = min;
    //                 //sample1.GetComponent<RectTransform>().anchorMax = max;
    //                 //sample1.GetComponent<RectTransform>().pivot = pivot;
    //                 //sample1.GetComponent<RectTransform>().localPosition = new Vector3(ax, ay, 0);//부모의 위치 세팅
    //                 //sample1.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //                 ////switch의 anchorPositon
    //                 //switchGrid.Add(switchIndex, new Vector2(ax,ay));


    //                 //접근해서 할성화 시키지 못함..원인 모름
    //                 //temp.GetComponent<Button>().onClick.AddListener(delegate () { sample1.GetComponentInChildren<UnityCard>().OnClickFunc(); });
    //                 //sample1.transform.Find("Panel").gameObject.SetActive(false);
    //                 #endregion

    //                 //TODO: GetComponent를 너무 많이 써서 늦어질 경우~ 수정필요
    //                 //TODO:카드의 화살표 가 애니작동하게 한다.  Card에 할 것인지 ,,여기서 할것인가?
    //                 //카드 자체에서 숫자를 얻어서 작동하게 하게 만들기
    //                 lsCards.Add(sample1.GetComponent<UnityCard>());
    //                 lsSwitchCards.Add(sample2.GetComponent<UnityCard>());
    //                 //카드에 버튼기능을 활성화 시켜준다.
    //                 switchIndex++;
    //         }

    //     }  //For END 
    //    #endregion


    //}
    #endregion
    //카드컬렉션에 아이템을 만든다.


    #region Not실행시키지 않음

    //public void UnSameCard()
    //{
    //    Dictionary<int, UnityCard> middle = new Dictionary<int, UnityCard>();

    //    for (int i = 0; i < lsCards.Count; i++)
    //    {
    //        middle.Add(lsCards[i].ID, lsCards[i]);
    //    }

    //    for (int i = 0; i < lsCards.Count; i++)
    //    {
    //        IList<UnityCard> ttemp = new List<UnityCard>();
    //        IList<UnityCard> result = new List<UnityCard>();
    //        if (middle.ContainsKey(lsCards[i].ID))
    //        {

    //            ttemp.Add(lsCards[i]); 
    //        }

    //        if (ttemp.Count > 1)
    //        {

    //            for (int j = 0; j < ttemp.Count; j++)
    //            {
    //                do
    //                {
    //                    result.Add(ttemp[j]);
    //                    continue;
    //                }
    //                while (false);

    //                ttemp.RemoveAt(j);

    //            }
    //        }

    //    }

    //}
    #endregion
    //같은 아이디가 있다면 하나는 삭제하게 한다.

    //버튼에 따른 엘릭서 가 바뀌게 한다.
    public void ArrayButton()
    {
        //현재 = 0// 1,2,3,
        ArrayNum++;
        if (ArrayNum > 3)
        {
            ArrayNum = 1;
        }

        switch (ArrayNum)
        {
            case 1:
                ArrayDatasButton();

                break;
            case 2:
                ArrayRartlyButton();

                break;
            case 3:
                ArrayElixerButton();

                break;
            default:
                break;
        }
    }

    //카드의 이미지 적용부분..찾아야됨..
    //string name = GameData.Instance.UnityDatas[i].Name;
    //tempCards2[i].GetComponentInChildren<UnityCard>().mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", name);
    void SetReFormCopy(List<int> arr)
    {
        UnityCard[] chs = switchPanel.GetComponentsInChildren<UnityCard>();
        //Debug----------------------
        //현재 switch의 위치가 변했는지 검사 
        for (int i = 0; i < chs.Count(); i++)
        {
            print("chs" + "[" + i + "]" + chs[i].ID);
        }
        //arr의 데이터가 제대로 불러와 지는가판단
        for (int i = 0; i < arr.Count; i++)
        {
            print("arr 배열" + arr[i]);
        }
        //Debug End-----------------------
        // tempCards.Add(i);  0 ~ 5 value
        // InDexID.Add(i.ID);
        Vector3[] tempList = new Vector3[switchGrid.Count];//temList 6, 0~5
                                                           //접근할때float a = switchGrid[j + 1].x;
                                                           //array,arrayElick,, 등 선택된 카드의 변화된 정렬id를 순서대로 얻어온다
                                                           //arr ---> IdIndex<int>()
                                                           //현재 어느 패널에 있는지 판단

        for (int rp = 0; rp < arr.Count; rp++)         //0~5
        {
            //현재 Switch폴더의 리스트와 비교***1,2,3,4,5,6
            //arrayGrneric 의 id순서 ==현재switchPanel의 id
            if (arr[rp] == chs[rp].ID)
            {
                chs[rp].gameObject.transform.parent = switchPanel;
                // arr :0,1,4,5,7,8 
                // arr :0,5,2,5,8,4,
                continue;
            }
            else
            {
                for (int i = 0; i < chs.Length; i++)
                {
                    //0 == 0 같으니까 건너띄고
                    // 1 -->> 0,1,2,3,4,5
                    if (arr[rp] == chs[i].ID)
                    {
                        //print("else 에서 같을때 위치가 변경됩니다." + chs[i].ID);         //rp 1-->6 이고 다른 것들은 0->5
                        chs[i].GetComponent<RectTransform>().localPosition = new Vector2(switchGrid[rp + 1].x, switchGrid[rp + 1].y);
                        chs[i].gameObject.transform.parent = switchPanel;
                    }
                }
            }
        }
    }

    void ArrayDatasButton()
    {
        tempCards.Clear();
        tempCards2.Clear();
        buttonCollectionLabel.text = string.Format("엘릭서 기준");
        //slot  
        var lists = GameData.Instance.panelSlots.Where(m => m.Elixer >= 0).OrderBy(m => m.Elixer).ThenBy(m => m.Elixer);
        //기존의 위치에 정렬되어 있다면 패널을 바꾸어 주어야 한다. 그래야 재정렬됨
        var listsBacks = GameData.Instance.panelBackSlots.Where(m => m.Elixer >= 0).OrderBy(m => m.Elixer).ThenBy(m => m.Elixer);

        foreach (var i in lists)
        {
            tempCards.Add(i);
        }

        foreach (var i in listsBacks)
        {
            tempCards2.Add(i);//hide된cardItem
        }

        for (int i = 0; i < tempCards.Count; i++)
        {
            //현재 위치하게될 위치
            //Vector3 temp =  lsSlots[i].transform.localPosition;
            //unityCard에 이동해야할 위치를 입력한다.
            int aa = tempCards[i].iD;
            print("tempCards[ ]  " + i + "[" + aa + "]");
            tempCards[i].GetComponentInChildren<UnityCard>().nextIndex = i;
            tempCards[i].GetComponentInChildren<UnityCard>().MoveSelectPanel();
            tempCards2[i].GetComponentInChildren<UnityCard>().nextIndex = i;
            tempCards2[i].GetComponentInChildren<UnityCard>().MoveSelectPanel();
            //tempCards2[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(false);
           
        } //END

    }

    void ArrayElixerButton()
    {

        tempCards.Clear();
        tempCards2.Clear();
        int idIndex = 0;
        buttonCollectionLabel.text = string.Format("엘릭서 기준");

        var lists      = GameData.Instance.panelSlots.Where(m => m.Elixer >= 0).OrderByDescending(m => m.Elixer).ThenBy(m => m.Elixer);
        var listsBacks = GameData.Instance.panelBackSlots.Where(m => m.Elixer >= 0).OrderByDescending(m => m.Elixer).ThenBy(m => m.Elixer);

        foreach (var i in lists)
        {
            tempCards.Add(i);
        }

        foreach (var i in listsBacks)
        {
            tempCards2.Add(i);//hide된cardItem
        }

        for (int i = 0; i < tempCards.Count; i++)
        {
            tempCards[i].GetComponentInChildren<UnityCard>().nextIndex = i;
            tempCards[i].GetComponentInChildren<UnityCard>().MoveSelectPanel();
            tempCards2[i].GetComponentInChildren<UnityCard>().nextIndex = i;
            tempCards2[i].GetComponentInChildren<UnityCard>().MoveSelectPanel();
        } //END

    }

    public void ArrayRartlyButton()
    {

        tempCards.Clear();
        tempCards2.Clear();

        buttonCollectionLabel.text = string.Format("희귀도 기준");
        var lists                  = GameData.Instance.panelSlots.Where(m => m.kindNum >= 0).OrderBy(m => m.Elixer).ThenBy(m => m.Elixer);
        var listsBacks             = GameData.Instance.panelBackSlots.Where(m => m.kindNum >= 0).OrderBy(m => m.Elixer).ThenBy(m => m.Elixer);

        foreach (var i in lists)
        {
            tempCards.Add(i);
        }

        foreach (var i in listsBacks)
        {
            tempCards2.Add(i);//hide된cardItem
        }


        for (int i = 0; i < tempCards.Count; i++)
        {
            tempCards[i].GetComponentInChildren<UnityCard>().nextIndex = i;
            tempCards[i].GetComponentInChildren<UnityCard>().MoveSelectPanel();
            tempCards2[i].GetComponentInChildren<UnityCard>().nextIndex = i;
            tempCards2[i].GetComponentInChildren<UnityCard>().MoveSelectPanel();
            //tempCards2[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(false);

        } //END

        #region Sample

        // if (temp.childCount <= 0)
        // {
        //     foreach (var i in list)
        //     {
        //         tempCards.Add(i);
        //         InDexID.Add(i.ID);

        //         i.transform.parent = temp;
        //     }
        //     //SetReFormCopy(InDexID);
        // }
        //else if (buttonPanel.childCount <= 0)
        // {
        //     foreach (var i in list)
        //     {
        //         tempCards.Add(i);
        //         InDexID.Add(i.ID);
        //         i.transform.parent = buttonPanel;
        //     }
        //    // SetReFormCopy(InDexID);
        // }
        #endregion
    }



    public void SetTopMenu()
    {
        addGold = GameData.Instance.curAddGold;
        //experinceText[0].text = string.Format("{0}/{1}", Num, defaultExp);
        experinceText[1].text = string.Format("{0}", GameData.Instance.players[playerindex].exp);

        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash("from", curGold, "to", curGold - addGold, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
        curGold -= addGold;
        jewText.text = string.Format("{0}", GameData.Instance.players[playerindex].jew);
    }

    //-----TODO:개별적으로 작동하게 함
    void UpdateGoldDisplay(int curGold)
    {
        //직접 top의 프러퍼티에 수정한다.이벤트를 위해서 임
        ItemDisplayer.instance_ItemDisplayer.CurhasGold = curGold;
        //coinText.text = string.Format("{0}", curGold);
    }

    void MarketSetDaily()
    {
        IList<DailySale> days = new List<DailySale>();
        days = marketSpecial.gameObject.GetComponentsInChildren<DailySale>();

        //리스트를 딕셔너리화 한다. 지정해서 특별한 기능을 추하또는 판단하기 위해서 
        for (int i = 0; i < days.Count; i++)
        {   //list =  파싱된 데이터를 days에 순서대로 대입
            //프로퍼티 ID를 호출해서 네임이 바뀌게 한다.
            days[i].ID          = GameData.Instance.dailys[i].ID;
            //TODO: Image
            string name         = GameData.Instance.UnityDatas[i].Name;
            days[i].priceCoin   = GameData.Instance.dailys[i].Gold;
           
            days[i].priceJew    = GameData.Instance.dailys[i].Jew;
            days[i].ea          = GameData.Instance.dailys[i].EA;
            days[i].main.sprite = SpriteManager.GetSpriteByName("Sprite", name);

          
            //index 와 스크립트를 어떻게 연결할 것인가?
        }

    }



    //전설상자,행운상자,히어로상자
    void MarketSetNor()
    {
        IList<Button_Base> Nors = new List<Button_Base>();
        Nors = marketNor.gameObject.GetComponentsInChildren<Button_Base>();
        for (int i = 0; i < Nors.Count; i++)
        {
            Nors[i].SetInfo(GameData.Instance.nors[i]);
        }
    }

    void MarketSetRoyle()
    {
        IList<RoyleSale> royle = new List<RoyleSale>();
        royle = marketRoyal.gameObject.GetComponentsInChildren<RoyleSale>();

        for (int i = 0; i < royle.Count; i++)
        {
            royle[i].priceJew = GameData.Instance.royles[i].Jew;
            royle[i].Name     = GameData.Instance.royles[i].CaseName;
            royle[i].BoxName  = GameData.Instance.royles[i].CaseIndex;

        }
    }



    #region NotNeedCode?

    ////Tab 에 아이템을 만든다.
    //void isSelectDeck()
    //{
    //    // is Show버튼 숫자 tab의 3중 하나
    //    bool[] isShow = new bool[cur.Length];

    //    foreach (KeyValuePair<int, int[]> pair in GameData.Instance.playerSelectDecks)
    //    {
    //        for (int i = 0; i < cur.Length; i++)
    //        {
    //            if (pair.Key == i)
    //            {
    //                int[] tem = pair.Value;
    //                if (0 < tem.Length)
    //                {  // 0,true,  1,true  2,true
    //                    isShow[i] = true;
    //                }
    //            }
    //        }

    //        for (int i = 0; i < isShow.Length; i++)
    //        {

    //            int isFalse = 0;
    //            int isTrue = 0;

    //            if (isShow[i] == false)
    //            {
    //                isFalse++;
    //            }
    //            else
    //            {
    //                isTrue++;
    //            }

    //            if (isTrue > 1 && isTrue <= isShow.Length)
    //            {
    //                //2,3     //1,2,3
    //                //GameData에서 저장된 tab를 가져와서 현재의 버튼저장
    //                curTabButton = GameData.Instance.curTab;
    //                //tab버튼을 SetActivate(true)
    //            }
    //            else if (isTrue == 1)
    //            {
    //                for (int j = 0; j < isShow.Length; j++)
    //                {
    //                    if (isShow[j] == true)
    //                    {    //tab 버튼 인덱스가 1부터라서 +1
    //                        curTabButton = j+1;
    //                        //게임데이터에도 설정해 준다.
    //                        GameData.Instance.curTab = j+1;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    #endregion

    //카드 선택시 중앙으로 이동하여 보여지는  카드 
    public void SwitchSelectCard()
    {
        //swtichStart가 동작할수 있도록 한다.카드가 전달 받아야 한다. 둘중하나는 없어도 됨..
        isSwitch = true;
        GameData.Instance.isSwitch = true;
        IList<UnityCard> chs;

        //자기 자신의 버튼탭에 있는 UnityCard 이부분 tab에서 받아 오면 될듯
        chs = GameData.Instance.panelSlots;


        for (int i = 0; i < chs.Count; i++)
        {
            if (chs[i].ID == GameData.Instance.curSwitchCard)
            {
                //스위치 되기 위해서는 같은 계층에 있어야 한다.
                // UIPanel.cs -UnityCards
                MoveSelect = Instantiate(cardObj);

                MoveSelect.transform.SetParent(switchMove, false);
                //부모에 따라서 상대위치가 달라지므로 다시 스케일 세팅해야됨
                MoveSelect.transform.localScale = new Vector3(1, 1, 1);

                // 임시로 좌표를  중심의 밑으로 설정한다.
                MoveSelect.transform.localPosition = new Vector3(0, 0, 0);
                MoveSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPos);

                //카드의 세팅
                string cardName = GameData.Instance.UnityDatas[GameData.Instance.curSwitchCard].Name;
                int elixir      = GameData.Instance.UnityDatas[GameData.Instance.curSwitchCard].Elixir;
                MoveSelect.GetComponentInChildren<VictoryCard>().iconName.text = cardName;
                MoveSelect.GetComponentInChildren<VictoryCard>().mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", cardName);
               
            
            }
        }
    }

    public void SwitchSelectMove()
    {
        Vector3 poss = GameData.Instance.toTopPos;
        //world좌표축
     
        if (MoveSelect != null && isSwitch)
        {
            iTween.MoveTo(MoveSelect, iTween.Hash( "islocal",false ,
                                                   "position", poss,
                                                   "oncomplete", "ReachSampleDestroy",
                                                   "oncompletetarget", gameObject,
                                                   "time", 0.3f,
                                                   "easetype", "easeOutQuart")
                                                 );
        }

    }

    public void ReachSampleDestroy()
    {
        //이동이 완료후에는 false로 해줌
        isSwitch = false;
        GameData.Instance.isSwitch = false;

        Destroy(MoveSelect.gameObject);
    }

    public void SwitchMoveStart()
    {
        if (isSwitch)
        {
            //ID를 보낸것을 다시 받아와야 해서 코루틴 사용
            StartCoroutine(coSwitchMoveStart());

        }

    }

    public void HideEnable()
    {   //curSwitchCard
        int ide = GameData.Instance.curSwitchCard;
        if (tempCards2.Count > 0)
        {
            for (int i = 0; i < tempCards2.Count; i++)
            {
                int a = tempCards2[i].GetComponentInChildren<UnityCard>().ID;

                tempCards2[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(false);
                if (ide == a)
                {
                    tempCards2[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(true);
                    GameData.Instance.ShowID   = a;
                    GameData.Instance.isShowID = true;

                }
            }
        }
        else
        {
            for (int i = 0; i < lsSwitchCards.Count; i++)
            {
                int a = lsSwitchCards[i].GetComponentInChildren<UnityCard>().ID;
                lsSwitchCards[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(false);
                if (ide == a)
                {
                    lsSwitchCards[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(true);
                    GameData.Instance.ShowID = a;
                    GameData.Instance.isShowID = true;
                }
            }
        }
    }


    public void AllHideButton()
    {

    }

    IEnumerator coSwitchMoveStart()
    {
        yield return new WaitForSeconds(0.1f);
        //tab의 경로
        int tab = GameData.Instance.CurTab;
        int ID = GameData.Instance.fromSwitchCard;
        GameObject MoveSelect = Instantiate(cardObj);
        MoveSelect.transform.SetParent(switchMove, false);
        //부모에 따라서 상대위치가 달라지므로 다시 스케일 세팅해야됨
        MoveSelect.transform.localScale = new Vector3(1, 1, 1);

        // 임시로 좌표를  중심의 밑으로 설정한다.
        MoveSelect.transform.localPosition = new Vector3(0, 0, 0);
        //선택된 카드의 좌표를 가져온다.
        MoveSelect.GetComponent<RectTransform>().anchoredPosition = new Vector2(GameData.Instance.fromSwitchPos.x, GameData.Instance.fromSwitchPos.y);
        //카드의 세팅
       
        int elixir = GameData.Instance.UnityDatas[ID].Elixir;

        //카드이름 이미지가 적용되지 않음 원인모름--ERROR-------------------
        //string cardName = GameData.Instance.UnityDatas[ID].Name;
        //MoveSelect.GetComponent<UnityCard>().mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", cardName);
        //-----------------------------------------------------------------
        //엘릭서 
        MoveSelect.GetComponent<UnityCard>().Elixer    = GameData.Instance.UnityDatas[ID].Elixir;
        //enum 으로 리스트를 위한 number을 얻는다.
        MoveSelect.GetComponent<UnityCard>().Kinds     = GameData.Instance.UnityDatas[ID].Kinds;
        MoveSelect.GetComponent<UnityCard>().ID        = GameData.Instance.UnityDatas[ID].Id;
        //TODO: isSwtich의 상태를 언제 바꿀지 결정한다. 
        MoveSelect.GetComponent<UnityCard>().Icon_name = GameData.Instance.UnityDatas[ID].Name;

    }

    private void OnMouseUpAsButton()
    {

    }


    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            isRed = !isRed;
            if (isRed)
            {
                print("OnMouse 감지 한거야?111111111111111111111");
            }
            else
            {

            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("=======OnPointerClick======");
    }
    //Level 단계
    //int[] Level = { 0,2, 4, 10, 20, 50, 100, 200, 400, 1000, 2000, 4000, 5000 };

    public int CardLevel(int cardNum)
    {
        int n = 0;
        for (int i = 0; i < GameData.Instance.Level.Length; i++)
        {
            if (cardNum - GameData.Instance.Level[i] >= 0)
            {
                continue;
            }
            else
            {
                n = i + 1;
                break;
            }
        }
        return n;
    }

    void UnFindCardSet()
    {

        IList<int> hasId = new List<int>();
        IList<int> datasId = new List<int>();

        //가지고 있는 카드의 리스트 
        foreach (KeyValuePair<int, Card> pair in GameData.Instance.hasCard)
        {
            hasId.Add(pair.Key); //ID ,
        }

        //데이터 에 있는 모든 카드의 인덱스 
        for (int i = 0; i < GameData.Instance.infoCards; i++)
        {
            datasId.Add(GameData.Instance.UnityDatas[i].Id);
        }

        for (int i = 0; i < GameData.Instance.infoCards; i++)
        {
            for (int j = 0; j < hasId.Count; j++)
            {
                if (datasId[i] == hasId[j])
                {
                    datasId.Remove(i);
                    datasId.Insert(i, 0);
                    break;
                }
                else
                {
                    continue;
                }
            }
        }

        foreach (var i in datasId)
        {
            if (i > 0)
            {   //가지고 있지 않은 카드 
                GameObject UnFindCard = Instantiate(cardObj);
                UnFindCard.transform.SetParent(unfindCards, false);
                UnFindCard.transform.localScale = new Vector3(1, 1, 1);
                UnFindCard.transform.localPosition = new Vector3(0, 0, 0);
             
                string cardName = GameData.Instance.UnityDatas[i].Name;
              
                UnFindCard.GetComponent<VictoryCard>().mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", cardName);
                UnFindCard.GetComponent<VictoryCard>().iconName.text = GameData.Instance.UnityDatas[i].Name;
            }
        }
    }

    //UI bottom메뉴에서 선택되지 않은 버튼의 화살표를 숨긴다.
    public void TurnOffPanelItem()
    {

        for (int i = 0; i < panelItems.Length; i++)
        {
            //PanelItem 1 ~4 
            if (i == (GameData.Instance.PanelItem - 1))
            {
                panelItems[i].GetComponentInChildren<UI_Icon>().SetActivateShowA();
            }
            else
            {
                panelItems[i].GetComponentInChildren<UI_Icon>().SetActivateHideA();
            }
        }

    }

    //상품 선물캔버스가 동작하게 한다.
    public void SelectCaseItem()
    {
        CaseItem[0].SetActive(true);
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //SetReFormCopy(List<int> arr);
        }
    }

    
}
