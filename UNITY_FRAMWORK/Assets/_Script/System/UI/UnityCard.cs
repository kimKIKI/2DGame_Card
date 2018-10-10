using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


public enum CARDOBJTYPE
{
  NON,
  TabSlotCard,          //ItemCard_SampleTop
  FrontSlotCard,        //ItemCard_SampleFront
  BackSloatCard         //ItemCard_SampleBack
}
public class UnityCard : MonoBehaviour, IPointerClickHandler
{
    //다음의 값에 접근하고 세팅할수 있어야 한다.
    //{"ID":0,"Name":"Ice Gollum ","ATK_Type":"Top","Kinds":"Unit","Coin":300,"Jew":10,"Elixir":2,"HP":400,"Speed":"Slow","Attack":4,"Atk_Zone":100,"BuildTime":1,"Sp_atk":4,"Up_Hp":10,"Up_atk":10,"Life":0,"EA":1},
    // card 오브젝트를 만드는데 필요한 기능 

    public CARDOBJTYPE eCardType = CARDOBJTYPE.NON;
    //외부 드레그 
    public GameObject SampleB;        //자신과 같은 버튼을 생성하기 위해서
    public GameObject purchaseBox;    //Update버튼이 눌렸을때 박스오브젝트
    public GameObject updateBox;      //canvasForm호출때 대입하기 위해서
    public GameObject updateShowObj;
    //public Transform  tr_updateBtn;
    [HideInInspector]
    public GameObject frontobj;       //업데이트후 리턴값을 받기 위해서 

    public Transform  panelSwitch;    //자신이 위치 하게될 부모루트
    public GameObject arrow;          //자동으로 컨트롤하기 위해서 
    public Transform  hideButton;     //버튼이 선택시 가려지게할 버튼
    public Transform  updateCard;     //업데이트를 위한 카드
    public Transform  updateShow = null;    //업데이트 선택후에 업데이트이펙트가 발생하는 쇼
    public Button     updateBtn  = null;     //업데이트에 필요한 버튼
    public Button     mainButton = null;     //선택카드박스를 열어주는 버튼
    public Button     tabButton = null;      //같은 클래스이므로 top에서 사용시 
                                             //버튼의 위치를 다르게 해서 사용했음
    public GameObject IsOneUpdate;            //클릭발생시 업데이트 버튼을 감춘다.

     public   delegate  void  ReScroll();
     public   static    event ReScroll    evReScroll;

    enum Card_Kinds
    {
        Unity,
        Rarity,
        Hero,
        Lengend
    }

    //Shape
    public Image  levelImg;     //레벨 뒤화면 이미지
    public Image  mainICon;     //메인이미지
    public Text   LevelNum;     //현재 카드의 Level
    public Text   tempLevelNum; //level과 num을 비교하기 위해서 설정
    public Text   ownCards;     //레벨업 될때까지 남은 카드의 숫자를 표시 한다.
    public Text   elixerNum;    //엘릭서 Num
    public Text   iconName;     //card의 이름
   
    public Slider slider;       //카드의 숫자를 받아 바 크기를 나타내 준다.
   
    int curUpLevel;             //업데이트가 됐을때 collback ref로 업데이트된 것을 받아온다.
    int xPadding = 10;
    int yPadding = 40;
    RectTransform rt;

    //캐릭터의 등급에 따라 업글할수 카드의 제한이 있다.
    //현재 보유한 카드의 숫자를 입력받는다.


    //int   atk_Type;        //유닛타입 지상,공중
    //int   coin;            //상점에서 살때의 고유가격
    //int   Jew;             //보석으로 살때의 가격
    //int   HP;              //고유 체력
    //float speed;           //이동시 속도
    //float attack;          //공격시 피해량
    //float atk_Zone;        //공격 광역법위
    //float build;           //생성속도
    //float life;            //건물일때 유지 시간
    //float up_atk_Zone;     //업글시 파괴영역 증가량
    //float up_atk;          //업글시 공격력 증가량
    //int   spwawnEA = 1;    //생성될 겟수 default = 1;

           int     CardNameID;//카드의 Id에 따라 이미지가 달라짐
           string  icon_name; //카드의icon Name;
    public int     kindNum;   //Enum인덱스로  숫자로 구한다.

