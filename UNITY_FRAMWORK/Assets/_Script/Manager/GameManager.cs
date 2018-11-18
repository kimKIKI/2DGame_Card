using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;


using System.IO;
using LitJson;

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
    int  RSP;   //주먹1,가위2,보3 //패배 1,무승부2,승리시3
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
    //player의 card데이터값
    IList<Card> SpriteSlot     = new List<Card>();
    IList<Card> SpriteSlotCom  = new List<Card>();
    //IList<string> SpriteName = new List<String>();
    Camera mainCamera;
    
    //컴퓨터의 카드 배분이벤트 전달
    public  delegate void degCardIn();
    public  static  event degCardIn  eveCardIn;
   
    //computer---------------------------------
    public Transform playerForm2;
    public CenterFormation centerformation2;
    public Transform kingTower2;
    public Transform combatShow;                      //승리 패배의 결과를 보여준다.
    public bool ______________________________________________________________________;

    Formation               formation;                //player의 base 카드세트 
    Formation               formation2;               //computer의 base 카드세트
    static GameCardSlot[]   centerSlots;              //중앙에 놓여질 카드 슬롯 위치
    static GameCardSlot2[]  centerSlots2;             //computer의 센터카드 
    GameCardSlot[]          cardSlots;                //Player의 카드 세팅
    GameCardSlot[]          cardSlots2;               //computer의 베이스세팅카드
    KingTower               kingT;                    //킹타워의 스크립트 
    KingTower               kingT2;
    CombatShow              combatShowC;
    GameObject              cardslotSample;           //객체를 복제해서 이동시키기 위해서 
   

    Text     labelText;                               //특정 글자 컨트롤 시자과 ending 때 사용할 문자 
    Button   btn;
  
    Color    imgbtnColor;
    Image    btnImg;

    public  eGameState    gameState = eGameState.STARTMOTION;
            eGameCardSize eCardSize = eGameCardSize.NON;

    int isCardNum = 0;
    int CardNumSet = 8; //배분될카드의 수량

    int[] cards;                                     //현재 데이터에서 받아온 가지고있는 전체카드종류

    int[] rock_paper_scissors       = new int[3];    //컴퓨터의 설정된 가위바위보
    int[] rock_paper_scissorsPlayer = new int[3];    //플레이어의 설정된 가위바위보
    int[] vicResult                 = new int[3];    //승리한 결과 value 

    
                                                      
    public delegate void  Rock_Paper_Scissors(int[] player,int[] com); //델리게이트로 가위,바위로보냄
    public static   event Rock_Paper_Scissors sendEventRPS; //슬롯의 가위바위보를 전달한다.
  
    public delegate void  deg_Victory(int[] index);
    public static   event deg_Victory evnt_Victory;  //슬롯의 이긴결과를 보낸다.

   

    bool tem               = false; //test bool ~~
    bool NotSlot           = false;                  //빈 슬롯이 있는지 판단
    bool particlesPlaying;

    int  stageVictory      = 0;                      // 1패배  2무승부 3승리
    int  MaxSlotCount      = 9;                      //기본최대 슬롯의 갯수는 9개이다.
    int tempIntCoutinueCount = 0;
    bool TempCorStop       = false;
    static GameCardSlot dgCardSlot;                  //델리게이트로 특정한 카드정보를 받아온다.

    //GameCardSlot pre;  //전역이 아니라서 실행이 되지 않나해서 전역으로설정 //상관없네 다시 지역으로변경할것
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
      
        cardslotSample = GameObject.Find("CanvasBasePlayer/Panel_/GameObjectCardSample");
        //만약 경호가 잘못됐을때 유효성 검사가 필요할것 같다.
        //if (mainCamera != null) //비어있는지 판단하지 못함..
         mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();

        GameData.Instance.prefabs  = Resources.LoadAll("RoadPrefabs", typeof(GameObject)).Cast<GameObject>().ToArray();
        //현재 비어있는 슬롯의 월드좌표를 넘겨준다.
        DragSlot.eveClick += PlayerBlankNum;
        //playerSlot가 모두 비었나판단해서 GameData에 넘겨준다.
        DragSlot.eveClick += PlayerBlankAll;
        //playerSlot 이동완료또는 변경후 다시 슬롯의 카드를 재정렬한다.
        DragSlot.eveClick += PlayerFormArray;

       //델리게이트로 접근
        GameCardSlot.CardSenderProperty  = PlayerKingTowerAdd;


    }

  
    private void Start()
    {
       
        particlesPlaying = false;
        cardSlots2       = formation2.cardsLength;
        // = cardSlots2 = playerForm2.GetComponentsInChildren<GameCardSlot>();
        cards            = new int[GameData.Instance.hasCard.Count];
        int num          = 0;
        foreach (KeyValuePair<int, Card> key in GameData.Instance.hasCard)
        {
            //순서대로 카드의 숫자를 배출하기 위해서 
            cards[num]   = key.Key;
            num++;
        }

        int curPanel     = GameData.Instance.PanelItem;
        StartCoroutine(AutoStep());
        //color를 설정
        imgbtnColor      = Color.black;
        //color을 컴포넌트에 적용
        btnImg.color     = imgbtnColor;

        //게임메니저가 실행될때마다 새로동적생성을 위해서 
        //여기 밑으로 에러부분 있음-------
        GameData.Instance.playerVic.Clear();
        GameData.Instance.comVic.Clear();
        GameData.Instance.defectPlayer.Clear();

       

        //******************DEBUG ****
        OverlayParticles.ShowParticles(10, gameObject.transform.position);
    }

   
    //Formati0n에서 정렬되면서 저장시킨 cardSlot의 정보를 가져온다
    void ReCountCardSlots2()
    {
        cardSlots2         = formation2.CardReNumCom();
    }


   

    void StartLabelEff(GameObject obj)
    {
        iTween.Stop(obj);
        Vector3 cur        = obj.transform.position; //540,1033
        
        //이렇게 하면  540,1033 ~> 0  현재 좌표로 이동한다.
        iTween.MoveFrom(obj, iTween.Hash("position",cur,"time", 1, "islocal", true,  "oncomplete", "LabelEffEndEff", "oncompletetarget", this.gameObject, "easetype", iTween.EaseType.easeInOutQuint));
    }

    //TODO:효과 시작시 시작이 끝나고 효과를 주기 위한메소드 사운드 는 안됨.
    void LabelEffEndEff()
    {
       
    }

    void EndingLavelEff(GameObject obj)
    {
        labelText.text       = string.Format("{0}", "GAME OVER");
    }

    void StartLabelEffClose(GameObject obj)
    {
        iTween.Stop(obj);
        Vector3 cur          = obj.transform.localScale;
       
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
        //TODO:현재선택된 슬롯의 정보를 가지고 와야함
        IList<int> ids = new List<int>();
        ids = GameData.Instance.curSlotCards;
       
        for (int i = 0; i < ids.Count; ++i)
        {
            Card player = new Card();

            player.IconName  = GameData.Instance.UnityDatas[ids[i]].Name;
            player.Name      = GameData.Instance.UnityDatas[ids[i]].Name;
            player.Spable    = GameData.Instance.UnityDatas[ids[i]].SpAble;  //특수 능력
            player.atk_type  = GameData.Instance.UnityDatas[ids[i]].Atk_Type;//공격타입
            player.Attack    = GameData.Instance.UnityDatas[ids[i]].Attack;  //성안 군사 공격
            player.AtK_zone  = GameData.Instance.UnityDatas[ids[i]].Atk_Zone;//성곽공격
            player.ID        = GameData.Instance.UnityDatas[ids[i]].Id;
            player.Up_Hp     = GameData.Instance.UnityDatas[ids[i]].Up_Hp;    //update될 HP량
            SpriteSlot.Add(player); //id만 받아서 
        }
    }

    void ComCardInfoData()
    {
        SpriteSlotCom.Clear();

        foreach (KeyValuePair<int, Card> i in GameData.Instance.hasCard)
        {   //id,hasNum,hasLevel 이후것을 추가해 주어야 한다.
            // Name;  Shap; //카드이름IconName;//카드이미지Spable; //현재 카드가 가지고 있는 특수한 능력
            i.Value.IconName   = GameData.Instance.UnityDatas[i.Key -1].Name;
            i.Value.Name       = GameData.Instance.UnityDatas[i.Key -1].Name;
            i.Value.Spable     = GameData.Instance.UnityDatas[i.Key -1].SpAble;
            i.Value.atk_type   = GameData.Instance.UnityDatas[i.Key -1].Atk_Type;
            i.Value.Attack     = GameData.Instance.UnityDatas[i.Key -1].Attack;  //성안 군사 공격
            i.Value.AtK_zone   = GameData.Instance.UnityDatas[i.Key -1].Atk_Zone;//성곽공격
            i.Value.Up_Hp      = GameData.Instance.UnityDatas[i.Key -1].Up_Hp;    //Update될 체력
            SpriteSlotCom.Add(i.Value); //id만 받아서 

        }
    }

    //Player의 카드를 슬롯에 적용
    void InputCard(int num)
    {
         cardSlots[num].SetCardInfo(SpriteSlot[num], eBelong.PLAYER, eCardType.SLOT);
         string  name                   = SpriteSlot[num].IconName;
         cardSlots[num].itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", name);
    }

    //****TODO: 정확한 배치카드 세팅이 결정되면 코드 수정할것:2018
    void InputCardP2(int num)
    {
        cardSlots2[num].SetCardInfo(SpriteSlotCom[num], eBelong.COM, eCardType.SLOT);
        string name                     = SpriteSlotCom[num].IconName;
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
            //이벤트 
            evnt_Victory(vicResult);
        }
    }
    
    /// 플레이어측의 카드가 자동정렬되게 한다.
    void PlayerFormArray()
    {
        formation.FormationCardsArc(eGameCardSize.BASE, eBelong.PLAYER, eCardType.SLOT);
    }

    //생존시 카드의 특수능력발동한다.
    void Alive_Spable(int index, ePlayer player)
    {
        //index 발동시키는  카드를 알기 위해서 
        switch (player)
        {
            //Acsss 
            // Com카드위치  centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo 
            //Player카드위치 centerSlots[index].GetComponentInChildren<GameCardSlot>()
            //int enemyHp = centerSlots2[index].GetComponentInChildren<GameCardSlot>().curLevelHp;
            //int playerTowerHp = GameData.Instance.curPlayerKingTowerHp;
            // int Hp = GameData.Instance.curComKingTowerHp;
            // Hp -= damage;
            // kingT2.Hit(damage);
            // kingT2.postHp(Hp);
            case ePlayer.PLAYER:
                //플레이어 생존시
               string id=  centerSlots[index].GetComponentInChildren<GameCardSlot>().cardInfo.Spable;
               int Level =  centerSlots[index].GetComponentInChildren<GameCardSlot>().cardInfo.level;
                //아군캐릭추가 Hp증가
                if (id == "ManHealing")
                {
                    int CardPlus = 1 + Level;//카드의 특수한 증가량
                    for (int i = 0; i < cardSlots.Length; i++)
                    {
                        if (centerSlots[i].GetComponentInChildren<GameCardSlot>().cardInfo != null)
                        {
                            centerSlots[i].GetComponentInChildren<GameCardSlot>().curLevelHp += CardPlus;
                        }
                        else
                        {   //만약 빈슬롯이 있다면 인덱스를 넘긴다.
                            continue;
                        }
                    }
                }
                else if (id == "TowerHealing")
                {
                    // SpAble =="TowerHealing"이면 타워의 Hp 증가 
                    int Plus = 3 + Level;
                    GameData.Instance.curPlayerKingTowerHp += Plus;
                    int curHp = GameData.Instance.curPlayerKingTowerHp;
                    kingT.postHp(curHp);

                }
                else if (id == "ManDisturbance")
                {   //if SpAble =="ManDisturbance"이면 적군 캐릭터 Hp 감소:교란

                    int CardPlus = 1 + Level;//카드의 특수한 증가량
                    for (int i = 0; i < cardSlots2.Length; i++)
                    {
                        if (centerSlots2[i].GetComponentInChildren<GameCardSlot>().cardInfo != null)
                        {
                            centerSlots2[i].GetComponentInChildren<GameCardSlot>().curLevelHp -= CardPlus;
                        }
                        else
                        {   //만약 빈슬롯이 있다면 인덱스를 넘긴다.
                            continue;
                        }
                    }
                }
                else if (id == "TowerDisturbance")
                {   //if SpAble == "TowerDisturbance"이면 적군 타워 Hp 감소


                    int Plus = 3 + Level;
                    GameData.Instance.curPlayerKingTowerHp -= Plus;
                    int curHp = GameData.Instance.curComKingTowerHp;
                    kingT2.postHp(curHp);

                }
                else if (id == "OneTo")
                {
                    //장군특성 승리시 한번더 적카드의 Hp공격
                    //통찰이 아닐때
                    string ignor = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo.Spable;
                    if (ignor != "InSight")
                    {
                        int Plus = 3 + Level;
                        centerSlots2[index].GetComponentInChildren<GameCardSlot>().curLevelHp -= Plus;
                    }
                   

                }
                break;
           //------------------------------------------------
            case ePlayer.COMPUTER:
                string id2 = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo.Spable;
                int Level2 = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo.level;
                //아군캐릭추가 Hp증가
                if (id2 == "ManHealing")
                {
                    int CardPlus = 1 + Level2;//카드의 특수한 증가량
                    for (int i = 0; i < cardSlots.Length; i++)
                    {
                        if (centerSlots2[i].GetComponentInChildren<GameCardSlot>().cardInfo != null)
                        {
                            centerSlots2[i].GetComponentInChildren<GameCardSlot>().curLevelHp += CardPlus;
                        }
                        else
                        {   //만약 빈슬롯이 있다면 인덱스를 넘긴다.
                            continue;
                        }
                    }
                }
                else if (id2 == "TowerHealing")
                {
                    // SpAble =="TowerHealing"이면 타워의 Hp 증가 
                    int Plus = 3 + Level2;
                    GameData.Instance.curComKingTowerHp += Plus;
                    int curHp = GameData.Instance.curComKingTowerHp;
                    kingT2.postHp(curHp);

                }
                else if (id2 == "ManDisturbance")
                {   //if SpAble =="ManDisturbance"이면 적군 캐릭터 Hp 감소:교란

                    int CardPlus = 1 + Level2;//카드의 특수한 증가량
                    for (int i = 0; i < cardSlots.Length; i++)
                    {
                        if (centerSlots[i].GetComponentInChildren<GameCardSlot>().cardInfo != null)
                        {
                            centerSlots[i].GetComponentInChildren<GameCardSlot>().curLevelHp -= CardPlus;
                        }
                        else
                        {   //만약 빈슬롯이 있다면 인덱스를 넘긴다.
                            continue;
                        }
                    }
                }
                else if (id2 == "TowerDisturbance")
                {   //if SpAble == "TowerDisturbance"이면 적군 타워 Hp 감소

                    int Plus = 3 + Level2;
                    GameData.Instance.curPlayerKingTowerHp -= Plus;
                    int curHp = GameData.Instance.curPlayerKingTowerHp;
                    kingT.postHp(curHp);

                }
                else if (id2 == "OneTo")
                {
                    string ignor = centerSlots[index].GetComponentInChildren<GameCardSlot>().cardInfo.Spable;

                    if (ignor != "InSight")
                    {
                        //장군특성 승리시 한번더 적카드의 Hp공격
                        int Plus = 3 + Level2;
                        //1차공격에서 카드가 파괴될수도 있나??ERROR 확인하기
                        centerSlots[index].GetComponentInChildren<GameCardSlot>().curLevelHp -= Plus;
                    }
                  

                }



                break;
        }

    }

    //void Receive 슬롯에 모두 있을때 실행됨
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
        //패배시킨 카드의 체력이 0일때 제거가능하게 하기 위해서 설정
        //player 센터슬롯 0 에서 주먹1 or 가위2 or 보3 를 냈을때
        //come이 센터슬롯 0 에서  1,2,3   1,2,3   1,2,3을 냈을때의 비교
        if (rock_paper_scissorsPlayer[index] == 1)  //--------------1--------------------------------------
        {      //player가 주먹을 냈을때
                    if (rock_paper_scissors[index] == 1)
                    {   //com이 주먹를 냈다면
                        Debug.Log("----비겼습니다.---------");
                        result = 2;
                    }
                    else if (rock_paper_scissors[index] == 2)
                    {   //com이 가위를 냈다면
                        Debug.Log("----------승리했습니다.--------...");
                        result = 3;

                        if (centerSlots2[index].GetComponentInChildren<GameCardSlot>() != null)
                        { //Com슬롯이 비어있지 않을때..이미 있지만 확인차원에서 
                               
                          int enemyHp = centerSlots2[index].GetComponentInChildren<GameCardSlot>().curLevelHp;
                          int damage  = centerSlots[index].cardInfo.AtK_zone;
                          enemyHp -= damage;

                            if (enemyHp <= 0)
                            {
                                //패배한 카드의 정보를 player카드에 담는다.
                                centerSlots[index].defectCards.Add(
                                 centerSlots2[index].GetComponentInChildren<GameCardSlot>().GetUnityInfo()
                                    );

                                //패배해서 사라질때의 연기이펙트 생성,com의위치에 연기생성
                                // EffCreate("EF_Smoke", centerSlots2[index].transform);
                                //AppUIEffect.instance.InstanceVFX(eEFFECT_NAME.Destroy_Card).transform.SetParent(centerSlots2[index].transform);
                                //AppSound.instance.SE_EXPLOSION.Play();
                              
                                int Hp = GameData.Instance.curComKingTowerHp;
                                Hp -= damage;
                              
                                kingT2.Hit(damage);
                                kingT2.postHp(Hp);
                                GameData.Instance.curComKingTowerHp = Hp;
                                centerSlots2[index].GetComponentInChildren<GameCardSlot>().curLevelHp = enemyHp;
                                //패배한 카드 삭제
                                Destroy(centerSlots2[index].GetComponentInChildren<GameCardSlot>().gameObject);
                                //카드의 특성에 따라 카드 추가 하기,여기서 GameData.Instance.platerVic작성됨 
                                Victory_AddCard(index, ePlayer.PLAYER);
                                Alive_Spable(index, ePlayer.PLAYER);
                            }
                            else if (enemyHp > 0)
                            {
                                int Hp = GameData.Instance.curComKingTowerHp;
                                Hp -= damage;
                             
                                kingT2.Hit(damage);
                                kingT2.postHp(Hp);
                                GameData.Instance.curComKingTowerHp = Hp;
                                centerSlots2[index].GetComponentInChildren<GameCardSlot>().curLevelHp = enemyHp;
                                //---------------------------------------------
                                //TODO:생존확인하고 특수한 능력을 추가한다
                                Alive_Spable(index, ePlayer.PLAYER);
                                //---------------------------------------------
                    }
                           
                        }
                      
                           
                    }
                    else if (rock_paper_scissors[index] == 3)
                    {      //com이 보를 냈다면
                            Debug.Log("------패배했습니다.----..");
                            result = 1;

                            if (centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo != null)
                            {//com카드가 존재시 승리 했을때
                                int atk_zone   = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo.AtK_zone;
                                int enemyHp    = centerSlots[index].GetComponentInChildren<GameCardSlot>().curLevelHp;
                               enemyHp -= atk_zone;
                                    if (enemyHp <= 0)
                                    {
                                            //패배한 플레이어가 슬롯에 카드가 비어있지 않았을 때 
                                            if (centerSlots[index].GetComponentInChildren<GameCardSlot>() != null)
                                            { //패배한 카드의 정보를 Com리스트에 담는다.
                                                centerSlots2[index].GetComponentInChildren<GameCardSlot>().defectCards.Add(
                                                                     centerSlots[index].GetUnityInfo()
                                                                    );
                                                //패배한 카드의 목록에 추가 한다.ERROR
                                                GameData.Instance.defectPlayer.Add(centerSlots[index].cardInfo.ID,
                                                                          centerSlots[index].defectCards
                                                                   );
                                                //EffCreate("EF_Smoke", centerSlots[index].transform);
                                                //AppUIEffect.instance.InstanceVFX(eEFFECT_NAME.Destroy_Card).transform.SetParent(centerSlots[index].transform);
                                                //TODO: 플레이어의 카드에 있는 Destroy를 실행
                                                int playerTowerHp = GameData.Instance.curPlayerKingTowerHp;
                                                playerTowerHp -= atk_zone;
                                             
                                                //TODO: 카드에 따라 피해량이 달라지므로 KingT에 먼저데이터를 보내야 된다.
                                                //슬라이더 적용(damage크기)
                                                kingT.Hit(atk_zone);
                                                //슬라이더의 Hp크기를 다시 세팅
                                                kingT.postHp(playerTowerHp);
                                                GameData.Instance.curPlayerKingTowerHp = playerTowerHp;
                                                centerSlots[index].GetComponentInChildren<GameCardSlot>().EFFECT();
                                                AppSound.instance.SE_EXPLOSION.Play();
                                                //null 로 만드는데 eBelong.PLAYER가 적용되지 않음,패배한 카드의 슬롯세팅
                                                centerSlots[index].GetComponentInChildren<GameCardSlot>().SetCardInfo(null, eBelong.PLAYER, eCardType.CENTERSLOT);
                                                //특별한 카드의 승리기능 실행
                                                Victory_AddCard(index, ePlayer.COMPUTER);
                                            }
                                    }
                                    else if(enemyHp > 0)
                                    {
                                        int playerTowerHp = GameData.Instance.curPlayerKingTowerHp;
                                        playerTowerHp -= atk_zone;
                                       
                                        //TODO: 카드에 따라 피해량이 달라지므로 KingT에 먼저데이터를 보내야 된다.
                                        //슬라이더 적용(damage크기)
                                        kingT.Hit(atk_zone);
                                        //슬라이더의 Hp크기를 다시 세팅
                                        kingT.postHp(playerTowerHp);
                                        GameData.Instance.curPlayerKingTowerHp = playerTowerHp;
                                        //TODO;카드의체력이 남아 있을때
                                        centerSlots[index].GetComponentInChildren<GameCardSlot>().curLevelHp = enemyHp;
                                        //---------------------------------------------
                                        //TODO:생존확인하고 특수한 능력을 추가한다
                                     
                                        //---------------------------------------------
                    }

                }
                          
                            }
        }   //Player가 가위를 냈을때
        else if (rock_paper_scissorsPlayer[index] == 2) //------------2--------------------------------------
        {
                      result = 1;
                    if (rock_paper_scissors[index] == 1)
                       {    //com이 주먹을 냈다면

                             Debug.Log("패배했습니다..");
                            if (centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo != null)
                            {//com카드가 존재시 승리 했을때

                                int atk_zone   = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo.AtK_zone;
                                int enemyHp    = centerSlots[index].GetComponentInChildren<GameCardSlot>().curLevelHp;

                                enemyHp -= atk_zone;
                                    if (centerSlots[index].GetComponentInChildren<GameCardSlot>() != null)
                                    {
                                            if (enemyHp <= 0)
                                            {
                                                centerSlots2[index].GetComponentInChildren<GameCardSlot>().defectCards.Add(
                                                         centerSlots[index].GetUnityInfo()
                                                         );
                                                //패배시 플레이어의 승리카드 기록을 기록한다.
                                                //TODO:
                                                Debug.Log("centerSlots[index].cardInfo.ID : " + centerSlots[index].cardInfo.ID);
                                                GameData.Instance.defectPlayer.Add(centerSlots[index].cardInfo.ID,
                                                centerSlots[index].defectCards);

                            //EffCreate("EF_Smoke", centerSlots[index].transform);
                            //AppUIEffect.instance.InstanceVFX(eEFFECT_NAME.Destroy_Card).transform.SetParent(centerSlots[index].transform);
                            //AppSound.instance.SE_EXPLOSION.Play();
                                                int playerTowerHp = GameData.Instance.curPlayerKingTowerHp;
                                               
                                                playerTowerHp -= atk_zone;
                                                //TODO: 카드에 따라 피해량이 달라지므로 KingT에 먼저데이터를 보내야 된다.
                                                //슬라이더 표시임(damage)
                                                kingT.Hit(atk_zone);
                                                //슬라이더의 Hp크기를 다시 세팅
                                                kingT.postHp(playerTowerHp);
                                                GameData.Instance.curPlayerKingTowerHp = playerTowerHp;
                                                centerSlots[index].GetComponentInChildren<GameCardSlot>().EFFECT();
                                                centerSlots[index].GetComponentInChildren<GameCardSlot>().SetCardInfo(null, eBelong.SYSCOM, eCardType.CENTERSLOT);
                                                Victory_AddCard(index, ePlayer.COMPUTER);
                                            }
                                            else if(enemyHp > 0)
                                            { //체력이 남아 있을때
                                              //TODO;카드의체력이 남아 있을때

                                                int playerTowerHp = GameData.Instance.curPlayerKingTowerHp;
                                             
                                                playerTowerHp -= atk_zone;
                                                //TODO: 카드에 따라 피해량이 달라지므로 KingT에 먼저데이터를 보내야 된다.
                                                //슬라이더 표시임(damage)
                                                kingT.Hit(atk_zone);
                                                //슬라이더의 Hp크기를 다시 세팅
                                                kingT.postHp(playerTowerHp);
                                                GameData.Instance.curPlayerKingTowerHp = playerTowerHp;
                                                centerSlots[index].GetComponentInChildren<GameCardSlot>().curLevelHp = enemyHp;
                                            }

                                
                                    } //CenterSlot END
                                 
                            } //슬롯에 Com카드가 있을때---------------
        
                    }
                    else if (rock_paper_scissors[index] == 2)
                     {     //com이 가위를 냈다면
                        Debug.Log("비겼습니다.");
                        result = 2;
                    }
                    else if (rock_paper_scissors[index] == 3)
                    {     //com이 보를 냈다면
                        Debug.Log("승리했습니다..");
                        result = 3;

                     
                       //적카드 제거
                         if (centerSlots2[index].GetComponentInChildren<GameCardSlot>() != null)
                            {
                                int damage = centerSlots[index].cardInfo.AtK_zone;
                                int enemyHp = centerSlots2[index].GetComponentInChildren<GameCardSlot>().curLevelHp;
                                    enemyHp -= damage;
                                    if (enemyHp <= 0)
                                    {
                                        //패배한 카드의 정보를 담는다.
                                        centerSlots[index].defectCards.Add(
                                        centerSlots2[index].GetComponentInChildren<GameCardSlot>().GetUnityInfo()
                                                      );
                                        //EffCreate("EF_Smoke", centerSlots2[index].transform);
                                        //AppUIEffect.instance.InstanceVFX(eEFFECT_NAME.Destroy_Card).transform.SetParent(centerSlots2[index].transform);
                                        //AppSound.instance.SE_EXPLOSION.Play();
                                        //카드의 특성에 따라 카드 추가 하기 
                                        int Hp = GameData.Instance.curComKingTowerHp;
                                       
                                        Hp -= damage;
                                        kingT2.Hit(damage);
                                        kingT2.postHp(Hp);
                                        GameData.Instance.curComKingTowerHp = Hp;
                                        Victory_AddCard(index, ePlayer.PLAYER);
                                        Alive_Spable(index, ePlayer.PLAYER);
                                        Destroy(centerSlots2[index].GetComponentInChildren<GameCardSlot>().gameObject);
                                      
                                      }
                                    else if (enemyHp > 0)
                                    {
                                        int Hp = GameData.Instance.curComKingTowerHp;
                                      
                                        Hp -= damage;
                                        kingT2.Hit(damage);
                                        kingT2.postHp(Hp);
                                        GameData.Instance.curComKingTowerHp = Hp;
                                        centerSlots2[index].GetComponentInChildren<GameCardSlot>().curLevelHp = enemyHp;
                             
                                        //---------------------------------------------
                                        //TODO:생존확인하고 특수한 능력을 추가한다 추가로 데이터가 진행된다.새애니가 필요함
                                        Alive_Spable(index, ePlayer.PLAYER);
                                        //---------------------------------------------
                                     }

                          }
                            //else
                            //{   //비어있을때는 부전승으로 승리 결과 실행
                            //    //TODO:비었을때 핸디캡을 추가 할수 있다.
                            //    int damage = centerSlots[index].cardInfo.AtK_zone;
                            //    kingT2.Hit(damage);
                            //    Victory_AddCard(index, ePlayer.PLAYER);
                            //}

            }
        }   //Player가 보를 냈을때
        else if (rock_paper_scissorsPlayer[index] == 3)  //------------3----------------------------------------
        {
                    if (rock_paper_scissors[index] == 1)
                    {   //컴이 주먹을 냈다면
                        Debug.Log("승리했습니다.");
                        result = 3;
                       
                       //적카드 제거
                        if (centerSlots2[index].GetComponentInChildren<GameCardSlot>() != null)
                        {

                            //타워에 피해량추가
                            int damage   = centerSlots[index].cardInfo.AtK_zone;
                            int enemyHp  = centerSlots2[index].GetComponentInChildren<GameCardSlot>().curLevelHp;
                            enemyHp -= damage;
                               if (enemyHp <= 0)
                                {
                                    centerSlots[index].defectCards.Add(
                                                 centerSlots2[index].GetComponentInChildren<GameCardSlot>().GetUnityInfo()
                                                   );

                                    //EffCreate("EF_Smoke", centerSlots2[index].transform);
                                    //AppUIEffect.instance.InstanceVFX(eEFFECT_NAME.Destroy_Card).transform.SetParent(centerSlots2[index].transform);
                                    //AppSound.instance.SE_EXPLOSION.Play();
                                    int Hp = GameData.Instance.curComKingTowerHp;
                                    Hp -= damage;
                                  

                                    kingT2.postHp(Hp);
                                    //타워데미지
                                    kingT2.Hit(damage);
                                    GameData.Instance.curComKingTowerHp = Hp;
                                    Alive_Spable(index, ePlayer.PLAYER);
                                    //카드의 특성에 따라 카드 추가 하기 
                                    Victory_AddCard(index, ePlayer.PLAYER);
                                    Destroy(centerSlots2[index].GetComponentInChildren<GameCardSlot>().gameObject);
                                  
                                }
                                else if (enemyHp > 0)
                                {
                                    int Hp = GameData.Instance.curComKingTowerHp;
                                    Hp -= damage;
                                  
                                    kingT2.postHp(Hp);
                                    //타워데미지
                                    kingT2.Hit(damage);
                                    GameData.Instance.curComKingTowerHp = Hp;
                                    centerSlots2[index].GetComponentInChildren<GameCardSlot>().curLevelHp = enemyHp;
                                    Alive_Spable(index, ePlayer.PLAYER);
                                }

                          
                        }
                        //else
                        //{   //비어있을때는 부전승으로 승리 결과 실행
                        //    Victory_AddCard(index, ePlayer.PLAYER);
                        //}
                     }
                    else if (rock_paper_scissors[index] == 2)
                    {      //player 보 이고 com이 가위를 냈다면
                        Debug.Log("패배했습니다..");
                        if (centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo != null)
                        {//com카드가 존재시 승리 했을때

                            int atk_zone   = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo.AtK_zone;
                            int enemyHp    = centerSlots[index].GetComponentInChildren<GameCardSlot>().curLevelHp;
                                   enemyHp -= atk_zone;
                            if (centerSlots[index].GetComponentInChildren<GameCardSlot>() != null)
                            {
                                if (enemyHp <= 0)
                                {
                                    centerSlots2[index].GetComponentInChildren<GameCardSlot>().defectCards.Add(
                                             centerSlots[index].GetUnityInfo()
                                             );
                                    //패배시 플레이어의 승리카드 기록을 기록한다.
                                    //TODO:
                                    Debug.Log("centerSlots[index].cardInfo.ID : " + centerSlots[index].cardInfo.ID);
                                    GameData.Instance.defectPlayer.Add(centerSlots[index].cardInfo.ID,
                                    centerSlots[index].defectCards);


                                        //EffCreate("EF_Smoke", centerSlots[index].transform);
                                        //AppUIEffect.instance.InstanceVFX(eEFFECT_NAME.Destroy_Card).transform.SetParent(centerSlots[index].transform);
                                        //AppSound.instance.SE_EXPLOSION.Play();
                                        int playerTowerHp = GameData.Instance.curPlayerKingTowerHp;
                                      
                                        playerTowerHp -= atk_zone;
                                        //TODO: 카드에 따라 피해량이 달라지므로 KingT에 먼저데이터를 보내야 된다.
                                        //슬라이더표시(damage)
                                        kingT.Hit(atk_zone);
                                        //슬라이더의 Hp크기를 다시 세팅
                                        kingT.postHp(playerTowerHp);
                                        GameData.Instance.curPlayerKingTowerHp = playerTowerHp;
                                        centerSlots[index].GetComponentInChildren<GameCardSlot>().EFFECT();
                                        centerSlots[index].GetComponentInChildren<GameCardSlot>().SetCardInfo(null, eBelong.SYSCOM, eCardType.CENTERSLOT);
                                        Victory_AddCard(index, ePlayer.COMPUTER);
                                }
                                else if(enemyHp > 0)
                                { //체력이 남아 있을때
                                  //TODO;카드의체력이 남아 있을때

                                    int playerTowerHp = GameData.Instance.curPlayerKingTowerHp;
                                   
                                    playerTowerHp -= atk_zone;
                                    //TODO: 카드에 따라 피해량이 달라지므로 KingT에 먼저데이터를 보내야 된다.
                                    //슬라이더표시(damage)
                                    kingT.Hit(atk_zone);
                                    //슬라이더의 Hp크기를 다시 세팅
                                    kingT.postHp(playerTowerHp);
                                    GameData.Instance.curPlayerKingTowerHp = playerTowerHp;
                                    centerSlots[index].GetComponentInChildren<GameCardSlot>().curLevelHp = enemyHp;
                                }
                            } //CenterSlot END
                        } //슬롯에 Com카드가 있을때---------------
                    }
                    else if (rock_paper_scissors[index] == 3)
                    {   //com이 보를 냈을때
                        //Debug.Log("비겼습니다.");
                        result = 2;
                    }
        }
        return result;
    }

    //비었는지 판단하고 승패 리턴한다. 승리3:패배1
     int Victory_pntEMPT(int index)
    {
       int result = 1;

        //각각의 slot가 비었는지 확인한다.-1이면 있는거고 0보다 크면 비어있는 상태임
       int emptIndexPlayer  = ReturnBlankSlotPlayer(index);
       int emptIndexCom     = ReturnBlankSlot(index);


        if (emptIndexPlayer > 0 && emptIndexCom < 0)
        {
            //player가 비고 컴은 슬롯에 채워져 있을때 패배
            //Debug.Log("패배했습니다.");
            //player  center의Card정보를 삭제한다.  센터카드가 sys로 바뀌는 의심구간
            //*************ERROR***********************************
            //작아지는 에러 발생의심구간
            int damage = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo.AtK_zone;
            int Hp = GameData.Instance.curComKingTowerHp;
            Hp -= damage;
            kingT2.Hit(damage);
            kingT2.postHp(Hp);
            GameData.Instance.curComKingTowerHp = Hp;

            centerSlots[index].GetComponentInChildren<GameCardSlot>().SetCardInfo(null, eBelong.PLAYER,eCardType.CENTERSLOT);
           //컴카드의 ATK값을 얻어와야 함
            Victory_AddCard(index, ePlayer.COMPUTER);
            result = 1;
        }
        else if (emptIndexPlayer < 0 && emptIndexCom > 0)
        {
            //player가 있고 컴이 비었을때 승리
            //if (centerSlots2[index].GetComponentInChildren<GameCardSlot>() != null)
            //Destroy(centerSlots2[index].GetComponentInChildren<GameCardSlot>().gameObject);
            //Debug.Log("승리했습니다.");
            //카드의 특성에 따라 카드 추가 하기 
            int damage = centerSlots[index].cardInfo.AtK_zone;
            int Hp = GameData.Instance.curComKingTowerHp;
            Hp -= damage;
            kingT2.Hit(damage);
            kingT2.postHp(Hp);
            GameData.Instance.curComKingTowerHp = Hp;

            Victory_AddCard(index, ePlayer.PLAYER);
            result = 3;
          
           
         

        }
            else if (emptIndexPlayer > 0 && emptIndexCom > 0)
        {
            //둘다 비었을때
            //Debug.Log("비겼습니다.");
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

    //승리시 했을때의 결과 처리로 카드의 특성에 맞게 기능을 추가한다.
    // 카드가 완전히 사라졌을때 ,부전승일때도 호출됨XXXX 부전승일때 호출되면 안됨 인덱스에서 에러발생,
    //카드가 파괴될때 호출한다.
    void Victory_AddCard(int index , ePlayer player)
    {
        string id           = null;
        GameObject copySlot = null;
        Transform  trans    = null;
       //bool enableCard    = false; //카드의 체력이 다 되었는지 판단해서 다 되었다면 true
        switch (player)
        {
            case ePlayer.PLAYER:
              
                if (centerSlots[index].cardInfo != null)
                {
                          id              = centerSlots[index].cardInfo.Spable;
                          copySlot        = playerForm.GetComponent<Formation>().card;
                          trans           = playerForm.transform;

                          
                          Card cardcur     = centerSlots[index].cardInfo;
                          Card defCard     = null;
                         //패배한 카드의 슬롯이 비어있지 않을때
                         if (centerSlots2[index].GetComponentInChildren<GameCardSlot>() != null)
                         {   //com슬롯의 카드정보를 담는다.
                             defCard  = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo;
                            //패배한 카드의 체력을 깍는다.승리한 카드의 공력력은 임시로 1로 정함       
                         }

                            //카드의 체력까지 완전히 패배시켰을때
                            //승리카드------------------------------------------------------
                            //TODO:승리했을때 카드슬롯에 접근해서 추가 할수 있는 기능부분
                 
                        //--------------------------------------------------------------

                    //승리한 카운트가 0일때
                    if (GameData.Instance.playerVic.Count == 0)
                            {
                                IList<Card> vicCar = new List<Card>();
                                GameData.Instance.playerVic.Add(cardcur.ID, vicCar);
                                GameData.Instance.playerVic[cardcur.ID].Add(defCard);
                            }//승리한 카운트가 0보다 클때
                            else if (GameData.Instance.playerVic.Count > 0)
                            {
                          
                                    if (GameData.Instance.playerVic.ContainsKey(cardcur.ID))
                                    {
                                    //승리한 카드의 리스트키값이 있으면 패배한 카드를 바로 추가하고 
                                    //DEGUG:패배한 카드가 확실한지 확인필요함..헷갈림
                                    GameData.Instance.playerVic[cardcur.ID].Add(defCard);
                                    }
                                    else
                                    { //새로운 승리한 카드일때는 새로이 리스트를 작성한다.
                                    IList<Card> vicCar = new List<Card>();
                                    GameData.Instance.playerVic.Add(cardcur.ID, vicCar);
                                    GameData.Instance.playerVic[cardcur.ID].Add(defCard);
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
                                            cardSlots[i].SetCardInfo(newCard, eBelong.SYSCOM,eCardType.SLOT);
                                            formation.FormationCardsArc(eGameCardSize.BASE,eBelong.PLAYER,eCardType.SLOT);
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
                                    newCardSlot.GetComponent<GameCardSlot>().SetCardInfo(newCard, eBelong.SYSCOM,eCardType.SLOT);
                                    formation.FormationCardsArc(eGameCardSize.BASE,eBelong.PLAYER,eCardType.SLOT);

                                    StartCoroutine(CoShowShowAddCardPlayer(0.3f));
                                }
                            }    // "AddCard"END
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
                        id              = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo.Spable;
                        copySlot        = playerForm2.GetComponent<Formation>().card;
                        trans           = playerForm2.transform;
                  

                

                    //승리카드
                    Card cardcur = centerSlots2[index].GetComponentInChildren<GameCardSlot>().cardInfo;
                       //패배카드
                     Card defCard = null;

                    //if(centerSlots[index].cardInfo != null)
                    //    defCard = centerSlots[index].cardInfo;


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
                                newCardSlot.GetComponent<GameCardSlot>().SetCardInfo(newCard, eBelong.SYSCOM,eCardType.SLOT);

                                formation2.FormationCardsArcCom(eGameCardSize.BASE,eBelong.COM,eCardType.SLOT);
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
            //Debug.Log("패배했습니다.");
            result = 1;
        }
        else if (emptIndexPlayer < 0 && emptIndexCom > 0)
        {
            //player가 있고 Com이 비었을때 승리
            //Debug.Log("승리했습니다...");
            result = 3;
        }
        else if (emptIndexPlayer > 0 && emptIndexCom > 0)
        {
            //둘다 비었을때
            //Debug.Log("비겼습니다.");
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
                //Debug.Log("비겼습니다.");
                result = 2;
            }
            else if (rock_paper_scissors[index] == 2)
            {
                //Debug.Log("승리했습니다...");
                result = 3;
            }
            else if (rock_paper_scissors[index] == 3)
            {
                //Debug.Log("패배했습니다.");
                result = 1;
            }
        }
        else if (rock_paper_scissorsPlayer[index] == 2) //------------2--------------------------------------
        {
            if (rock_paper_scissors[index] == 1)
            {
                //Debug.Log("패배했습니다..");
                result = 1;
            }
            else if (rock_paper_scissors[index] == 2)
            {
                //Debug.Log("비겼습니다.");
                result = 2;
            }
            else if (rock_paper_scissors[index] == 3)
            {
                //Debug.Log("승리했습니다..");
                result = 3;
            }

        }
        else if (rock_paper_scissorsPlayer[index] == 3)  //------------3----------------------------------------
        {
            if (rock_paper_scissors[index] == 1)
            {
                //Debug.Log("승리했습니다.");
                result = 3;
            }
            else if (rock_paper_scissors[index] == 2)
            {
                //Debug.Log("패배했습니다.");
                result = 1;
            }
            else if (rock_paper_scissors[index] == 3)
            {
                //Debug.Log("비겼습니다.");
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

            cardSlots[temp[0]].SetCardInfo(null, eBelong.SYSCOM,eCardType.SLOT);
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
                //kingT2.TextEFF();

                //승리한 카드에 접근할수 있는가?
                //승리한 카드의 슬롯뱅크숫자는? 
                // 바보  result는 승리판정이지 인덱스가 아님
               

                break;
            case 2:


                break;
            case 1:
                //kingT.TextEFF();

                break;
        }
    }

    //컴퓨터의 Base카드에서 랜덤으로 선택되어 센터로 이동하게 된다.
    void MoveChoiceCom()
    {
        int RandomNum    = cardSlots2.Length;
        int[] choNum     = new int[3];

        for (int i = 0; i < choNum.Length;i++)
        {
            int choice    = UnityEngine.Random.Range(0, RandomNum);
            choNum[i]     = choice;
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
            int nA            = UnityEngine.Random.Range(0, cardSlots2.Length) % cardSlots2.Length;
            int nB            = UnityEngine.Random.Range(0, cardSlots2.Length) % cardSlots2.Length;
            GameCardSlot temp = cardSlots2[nA];
               cardSlots2[nA] = cardSlots2[nB];
               cardSlots2[nB] = temp;
        }
    }

    //카드가 센터에 이동하는 메소드
    void BatchSlot(GameCardSlot card,int dex)
    {
        if (!NotSlot)
        {
            if (card.gameObject )
            {
                    // NotSlot가 false일때 실행된다.
                    Vector3 to = centerSlots2[dex].transform.position;
                    card.gameObject.GetComponentInChildren<GameCardSlot>().MoveSlot(to);
                    //card.gameObject.GetComponentInChildren<GameCardSlot>().enCenterSlot = true;
                    card.gameObject.GetComponentInChildren<GameCardSlot>().eType        = eCardType.CENTERSLOT;
                    card.gameObject.GetComponentInChildren<GameCardSlot>().eBelongState = eBelong.SYSCOM;
                    //이동시킨것의 부모를 변경한다.
                    card.gameObject.transform.SetParent(centerSlots2[dex].transform);
                        if (card.cardInfo != null)
                        {
                            //---TODO:Com에 Up Hp를 추가 한다-------------------

                            int hp = GameData.Instance.curComKingTowerHp;
                            int index = card.cardInfo.ID - 1;
                            int add = GameData.Instance.UnityDatas[index].Up_Hp;
                            hp += add;
                            kingT2.postHp(hp);
                            GameData.Instance.curComKingTowerHp = hp;
                            //-----------------------------------------------
                        }

                   //TODO : slot2에 코드시 호출이간 모든 slot에서 AppUiEffect를 생성시켜서 여기서 생성함
                  GameObject ef = AppUIEffect.instance.InstanceVFX(eEFFECT_NAME.Destroy_Card);
                  ef.transform.SetParent(centerSlots2[dex].transform);
                  ef.transform.localScale = new Vector3(50, 50, 50);
                  //screen -->world------------------------------
                  var wor = mainCamera.ScreenToWorldPoint(Vector3.zero);
                  ef.transform.localPosition = new Vector3(wor.x, wor.y, wor.z + 10);

                  //카드 배분 알리는 이벤트
                  eveCardIn();
                  ReCountCardSlots2();
                  //변경된 인덱스와 갯수로 베이스의 거리를 다시 재정렬한다.
                  formation2.FormationCardsArcCom(eGameCardSize.BASE,eBelong.COM,eCardType.SLOT);
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

       // 슬롯이 비었다면 0,1,2 리턴하고 카드가 있다면 -1리턴
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
           // Debug.Log("버튼에 의해서 활성화");
        }
       
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("5_Result_Scene");
        //nextScene = "2_Main_Scene";
    }


    //Com의 카드 조절
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

    //플레이어 센터의 빈슬롯을 반환한다.비어있는 우선순위대로 반환한다.
    void  PlayerBlankNum()
    {
        Vector3 to;
      
        int num = -1;
        for (int i = 0; i < 3; ++i)
        {
           num =  ReturnBlankSlotPlayer(i);

            if (num >= 0)
            {
                to = centerSlots[num].transform.position;
                GameData.Instance.DrageCardInfoVector3 = to;
                break;
            }
          
        }
      
    }

    //플레이어의 slot 가 모두 비었나 판단
    void PlayerBlankAll()
    {
        int checkCout = 0;
        int num       = -1;
        for (int i = 0; i < 3; ++i)
        {
            num = ReturnBlankSlotPlayer(i);
            if (num >= 0)
            {
               //슬롯에 카드가 있을때 -1
                GameData.Instance.bolPlayerBlankAll = false;
                break;
            }
            else 
            {
                checkCout++;
                if(checkCout>=3)
                GameData.Instance.bolPlayerBlankAll = true;
                //Debug.Log("bolPlayerBlankAll"+ GameData.Instance.bolPlayerBlankAll);
            }
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
            //Debug.Log("슬롯 둘다 있습니다..");
            combatShowC.SetInfo(newSlot0, newSlot1, idex);
        }
        else if (centerSlots[idex].cardInfo == null && centerSlots2[idex].GetComponentInChildren<GameCardSlot>() != null)
        {  //player의 슬롯에 카드정보가 없을때
            Card newSlot0 = centerSlots2[idex].GetComponentInChildren<GameCardSlot>().GetUnityInfo();
            //TODO: 정보가 없을때 ...수정해야될 부분
            Card newSlot1 = centerSlots[idex].GetUnityInfo();
            combatShowC.SetInfo(newSlot0, newSlot1, idex);
            //Debug.Log(" com 의 슬롯이 비었습니다.");
        }
        else if (centerSlots[idex].cardInfo != null && centerSlots2[idex].GetComponentInChildren<GameCardSlot>() == null)
        {  //com의 슬롯에 카드정보가 없을때
            //TODO:  슬롯에 없을때 newSlot0 :com <--->newSlot1 :player
            Card newSlot0 = null;
            Card newSlot1 = centerSlots[idex].GetUnityInfo();
            combatShowC.SetInfo(newSlot0, newSlot1, idex);
            //Debug.Log("player   의 슬롯이 비었습니다.");
        }
    }

    //player와 com의 타워 레벨을 세팅한다.
    //TODO:좀더 정교한 레벨 조정이 필요함 타워가드의 인원수 조정필요
    void LevelSet()
    {
        int playerLevel = GameData.Instance.players[1].exp;
        int playerLevelCount = GameData.Instance.players[1].expCount;


        int LevelMax   = 1;
        int max        = playerLevel + LevelMax;
        int min        = playerLevel + LevelMax * -1;

        int LevelMaxCount = 100;
        int maxCount   = playerLevelCount + LevelMaxCount;
        int minCount   = playerLevelCount + LevelMaxCount * -1;

        int Randomlevel = UnityEngine.Random.Range(min, max);
        int RandomCount = UnityEngine.Random.Range(minCount, maxCount);

        kingT.postHp(playerLevel);
        GameData.Instance.curPlayerKingTowerHp = playerLevel;

        //kingT.postMan(playerLevelCount);
        //Com의 체력을 임의로 6설정함
        kingT2.postHp(6);
        GameData.Instance.curComKingTowerHp = 6;
        //kingT2.postMan(RandomCount);
    }

    bool KingTowerCheck()
    {
        bool deleve = false;
            //플레이어의 성과 인력을 모두 판단한다.
            // if (kingT.GetHpBar() <= 0 || kingT.manNum <= 0 || kingT2.GetHpBar() <= 0 || kingT2.manNum <= 0)
            if (kingT.GetHpBar() <= 0  || kingT2.GetHpBar() <= 0 )
            {
               deleve = true;
               //만약  player 와 com이 동시에 승리 조건에 달했을때를 분리해야 한다.
               //무승부조건 출력
            if (kingT.GetHpBar() <= 0 && kingT2.GetHpBar() <= 0)
            {  //player가 성벽파워가0이 됐을때 동시에  com의 성벽파워가 0이 됐을때
               //무승부
               stageVictory = 2;
               //게임을 중지하고 승리판정을 한다.
               gameState = eGameState.VICTORY_OR_DEFEAT;
               //Debug.Log("KingTowerCheck() 체크하고 있는거야kingT.wallNum <= 0 && kingT2.wallNum <= 0)");

            }
           // else if (kingT.GetHpBar() <= 0 && kingT2.manNum <= 0)
           // {   //player가 성벽파워가0이 됐을때 동시에  com의 성벽수비수가 됐을때
           //     //무승부
           //     stageVictory = 2;
           //     //게임을 중지하고 승리판정을 한다.
           //     gameState = eGameState.VICTORY_OR_DEFEAT;
           //     Debug.Log("KingTowerCheck() 체크하고 있는거야kingT.wallNum <= 0 && kingT2.manNum <= 0)");

           // }
           //else if (kingT.manNum <= 0 && kingT2.GetHpBar() <= 0)
           // {
           //     //무승부
           //     stageVictory = 2;
           //     //게임을 중지하고 승리판정을 한다.
           //     gameState = eGameState.VICTORY_OR_DEFEAT;
           //     Debug.Log("KingTowerCheck() 체크하고 있는거야kingT.manNum <= 0 && kingT2.wallNum <= 0)");

           // }
            //else if (kingT.manNum <= 0 && kingT2.manNum <= 0)
           
            //{
            //    //무승부
            //    stageVictory = 2;
            //    //게임을 중지하고 승리판정을 한다.
            //    gameState = eGameState.VICTORY_OR_DEFEAT;
            //    Debug.Log("KingTowerCheck() 체크하고 있는거야kingT.manNum <= 0 && kingT2.manNum <= 0");

            //}
           // else if (kingT.manNum <= 0 || kingT.GetHpBar() <= 0)
            else if ( kingT.GetHpBar() <= 0)
            {  //com승리
                stageVictory = 1;
                //게임을 중지하고 승리판정을 한다.
                gameState = eGameState.VICTORY_OR_DEFEAT;
               // Debug.Log("KingTowerCheck() 체크하고 있는거야kingT.manNum <= 0 || kingT.wallNum <= 0");

            }
            //else if (kingT2.manNum <= 0 || kingT2.GetHpBar() <= 0)
            else if ( kingT2.GetHpBar() <= 0)
            { //player 승리
               // Debug.Log("player 승리선택 된거야 ");
                stageVictory = 3;
                //게임을 중지하고 승리판정을 한다.
                gameState = eGameState.VICTORY_OR_DEFEAT;
                //Debug.Log("KingTowerCheck() 체크kingT2.manNum <= 0 || kingT2.wallNum <= 0거야:::"+ stageVictory);
            }
        }
        return deleve;
    }

  
    //중간에 승패가 갈리면 step의 코루틴을 빠져 나와야 한다.
    IEnumerator VICTORY_OR_DEFEAT()
    {
        yield return new WaitForSeconds(0.1f);
     
            eText.text = String.Format("{0} :", "VICTORY_OR_DEFEAT");
            //1:컴승리,2: ,3:player승리
            GameData.Instance.vicResult = stageVictory;
            switch (stageVictory)
            {
                case 1:
                    //  패배 

                    //TODO: 컴퓨터의 경우 
                    #region comVic의 경우예시
                    //GameData.Instance.comVic = new Dictionary<int, IList<Card>>();
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    if (centerSlots2[i].GetComponentInChildren<GameCardSlot>().cardInfo != null)
                    //    {
                    //        GameData.Instance.comVic.Add(centerSlots2[i].GetComponentInChildren<GameCardSlot>().cardInfo.ID,
                    //            centerSlots2[i].GetComponentInChildren<GameCardSlot>().defectCards);
                    //    }
                    //}

                    //for (int i = 0; i < cardSlots2.Length; i++)
                    //{
                    //    if (cardSlots2[i].cardInfo != null)
                    //    {
                    //        GameData.Instance.comVic.Add(cardSlots2[i].cardInfo.ID, null);
                    //    }
                    //}
                    #endregion

                    startLabel.SetActive(true);
                    labelText.text = string.Format("{0}", " DEFEATED ");
                    StartLabelEff(startLabel);
                    yield return new WaitForSeconds(1f);
                    TempCorStop = true;
                    SceneChange();

                    yield return new WaitForSeconds(0.5f);

                  

                    break;
                case 2:

                    break;
                case 3:
                    //승리

                    startLabel.SetActive(true);
                    labelText.text = string.Format("{0}", " VICTORY");
                    StartLabelEff(startLabel);
                    yield return new WaitForSeconds(1f);
                    TempCorStop = true;
                    SceneChange();
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
            //최후 마지막 
    }

   
    // Card가 배치될때 kingTower에 카드가 가지고 있는 고유한 Up_Hp를 더해준다.
    public void PlayerKingTowerAdd(GameCardSlot gs)
    {
        if (gs.eBelongState == eBelong.PLAYER)
        {
            int hp      = GameData.Instance.curPlayerKingTowerHp;
            // gs.ID  여기 gs.ID -1 해야되나 항상헷갈림
            int add     = GameData.Instance.UnityDatas[gs.ID-1].Up_Hp;
            string name = gs.cardInfo.Name;
            Debug.Log("name :"+name);
            hp         += add;
            kingT.postHp(hp);
            GameData.Instance.curPlayerKingTowerHp = hp;
        }
    }

   

    private void OnDestroy()
    {
        DragSlot.eveClick -= PlayerBlankNum;
        //playerSlot가 모두 비었나판단해서 GameData에 넘겨준다.
        DragSlot.eveClick -= PlayerBlankAll;
        //playerSlot 이동완료또는 변경후 다시 슬롯의 카드를 재정렬한다.
        DragSlot.eveClick -= PlayerFormArray;
    }

  

    public void Update()
    {
        //com의 패를 하나씩 낸다.
        if (Input.GetKeyDown(KeyCode.B))
        {
            int index = ReturnBlankSlot(0);
            BatchSlot(cardSlots2[0],index);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            //TempCorStop = true;
            //AddKingTower();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //tem = true;
            //Debug.Log("키에 의해서 활성화");
            //PlayerCardSlotMove();
            if (GameData.Instance.DrageCardInfo == null)
            {
                //Debug.Log("비어있음 : ");
            }
            else if (GameData.Instance.DrageCardInfo !=null)
            {
              Vector3 a =   GameData.Instance.DrageCardInfoVector3;
               // Debug.Log("비어있지 앟음 좌표는 : "+a);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            gameState = eGameState.VICTORY_OR_DEFEAT;

        }

    }


}
