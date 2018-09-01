using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public enum eGameState
{
    NONE,
    STARTMOTION,
    START,
   
    CARDDISTRIBUTION,         //com카드배분
    COM_CARDMOVE,             //com의카드가 이동후 슬롯에 모두 있는지 판단한다.
    PLAYER_CARDDISTRIBUTION,  //Player가 카드를 인덱스에 배치한다.

    PLAYER_CARDCHOICE,        //player가 카드에 가위바위보 설정
    COM_DISTRIBUTION,         //승패이후 컴이 가위 바위 보를 다시 냄
    JUDGEMENT,                //가위바위보 판별
    JUDGEMENT_NEXT,           //판정후  다음단계판정
    JUDGEMENT_ANI,            //판정애니가 실행되게 한다.
    JUDGEMENT_RESULT,         //ANI 이후에 중앙 에서의 화면처리
    VICTORY_OR_DEFEAT,        //승리출력
    GAMERETREAT,              //게임을 다시 시작할지 결정 또는 포획상품결졍
    CARDLOCATION,
}

public enum ePlayer
{
  NONE,
  PLAYER,
  COMPUTER,
}

  struct  StCard
  {
    Card card;
    int  RSP;   //가위,바위,보
  }



public partial class GameManager : MonoBehaviour {

    public  GameObject  startLabel;                    //시작시 시작을 알려주는 Start라벨
    public  Text        eText;                         //현재 state상태를 알기 위해서 설정
    public  Transform   buttonTurn;
    public  Transform   playerCombatPos;               //결전시 화면의 중앙으로 이동될 좌표
    //player----------------------------------
    public  Transform  playerForm;                     //카드슬롯에 카드를 배치하고 컨트롤할수 있게 위치에 연결할수 있게한다.
    public  CenterFormation centerformation;
    public  Transform  kingTower;                      //player킹타워 
    public  Transform  comCombatPos;
    IList<Card> SpriteSlot    = new List<Card>();
    IList<Card> SpriteSlotCom = new List<Card>();
    //IList<string> SpriteName = new List<String>();

    //최종살아남은 카드와 그카드가 해치운 카드의 정보를 담는다.
    
   

    //computer---------------------------------
    public Transform playerForm2;
    public CenterFormation centerformation2;
    public Transform kingTower2;
    public Transform combatShow;                      //승리 패배의 결과를 보여준다.
    public bool ______________________________________________________________________;

    Formation        formation;                       //player의 base 카드세트 
    Formation        formation2;                      //computer의 base 카드세트
    static GameCardSlot[]   centerSlots;              //중앙에 놓여질 카드 슬롯 위치
    static GameCardSlot2[]  centerSlots2;             //computer의 센터카드 
    GameCardSlot[]   cardSlots;                       //Player의 카드 세팅
    GameCardSlot[]   cardSlots2;                      //computer의 베이스세팅카드
    KingTower        kingT;                           //킹타워의 스크립트 
    KingTower        kingT2;
    CombatShow       combatShowC;
    


    Text     labelText;                               //특정 글자 컨트롤 시자과 ending 때 사용할 문자 
    Button   btn;
  
    Color    imgbtnColor;
    Image    btnImg;

    public  eGameState    gameState = eGameState.STARTMOTION;
            eGameCardSize eCardSize = eGameCardSize.NON;

    int isCardNum = 0;
   

    int[] cards;                                     //현재 데이터에서 받아온 가지고있는 전체카드종류

    int[] rock_paper_scissors       = new int[3];    //컴퓨터의 설정된 가위바위보
    int[] rock_paper_scissorsPlayer = new int[3];    //플레이어의 설정된 가위바위보
    int[] vicResult                 = new int[3];    //승리한 결과 value 

    
                                                      
    public delegate void  Rock_Paper_Scissors(int[] player,int[] com); //델리게이트로 가위,바위로보냄
    public static   event Rock_Paper_Scissors sendEventRPS; //슬롯의 가위바위보를 전달한다.
  
    public delegate void  deg_Victory(int[] index);
    public static   event deg_Victory evnt_Victory;  //슬롯의 이긴결과를 보낸다.


    bool tem = false; //test bool ~~
    bool NotSlot = false;                            //빈 슬롯이 있는지 판단
    bool particlesPlaying;

    int  stageVictory = 0;                           // 1패배  2무승부 3승리
    int  MaxSlotCount = 9;                           //기본최대 슬롯의 갯수는 9개이다.
    int tempIntCoutinueCount = 0;
    private void Awake()
    {
        formation    = playerForm.GetComponent<Formation>();
        cardSlots    = playerForm.GetComponentsInChildren<GameCardSlot>();
       
        centerSlots  = centerformation.GetComponentsInChildren<GameCardSlot>();
        kingT        = kingTower.GetComponent<KingTower>();

        formation2   = playerForm2.GetComponent<Formation>();
        centerSlots2 = centerformation2.GetComponentsInChildren<GameCardSlot2>();
        kingT2       = kingTower2.GetComponent<KingTower>();

        combatShowC  = combatShow.GetComponent<CombatShow>();

        labelText    = startLabel.GetComponentInChildren<Text>();
        btn          = buttonTurn.GetComponent<Button>();
        btnImg       = buttonTurn.GetComponent<Image>();
        imgbtnColor  = btnImg.color;


      GameData.Instance.prefabs  = Resources.LoadAll("RoadPrefabs", typeof(GameObject)).Cast<GameObject>().ToArray();
    }