    //임시로 public 
    public int     iD;         //datas에서 고유아이디
  

           int     hasCardNum;  //전체 카드 숫자
           int     levelCardNum;//현재레벨업된 카드숫자
          
    //UnityForm      updateBox;   //box
    UnityForm      updateBox2;  //Show
    public int     SlotIndex;  //슬록몇번째에 있는가 확인, 교체시 사용하게됨

    [SerializeField]
    private int level;       //카드의 현재 레벨
    [SerializeField]
    public int     LevelCards;  //Level*필요한 카드숫자 ..즉 Leve[ 0,2,4,10,...]


    public int rootIndex;       //설정된 부모 slot

    [HideInInspector]
    public float width;         //slot 의 widthf간결
    [HideInInspector]
    public float height;        //slot의   height간격

    bool isSwitch;
    Vector3 localScale;
    Vector3 curScale;

    [HideInInspector]
    public Vector3 fromVector3;  //현재 slot의 좌표를 입력받는다.
    [HideInInspector]
    public Vector3 moveToVector3; //현재의 카드가 다음위치로이동해야할 좌표

    public int nextIndex;         //다음 이동할 인덱스   
   
    //프로퍼티 ====================================================
           int cardNum;
    public int elixer;
    string kinds;                 //카드종류 네임
    private Vector3 vcGridPos;    //그리드의 아이디를 적용해서 위치를 저장한다.
    int remainCardNum;            //레벨업빼고 남은 카드숫자
                                  //============================================================
                                  //float curPositionY;
                                  //float toPositionY;
    bool UpEnable;
    bool IsOpenbutton;            //update&기타 버튼이 활성화된 상태인지 판단한다.

    private void Awake()
    {
        //if (updateCard != null)
        //    updateBox = updateCard.gameObject.GetComponent<UnityForm>();

        //if (updateShow != null)
        //    updateBox2 = updateShow.gameObject.GetComponent<UnityForm>();
        // TODO: enum할당을 Start에서 하는데 문제 있을지
     
                if (updateBtn)
                {
                    updateBtn.onClick.AddListener(delegate
                    {
                        Debug.Log(" updateBtn 작동합니다.");
                        if (enableUpdate(ID, UpEnable))
                        {

                            //주의>> 여기서 대입해줘야 호출이 가능하다.
                            CanvasForm.Instance.purchaseBox = this.purchaseBox;
                            CanvasForm.Instance.updateBox   = this.updateBox;
                            CanvasForm.Instance.updateShow  = updateShowObj;
                            CanvasForm.Instance.curLevel    = Level;
                            CanvasForm.Instance.card        = this;

                            CanvasForm.Instance.updateBox.SetActive(true);
                            CanvasForm.Instance.SetUpdate(ID, updateBox);

                            IsOpenbutton = true;  
                            //TODO: 2개의 카드로 위치를 보여주게 되서 생긴문제.
                            if (this.frontobj)
                            {
                                CanvasForm.Instance.frontObj = this.frontobj;
                            }
                        }
                    });
               
        }
        //null이니까 실행되면 안되는 부분인데 근데 실행되고 exception발생

      
                if (mainButton)
                {
                mainButton.onClick.AddListener(delegate
                    {
                        Debug.Log("mainButton클릭");
                        //뒤에 활성화 되는 카드에 자신을 연결하기 위해서 
                        UI_TabControl.Instance.forntObject = this.gameObject;
                        SendID();
                        UI_TabControl.Instance.showSelectItem();
                    });
                }
      
                if (tabButton)
                {    // ItemSampleTop선택중일때 스와치 이동이 되게 한다.
                    tabButton.onClick.AddListener(delegate
                    {
                        Debug.Log("tabButton작동합니다.");
                        //SetUpdateShowOne(ID, card.gameObject, updateShow);
                        //를실행시키기 위해서 true 업데이트 버튼이 활성화 된상태 
                        //ID만전달
                        SendID();
                        //좌표만전달
                        SendTopPos();   
                        //스와프선택중일때 이동애니 실행
                        Main_Scene.Instance.SwitchSelectMove();
                        //이동시 정렬애니실행
                        ToArraySlotMove();
                        //이동후 아이디 교체
                        UI_TabControl.Instance.SwitchSlot();
                        //개별적으로 ButtonS를 찾아서 꺼주는 역할
                        UI_TabControl.Instance.HideSelectItem();

                        //tabButton.gameObject.GetComponentInChildren<Image>().enabled = true;
                        //tabButton.gameObject.GetComponentInChildren<Text>().enabled = true;

                        //만약 선택중이 아닐때는 Update버튼을 활성화 한다.
                        if (!GameData.Instance.isSwitch)
                        {
                            if (GameData.Instance.IsShowCard || IsOpenbutton)
                            {
                                Debug.Log("updateBtn 비활성화 성공");
                                if (updateBtn.gameObject.activeSelf == true)
                                {
                                    updateBtn.gameObject.SetActive(false);
                                    IsOpenbutton = false;
                                    CanvasForm.Instance.IsTop = false;
                                }
                               
                            }
                            else if (!GameData.Instance.IsShowCard)
                            {
                                if (updateBtn.gameObject.activeSelf == false)
                                {
                                    Debug.Log("updateBtn 활성화 성공");
                                    CanvasForm.Instance.IsTop = true;
                                    updateBtn.gameObject.SetActive(true);
                                    IsOpenbutton = true;

                                }

                            }
                        }
                    });
                } 
    }

   

