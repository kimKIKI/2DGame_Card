using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UnityCard : MonoBehaviour {
    //다음의 값에 접근하고 세팅할수 있어야 한다.
    //{"ID":0,"Name":"Ice Gollum ","ATK_Type":"Top","Kinds":"Unit","Coin":300,"Jew":10,"Elixir":2,"HP":400,"Speed":"Slow","Attack":4,"Atk_Zone":100,"BuildTime":1,"Sp_atk":4,"Up_Hp":10,"Up_atk":10,"Life":0,"EA":1},
    // card 오브젝트를 만드는데 필요한 기능 
   
    //외부 드레그 
    public GameObject SampleB;       //자신과 같은 버튼을 생성하기 위해서
    public Transform  panelSwitch;   //자신이 위치 하게될 부모루트
    public GameObject arrow;         //자동으로 컨트롤하기 위해서 
    public Transform  hideButton;     //버튼이 선택시 가려지게할 버튼

    enum Card_Kinds
    {
      Unity,
      Rarity,
      Hero,
      Lengend
    }

   
    
    //Shape
    public Image  levelImg;     //레벨 뒤화면 이미지
    public Text   LevelNum;     //현재 카드의 엘릭서 숫자
    public Text   tempLevelNum; //level과 num을 비교하기 위해서 설정
    public Text   ownCards;     //레벨업 될때까지 남은 카드의 숫자를 표시 한다.
    public Image  mainICon;     //메인이미지
    public Slider slider;       //카드의 숫자를 받아 바 크기를 나타내 준다.
    public Text   elixerNum;    //엘릭서 Num

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

    int         CardNameID;   //카드의 Id에 따라 이미지가 달라짐
    string      icon_name;    //카드의icon Name;
    public int  kindNum;      //Enum인덱스로  숫자로 구한다.

    //임시로 public 
    public int iD;          //datas에서 고유아이디
    public Text iconName;   //card의 이름

    int hasCardNum;        //전체 카드 숫자
    int levelCardNum;      //현재레벨업된 카드숫자
    int currLevel;

    public int SlotIndex;  //슬록몇번째에 있는가 확인, 교체시 사용하게됨

    [SerializeField]
    public int Level;       //카드의 현재 레벨
    [SerializeField]
    public int LevelCards;  //Level*필요한 카드숫자 ..즉 Leve[ 0,2,4,10,...]


    public int rootIndex; //설정된 부모 slot

    [HideInInspector]
    public float width;   //slot 의 widthf간결
    [HideInInspector]
    public float height;  //slot의   height간격

    bool    isSwitch;
    Vector3 localScale;
    Vector3 curScale;

    [HideInInspector]
    public Vector3 fromVector3;  //현재 slot의 좌표를 입력받는다.
    [HideInInspector]
    public Vector3 moveToVector3; //현재의 카드가 다음위치로이동해야할 좌표

    public int nextIndex;         //다음 이동할 인덱스                          

    //프로퍼티 ====================================================
            int       cardNum;
    public  int       elixer;
            string    kinds;             //카드종류 네임
    private Vector3   vcGridPos;         //그리드의 아이디를 적용해서 위치를 저장한다.
            int       remainCardNum;     //레벨업빼고 남은 카드숫자
    //============================================================
    //float curPositionY;
    //float toPositionY;

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

   

    public void ReSeting()
    {
        //고유ID를 확인한다
        string cardName = GameData.Instance.UnityDatas[ID].Name;
        int elixir      = GameData.Instance.UnityDatas[ID].Elixir;
        LevelNum.text   = string.Format("" + elixir);
        iconName.text   = string.Format("" + cardName);
       
    }

     //GameData로 현재의 ID를 전달한다.
    public void SendID()
    {
        GameData.Instance.curSwitchCard = ID;
        GameData.Instance.CurSlotID     = SlotIndex;
    }

     //최종 선택되었을때 이동하기 위해서 GameData에 저장한다.
    public void SendSwitchCard()
    { //ID와 어떤차이가 있는지?확인필요
        GameData.Instance.fromSwitchId = ID; 
        GameData.Instance.fromSwitchSlot = SlotIndex;                 
    }
    //tob Top---------> ArrayTab 
    public void ToArraySlotMove()
    {
        if (GameData.Instance.isSwitch)
        {
                int toindex   = GameData.Instance.fromSwitchSlot; //670  -- id  2
                int fromIndex = SlotIndex; //0  현재 오브젝트의 인덱스 id  0,1,2,3 이될수 있다.

                if (fromIndex >= 4)
                {
                   fromIndex = fromIndex / 4;
                }

                int width    = 260;
                int height   = 450;
                int paddingX = 10;
                int paddingY = 10;
      
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
                Debug.Log("pos3 :top :" + pos3);

                iTween.MoveTo(gameObject, iTween.Hash("islocal",true, "position", pos3,
                                                      "time", 0.3f, "oncomplete", "ReachTransID",
                                                      "easetype", "easeOutQuart"));



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

       isSwitch = GameData.Instance.isSwitch;
       Vector3 pos;
        if (isSwitch) //switch준비가 끝났을때 움직이게 하기
        {
          
            if (ID == GameData.Instance.curSwitchCard)
            {
                pos = GameData.Instance.toSwitchPos;
            }
            else
            {
               // pos = GameData.Instance.fromSwitchPos;
                pos = GameData.Instance.toSwitchPos;
            }

            iTween.MoveTo(gameObject, iTween.Hash("islocal",true,
                                                 "position", pos,
                                                 "time", 0.3f,
                                                 "easetype", "easeOutQuart",
                                                 "oncomplete","StopMove"));

        }
    }

    //카드의 정렬이바뀌었을때위치가 이동하게 한다.
    public void MoveSelectPanel()
    {
        //현재 위치하게될 위치
        int x = rootIndex % 4;
        int nextX = nextIndex % 4;
        int result = x - nextX;
        float widthAdd;
        float heightAdd;
        int y = rootIndex / 4; //cur
        int nextY = nextIndex / 4;

        if (y > nextY)
        {
            int yy = Mathf.Abs(y - nextY);
            heightAdd = yy * height +(yy*yPadding);
        }
        else if (y == nextY)
        {
            heightAdd = 0;
        }
        else
        {
            int yy = Mathf.Abs(y - nextY);
            heightAdd = yy* height * (-1)-(yy*yPadding);
          
        }


        if (x < nextX)
        {
            //현재의 좌표에  width만큼 더한다.
            int xx = Mathf.Abs(x - nextX);
            widthAdd = xx * width+(xx*xPadding);
        }
        else
        {
            int xx = Mathf.Abs(x - nextX);
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

   
    //카드가 선택됐을때 기능
    public void OnClickFunc()
    {
        gameObject.SetActive(true);
        panelSwitch.GetComponent<Toggle_SwitchEff>().curID = ID;
        //Sitch 패널에 활성화시 하나만 활성화 되게 하기
        panelSwitch.GetComponent<Toggle_SwitchEff>().Toggle();
    }

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
                Elixer = GameData.Instance.UnityDatas[value].Elixir;
            }

        }
    }

    //==============================프로퍼티 END

}