    private void Start()
    {
        particlesPlaying = false;
        cardSlots2 = formation2.cardsLength;
        // = cardSlots2 = playerForm2.GetComponentsInChildren<GameCardSlot>();
        cards = new int[GameData.Instance.hasCard.Count];
        int num = 0;
        foreach (KeyValuePair<int, Card> key in GameData.Instance.hasCard)
        {
            //순서대로 카드의 숫자를 배출하기 위해서 
            cards[num] = key.Key;
            num++;
        }
        StartCoroutine(AutoStep());
        //color를 설정
        imgbtnColor = Color.black;
        //color을 컴포넌트에 적용
        btnImg.color = imgbtnColor;

        //게임메니저가 실행될때마다 새로동적생성을 위해서 
        GameData.Instance.playerVic = new Dictionary<int, IList<Card>>();
        GameData.Instance.comVic = new Dictionary<int, IList<Card>>();

        OverlayParticles.ShowParticles(10, gameObject.transform.position);


      
    }


    //Formati0n에서 정렬되면서 저장시킨 cardSlot의 정보를 가져온다
    void ReCountCardSlots2()
    {
        cardSlots2 = formation2.CardReNumCom();
    }

    void StartLabelEff(GameObject obj)
    {
        iTween.Stop(obj);
        Vector3 cur = obj.transform.position; //540,1033
        
        //이렇게 하면  540,1033 ~> 0  현재 좌표로 이동한다.
        iTween.MoveFrom(obj, iTween.Hash("position",cur,"time", 1, "islocal", true,  "oncomplete", "LabelEffEndEff", "oncompletetarget", this.gameObject, "easetype", iTween.EaseType.easeInOutQuint));
    }

    //TODO:효과 시작시 시작이 끝나고 효과를 주기 위한메소드 사운드 는 안됨.
    void LabelEffEndEff()
    {
       
    }

    void EndingLavelEff(GameObject obj)
    {
        labelText.text = string.Format("{0}", "GAME OVER");
    }

    void StartLabelEffClose(GameObject obj)
    {
        iTween.Stop(obj);
        Vector3 cur = obj.transform.localScale;
       
        //시작후 다음 단계로 넘어갈때 글자가 없어지게 한다.
        iTween.ScaleFrom(obj, iTween.Hash("x",cur.x + 0.3f, "y", cur.y + 0.3f, "oncomplete", "ImgClose", "oncompletetarget", this.gameObject, "time", 0.3f));
    }

    void ImgClose()
    {
        startLabel.SetActive(false);
    }


       //computer의 슬롯의 이미지를 변경하는데..
    IEnumerator ImgSet()
    {

        //현재 가지고 있는 카드의 Id 로 스프라이트 네임을 얻어온다.
         IList<Card> SpriteNumber = new List<Card>();
         IList<string> SpriteName = new List<String>();

        
        //현재 가지고 있는 card로 리스트에 담는다.
         foreach(KeyValuePair<int,Card> i in GameData.Instance.hasCard)
         {
            SpriteNumber.Add(i.Value); //id만 받아서 
         }
        yield return new WaitForSeconds(0.01f);

        //ID 를 통해서 Name을 얻는다. Name이 곧 Sprite 네임이다.
        for (int i = 0; i < SpriteNumber.Count; i++)
         {
            string name = GameData.Instance.UnityDatas[SpriteNumber[i].ID].Name;
            string spA  = GameData.Instance.UnityDatas[SpriteNumber[i].ID].SpAble;
            SpriteName.Add(name);
         }

        yield return new WaitForSeconds(0.01f);
        //cardSlots2[0].itemIcon.sprite = SpriteManager.GetSpriteByName("Textures", name);

        for (int i = 0; i < SpriteNumber.Count; i++)
        {
            cardSlots2[i].itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", SpriteName[i]);
        }
       
    }
           //GameData에서 현재 플레이어가 가지고 있는 카드정보를 리스트에 담는다.
           //이때 UnitData와 연동시켜서 Card 를 완성시켜서 저장한다.
    void PlayerCardInfoData()
    {
        SpriteSlot.Clear();

        foreach (KeyValuePair<int, Card> i in GameData.Instance.hasCard)
        {   //id,hasNum,hasLevel 이후것을 추가해 주어야 한다.
            // Name;  Shap; //카드이름IconName;//카드이미지Spable; //현재 카드가 가지고 있는 특수한 능력
            i.Value.IconName = GameData.Instance.UnityDatas[i.Key].Name;
            i.Value.Name     = GameData.Instance.UnityDatas[i.Key].Name;
            i.Value.Spable   = GameData.Instance.UnityDatas[i.Key].SpAble;  //특수 능력
            i.Value.atk_type = GameData.Instance.UnityDatas[i.Key].Atk_Type;//공격타입
            i.Value.Attack   = GameData.Instance.UnityDatas[i.Key].Attack;  //성안 군사 공격
            i.Value.AtK_zone   = GameData.Instance.UnityDatas[i.Key].Atk_Zone;//성곽공격

            SpriteSlot.Add(i.Value); //id만 받아서 
           
        }
    }

    void ComCardInfoData()
    {
        SpriteSlotCom.Clear();

        foreach (KeyValuePair<int, Card> i in GameData.Instance.hasCard)
        {   //id,hasNum,hasLevel 이후것을 추가해 주어야 한다.
            // Name;  Shap; //카드이름IconName;//카드이미지Spable; //현재 카드가 가지고 있는 특수한 능력
            i.Value.IconName = GameData.Instance.UnityDatas[i.Key].Name;
            i.Value.Name     = GameData.Instance.UnityDatas[i.Key].Name;
            i.Value.Spable   = GameData.Instance.UnityDatas[i.Key].SpAble;
            i.Value.atk_type = GameData.Instance.UnityDatas[i.Key].Atk_Type;
            i.Value.Attack   = GameData.Instance.UnityDatas[i.Key].Attack;  //성안 군사 공격
            i.Value.AtK_zone   = GameData.Instance.UnityDatas[i.Key].Atk_Zone;//성곽공격
            SpriteSlotCom.Add(i.Value); //id만 받아서 

        }
    }

    //Player의 카드를 슬롯에 적용
    void InputCard(int num)
    {
         cardSlots[num].SetCardInfo(SpriteSlot[num],eCardType.SLOT);
         string  name = SpriteSlot[num].IconName;
         cardSlots[num].itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", name);
    }