    private void Start()
    {
        //curPositionY = arrow.transform.localPosition.y;
        //toPositionY = curPositionY + 20;
        //null Sample2에서는 arrow가 없기 때문에 생성시에 null발생시킴
        if (arrow != null)
        {
            localScale = arrow.transform.localScale;
        }
        rt = gameObject.GetComponent<RectTransform>();
      
    }

    void OnEnable()
    {
        //StartCoroutine(TEST());
        //Debug.Log("초기 시작 itemsample : GetID = " + gameObject.GetInstanceID());
        CanvasForm.eveLevelUp += ReSeting;
        //CanvasForm.collLeveUp += LevelUp;
        ReSeting();
    }

    void OnDisable()
    {
        CanvasForm.eveLevelUp -= ReSeting;
       // CanvasForm.collLeveUp -= LevelUp;
    }
    
    //이벤트메니저에 등록되는 시점
    public void EventListenrStart()
    {
        //Debug.Log("이벤트 메니저에 등록합니다.");
        EventManager.Instance.AddListener(EVENT_TYPE.LEVEL_CHANGE, OnEvent);
        int wonID  = gameObject.GetInstanceID();
        //Debug.Log("unityCard(clone) :" +wonID);
    }
    //move로 슬롯이 변경되고 다시 세팅될때 호출된는 메소드
    public void ReSeting()
    {
        //고유ID를 확인한다
        //전달 받은 ID 가 불분명하다 .  UnityData[0].Id 인지
        // UnityData[index] 인지 명확하지 않다.
        if (ID >= 1)
        {
            string cardName = GameData.Instance.UnityDatas[ID - 1].Name;
            elixerNum.text = GameData.Instance.UnityDatas[ID - 1].Elixir.ToString();
            mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", cardName);
            iconName.text = cardName;
        }
       
        //스트롤이 가능하게 한다.
        evReScroll();

        if (eCardType == CARDOBJTYPE.TabSlotCard)
        {
            IsOneUpdate.gameObject.SetActive(false);
            IsOpenbutton = false;
            GameData.Instance.IsShowCard = false;
        }
       
       
        //                           UnityData[0].Id 
        if (GameData.Instance.hasCard.ContainsKey(ID))
        {   //가지고 있는 카드의 tatal value
            int  hasCards       = GameData.Instance.hasCard[ID].hasCard;
            //현재 카드가 레벨업된value 
            int  level          = GameData.Instance.hasCard[ID].level;
            //현재까지의 레벨로 총카드숫자로 변환
            int curLevel        = GameData.Instance.Level[level-1];
            //업데이트하고 남은 카드의 value
            int remainCards     = LevelUpRemain(ID);
            //next level의 전체 카드 value 
            int nextRemainCards = GameData.Instance.Level[level];
            //현재레벨에서 다음레벨까지  필요한 카드의 value
            int curRemainCards  = nextRemainCards - curLevel;
          
            ownCards.text = string.Format("{0} / {1}", remainCards, nextRemainCards);
            LevelNum.text = string.Format("레벨 : {0} ", level);
            Slider();

            //다음 레벨에  업글가능한 카드가 존재할때
            if (remainCards >= curRemainCards)
            {
               
               
            }
        }
    }

