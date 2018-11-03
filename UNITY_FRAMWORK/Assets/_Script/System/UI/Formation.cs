using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Formation : MonoBehaviour {
    //중앙에서 부터 배치가 되어야 한다.
    //카드가 클릭이 됐을때 확대 가능한 애니가 발생되어야 한다.
    //카드가 드레그 되어야 한다
    //카드의 자유스러운 배치 모양이 가능해야 한다.
    //전체 카드 배치의 넚이가 정해져 있어어한다.

    enum PostinCenter
    {
        NON, CENTER,LEFT,
    }

    enum UIState
    {
        NON,
        MINI,
        EXPAND,
    }

    enum FormationShap
    {
        NON,
        ARC,
        LINE,
    }

   PostinCenter ePos = PostinCenter.CENTER;

   public   GameObject            card;
   public   GameCardSlot[]        cardsLength;
           //GameCardSlot[]       hascardsLength;
   public   IList<GameCardSlot>   lscardsLength = new List<GameCardSlot>(); //cardInfo를 가지고 있는 slot

   public   float            width;  //카드의 너비
   public   float            height; //카드의 높이
   public   int              cards;  //카드의 숫자
            float            maxRimited = 1080f; //전체 크기 

   public   float            centerX;
   public   float            centerY;
            float            fade = 60f;

            int              _slotCardNum = 0;

    private void Awake()
    {
         cardsLength = gameObject.transform.GetComponentsInChildren<GameCardSlot>();
        _slotCardNum = cardsLength.Length;
    }

    private void Start()
    {
        //시작시 모든 게임슬롯을 null로해서 아이콘 이미지를 비활성화 시킨다.
        //FormationCards();
        //FormationCardsArc();
        //TODO: 이벤트 메니저 확장을 위해서 설정함 만약 쓰지 않을 경우 삭제
        EventManager.Instance.AddListener(EVENT_TYPE.SLOTNUMBER, OnEvent);
    }

    //계산식 전체 크기를 벗어나지 않는 메소드 
   public  void FormationCards(eGameCardSize eState)
    {
        int num = CardFindsNum(); //cardInfo가 null이 아닌 숫자
        //카드의 숫자를 넘겨 받습니다.
        float curWidth = maxRimited - fade - width; 
        //value 는 길이 
        float widthEa  = curWidth / num;   
        //value 는 길이 
        float endPosition =  (0.5f * maxRimited) - (width * 0.5f) - (fade * 0.5f);
        //마지막 카드의 배치좌표
       
        //할당 너비보다 카드가 크다면 겹쳐져야 한다.
        if (widthEa <= width)
        {   //첫카드의 배치좌표
            float startPos = -1*((0.5f * maxRimited) - (width * 0.5f) - (fade * 0.5f)) ;               
            float cha = width - widthEa;

            //전체좌표를 적용해야 된다.
            //하나의 카드의 중심좌표로 배치하는가? 즉 Anchor을 파악해야된다.
            for (int i = 0; i < num; i++)
            {
                //if()
                cardsLength[i].gameObject.transform.localPosition = new Vector3(startPos + (i * widthEa), 0, 0);
                cardsLength[i].CardSetSize(eState);
            }
        }
        else if (widthEa > width) 
        {
            float startPos = 0 - 0.5f * (maxRimited);
            //카드 사이의 공간
            float aSpace = curWidth/(num+1) - num*width /(num+1);

            //공간이 남는다면 공간의 여백을 더 추가 해 줘야 한다.
            for (int i = 0; i < num; i++)
            {
                cardsLength[i].gameObject.transform.localPosition = new Vector3(startPos+aSpace+width+ (i * (width + aSpace)), 0, 0);
                cardsLength[i].CardSetSize(eState);
            }
        }
    }

   
     //EventManager를 통해서 모든UI 요소에 전달하기 위해서 설정
    public int SlotCardNum
    {
        get
        {
            return _slotCardNum;
        }

        set
        {
            //_slotCardNum = value;
            _slotCardNum = Mathf.Clamp(value, 0, 9);
            EventManager.Instance.PostNotification(EVENT_TYPE.SLOTNUMBER, this, _slotCardNum);
        }
    }

   

    //계산식 전체 크기를 벗어나지 않는 메소드 
    public void FormationCardsArc(eGameCardSize eState,eBelong user,eCardType type)
    {    //카드의 숫자를 넘겨 받습니다.
        int num           = CardFindsNum();
        if (num >= 2)
        {

                //value 는 길이 
                float curWidth    = maxRimited - fade - width;
                //value 는 길이 
                float widthEa     = curWidth / num;
                //마지막 카드의 배치좌표
                float endPosition =  0.5f *(maxRimited - width  - fade );
                //카드가 기울어질 최대각도
                float maxRot      = 10;
                //Sin의  한번 반복량
                float rot         = 180/(num-1);
                //기울기 량 
                float angle       = (maxRot*2)/ (num-1);
                //아크의 곡선량*y값의 비율
                float amount     = 70f;      
     

                //할당 너비보다 카드가 크다면 겹쳐져야 한다.
                if (widthEa <= width)
                {
                    //첫카드의 배치좌표  410 //전체 보정값이 필요함  (endPos - lastCard)/2 
                    float startPos    = -1 * (  0.5f *(maxRimited - width  - fade) );
                    float cha         = width - widthEa;
                    float lastCardPos = startPos + ((num-1) * widthEa);
                    float startFull   = startPos  + ( endPosition - lastCardPos)/2;

                    //전체좌표를 적용해야 된다.
                    //하나의 카드의 중심좌표로 배치하는가? 즉 Anchor을 파악해야된다.
                    for (int i = 0; i < num; i++)
                    {
                        float curAng = i * rot;
                        if (curAng > 90)
                        {
                            curAng = 180 - (i * rot);
                        }
              
                        //lscardsLength[i].gameObject.transform.localPosition = new Vector3(startFull + (i * widthEa), amount * Mathf.Sin(curAng * Mathf.Deg2Rad), 0);
                        Vector3 targetPos = new Vector3(startFull + (i * widthEa), amount * Mathf.Sin(curAng * Mathf.Deg2Rad), 0);
                        float curAngle = maxRot - i * angle; //30 ~ -30
                        lscardsLength[i].gameObject.transform.localRotation = Quaternion.Euler(0, 0, curAngle);
                        lscardsLength[i].CardSetSize(eState);
                        lscardsLength[i].eBelongState = eBelong.PLAYER;
                        lscardsLength[i].eType        = eCardType.SLOT;

                    iTween.MoveTo(lscardsLength[i].gameObject, iTween.Hash("islocal", true,
                                                             "position", targetPos,
                                                              "oncompletetarget", lscardsLength[i].gameObject,
                                                              "time", 0.5f,
                                                              "easetype", "easeOutQuart")
                                                            );
                    }
                }
                else if (widthEa > width)
                {   //공간이 많이 남을때
                    float startPos = 0 - 0.5f * (maxRimited);
                    //카드 사이의 공간
                    float aSpace = curWidth / (num + 1) - num * width / (num + 1);
                    //공간이 남는다면 공간의 여백을 더 추가 해 줘야 한다.
                    for (int i = 0; i < num; i++)
                    {
                        lscardsLength[i].gameObject.transform.localPosition = new Vector3(startPos + aSpace + width + (i * (width + aSpace)), 0, 0);
                        lscardsLength[i].CardSetSize(eState);
                        lscardsLength[i].eBelongState = eBelong.PLAYER;
                        lscardsLength[i].eType        = eCardType.SLOT;

                        Vector3 targetPos        = new Vector3(startPos + aSpace + width + (i * (width + aSpace)), 0, 0);
                        iTween.MoveTo(lscardsLength[i].gameObject, iTween.Hash("islocal", true,
                                                            "position", targetPos,
                                                             "oncompletetarget", lscardsLength[i].gameObject,
                                                             "time", 0.5f,
                                                             "easetype", "easeOutQuart")
                                                           );
                    }
                }
        }

    }//Method END

    //계산식 전체 크기를 벗어나지 않는 메소드 
    public void FormationCardsArcCom(eGameCardSize eState,eBelong user ,eCardType type)
    {   //카드의 숫자를 넘겨 받습니다.
        int num = CardFindsNumCom();

        if (num > 1)
        {

                //value 는 길이 
                float curWidth = maxRimited - fade - width;
                //value 는 길이 
                float widthEa = curWidth / num;
                //마지막 카드의 배치좌표
                float endPosition = 0.5f * (maxRimited - width - fade);
                //카드가 기울어질 최대각도
                float maxRot = -10;
                //Sin의  한번 반복량
                float rot = 180 / (num - 1);
                //기울기 량 
                float angle = (maxRot * 2) / (num - 1);
                //아크의 곡선량*y값의 비율
                float amount = 70f;


                //할당 너비보다 카드가 크다면 겹쳐져야 한다.
                if (widthEa <= width)
                {
                    //첫카드의 배치좌표  410 //전체 보정값이 필요함  (endPos - lastCard)/2 
                    float startPos = -1 * (0.5f * (maxRimited - width - fade));
                    float cha = width - widthEa;
                    float lastCardPos = startPos + ((num - 1) * widthEa);
                    float startFull = startPos + (endPosition - lastCardPos) / 2;

                    //전체좌표를 적용해야 된다.
                    //하나의 카드의 중심좌표로 배치하는가? 즉 Anchor을 파악해야된다.
                    for (int i = 0; i < num; i++)
                    {
                        float curAng = i * rot;
                        //반대편 
                        if (curAng > 90)
                        {
                            curAng = 180 - (i * rot);
                        }

                        Vector3 moveTarget  = new Vector3(startFull + (i * widthEa), amount * Mathf.Sin(curAng * Mathf.Deg2Rad) * -1, 0);
                        //cardsLength[i].gameObject.transform.localPosition = new Vector3(startFull + (i * widthEa), amount * Mathf.Sin(curAng * Mathf.Deg2Rad) * -1, 0);


                        float curAngle      = maxRot - i * angle; //-20 ~ 20
                        Quaternion rotation = Quaternion.Euler(0, 0, curAngle);

                        cardsLength[i].gameObject.transform.localRotation = Quaternion.Euler(0, 0, curAngle);
                        cardsLength[i].CardSetSize(eState);
                        cardsLength[i].eBelongState = eBelong.COM;
                        cardsLength[i].eType        = eCardType.SLOT;


                    //TODO: 출발하는 위치 값을 여기서 설정해 주어야 한다.
                    iTween.MoveTo(cardsLength[i].gameObject, iTween.Hash("islocal", true,
                                                       "position", moveTarget,
                                                        "oncompletetarget", cardsLength[i].gameObject,
                                                        "time", 0.5f,
                                                       "easetype", "easeOutQuart")
                                                      );


                    }
           
                }
                else if (widthEa > width)
                {   //공간이 많이 남을때
                    float startPos = 0 - 0.5f * (maxRimited);
                    //카드 사이의 공간
                    float aSpace = curWidth / (num + 1) - num * width / (num + 1);
                    //공간이 남는다면 공간의 여백을 더 추가 해 줘야 한다.
                    for (int i = 0; i < num; i++)
                    {
                        //cardsLength[i].gameObject.transform.localPosition = new Vector3(startPos + aSpace + width + (i * (width + aSpace)), 0, 0);

                        Vector3 moveTarget = new  Vector3(startPos + aSpace + width + (i * (width + aSpace)), 0, 0);

                        iTween.MoveTo(cardsLength[i].gameObject, iTween.Hash("islocal", true,
                                                      "position", moveTarget,
                                                       "oncompletetarget", cardsLength[i].gameObject,
                                                       "time", 0.5f,
                                                      "easetype", "easeOutQuart")
                                                     );
                    }
                }
        } //IF END
    }

    // 메소드 
    public void FormationCardsGrid(eGameCardSize eState, eBelong user, eCardType type)
    {
        int num = CardFindsNumCom();

        if (num > 1)
        {

        }
    }

    //현재슬롯에 있는 갯수를 판단한다.
    public  int   CardFindsNum()
    {
         
        int Length = 0;
        lscardsLength.Clear();
        //hascardsLength = new List<GameCardSlot>();
        // 리턴되는 값으로 슬롯의 거리를 재조정하게 된다.IsCardInfo
        cardsLength = gameObject.transform.GetComponentsInChildren<GameCardSlot>();

        for (int i = 0; i < cardsLength.Length; i++)
        {
            if (cardsLength[i].cardInfo != null)
            {
                Length++;
                //GameCardSlot temp   = cardsLength[i];
                lscardsLength.Add(cardsLength[i]);
            }
        }
        return Length;
    }


    public int CardFindsNumCom()
    {
        // 리턴되는 값으로 슬롯의 거리를 재조정하게 된다.IsCardInfo
        cardsLength = gameObject.transform.GetComponentsInChildren<GameCardSlot>();
        //Slot의 카드의 수량을 바꿔준다.
        SlotCardNum = cardsLength.Length;
        return cardsLength.Length;
    }

    public GameCardSlot[] CardReNumCom()
    {
        cardsLength = gameObject.transform.GetComponentsInChildren<GameCardSlot>();
        return cardsLength;
    }

    //하위의 모든 슬롯을 비활성화 해서 아이콘 이미지가 보이지 않게 한다.
    void AllSlotFalse()
    {
        cardsLength = gameObject.transform.GetComponentsInChildren<GameCardSlot>();
        for (int i = 0; i < cardsLength.Length; i++)
        {
            cardsLength[i].SetCardInfo(null,eCardType.SLOT);
        }
    }

    //한줄에 배열 가능한 카드의 숫자를 구한다.
    int LimitedCarNum()
    {
        int enableCard = 0;
        int num = CardFindsNum();
        float curWidth = maxRimited - fade;
        float widthEa = curWidth / num;
        float endPosition = (0.5f * maxRimited) - (width * 0.5f) - (fade * 0.5f);

        for (int i = 0; i < num; i++)
        {
            float startPos = -1 * ((0.5f * maxRimited) - (width * 0.5f) - (fade * 0.5f));
            if ((startPos + (i * widthEa)) < endPosition)
            {
                enableCard++;
            }
        }

        return enableCard;
    }

    #region 확장이벤트메니저
    ///--------------TEMP----------확장 이벤트 메니저 -------------------------------------------------------
    ///

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        //Detect event type
        switch (Event_Type)
        {
            case EVENT_TYPE.SLOTNUMBER:
                OnSlotChange(Sender, (int)Param);
                break;
        }
    }

    void OnSlotChange(Component Enemy, int NewHealth)
    {
       // if (this.GetInstanceID() != Enemy.GetInstanceID()) return;
       //Debug.Log("Object: " + gameObject.name + " Health is: " + NewHealth.ToString());
    }
    #endregion
    //-----------------------------------------------------------------------------------------------------------
}