    //****TODO: 정확한 배치카드 세팅이 결정되면 코드 수정할것:2018
    void InputCardP2(int num)
    {
        cardSlots2[num].SetCardInfo(SpriteSlotCom[num], eCardType.SLOT);
        string name = SpriteSlotCom[num].IconName;
        cardSlots2[num].itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", name);

    }

    //컴퓨터가 자동으로 랜덤한 숫자(가위,바위,보)를 할당한다.
    void InputRPS()
    {
        //새로 컴퓨터가 세팅이 되어야 하므로 슬롯이 작동하게 한다.
        //TODO: 모든 판정이 끝난후 애니메이션 이펙트와 함께 필요함
        NotSlot = false;

        int one =  UnityEngine.Random.Range(1, 3);
                     //비교하기 위해서 인덱스 입력한다.
                     rock_paper_scissors[0] = one; 
                     //센터에 표시하기위해서 프로퍼티로 접근한다.
                     centerSlots2[0].RpkNumber = one; 

         int two = UnityEngine.Random.Range(1, 3);
                     rock_paper_scissors[1] = two;
                     centerSlots2[1].RpkNumber = two;

         int third = UnityEngine.Random.Range(1, 3);
                     rock_paper_scissors[2] = third;
                     centerSlots2[2].RpkNumber = third;
    }

   
    //Computer의 센터에 배정된 카드가 있다면 숫자를 돌려준다.
    int ComRemainCenterCard()
    {
        int remain = 0;
        for (int i = 0; i < centerSlots2.Length; i++)
        {
            if (centerSlots2[i].GetComponentInChildren<GameCardSlot>())
                remain++;
           
        }
            return remain;
    }

    int RemainCenterCard()
    {
        int remain = 0;
        for (int i = 0; i < centerSlots2.Length; i++)
        {
            if (centerSlots[i].GetComponentInChildren<GameCardSlot>().cardInfo != null)
                remain++;

        }
        return remain;
    }


    //player가 설정한 주먹가위보를 얻어온다.
    void PlayerRPS()
    {
        rock_paper_scissorsPlayer[0] = centerSlots[0].RpkNumber;
        rock_paper_scissorsPlayer[1] = centerSlots[1].RpkNumber;
        rock_paper_scissorsPlayer[2] = centerSlots[2].RpkNumber;
    }

    //델리게이트로  CombatShow에 player,com의 가위,바위,보를 전달하게된다.
    void SendRPS(int[] player,int[] com)
    {
        if (sendEventRPS != null)
        {
            sendEventRPS(player, com); //player int[] ,com int[]의 데이터를 보낸다.
        }
    }

    //결과를 보낸다.배열로 보낸다.
    void SendRPSResult()
    {
        if (evnt_Victory != null)
        {
            vicResult[0] = onlyVictoryResult(0);
            vicResult[1] = onlyVictoryResult(1);
            vicResult[2] = onlyVictoryResult(2);

            evnt_Victory(vicResult);
        }
    }

    //void Receive
    //player와 컴퓨터의 결과값을 비교하여 승패를 출력한다. 슬롯의 인덱스에 접근할수있는 루트
    //index -->>슬롯위치 0,1,2
    int Victory_pnt(int index)
    {
        #region RPK설명

        //임시로 한꺼번에 출력한다. 그러나 하나씩 이펙트로 순차비교해야함
        //1주먹,2 가위 ,3 보
        //rock_paper_scissorsPlayer[0]  rock_paper_scissors[0]
        //rock_paper_scissorsPlayer[1]  rock_paper_scissors[1]
        //rock_paper_scissorsPlayer[2]  rock_paper_scissors[2]
        #endregion
        int result = 1;
        //player 센터슬롯 0 에서 가위1 or 바위2 or 보3 를 냈을때
        //come이 센터슬롯 0 에서  1,2,3   1,2,3   1,2,3을 냈을때의 비교

        if (rock_paper_scissorsPlayer[index] == 1)  //--------------1--------------------------------------
        {
                    if (rock_paper_scissors[index] == 1)
                    {
                        Debug.Log("비겼습니다.");
                        result = 2;
                    }
                    else if (rock_paper_scissors[index] == 2)
                    {
                        Debug.Log("승리했습니다...");
                        result = 3;
                       
                            if (centerSlots2[index].GetComponentInChildren<GameCardSlot>() != null)
                            {
                            //centerSlots2[index].GetComponentInChildren<GameCardSlot>().SetCardInfo(null);
                            //centerSlots2[index].GetComponentInChildren<GameCardSlot>().transform.SetParent(formation2.transform);

                            //연기이펙트 생성
                            EffCreate("EF_Smoke", centerSlots2[index].transform);
                            AppSound.instance.SE_EXPLOSION.Play();
                            Destroy(centerSlots2[index].GetComponentInChildren<GameCardSlot>().gameObject);
                             }
                   
                           //카드의 특성에 따라 카드 추가 하기 
                            Victory_AddCard(index,ePlayer.PLAYER);
                           
                    }
                    else if (rock_paper_scissors[index] == 3)
                    {
                        Debug.Log("패배했습니다.");

                            if (centerSlots[index].GetComponentInChildren<GameCardSlot>() != null)
                            {
                           
                            EffCreate("EF_Smoke", centerSlots[index].transform);
                            AppSound.instance.SE_EXPLOSION.Play();
                            centerSlots[index].GetComponentInChildren<GameCardSlot>().SetCardInfo(null, eCardType.CENTERSLOT);
                            }
                
                           Victory_AddCard(index, ePlayer.COMPUTER);
                           //ShowAddCardCom();
                           result = 1;
                    }
        }
        else if (rock_paper_scissorsPlayer[index] == 2) //------------2--------------------------------------
        {
                    if (rock_paper_scissors[index] == 1)
                    {
                            Debug.Log("패배했습니다..");
                            if (centerSlots[index].GetComponentInChildren<GameCardSlot>() != null)
                            {

                                centerSlots[index].GetComponentInChildren<GameCardSlot>().SetCardInfo(null,eCardType.CENTERSLOT);
                            }
                           
                            Victory_AddCard(index, ePlayer.COMPUTER);
                           // ShowAddCardCom();
                            result = 1;
                    }
                    else if (rock_paper_scissors[index] == 2)
                    {
                        Debug.Log("비겼습니다.");
                        result = 2;
                    }
                    else if (rock_paper_scissors[index] == 3)
                    {
                        Debug.Log("승리했습니다..");
                        result = 3;
                            //적카드 제거
                            if (centerSlots2[index].GetComponentInChildren<GameCardSlot>() != null)
                            {
                            //centerSlots2[index].GetComponentInChildren<GameCardSlot>().SetCardInfo(null);
                            //centerSlots2[index].GetComponentInChildren<GameCardSlot>().transform.SetParent(formation2.transform);
                            EffCreate("EF_Smoke", centerSlots2[index].transform);
                            AppSound.instance.SE_EXPLOSION.Play();
                            Destroy(centerSlots2[index].GetComponentInChildren<GameCardSlot>().gameObject);
                            }
                         //카드의 특성에 따라 카드 추가 하기 
                          Victory_AddCard(index, ePlayer.PLAYER);
                    }
        }
        else if (rock_paper_scissorsPlayer[index] == 3)  //------------3----------------------------------------
        {
                    if (rock_paper_scissors[index] == 1)
                    {
                        Debug.Log("승리했습니다.");
                        result = 3;
                            //적카드 제거
                            if (centerSlots2[index].GetComponentInChildren<GameCardSlot>() != null)
                            {

                            EffCreate("EF_Smoke", centerSlots2[index].transform);
                            AppSound.instance.SE_EXPLOSION.Play();
                            Destroy(centerSlots2[index].GetComponentInChildren<GameCardSlot>().gameObject);
                            }

                          //카드의 특성에 따라 카드 추가 하기 
                          Victory_AddCard(index, ePlayer.PLAYER);
                          
                     }
                    else if (rock_paper_scissors[index] == 2)
                    {
                            Debug.Log("패배했습니다.");
                               //player  center의Card정보를 삭제한다. 
                            if (centerSlots[index].GetComponentInChildren<GameCardSlot>() != null)
                             {
                                EffCreate("EF_Smoke", centerSlots[index].transform);
                                AppSound.instance.SE_EXPLOSION.Play();
                                centerSlots[index].GetComponentInChildren<GameCardSlot>().SetCardInfo(null,eCardType.CENTERSLOT);
                             }
                                
                    
                            Victory_AddCard(index, ePlayer.COMPUTER);
                            //ShowAddCardCom();
                            result = 1;
                    }
                    else if (rock_paper_scissors[index] == 3)
                    {
                        Debug.Log("비겼습니다.");
                        result = 2;
                    }
        }
        return result;
    }