     //GameData로 현재의 ID를 전달한다.
    public void SendID()
    {    //UnityData[0].Id 전달
        GameData.Instance.curSwitchCard = ID;
        GameData.Instance.CurSlotID     = SlotIndex;
    }


    //최종 선택되었을때 이동하기 위해서 GameData에 저장한다.
    public void SendSwitchCard()
    { //ID와 어떤차이가 있는지?확인필요
        GameData.Instance.fromSwitchId   = ID; 
        GameData.Instance.fromSwitchSlot = SlotIndex;                 
    }
    //tob Top---------> ArrayTab 
    public void ToArraySlotMove()
    {
        if (GameData.Instance.isSwitch)
        {
                int toindex      = GameData.Instance.fromSwitchSlot; //670  -- id  2
                int fromIndex    = SlotIndex; //0  현재 오브젝트의 인덱스 id  0,1,2,3 이될수 있다.

                if (fromIndex >= 4)
                {
                   fromIndex      = fromIndex / 4;
                }

                int width         = 260;
                int height        = 450;
                int paddingX      = 10;
                int paddingY      = 10;
      
                float rectTopY    = -730f;   //slot Y 
                float rectTSlotY  = -2211f;  //Panel_Button + ItemSlot Y
                float baseY       = -(Mathf.Abs(rectTSlotY) - Mathf.Abs(rectTopY));
                Vector2 rectPos   = new Vector2(width * (fromIndex + 1) + paddingX * fromIndex - width / 2,
                                             (height * (((fromIndex) / 4) + 1) - paddingY * (fromIndex / 4)) - height / 2);

                rectPos.y = baseY + rectPos.y - 300;
               //현재와 인덱스와 비교해서  큰지 작은지판단
                int chint = toindex - fromIndex;
                if (chint < 0)
                {
                    //0- width;
                   rectPos.x =   (chint * width);
                }
                else
                {
                    rectPos.x = chint * width;
                }

                 Vector3 pos3 = new Vector3(rectPos.x, rectPos.y, 0);
                  //Debug.Log("pos3 :top :" + pos3);

                iTween.MoveTo(gameObject, iTween.Hash("islocal",true, "position", pos3,
                                                      "time", 0.3f, "oncomplete", "ReachTransID",
                                                      "easetype", "easeOutQuart"));
        }

    }

    void toggleEnable()
     {
        if (hideButton)
        {
          bool a =  hideButton.gameObject.activeSelf;
        }
     }
    


    // slot[]에 이동하게 한다.
    public void SendTopPos()
      {
        //인덱스로  Grid Layout Group에서의 rect를 구한다.
        //비활성화 상태로 구해지지 않아서 공식으로 구한다.
        //int slot         = SlotIndex; //인덱스 0 부터 이므로 slot +1
        //int width        = 230;
        //int height       = 300;
        //int paddingX     = 20;
        //int paddingY     = 80;
        //int parentRectX  = 61;
        //int parentRectY  = -730;
      
        // Vector2 rectPos = new Vector2( width * (slot+1) + paddingX *slot   - width/2,
        //                             (height * (( (slot)/ 4) + 1) - paddingY * (slot / 4)) - height/2);

        //rectPos.x = rectPos.x + parentRectX;
        //rectPos.y = parentRectY + rectPos.y;
        //GameData.Instance.toTopPos = rectPos;

        GameData.Instance.toTopPos = gameObject.transform.position;
      }

    //slot[]에 도착한후 원래위치에서 from으로 세팅한다.
    public void ReachTransID()
    {
        //자동으로 엘릭서가 세팅되게 한다
       ID = GameData.Instance.fromSwitchId;
       ReSeting();
       rt.anchoredPosition = new Vector2(0, 0);
    }

