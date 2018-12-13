using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum eGameCardSize
{
    NON,
    BASE,
    MINI,
    EXPAND,
}

public enum eBelong
{
   NON,
   SYSNON,  
   SYSCOM,  
   COM,     
   PLAYER,
   
}

public enum eCardType
{
  NON,
  CENTERSLOT,
  SLOT,
}

public class GameCardSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void CardSenderDelegate(GameCardSlot gs);
    public static CardSenderDelegate CardSenderProperty;
 

    Image slotImage;         // 슬롯 이미지
    Formation formation;
    int curIndex;          //현재 소속된 인덱스번호
    [HideInInspector]
    public Image itemIcon;          // 아이템 아이콘(이미지)
                                    // public Image itemIcon;
    public Text txPlus;
    private int rpkNumber;         //가위바위보를 숫자로 출력
    private Text rpk;               //주먹,가위,보의 문자출력
    string Sp;                //특별한 장군능력 출력

    [HideInInspector]
    public int ID;                //GetComponent했을때 카드의 정렬순서를 확인을 위해서 임시로 


    public Transform explainCard;        //카드의 기능을 보여줄 이미지

    public IList<Card> defectCards = new List<Card>();
    public eBelong eBelongState = eBelong.NON;   //com & player인지 판단한다.
    public eCardType eType = eCardType.NON; //센터에 있는지 위치판단

    Text txExplainCard;
    public int plus;
    //----------------내부---------------------------------
    public Card cardInfo = null;
    public int curLevelHp;                   //레벨을 체력으로 적용함..레벨이 놀을수록 체력증가
    public Transform Cardbackground = null;  //카드의 뒤배경
    Image cardBack;                 //배열되는 카드의 뒤배경
    public Transform RPKImg;                //가위바위보 이미지
    Image rpkImg;
    private bool isPicked = false;  // 픽킹 했는가?

    bool isPlayer2;         //player2  y값으로 반쪽위를 com의 영역으로 분리하기위해 사용
    bool isPlayer;          //Player인지 판단한다.
    bool IsOne = true;      //한번만 실행되도록 판단하기 위해서 설정

    Camera mainCamera;

    protected void Awake()
    {
        slotImage = this.GetComponent<Image>();
        itemIcon = transform.Find("ItemCard").GetComponentInChildren<Image>();
      
          
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();

        if (gameObject.transform.parent != null)
        {
            formation = gameObject.transform.parent.GetComponent<Formation>();
        }

        if (Cardbackground != null)
        {
            cardBack = Cardbackground.gameObject.GetComponent<Image>();
        }

        if (RPKImg != null)
        {
            rpkImg = RPKImg.gameObject.GetComponent<Image>();
            rpk = transform.Find("Image/Text_RPS").GetComponent<Text>();
            rpk.text = null;
        }
    }

    void Start()
    {
        SetCardInfo(null);
        //collider를 통해서 얻어와야 얻어짐...ㅠ.ㅠ
        var other = gameObject.GetComponent<Collider2D>().tag;
        if (other == "Player")
        {
            eBelongState = eBelong.PLAYER;
            eType = eCardType.CENTERSLOT;
        }
    }

    void OnEnable()
    {
        //if (explainCard.gameObject == null)
        //{
        //    Debug.Log(" ++++++++++++++++++++++++++");
        //}
        //else
        //{
        //    txExplainCard = explainCard.gameObject.GetComponentInChildren<Text>();
        //    Debug.Log("==================================");
        //}

        //상속받은 DragSlot에서는 explainCard가 존재하지 안기 때문에 null Exception발생함

        //Debug.Log("gameCardSlot : OnEnable");
        if (explainCard ?? false)
        {

            txExplainCard = explainCard.gameObject.GetComponentInChildren<Text>();
            explainCard.gameObject.SetActive(false);
        }


    }



    public void PlusHp()
    {
        if (txPlus != null)
        {
            txPlus.text = string.Format("+{0}", plus);
            iTween.ScaleFrom(txPlus.gameObject, iTween.Hash("x", 2, "y", 2, iTween.EaseType.easeInOutBounce, .3f));
            StartCoroutine(reHide(2f));
        }
      
    }

    IEnumerator reHide(float t)
    {
        yield return new WaitForSeconds(t);
        txPlus.text = null;
    }

    public void AddHpSender()
    {
        if (CardSenderProperty != null)
            CardSenderProperty(this);
    }


    public void SetCardInfo(Card newInfo)
    {
        //Debug.Log("SetCardInfo(Card newInfo)");
        if (newInfo != null)
        {
            cardInfo = newInfo;
            string name = cardInfo.IconName;
            if (itemIcon == null)
            {
                itemIcon = transform.Find("ItemCard").GetComponentInChildren<Image>();
            }
            itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", name);
            itemIcon.enabled = true;
            ID = cardInfo.ID;
            Sp = newInfo.Spable;
            curLevelHp = newInfo.level;
            gameObject.transform.localScale = new Vector3(2, 2, 1);
            cardBack.enabled = true;


        }
        else
        {
            cardInfo = null;
            itemIcon.enabled = false;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            if (cardBack != null)
            {
                cardBack.enabled = false;
            }
        }

    }

    ////슬롯에 오브젝트 카드를 위치 시킨다.
    public void SetCardInfo(Card newInfo, eCardType type)
    {
        //Debug.Log("--------SetCardInfo(Card newInfo, eCardType type)----------");
        switch (type)
        {
            case eCardType.SLOT:
                eType = type;
                if (newInfo != null)
                {
                    cardInfo = newInfo;
                    string name = cardInfo.IconName;
                    if (itemIcon == null)
                    {
                        itemIcon = transform.Find("ItemCard").GetComponentInChildren<Image>();
                    }
                    itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", name);
                    itemIcon.enabled = true;
                    ID = cardInfo.ID;
                    Sp = newInfo.Spable;
                    curLevelHp = newInfo.level;
                    Debug.Log("setCardInfo  ID : " + ID);

                    //처음 생성될때는 적용되지 안고 있음
                    if (cardBack != null)
                    {
                        cardBack.enabled = true;
                    }

                }
                else
                {
                    cardInfo = null;
                    itemIcon.enabled = false;
                    if (cardBack != null)
                    {
                        cardBack.enabled = false;
                    }
                }
                break;
            case eCardType.CENTERSLOT:
                eType = type;
                if (newInfo != null)
                {
                    cardInfo = newInfo;
                    string name = cardInfo.IconName;
                    if (itemIcon == null)
                    {
                        itemIcon = transform.Find("ItemCard").GetComponentInChildren<Image>();
                    }
                    itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", name);
                    itemIcon.enabled = true;
                    ID = cardInfo.ID;
                    Sp = newInfo.Spable;
                  
                    plus = GameData.Instance.UnityDatas[ID - 1].Up_Hp;
                    PlusHp();
                    curLevelHp = newInfo.level;
                    gameObject.transform.localScale = new Vector3(2, 2, 1);

                    //"Atk_Zone" 적 공격력 "Up_Hp"성벽Hp증가량 Up_atk":공격증가량,
                    //"SpAble":"non" 특수능력
                    if (explainCard ?? false)
                    {
                        if (ID >= 1)
                        {
                            //Debug.Log(" GameCardSlot ----ID :" + ID);
                            string iconName = GameData.Instance.UnityDatas[ID - 1].Name;
                            string Spable = GameData.Instance.UnityDatas[ID - 1].SpAble;      //특수 능력
                            int AtK_zone = GameData.Instance.UnityDatas[ID - 1].Atk_Zone;     //성곽공격
                            int Up_Hp = GameData.Instance.UnityDatas[ID - 1].Up_Hp;
                            int Up_atk = GameData.Instance.UnityDatas[ID - 1].Up_atk;
                            txExplainCard.text = string.Format("\n\n{0}\n {1}\n 기지공격력:{2}\n 체력증가:{3}\n 공력증가:{4}\n", iconName, Spable, AtK_zone, Up_Hp, Up_atk);
                        }

                    }

                }
                else
                {
                    cardInfo = null;
                    itemIcon.enabled = false;
                    gameObject.transform.localScale = new Vector3(1, 1, 1);

                }
                break;
        }
    }

    //point in일때 카드가 확대해서 보여지게 한다.
    void InvExplainCard()
    {
        //com에 마우스 포인터가 왔을때 확대
        if (eBelongState == eBelong.SYSCOM)
        {
            //TODO: 카드배열에서 선택된 카드가 맨위에 올라오게 하기 위해서 SetAsLastSbling
            if (eType == eCardType.SLOT)
            {
                curIndex = gameObject.transform.GetSiblingIndex();
                gameObject.transform.SetAsLastSibling();
                CardSetSize(eGameCardSize.EXPAND);

            }

            if (eType == eCardType.CENTERSLOT)
            {
                CardSetSizeCenter(eGameCardSize.EXPAND);

            }
            else
            {
                CardSetSize(eGameCardSize.EXPAND);
            }
        }

        //Player의 카드베이스의 확대-----------------ERROR부분------------------------
        if (eBelongState == eBelong.PLAYER)
        {
            if (eType == eCardType.SLOT)
            {
                curIndex = gameObject.transform.GetSiblingIndex();
                gameObject.transform.SetAsLastSibling();
                //카드의 이미지의 크기를 2배로 확대한다.
                CardSlotPlayerImgBig(true);
                isPlayer = true;
                explainCard.gameObject.SetActive(true);

            }
            else if (eType == eCardType.CENTERSLOT)
            {
                CardSlotPlayerImgBig(true);
                isPlayer = true;
            }
        }
    }

    ////슬롯에 오브젝트 카드를 위치 시킨다.
    public void SetCardInfo(Card newInfo, eBelong man, eCardType type)
    {
        eBelongState = man;
        eType = type;

        switch (type)
        {
            case eCardType.SLOT:

                if (newInfo != null)
                {
                    cardInfo = newInfo;

                    if (itemIcon == null)
                    {
                        itemIcon = transform.Find("ItemCard").GetComponentInChildren<Image>();
                    }

                    string name = cardInfo.IconName;
                    itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", name);
                    itemIcon.enabled = true;
                    ID = cardInfo.ID;

                    Sp = newInfo.Spable;
                    curLevelHp = newInfo.level;
                    //null이 아닐때 gameobject null이면 false
                    if (explainCard ?? false)
                    {
                        if (ID >= 1)
                        {
                            string iconName = GameData.Instance.UnityDatas[ID - 1].Name;
                            string Spable = GameData.Instance.UnityDatas[ID - 1].SpAble;      //특수 능력
                            int AtK_zone = GameData.Instance.UnityDatas[ID - 1].Atk_Zone;     //성곽공격
                            int Up_Hp = GameData.Instance.UnityDatas[ID - 1].Up_Hp;
                            int Up_atk = GameData.Instance.UnityDatas[ID - 1].Up_atk;
                            txExplainCard.text = string.Format("\n\n{0}\n {1}\n 기지공격력:{2}\n 체력증가:{3}\n 공력증가:{4}\n", iconName, Spable, AtK_zone, Up_Hp, Up_atk);
                        }

                    }


                    if (cardBack != null)
                    {
                        cardBack.enabled = true;
                    }

                    //ERROR ------->>>원인모름
                    //"Atk_Zone" 적 공격력 "Up_Hp"성벽Hp증가량 Up_atk":공격증가량,
                    //"SpAble":"non" 특수능력

                }
                else
                {
                    cardInfo = null;
                    itemIcon.enabled = false;

                    if (cardBack != null)
                    {
                        cardBack.enabled = false;
                    }

                }
                break;
            case eCardType.CENTERSLOT:

                if (newInfo != null)
                {
                    cardInfo = newInfo;
                    string name = cardInfo.IconName;
                    if (itemIcon == null)
                    {
                        itemIcon = transform.Find("ItemCard").GetComponentInChildren<Image>();
                    }
                    itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", name);
                    itemIcon.enabled = true;
                    ID = cardInfo.ID;
                    Sp = newInfo.Spable;
                    plus = GameData.Instance.UnityDatas[ID - 1].Up_Hp;
                    PlusHp();
                    curLevelHp = newInfo.level;

                    gameObject.transform.localScale = new Vector3(2, 2, 1);

                  
                  
                }
                else
                {
                    cardInfo = null;
                    itemIcon.enabled = false;
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                    if (explainCard ?? false)
                    {

                        string iconName = null;
                        string Spable = null;
                        int AtK_zone = 0;
                        int Up_Hp = 0;
                        int Up_atk = 0;
                        txExplainCard.text = string.Format("\n\n{0}\n {1}\n 기지공격력:{2}\n 체력증가:{3}\n 공력증가:{4}\n", iconName, Spable, AtK_zone, Up_Hp, Up_atk);
                    }
                }
                break;
        }
    }


    public bool ISIconImg()
    {
        if (itemIcon.enabled)
        {
            return true;
        }
        return true;
    }


    public Card GetUnityInfo()
    {
        if (cardInfo == null)
        {
            Debug.Log("Card 정보가 null입니다.");
        }
        return cardInfo;
    }



    public void OnDrag(PointerEventData eventData)
    {
        if (isPicked == false) return;

        DragSlot.Instance.DragMove(eventData.position);

        if (eType == eCardType.CENTERSLOT) return;
    }

    //슬롯을 클릭했을때  //TODO: 중복해서 빠르게 발생하지 안도록 지연시켜야 한다.
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("-------OnPointerDown----");
        if (cardInfo == null) return; //빈칸클릭

        //센터에 있을때 드레깅을 위한 다운이 안되게 한다.
        if (eType == eCardType.CENTERSLOT)
        {
            return;
        }



        //플레이어의 베이스카드가 다시 재정렬되게 한다.
        if (!isPlayer2)
        {
            //임시로 꺼줌
            //formation.FormationCardsArc(eGameCardSize.BASE, eBelong.PLAYER,eCardType.SLOT);
            AppSound.instance.SE_Card_Down.Play();
            if (GameData.Instance.IsOnePickUp && this.eBelongState == eBelong.PLAYER)
            {
                DragSlot.Instance.PickUp(this, eventData.position);
                //게임슬롯별이 아닌 전체 게임슬롯타임의적용문제 
                isPicked = true;
                StartCoroutine(coIsOneDelay(0.5f));
            }
            //TODO: 드레깅 발생시 자신의 Info상태를 GameData에 전달한다.
            //throw exception 적용해 볼것
        }

        if (IsInvoking("InvExplainCard"))
        {
            CancelInvoke("InvExplainCard");
        }
        //커졌다면 원래대로 돌려놓는다.
        if (eBelongState == eBelong.PLAYER)
        {
            CardSlotPlayerImgBig(false);
            //설명함 꺼지게 한다.
            explainCard.gameObject.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("-------OnPointerUp-----");
        if (isPicked == false) return;

        if (isPicked)
        {
            AppSound.instance.SE_Card_PickUp.Play();
            DragSlot.Instance.Drop();
            isPicked = false;
            //드레깅을 false로 돌려 놓는다.
        }


        if (IsInvoking("InvExplainCard"))
        {
            CancelInvoke("InvExplainCard");
        }
        if (eBelongState == eBelong.PLAYER)
            CardSlotPlayerImgBig(false);

        //if (eBelongState == eBelong.PLAYER && eType ==  eCardType.SLOT)
        //           CardSlotPlayerImgBig(false);

    }

    IEnumerator coIsOneDelay(float t)
    {
        GameData.Instance.IsOnePickUp = false;
        yield return new WaitForSeconds(t);
        GameData.Instance.IsOnePickUp = true;
    }
    //주의 OnPointerEnter은 업데이트처럼 계속호출되고 있다.
    //따라서 코루틴을 사용하면 코루틴도 계쏙 호출이 되서 이상한 결과가 나온다.
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("-------OnPointerEnter-----");
        //현재카드의 상태가 슬롯이고 player일때 GameData에 등록한다.
        if (eBelongState == eBelong.PLAYER && eType == eCardType.CENTERSLOT)
        {
            GameData.Instance.DrageCardInfo = cardInfo;
            GameData.Instance.DrageCardInfoVector3 = transform.position;
            explainCard.gameObject.SetActive(true);
        }

        float clampY = this.gameObject.transform.position.y;

        if (clampY > 1300f)
        {    //clampY 1300 컴의 상단위치
            isPlayer2 = true;
        }
        else
        {
            isPlayer2 = false;
        }


        if (IsInvoking("InvExplainCard"))
        {
            CancelInvoke("InvExplainCard");
        }
        Invoke("InvExplainCard", 1f);

        // throw new System.NotImplementedException();
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("-------OnPointerExit-----");
        if (IsInvoking("InvExplainCard"))
        {
            CancelInvoke("InvExplainCard");
        }
        if (eBelongState == eBelong.PLAYER)
            CardSlotPlayerImgBig(false);

        if (eBelongState == eBelong.SYSCOM)
        {
            if (eType == eCardType.SLOT)
                gameObject.transform.SetSiblingIndex(curIndex);

            if (eType == eCardType.CENTERSLOT)
            {
                CardSetSizeCenter(eGameCardSize.BASE);
            }
            else
            {
                CardSetSize(eGameCardSize.BASE);
            }
        }
        //현재의 게임카드가 플레이어 && 센터슬롯에 있을때 사이즈 조절
        if (eBelongState == eBelong.PLAYER && eType == eCardType.CENTERSLOT)
        {
            transform.Find("ItemCard").GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 120f);
        }

    }

    //슬롯에 있을때 카드가 확대해서 보이게 한다.
    public void CardSlotPlayerImgBig(bool able)
    {
        var min = 1f;
        if (able)
        {
            //이미 커진 상태라면 더이사 커지지 않게 한다.
            if (itemIcon.transform.localScale.x <= 1)
                itemIcon.transform.localScale = new Vector3(1 * min, 1 * min, 1);

        }
        else
        {
            itemIcon.transform.localScale = new Vector3(1, 1, 1);

        }

        explainCard.gameObject.SetActive(false);

    }

    public void CardSetSize(eGameCardSize eState)
    {
        switch (eState)
        {
            case eGameCardSize.BASE:
                gameObject.transform.localScale = new Vector3(1, 1, 1);

                break;
            case eGameCardSize.MINI:

                gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);

                break;
            case eGameCardSize.EXPAND:

                gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
                break;
        }
    }

    public void CardSetSizeCenter(eGameCardSize eState)
    {
        float sm = 0.5f;
        switch (eState)
        {
            case eGameCardSize.BASE:
                gameObject.transform.localScale = new Vector3(1 * sm, 1 * sm, 1);

                break;
            case eGameCardSize.MINI:
                gameObject.transform.localScale = new Vector3(0.5f * sm, 0.5f * sm, 1);

                break;
            case eGameCardSize.EXPAND:
                gameObject.transform.localScale = new Vector3(1.5f * sm, 1.5f * sm, 1);
                break;
        }
    }

    //TODO: ERROR로 수정필요함
    //public void SlotOff()
    //{
    //원래의 슬롯색으로 바꾸어 놓는다.
    // formation.FormationCardsArc(eGameCardSize.BASE,eBelong.PLAYER,eCardType.SLOT);
    //}

    public void EFFECT(eEFFECT_NAME SPAWN)
    {
        var wor = mainCamera.ScreenToWorldPoint(Vector3.zero);
        //TODO:이펙트 발생시키는 애니 실행시킨다.
        GameObject ef = AppUIEffect.instance.InstanceVFX(SPAWN);
        //ERROR : 픽업되는 카드가 부모가 됨..놓는위치가 부모가 되어야 한다.수정
        ef.transform.SetParent(transform);
        ef.transform.localPosition = new Vector3(wor.x, wor.y, wor.z + 10);
    }

    // public void SlotOn()
    //{
    //카드가 빠졌을때 다시 재정렬한다.
    //    if (gameObject.transform.parent != null)
    //    {
    //formation.FormationCardsArc();
    //    }
    //}

    //버큰이 눌릴때마다 가위바위보 현재 상태를 보여준다.
    public void BTN_RPK()
    {
        RpkNumber++;
        if (RpkNumber >= 4)
        {
            RpkNumber = 1;
        }
    }

    public int RpkNumber
    {
        get
        {
            return rpkNumber;
        }

        set
        {
            if (value < 0)
            {
                rpk.gameObject.SetActive(false);
            }
            else if (value > 0)
            {
                rpk.gameObject.SetActive(true);
                switch (value)
                {
                    case 1:
                        rpk.text = string.Format("{0}", "가위");
                        string name1 = "rps_0";
                        rpkImg.sprite = SpriteManager.GetSpriteByName("Sprite", name1);

                        break;

                    case 2:
                        rpk.text = string.Format("{0}", "주먹");
                        string name = "rps_1";
                        rpkImg.sprite = SpriteManager.GetSpriteByName("Sprite", name);
                        break;

                    case 3:
                        rpk.text = string.Format("{0}", " 보");
                        string name3 = "rps_2";
                        rpkImg.sprite = SpriteManager.GetSpriteByName("Sprite", name3);
                        break;
                }
            }
            //Debug.Log("주먹(1), 가위(2),보(3) value :" + value);
            rpkNumber = value;
        }
    }



    public void MoveSlot(Vector3 to)
    {
        iTween.MoveTo(gameObject, iTween.Hash("islocal", false,
                                      "position", to,
                                      "oncompletetarget", gameObject,
                                      "time", 0.3f,
                                      "easetype", "easeOutQuart")
                                       );
    }

    public void CombatMove(Vector3 to)
    {
        iTween.MoveTo(gameObject, iTween.Hash("islocal", false,
                                     "position", to,
                                     "oncompletetarget", gameObject,
                                     "time", 0.3f,
                                     "easetype", "easeOutQuart")
                                      );
    }


    public void DestroyEffect()
    {
        if (mainCamera != null)
        {
            //캔버스에서 0,0이되는 좌표를 구한다.
            var wor = mainCamera.ScreenToWorldPoint(Vector3.zero);
            GameObject ef = AppUIEffect.instance.InstanceVFX(eEFFECT_NAME.Destroy_Card);
            //캔버스내 부모를 기준으로 0,0에 위치 하게 한다.사라지므로 현재의 부모를 부모로 세팅한다.
            ef.transform.SetParent(transform.parent);
            ef.transform.localPosition = new Vector3(wor.x, wor.y, wor.z + 10);
        }
    }

  


}