    //player의 뱅크가 비어있을 경우
     int Victory_pntEMPT(int index)
    {
       int result = 1;
        //각각의 slot가 비었는지 확인한다. -1이면비었는상태임
       int emptIndexPlayer  = ReturnBlankSlotPlayer(index);
       int emptIndexCom     = ReturnBlankSlot(index);


        if (emptIndexPlayer > 0 && emptIndexCom < 0)
        {
            //player가 비고 컴은 슬롯에 채워져 있을때 패배
            Debug.Log("패배했습니다.");
            //player  center의Card정보를 삭제한다. 
            if (centerSlots[index].GetComponentInChildren<GameCardSlot>() != null)
            {
                centerSlots[index].GetComponentInChildren<GameCardSlot>().SetCardInfo(null,eCardType.CENTERSLOT);
            }

            Victory_AddCard(index, ePlayer.COMPUTER);
           
            result = 1;
        }
        else if (emptIndexPlayer < 0 && emptIndexCom > 0)
        {
            //player가 있고 컴이 비었을때 승리
            Debug.Log("승리했습니다...");
            if (centerSlots2[index].GetComponentInChildren<GameCardSlot>() != null)
            {
                //centerSlots2[index].GetComponentInChildren<GameCardSlot>().SetCardInfo(null);
                //centerSlots2[index].GetComponentInChildren<GameCardSlot>().transform.SetParent(formation2.transform);
                Destroy(centerSlots2[index].GetComponentInChildren<GameCardSlot>().gameObject);
            }

            //카드의 특성에 따라 카드 추가 하기 
            Victory_AddCard(index, ePlayer.PLAYER);
            result = 3;
        }
        else if (emptIndexPlayer > 0 && emptIndexCom > 0)
        {
            //둘다 비었을때
            Debug.Log("비겼습니다.");
            result = 2;
        }
        else if (emptIndexPlayer < 0 && emptIndexCom < 0)
        {
            //둘다 슬롯이 채워져 있을때
            result = Victory_pnt(index);
        }
        return result;
    }

    //Resources 에서 이팩트를 찾아서 생성한다.
    GameObject EffCreate(string name,Transform parent)
    {
   
        GameObject newObj = null;
        // prefabs"EF_Smoke"
        for (int i = 0; i < GameData.Instance.prefabs.Count; i++)
        {
            if (GameData.Instance.prefabs[i].name == name)
            {
                 newObj = Instantiate(GameData.Instance.prefabs[i]);
                 newObj.transform.SetParent(parent);
                 newObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                 break;
            }
        }
        return newObj;
    }