    public void Move()
    {
       isSwitch      = GameData.Instance.isSwitch;
       Vector3 pos;
        if (isSwitch) //switch준비가 끝났을때 움직이게 하기
        {
          
            if (ID == GameData.Instance.curSwitchCard)
            {
                pos  = GameData.Instance.toSwitchPos;
            }
            else
            {
               // pos = GameData.Instance.fromSwitchPos;
                pos  = GameData.Instance.toSwitchPos;
            }

            iTween.MoveTo(gameObject, iTween.Hash("islocal",true,
                                                 "position", pos,
                                                 "time", 0.3f,
                                                 "easetype", "easeOutQuart",
                                                 "oncomplete","StopMove"));

        }
    }
    //레벨업을 갱신할때 사용하기 위한 메소드
    void LevelUp()
    {
       ID =  GameData.Instance.hasCard[ID].level;
    }
    //카드의 정렬이바뀌었을때위치가 이동하게 한다.
    public void MoveSelectPanel()
    {
        //현재 위치하게될 위치
        int x         = rootIndex % 4;
        int nextX     = nextIndex % 4;
        int result    = x - nextX;
        float widthAdd;
        float heightAdd;
        int y         = rootIndex / 4; //cur
        int nextY     = nextIndex / 4;

        if (y > nextY)
        {
            int yy    = Mathf.Abs(y - nextY);
            heightAdd = yy * height +(yy*yPadding);
        }
        else if (y == nextY)
        {
            heightAdd = 0;
        }
        else
        {
            int yy    = Mathf.Abs(y - nextY);
            heightAdd = yy* height * (-1)-(yy*yPadding);
        }


        if (x < nextX)
        {
            //현재의 좌표에  width만큼 더한다.
            int xx   = Mathf.Abs(x - nextX);
            widthAdd = xx * width+(xx*xPadding);
        }
        else
        {
            int xx   = Mathf.Abs(x - nextX);
            widthAdd = xx * width * (-1)-(xx*xPadding);
        }
         
  
        Vector3 poss = new Vector3(widthAdd, heightAdd, 0);
        iTween.MoveTo(gameObject, iTween.Hash("islocal", true,
                                               "position", poss,
                                               "time", 0.3f,
                                               "easetype", "easeOutQuart"
                                               ));
    }
    

    void StopMove()
    {
    
        isSwitch = false;
        //TODO:여기 서 움직임 완료 결정
        GameData.Instance.isSwitch = false;
    }

    void Slider()
    {
        //업데이트하고 남은 카드의 value
        int remainCards = LevelUpRemain(ID);
        //next level의 전체 카드 value 
        int nextRemainCards = GameData.Instance.Level[level];
        //value 카드단위당 증가할 vale설정 백분율
        float uni = 1f / nextRemainCards;
        //최대값이 1이므로 1실수율로 변환
        float value = remainCards * uni;
        //구매시 애니가 작동되어야 한다.
        slider.value = value;
    }

    bool enableUpdate(int ID,bool enable)
    {
       
        int remain           = LevelUpRemain(ID);
        int level            = GameData.Instance.hasCard[ID].level;
        int nextRemainCards  = GameData.Instance.Level[level];

        if (nextRemainCards < remain)
        {
            //가능할때 true;
            enable = true;
        }
        return enable;
    }

