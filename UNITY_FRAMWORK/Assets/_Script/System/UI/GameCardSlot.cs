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

public class GameCardSlot : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
   

    //eGameCardSize eGameCardState = eGameCardSize.BASE;

            Image     slotImage;        // 슬롯 이미지
            Formation formation;
            int       curIndex;         //현재 소속된 인덱스번호
    [HideInInspector]
    public Image      itemIcon;          // 아이템 아이콘(이미지)
                                         // public Image itemIcon;
    private int       rpkNumber;         //가위바위보를 숫자로 출력
    private Text      rpk;               //주먹,가위,보의 문자출력
            string    Sp;                //특별한 장군능력 출력
            bool      isPlayer2;         //player2 (com)의 카드가 드레깅되지 않게 한다.

    [HideInInspector]
    public  Card      cardInfo = null;

    private bool      isPicked = false;  // 픽킹 했는가?

    [HideInInspector]
    public int        ID;                //GetComponent했을때 카드의 정렬순서를 확인을 위해서 임시로 


    public eBelong eBelongState = eBelong.NON;
    //eCardType

    protected  void Awake()
    {
        slotImage = this.GetComponent<Image>();
        itemIcon  = transform.Find("ItemCard").GetComponentInChildren<Image>();
        rpk       = transform.GetComponentInChildren<Text>();
      

        if (gameObject.transform.parent != null)
        {
            formation = gameObject.transform.parent.GetComponent<Formation>();
        }
    }

    void Start()
    {
        
        SetCardInfo(null, eCardType.CENTERSLOT);
    }

    ////슬롯에 오브젝트 카드를 위치 시킨다.
    public void SetCardInfo(Card newInfo, eCardType type)
    {
        switch (type)
        {
            case eCardType.SLOT:
                if (newInfo != null)
                {
                    cardInfo = newInfo;
                    itemIcon.sprite.name = cardInfo.IconName;
                    itemIcon.sprite      = SpriteManager.GetSpriteByName("Sprite", itemIcon.sprite.name);
                    itemIcon.enabled     = true;
                    ID                   = cardInfo.ID;
                    Sp                   = newInfo.Spable;

                }
                else
                {
                    cardInfo = null;
                    itemIcon.enabled = false;
                }
                break;
            case eCardType. CENTERSLOT:

                if (newInfo != null)
                {
                    cardInfo = newInfo;
                    itemIcon.sprite.name = cardInfo.IconName;
                    itemIcon.sprite      = SpriteManager.GetSpriteByName("Sprite", itemIcon.sprite.name);
                    itemIcon.enabled     = true;
                    ID                   = cardInfo.ID;
                    Sp                   = newInfo.Spable;
                    gameObject.transform.localScale = new Vector3(2,2,1);
                }
                else
                {
                    cardInfo              = null;
                    itemIcon.enabled      = false;
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
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
    }

    //슬롯을 클릭했을때 
    public void OnPointerDown(PointerEventData eventData)
    {
        if (cardInfo == null) return; //빈칸클릭

       

        if (!isPlayer2)
        {
            isPicked = true;
            formation.FormationCardsArc(eGameCardSize.BASE, eBelong.PLAYER);
         
            AppSound.instance.SE_Card_Down.Play();
            DragSlot.Instance.PickUp(this, eventData.position);
        }
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPicked == false) return;


        AppSound.instance.SE_Card_PickUp.Play();
        DragSlot.Instance.Drop();
        isPicked = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("마우스가 위에 올라 왓네요");
        if (eBelongState == eBelong.PLAYER || eBelongState == eBelong.SYSCOM )
        {
            curIndex = gameObject.transform.GetSiblingIndex();
            gameObject.transform.SetAsLastSibling();
            CardSetSize(eGameCardSize.EXPAND);
        }

        float clampY = this.gameObject.transform.position.y;
        Debug.Log("슬롯이 클릭다운되었습니다.");
        if (clampY > 1300f)
        {    //clampY 1300 컴의 상단위치
            isPlayer2 = true;
        }
        else
        {
            isPlayer2 = false;
        }

        // throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("마우스가 빠져 나갔습니다.");
        if (eBelongState == eBelong.PLAYER || eBelongState == eBelong.SYSCOM)
        {
            gameObject.transform.SetSiblingIndex(curIndex);
            CardSetSize(eGameCardSize.BASE);
        }
        // throw new System.NotImplementedException();
    }
     
   
    public  void  CardSetSize (eGameCardSize eState)
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
                gameObject.transform.localScale = new Vector3(2, 2, 1);
                break;
        }
    }
       

    

    public void SlotOff()
    {
        //원래의 슬롯색으로 바꾸어 놓는다.
        formation.FormationCardsArc(eGameCardSize.BASE,eBelong.PLAYER);
    }

    public void SlotOn()
    {
        //카드가 빠졌을때 다시 재정렬한다.
        if (gameObject.transform.parent != null)
        {
           // formation.FormationCardsArc();
        }
    }

    //버큰이 눌릴때마다 가위바위보 현재 상태를 보여준다.
    public void BTN_RPK()
    {

        RpkNumber++;
        if (RpkNumber >= 4)
        {
            RpkNumber = 0;
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
                        rpk.text = string.Format("{0}", "주먹");
                       
                        break;

                    case 2:
                        rpk.text = string.Format("{0}", "가위");
                        
                        break;

                    case 3:
                        rpk.text = string.Format("{0}", " 보");
                        
                        break;
                }
            }
          
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

 
}