    //승리시 해당 슬롯의 카드를 읽어서추가 한다.
    //센터 슬롯에 카드가 모두 있을때
    void Victory_AddCard(int index , ePlayer player)
    {
        string id           = null;
        GameObject copySlot = null;
        Transform  trans    = null;
      

        switch (player)
        {
            case ePlayer.PLAYER:
              
                if (centerSlots[index].cardInfo != null)
                {
                          id       = centerSlots[index].cardInfo.Spable;
                          copySlot = playerForm.GetComponent<Formation>().card;
                          trans    = playerForm.transform;

                          //com의 디마지값을 설정해주어야 한다. 시간오차 발생할수 있는 부분
                          
                          int attack = centerSlots[index].cardInfo.Attack;
                          int atk_zone = centerSlots[index].cardInfo.AtK_zone;
                          kingT2.Damage = atk_zone*-1;
                          kingT2.DamageMan = attack*-1;
                            //승리카드
                          Card cardcur = centerSlots[index].cardInfo;
                          Card defCard = null;
                           //패배한 카드의 슬롯이 비었을때
                           if (centerSlots2[index].GetComponentInChildren<GameCardSlot>() != null)
                             { 
                              defCard = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo;
                             }
                            

                    if (GameData.Instance.playerVic.Count == 0)
                    {
                        IList<Card> vicCar = new List<Card>();
                        GameData.Instance.playerVic.Add(cardcur.ID, vicCar);
                        GameData.Instance.playerVic[cardcur.ID].Add(defCard);
                    }
                    else if (GameData.Instance.playerVic.Count > 0)
                    {
                        for (int i = GameData.Instance.playerVic.Count - 1; i >= 0; i--)
                        {   //승리 카드가 있는지 확인하고 리스트를 만들어등록한다.
                            if (!GameData.Instance.playerVic.ContainsKey(cardcur.ID))
                            {
                                IList<Card> vicCar = new List<Card>();
                                GameData.Instance.playerVic.Add(cardcur.ID, vicCar);
                                GameData.Instance.playerVic[cardcur.ID].Add(defCard);
                            }
                            else
                            {
                                //만약 같은 카드가 있다면 패배한 카드를 리스트에 추가하다.
                                //DEGUG:패배한 카드가 확실한지 확인필요함..헷갈림
                                GameData.Instance.playerVic[cardcur.ID].Add(defCard);
                            }
                        }
                    } //vicList에 추가 END
                    
                    if (id == "AddCard")
                    {
                        int MaxCount = 1;

                        for (int i = 0; i < cardSlots.Length; i++)
                        {
                            if (cardSlots[i].cardInfo != null)
                            {
                                MaxCount++;
                                continue;
                            }
                            else
                            {
                                Card newCard = new Card();
                                //TODO:여기에 
                                //ID;level;hasCard; Name; Shap;IconName;Spable;
                                newCard.IconName = "Miner";
                                cardSlots[i].SetCardInfo(newCard,eCardType.SLOT);
                                formation.FormationCardsArc(eGameCardSize.BASE,eBelong.PLAYER);
                                break;
                            }
                        }
                        //모든 슬롯이 다 채워져 있을경우 
                        // maxCount 는 최대카운트10   > 전체 기본갯수9
                        if (MaxCount > MaxSlotCount)
                        {
                            //복제대상playerForm.GetComponent<Formation>().card
                            GameObject newCardSlot = Instantiate(copySlot, trans);
                            Card newCard = new Card();
                            //TODO:여기에 
                            //ID;level;hasCard; Name; Shap;IconName;Spable;
                            newCard.IconName = "Miner";

                            //Error  발생
                            newCardSlot.GetComponent<GameCardSlot>().SetCardInfo(newCard,eCardType.SLOT);
                            formation.FormationCardsArc(eGameCardSize.BASE,eBelong.PLAYER);

                            StartCoroutine(CoShowShowAddCardPlayer(0.3f));
                        }
                    } //END

                    else if (id == "RemoveCard")
                    {
                        //컴퓨터의 base 카드중 하나를 랜덤하게 제거한다.
                        if (playerForm2.childCount > 0)
                        {   //인덱스 로 찾아야 하므로 0 ~시작
                            int choice = UnityEngine.Random.Range(0, playerForm2.childCount - 1);
                            Destroy(cardSlots2[choice].gameObject);
                        }
                    }
                }
                
                break;
            case ePlayer.COMPUTER:

                if (centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo != null)
                {
                        id = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo.Spable;
                        copySlot = playerForm2.GetComponent<Formation>().card;
                        trans = playerForm2.transform;
                        //TODO:playerTower의 피해량 설정
                        int attack = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo.Attack;
                        int atk_zone = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo.AtK_zone;
                        kingT.Damage = atk_zone*-1;
                        kingT.DamageMan = attack*-1;
                    //승리카드
                    Card cardcur = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo;
                    //패배카드
                    Card defCard = null;

                    if(centerSlots[index].cardInfo != null)
                        defCard = centerSlots[index].cardInfo;


                    if (GameData.Instance.comVic.Count == 0)
                    {
                        IList<Card> vicCar = new List<Card>();
                        GameData.Instance.comVic.Add(cardcur.ID, vicCar);
                        GameData.Instance.comVic[cardcur.ID].Add(defCard);
                    }
                    else if (GameData.Instance.comVic.Count > 0)
                    {
                        for (int i = GameData.Instance.comVic.Count - 1; i >= 0; i--)
                        {   //승리 카드가 있는지 확인하고 리스트를 만들어등록한다.
                            if (!GameData.Instance.comVic.ContainsKey(cardcur.ID))
                            {
                                IList<Card> vicCar = new List<Card>();
                                GameData.Instance.comVic.Add(cardcur.ID, vicCar);
                                GameData.Instance.comVic[cardcur.ID].Add(defCard);
                            }
                            else
                            {
                                //만약 같은 카드가 있다면 패배한 카드를 리스트에 추가하다.
                                //DEGUG:패배한 카드가 확실한지 확인필요함..헷갈림
                                GameData.Instance.comVic[cardcur.ID].Add(defCard);
                            }
                        }
                    } //vicList에 추가 END



                    if (id == "AddCard")
                        {
                                //복제대상playerForm.GetComponent<Formation>().card
                                GameObject newCardSlot = Instantiate(copySlot, trans);
                                Card newCard = new Card();
                                newCard.IconName = "Miner";
                                newCardSlot.GetComponent<GameCardSlot>().SetCardInfo(newCard,eCardType.SLOT);

                                formation2.FormationCardsArcCom(eGameCardSize.BASE,eBelong.COM);
                                //이동중에 코드적용을 어디선가 막고있음그래서 코루틴적용
                                StartCoroutine(CoShowAddCardCom(0.1f));
                        }       //END
                        else if (id == "RemoveCard")
                        {
                        //card정보가 있는 것중 앞에것을 제거한다.
                        StartCoroutine(coRemovePlayer(0.2f));
                       
                        }
                }
                break;
        }

        //다른 필드값이 되어야 할것 centerSlots--- centerSlots2,cardSlots[] --->>cardSlots2[]
        //TODO: 승리했을때 승리한 player의 특성을 얻어온다.
    }