    //현재레벨까지 없데이트된후 남은량
    int LevelUpRemain(int ID)
    {
        //ID별 가지고 있는 카드량
        int hasCards = GameData.Instance.hasCard[ID].hasCard;
        //현재 카드가 레벨업된value 
        int level   = GameData.Instance.hasCard[ID].level;
        //레벨에 따른 카드의 수량배열
        int curLevel;

        for (int i = 0; i < level; ++i)
        {
            curLevel = GameData.Instance.Level[i];
            hasCards -= curLevel;
        }
        return hasCards;
    }

 
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.LEVEL_CHANGE:
                OnLevelChange(Sender, (int)Param);
                break;
        }
    }

    void OnLevelChange(Component Card, int level)
    {
        //TODO:레벨업이 발생시 모든UnityCard가 자신의 레벨을 업데이트한다.
        //코드 수정이 필요함ㅠ.ㅠ
        // GetInstnce가 달라서 사용할수 없음
        if (this.GetInstanceID() != Card.GetInstanceID()) return;
        //LevelNum.text = string.Format("레벨 : {0} ", GameData.Instance.hasCard[ID].level);
        //Debug.Log("aaaaa"+Card.name);
       //다음 레벨에  업글가능한 카드가 존재할때
    }



    //카드가 선택됐을때 기능
    public void OnClickFunc()
    {
        gameObject.SetActive(true);
        panelSwitch.GetComponent<Toggle_SwitchEff>().curID = ID;
        //Sitch 패널에 활성화시 하나만 활성화 되게 하기
        panelSwitch.GetComponent<Toggle_SwitchEff>().Toggle();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
          bool a =   GameData.Instance.isSwitch;
          bool b =   GameData.Instance.IsShowCard;
          // IsOpenbutton
          Debug.Log("isSwitch:  "+a+" IsShowCard:"+b +" ISOpenbutton :"+IsOpenbutton);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
      
    }


  

    ////슬롯을 클릭했을때 
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    if (cardInfo == null) return; //빈칸클릭
    //    if (!isPlayer2)
    //    {
    //        isPicked = true;
    //        formation.FormationCardsArc(eGameCardSize.BASE, eBelong.PLAYER);
    //        AppSound.instance.SE_Card_Down.Play();
    //        DragSlot.Instance.PickUp(this, eventData.position);
    //    }
    //}

    //==========프러퍼티================================
    public string Icon_name
    {
        get
        {
            return icon_name;
        }
        set
        {
            icon_name     = value;
            iconName.text = value;
        }
    }


    public int Elixer
    {
        get
        {
            return elixer;
        }

        set
        {
            elixer = value;
            //int형을  text에 넣기 위해서 변환함
            elixerNum.text = string.Format("{0}", value);
        }
    }


    public int CardNum
    {
        get
        {
            return cardNum;
        }

        set
        {
            cardNum = value;
        }
    }

    //카드의 종류를 저장한다.
    public string Kinds
    {
        get { return kinds; }
        set
        {
            kinds = value;
            Array arr = Enum.GetValues(typeof(Card_Kinds));
            List<string> Kinds = new List<string>(arr.Length);

            for (int i = 0; i < arr.Length; i++)
            {
                int n = arr.GetValue(i).ToString().CompareTo(kinds);
                if (n == 0)
                {
                    kindNum = i;
                }
            }
        }
    }

    //전체카드의 숫자를 입력받는다.
    public int HasCardNum
    {
        get
        {
            return hasCardNum;
        }

        set
        {
            hasCardNum = value;
        }
    }

  

    //레벨업된 카드의 숫자
    public int LevelCardNum1
    {
        get
        {
            return levelCardNum;
        }

        set
        {
            levelCardNum = value;
        }
    }

    //레벨업되고 남은 카드의 value
    public int RemainCardNum
    {
        get
        {
            return remainCardNum;
        }

        set
        {
            remainCardNum = value;
            if (value > LevelCards)
            {
                iTween.MoveBy(arrow, iTween.Hash("y", .06f, "time", 1f, "easetype", "easeInBack", "loopType", iTween.LoopType.loop));
            }
        }
    }

    public Vector3 VcGridPos
    {
        get
        {
            return vcGridPos;
        }

        set
        {
            vcGridPos = value;
        }
    }

    //TODO: 여기 ID 가 뭐지? 아이템 아이디인지 엘릭서크기 인지 헷갈림
    //Datas의 고유아이디?
    public int ID
    {
        get
        {
            return iD;
        }

        set
        {
            iD = value;
            if (value >= 0)
            {   //엘릭서를 설정
                Elixer = GameData.Instance.UnityDatas[value-1].Elixir;
            }

        }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
            //플레이어의 골드가 바뀔때 마다 체크한다.
            //level = Mathf.Clamp(value, 0, 49);
            EventManager.Instance.PostNotification(EVENT_TYPE.LEVEL_CHANGE, this, level);
        }
    }

    //==============================프로퍼티 END


}
