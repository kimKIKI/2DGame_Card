using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class DragSlot : GameCardSlot,IComparer<GameCardSlot>
 {
    private static DragSlot sInstance;

    public static DragSlot Instance
    {
        get
        {
            return sInstance;
        }
    }
    public delegate void degClick();
    public static event degClick eveClick;
    public List<GameCardSlot> lstTargetSlot = new List<GameCardSlot>();
    public GameCardSlot  targetSlot         = null; 
    public GameCardSlot  pickedSlot         = null;
           //슬롯이 꽉차서 이동하지 못할경우 원래위치로 복원
    GameCardSlot  baseSlot                  = null;
   
    Camera mainCameraA;        //캔버스의 스크린좌표를 월드 좌표로 구하기 위해서 사용함
    bool IsDragging = false;
    private new void Awake()
    {
        sInstance = this;
        this.gameObject.SetActive(false);
        mainCameraA = GameObject.Find("MainCamera").GetComponent<Camera>();
        base.Awake();   
    }

    //부모의 스타트 함수 호출방지
    private void Start() { }

    private void Update()
    {
        if (lstTargetSlot.Count > 0)
        {
         
            lstTargetSlot.Sort(this);
            targetSlot = lstTargetSlot[0];
          
            //클릭 됐을때의 동작 gameCardSlot
            //targetSlot.SlotOn();

            //TODO:여기 이거 뭔지 모르겠음
            //for (int i = 1; i < lstTargetSlot.Count; ++i)
            //   lstTargetSlot[i].SlotOff(); // 나머지 전부 오프
        }
        else
        {
            targetSlot = null;
        }
    }


    public int Compare(GameCardSlot x, GameCardSlot y)
    {
        // 슬롯 x 와 드래그 슬롯의 거리
        float distX = Vector3.Distance(x.transform.position, this.transform.position);
        // 슬롯 y 와 드래그 슬롯의 거리
        float distY = Vector3.Distance(y.transform.position, this.transform.position);

        if (distX < distY)
            return -1;  // x가 더 가까운 경우
        else if (distX > distY)
            return 1;   // y가 더 가까운 경우

        return 0;       // 같은 경우
    }

    //드레그 박스에 충돌시 리스트에 담는다.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameCardSlot itemSlot = collision.GetComponent<GameCardSlot>();
      
        if (itemSlot != null)
            lstTargetSlot.Add(itemSlot);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameCardSlot itemSlot = collision.GetComponent<GameCardSlot>();
        if (itemSlot != null)
        {
            lstTargetSlot.Remove(itemSlot);
            //원래의 슬롯 색으로 바뀐다.
            //TODO: 드레그가 끝났을때 
        }
    }

   
    public void PickUp(GameCardSlot itemSlot, Vector2 screenPos)
    {
        eveClick();
        //빈슬롯을 받는 이벤트 발동 GameManager에 빈슬롯을 찾아서 GameData저장하도록한다.
        //자신의 오브젝트를 활성화 시킨다.
        this.gameObject.SetActive(true);
        //젤위에 그러지도록 맨밑으로 슬롯을 이동시킨다.
        this.transform.SetAsLastSibling();
        //아이템을 집은곳의 슬롯정보를 기억
        pickedSlot = itemSlot;
        baseSlot   = itemSlot;
        //itemSlot.GetUnityInfo

        //pick아이템정보를 드래그 슬롯에 세팅
        SetCardInfo(itemSlot.GetUnityInfo(), eCardType.SLOT);
            //아이템을 집은곳의 슬록에는 빈칸으로 정보셋팅
            itemSlot.SetCardInfo(null);
            // 마우스 위치로 셋팅 100 playerDistance
            //this.transform.position = new Vector3(screenPos.x, screenPos.y,0);
     
        var pos            = new Vector3(screenPos.x, screenPos.y, 100);
        transform.position = mainCameraA.ScreenToWorldPoint(pos);
    }

  
    public void DragMove(Vector2 screenPos)
    {
        //this.transform.position = new Vector3(screenPos.x,screenPos.y,0);
        var pos = new Vector3(screenPos.x, screenPos.y, 100);
        transform.position = mainCameraA.ScreenToWorldPoint(pos);
        IsDragging = true;
    }


    public void Drop()
    {

        //바꾸기 위해 이동한 지점의 슬롯 아무것도 없을때
        //드레깅 trigger에 아무것도 잡히지 않을때
        if (targetSlot == null )
        {   //null이 되면 안되는 부분
            //플레이어의슬롯이꽉차 있지 안을때
            if (!GameData.Instance.bolPlayerBlankAll)
            {

                Vector3 world = transform.position;
                transform.position = world;
                transform.localScale = new Vector3(1, 1, 1);
                Vector3 to = GameData.Instance.DrageCardInfoVector3;

                iTween.MoveTo(gameObject, iTween.Hash("islocal", false,
                                      "position", to,
                                      "oncomplete", "PlayerCenterSwitch",
                                      "oncompletetarget", gameObject,
                                      "time", 0.3f,
                                      "easetype", "easeOutQuart")
                                       );



            }
            else
            {

                baseSlot.SetCardInfo(GetUnityInfo(), eCardType.SLOT);
                gameObject.SetActive(false);

            }
            IsDragging = false;
        }
        else
        {   //드레깅중이고 슬롯이 모두 차있지 않은 상태이고
            if (!GameData.Instance.bolPlayerBlankAll)
            {        
                    // 내려 놓을 칸에 있는 정보를 아이템을 집은 칸에 셋팅한다. (스왑)
                    if (pickedSlot.eType == eCardType.SLOT )
                    {   //슬롯에 이미 데이터가 있을때 판단이 없음
                        //타겟이 있을때 center이면

                        if (targetSlot.eType == eCardType.CENTERSLOT)
                        {  // 1 --->>>놓을때 타겟이 있을때 타입이 센터라면--------------------
                            if (targetSlot.cardInfo == null)
                            {
                                Debug.Log("Cardinfo가 없습니다.");
                                targetSlot.SetCardInfo(GetUnityInfo(), eCardType.CENTERSLOT);
                                targetSlot.EFFECT();

                                lstTargetSlot.Clear();
                                SetCardInfo(null);
                                pickedSlot = null;
                                this.gameObject.SetActive(false);
                            }
                            else
                            {
                            Debug.Log("Cardinfo가 있습니다.");
                        
                           
                            baseSlot.SetCardInfo(GetUnityInfo(),eCardType.SLOT);
                            targetSlot.EFFECT();
                            lstTargetSlot.Clear();
                            SetCardInfo(null);
                            pickedSlot = null;
                            this.gameObject.SetActive(false);
                            }
                             
                           
                        }
                        else if (targetSlot.eType == eCardType.SLOT)
                        {   ////2---->제자리에서 드레그해서 제자리에 놓았을 경우------------------

                                Vector3 world        = transform.position;
                                Card pickupCardInfo  = GetUnityInfo();
                                transform.position   = world;
                                transform.localScale = new Vector3(1, 1, 1);
                                Vector3 to           = GameData.Instance.DrageCardInfoVector3;

                                iTween.MoveTo(gameObject, iTween.Hash("islocal", false,
                                                      "position", to,
                                                      "oncomplete", "PlayerCenterSwitch",
                                                      "oncompletetarget", gameObject,
                                                      "time", 0.3f,
                                                      "easetype", "easeOutQuart")
                                                       );
                           

                        }

                    }
              
              
            } //bolPlayerBlankAll
              //TODO 슬롯이 꽉차 있을때 원래 위치로 복원
            else
            {
                baseSlot.SetCardInfo(GetUnityInfo(), eCardType.SLOT);
                gameObject.SetActive(false);
            }

            IsDragging = false;
        }//else END
    }


    public void PlayerCenterSwitch()
    {
       
        if (targetSlot.eType == eCardType.CENTERSLOT)
        {
           targetSlot.SetCardInfo(GetUnityInfo(), eCardType.CENTERSLOT);
            //player의 센터가 자꾸작아져서 확인겸 추가 한 코드 문제 해결되지 않으면
            //이부분 삭제해도 무방함
           if( targetSlot.eBelongState != eBelong.PLAYER)
                targetSlot.eBelongState = eBelong.PLAYER;
           // Debug.Log("스위치 작동함..targetSlot .setCardInfo");

           targetSlot.EFFECT();
            //TODO:카드의 특성정보를 읽어서 player타워에 적용--------------
            targetSlot.AddHpSender();
            //----------------------------------------------------------
            lstTargetSlot.Clear();
            SetCardInfo(null);
            pickedSlot = null;
            this.gameObject.SetActive(false);
          
        }

    }




}