    //승패를 처리하는 메소드 ,행동이 없는 결과값
    int onlyVictoryResult(int index)
    {

        int result = 1;
        int emptIndexPlayer = ReturnBlankSlotPlayer(index);
        int emptIndexCom    = ReturnBlankSlot(index);


        if (emptIndexPlayer > 0 && emptIndexCom < 0)
        {
           
            Debug.Log("패배했습니다.");
            result = 1;
        }
        else if (emptIndexPlayer < 0 && emptIndexCom > 0)
        {
            //player가 있고 컴이 비었을때 승리
            Debug.Log("승리했습니다...");
            result = 3;
        }
        else if (emptIndexPlayer > 0 && emptIndexCom > 0)
        {
            //둘다 비었을때
            Debug.Log("비겼습니다.");
            result = 2;

        }
        else if (emptIndexPlayer < 0 && emptIndexCom < 0)
        {
            //둘다 슬롯이 채워져 있을때
            result = onlyVictoryResult1(index);

        }

        return result;
    }
    //양쪽에 카드가 모두존재할때 처리없는 메소드
    int onlyVictoryResult1(int index)
    {
        int result = 1;
        //player 센터슬롯 0 에서 가위1 or 바위2 or 보3 를 냈을때
        //come이 센터슬롯 0 에서  1,2,3   1,2,3   1,2,3을 냈을때의 비교

        if (rock_paper_scissorsPlayer[index] == 1) 
        {
            if (rock_paper_scissors[index] == 1)
            {
                Debug.Log("비겼습니다.");
                result = 2;
            }
            else if (rock_paper_scissors[index] == 2)
            {
                Debug.Log("승리했습니다...");
                result = 3;

            }
            else if (rock_paper_scissors[index] == 3)
            {
                Debug.Log("패배했습니다.");
                result = 1;
            }
        }
        else if (rock_paper_scissorsPlayer[index] == 2) //------------2--------------------------------------
        {
            if (rock_paper_scissors[index] == 1)
            {
                Debug.Log("패배했습니다..");
                result = 1;
            }
            else if (rock_paper_scissors[index] == 2)
            {
                Debug.Log("비겼습니다.");
                result = 2;
            }
            else if (rock_paper_scissors[index] == 3)
            {
                Debug.Log("승리했습니다..");
                result = 3;
            }

        }
        else if (rock_paper_scissorsPlayer[index] == 3)  //------------3----------------------------------------
        {
            if (rock_paper_scissors[index] == 1)
            {
                Debug.Log("승리했습니다.");
                result = 3;
            }
            else if (rock_paper_scissors[index] == 2)
            {
                Debug.Log("패배했습니다.");
                result = 1;
            }
            else if (rock_paper_scissors[index] == 3)
            {
                Debug.Log("비겼습니다.");
                result = 2;
            }
        }
        return result;
    }




    void MoviesEffect(int index)
    {
        centerSlots[index].transform.SetAsLastSibling();
        centerSlots2[index].transform.SetAsLastSibling();

        centerSlots[index].CombatMove(playerCombatPos.position);
        centerSlots2[index].GetComponentInChildren<GameCardSlot>().CombatMove(comCombatPos.position);
        
    }

    //버튼의 가위바위보 선택시 음향
    public void SlotChoiceSound()
    {
        AppSound.instance.SE_SLOT_CHOICE.Play();
    }


    IEnumerator coRemovePlayer(float t)
    {
        IList<int> temp = new List<int>();
        int nullCount = 1;
        if (playerForm.childCount > 0)
        {
            for (int i = 0; i < playerForm.childCount; i++)
            {
                if (cardSlots[i].cardInfo != null)
                {
                    temp.Add(i);
                }
                else
                {
                    nullCount++;
                    if (nullCount >= playerForm.childCount)
                    {
                       yield break;
                    }
                }
            }
       
        yield return new WaitForSeconds(t);

            cardSlots[temp[0]].SetCardInfo(null,eCardType.SLOT);
        }
    }

    //판정결과에 따라 각각을 처리한다.성곽 결과값을 보여준다.
    //------------------------------------------------------
    // 3:플레이어승리        2:무승부      1:패배
    // 컴퓨터감소                         플레이어감소
    // kingT2.texEFF                     kingT.texEFF       
    //-----------------------------------------------------
    void Victory_Result(int result)
    {
        switch (result)
        {
            case 3:
                //player승리 data처리
                //TODO: 승리시 이루어 져야할처리 
                //com의 체력감소
                //카드의 속성에 따라 더큰 데미지를 준다.
                //공격 ,방어 ,도주 , 공격<-->공격 , 공격<--방어,방어<-->방어,후퇴<-->후퇴,
                //행동마나 필요:(선택의 전략성)
                //주둔 -->> 카드의 상태에 따라 공격력증가 감소 상태 체크 한다.
                //장군특성은 어떻게?
                kingT2.TextEFF();

                //승리한 카드에 접근할수 있는가?
                //승리한 카드의 슬롯뱅크숫자는? 
                // 바보  result는 승리판정이지 인덱스가 아님
               

                break;
            case 2:


                break;
            case 1:
                kingT.TextEFF();

                break;
        }
    }

