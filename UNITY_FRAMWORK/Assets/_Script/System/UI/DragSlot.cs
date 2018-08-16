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

    public List<GameCardSlot> lstTargetSlot = new List<GameCardSlot>();
    public GameCardSlot  targetSlot = null; 
    public GameCardSlot  pickedSlot = null;


    private new void Awake()
    {
        sInstance = this;
        this.gameObject.SetActive(false);
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
            targetSlot.SlotOn();

            for (int i = 1; i < lstTargetSlot.Count; ++i)
                lstTargetSlot[i].SlotOff(); // 나머지 전부 오프
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
        //TODO: 활성화 시키기 전에 이미지를 바꾸어 줘야 한다.
        //자신의 오브젝트를 활성화 시킨다.
        this.gameObject.SetActive(true);
        //젤위에 그러지도록 맨밑으로 슬롯을 이동시킨다.
        this.transform.SetAsLastSibling();

        //아이템을 집은곳의 슬롯정보를 기억
        pickedSlot = itemSlot;
       // itemSlot.GetUnityInfo
      
        //pick아이템정보를 드래그 슬롯에 세팅
        SetCardInfo(itemSlot.GetUnityInfo(),eCardType.SLOT);
        //아이템을 집은곳의 슬록헤는 빈칸으로 정보셋팅
        itemSlot.SetCardInfo(null,eCardType.SLOT);

      
        // 마우스 위치로 셋팅
        this.transform.position = new Vector3(screenPos.x, screenPos.y, 0);
    }

    public void DragMove(Vector2 screenPos)
    {
        this.transform.position = new Vector3(screenPos.x,screenPos.y,0);
    }

   

    public void Drop()
    {
        if (targetSlot == null)
        {
            pickedSlot.SetCardInfo(GetUnityInfo(),eCardType.SLOT);
           
        }
        else
        {
            // 내려 놓을 칸에 있는 정보를 아이템을 집은 칸에 셋팅한다. (스왑)
            pickedSlot.SetCardInfo(targetSlot.GetUnityInfo(),eCardType.SLOT);
            targetSlot.SetCardInfo(GetUnityInfo(),eCardType.CENTERSLOT);
            //슬롯의 자리배경이 이루어 지면 안된다.
            //targetSlot.CardSetSize(eGameCardSize.EXPAND);
            //targetSlot.SlotOff();
        }

        lstTargetSlot.Clear();
        SetCardInfo(null,eCardType.SLOT);
        pickedSlot = null;
        this.gameObject.SetActive(false);

        //현재 게임상태를 변경하게 한다.이뒤에 새로 슬롯의 위치가 정렬되는 이벤트가 발생해야 한다.
        //CenterFormation.TurnMove();작동시켜야함
        //대상이 싱글톤이 아니다.CenterFormation
        //GameData.Instance.eGamestate = GameState.CARDLOCATION; 복잡한 연결처리 발생

    }


}