    //컴퓨터의 Base카드에서 랜덤으로 선택되어 센터로 이동하게 된다.
    void MoveChoiceCom()
    {
        int RandomNum = cardSlots2.Length;
        int[] choNum = new int[3];

        for (int i = 0; i < choNum.Length;i++)
        {
            int choice = UnityEngine.Random.Range(0, RandomNum);
            choNum[i] = choice;
            if (choNum[i] != choNum[i+1])
            {
                choNum[i] = choice;
            }
        }
    }

    //컴퓨터의 카드가 랜덤하게 겹치지 않게 추출하기 위해서 셔플한다.
    void ComSuffle()
    {
        // GameCardSlot[] cardSlots2;  Card가 배치된 스롯위치
        for (int i = 0; i < cardSlots2.Length * 10; i++)
        {
            int nA = UnityEngine.Random.Range(0, cardSlots2.Length) % cardSlots2.Length;
            int nB = UnityEngine.Random.Range(0, cardSlots2.Length) % cardSlots2.Length;
            GameCardSlot temp = cardSlots2[nA];
               cardSlots2[nA] = cardSlots2[nB];
               cardSlots2[nB] = temp;
        }
    }

    //호출시 하나씩 위치로 이동시킨다.
    //도착지의 슬롯값을 어떻게 받아 오는가?
    void BatchSlot(GameCardSlot card,int dex)
    {
        if (!NotSlot)
        {
            if (card.gameObject != null)
            {
                    // NotSlot가 false일때 실행된다.
                    Vector3 to = centerSlots2[dex].transform.position;
                    card.gameObject.GetComponentInChildren<GameCardSlot>().MoveSlot(to);
                    card.gameObject.GetComponentInChildren<GameCardSlot>().eBelongState = eBelong.SYSCOM;
                    //이동시킨것의 부모를 변경한다.
                    card.gameObject.transform.SetParent(centerSlots2[dex].transform);
                    //부모가 변경되었을면 gameCardSlot[]를 다시 카운터해서 인덱스를 설정한다.
                    ReCountCardSlots2();
                    //변경된 인덱스와 갯수로 베이스의 거리를 다시 재정렬한다.
                    formation2.FormationCardsArcCom(eGameCardSize.BASE,eBelong.COM);  
            }
        }
       
    }


    //com중앙센터의 비었을때 좌표를 차례로 하나씩 전달한다.
    int ReturnBlankSlot(int index)
    {
        int BankNum = -1;
        #region TypeA
        //for (int i = 0; i < slotCount; i ++)
        //{
        //    if (centerSlots2[i].GetComponentInChildren<GameCardSlot>() == null)
        //    {
        //        BankNum = i;
        //    }
        //    else
        //    {
        //        NotEmpt++;  //1
        //        if (NotEmpt > 2)
        //        {
        //            // BatchSlot(cardSlots2[0],index); 이 작동되지 않게 한다.
        //            //메소드간 커플링 발생함 ㅠ.ㅠ  빈슬롯이 없을때 true발생시킴
        //            NotSlot = true;
        //        }
        //        else
        //        {
        //             NotSlot = false; 
        //        }
        //    }
        // }
        #endregion

       
        if (centerSlots2[index].GetComponentInChildren<GameCardSlot>() == null)
        {
            BankNum = index;
        }
        else
        {
            NotSlot = false;
            BankNum = -1;
        }
           
        return BankNum;
    }

       // 슬롯이 비었다면 0,1,2 리턴하고 비었다면 -1리턴
    int ReturnBlankSlotPlayer(int index)
    {
        int BankNum = -1;

        if (centerSlots[index].GetComponentInChildren<GameCardSlot>().cardInfo == null)
        {
            BankNum = index;
        }
        else
        {
            NotSlot = false;
            BankNum = -1;
        }

        return BankNum;
    }


    //델리게이트 이벤트 형식으로 바꾸기
    public void ComCheckRemainCard()
    {
        int num = formation2.CardFindsNumCom();
        if (num <= 0)
        {
            NotSlot = true;
        }
    }

    //버튼이 눌렸을때 value
   public void Btn_Turn()
    {
      if (gameState == eGameState.PLAYER_CARDDISTRIBUTION)
        {
            //작동가능할때 이미지가 달라져야 한다.
            tem = true;
            Debug.Log("버튼에 의해서 활성화");
        }
       
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("2_Main_Scene");
        //nextScene = "2_Main_Scene";
    }


    //여기서 숫자를 조절할수 있는가?
    IEnumerator CardMove(float ti)
    {
        int count    = 0;
        int MaxCount = 3;
        

        while (true)
        {
            yield return new WaitForSeconds(ti);
            //TODO: 있던없던 무조건3번 호출이 된다.
            //ERROR 만약 cardSlots2[0] 에 아무것도 없는데 호출이 발생해서 생기는 원인?
            // 0,1,2 --- 없음 -1
           
            int index = ReturnBlankSlot(count);
                        count++;

           
            int child  = formation2.transform.childCount;
            if (child <= 0)
                break;
            

            if (child >= 1 && index >= 0)
            {
                BatchSlot(cardSlots2[0], index);
                AppSound.instance.SE_CARD_MOVE.Play();
            }

            if (count >= MaxCount)
                break;
            
        }

    }

    //새로추가된 마지막 카드가 보이도록한다.
    // 문제 발생으로 추가한 부분
   IEnumerator CoShowAddCardCom(float t)
    {
        yield return new WaitForSeconds(t);
        cardSlots2 = playerForm2.GetComponentsInChildren<GameCardSlot>();
        int last = cardSlots2.Length;
        cardSlots2[last - 1].itemIcon.enabled = true;
    }

    IEnumerator CoShowShowAddCardPlayer(float t)
    {
        yield return new WaitForSeconds(t);
        cardSlots = playerForm.GetComponentsInChildren<GameCardSlot>();
        int last = cardSlots.Length;
        cardSlots[last - 1].itemIcon.enabled = true;
    }

    void ResultAniStart(int idex)
    {
        //슬롯에 cardInfo가 모두 있을때의  인덱스를 넘겨준다.
        //centerSlots2[idex].GetComponentInChildren<GameCardSlot>() != null;
       // centerSlots2[idex].GetComponentInChildren<GameCardSlot>().cardInfo != null
        if (centerSlots[idex].cardInfo != null && centerSlots2[idex].GetComponentInChildren<GameCardSlot>() != null)
        {

            Card newSlot0 = centerSlots2[idex].GetComponentInChildren<GameCardSlot>().GetUnityInfo();
            Card newSlot1 = centerSlots[idex].GetUnityInfo();
            //결과를 보여주는 show애니실행시킨다.SetInfo 메소드 안에 auto코루틴이 있어서
            //호출과 동시에 애니가 실행된다;플레이 쇼가 끝난후 결과 처리가 이루어 져야 한다.
            Debug.Log("슬롯 둘다 있습니다..");
            combatShowC.SetInfo(newSlot0, newSlot1, idex);
        }
        else if (centerSlots[idex].cardInfo == null && centerSlots2[idex].GetComponentInChildren<GameCardSlot>() != null)
        {  //player의 슬롯에 카드정보가 없을때
            Card newSlot0 = centerSlots2[idex].GetComponentInChildren<GameCardSlot>().GetUnityInfo();
            //TODO: 정보가 없을때 ...수정해야될 부분
            Card newSlot1 = centerSlots[idex].GetUnityInfo();
            combatShowC.SetInfo(newSlot0, newSlot1, idex);
            Debug.Log(" com 의 슬롯이 비었습니다.");
        }
        else if (centerSlots[idex].cardInfo != null && centerSlots2[idex].GetComponentInChildren<GameCardSlot>() == null)
        {  //com의 슬롯에 카드정보가 없을때
            //TODO:  슬롯에 없을때 newSlot0 :com <--->newSlot1 :player
            Card newSlot0 = null;
            Card newSlot1 = centerSlots[idex].GetUnityInfo();
            combatShowC.SetInfo(newSlot0, newSlot1, idex);
            Debug.Log("player   의 슬롯이 비었습니다.");
        }

    }

    //player와 com의 타워 레벨을 세팅한다.
    //TODO:좀더 정교한 레벨 조정이 필요함 타워가드의 인원수 조정필요
    void LevelSet()
    {
        int playerLevel = GameData.Instance.players[1].exp;
        int playerLevelCount = GameData.Instance.players[1].expCount;


        int LevelMax = 3;
        int max = playerLevel + LevelMax;
        int min = playerLevel + LevelMax * -1;

        int LevelMaxCount = 1000;
        int maxCount = playerLevelCount + LevelMaxCount;
        int minCount = playerLevelCount + LevelMaxCount * -1;

        int Randomlevel = UnityEngine.Random.Range(min, max);
        int RandomCount = UnityEngine.Random.Range(minCount, maxCount);

        kingT.wallNum = playerLevel;
        kingT.manNum = playerLevelCount;
        kingT2.wallNum = Randomlevel;
        kingT2.manNum = RandomCount;
    }

    void KingTowerCheck()
    {
        
         //플레이어의 성과 인력을 모두 판단한다.
        if (kingT.wallNum <= 0 || kingT.manNum <= 0 || kingT2.wallNum <= 0 || kingT2.manNum <= 0)
        {
            //게임을 중지하고 승리판정을 한다.
            gameState = eGameState.VICTORY_OR_DEFEAT;

            //만약  player 와 com이 동시에 승리 조건에 달했을때를 분리해야 한다.
            //무승부조건 출력
            if (kingT.wallNum <= 0 && kingT2.wallNum <= 0)
            {//player가 성벽파워가0이 됐을때 동시에  com의 성벽파워가 0이 됐을때
             //무승부
                stageVictory = 2;
            
            }
            else if (kingT.wallNum <= 0 && kingT2.manNum <= 0)
            {//player가 성벽파워가0이 됐을때 동시에  com의 성벽수비수가 됐을때
                //무승부
                stageVictory = 2;
              
            }
            else if (kingT.manNum <= 0 && kingT2.wallNum <= 0)
            {
                //무승부
                stageVictory = 2;
             
            }
            else if (kingT.manNum <= 0 && kingT2.manNum <= 0)
            {
                //무승부
                stageVictory = 2;
            
            }
            else if (kingT.manNum <= 0 || kingT.wallNum <= 0)
            {//com승리
                stageVictory = 1;
             
            }
            else if (kingT2.manNum <= 0 || kingT2.wallNum <= 0)
            { //player 승리

                stageVictory = 3;
             
            }
            //result Scene이동
            SceneChange();
        }
        
    }


    public void Update()
    {
        //com의 패를 하나씩 낸다.
        if (Input.GetKeyDown(KeyCode.B))
        {
            int index = ReturnBlankSlot(0);
            BatchSlot(cardSlots2[0],index);
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            tem = true;
            Debug.Log("키에 의해서 활성화");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            int  a = formation2.CardFindsNumCom();
            int  bb = centerSlots2.Length;
            int  b = RemainCenterCard();
            int  aa = formation.CardFindsNum();
            int  remaincenter = RemainCenterCard();
            int  count = cardSlots2[0].transform.childCount;
            int  cCount = formation2.transform.childCount;
            if (centerSlots[0].cardInfo == null)
            {
                Debug.Log(" ----centerSlots  null ---");
            }

            Debug.Log(" base com " + a);
            Debug.Log(" center com " + bb);
            Debug.Log(" base END " + aa);
            Debug.Log(" centerEND " + remaincenter);
            Debug.Log(" count" + count);
            Debug.Log(" formation2" + cCount);


        }

    }

  




}
