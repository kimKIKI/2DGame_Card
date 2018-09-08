using UnityEngine;
using UnityEngine.UI;
using System.Collections;




public class ItemDisplayer : MonoBehaviour {

	// Reference to the Text component displaying the item collected quantity
	[Tooltip("Reference to the Text component displaying the item collected quantity")]
	public Text _itemDisplay;

    // 현재 아이템의 양
   
    public delegate void GoldCallBack();
    public static event GoldCallBack goldEvent;
    

    int curhasGold;       
    public static ItemDisplayer instance_ItemDisplayer;

    public int CurhasGold
    {
        get { return curhasGold; }
        set
        {
            //플레이어의 골드가 바뀔때 마다 체크한다.
            curhasGold = Mathf.Clamp(value, 0, curhasGold);
            EventManager.Instance.PostNotification(EVENT_TYPE.GOLD_CHANGE, this, curhasGold);
           
        }
    }



    private void Start()
    {
        instance_ItemDisplayer = this;
        curhasGold = GameData.Instance.players[1].coin;
        EventManager.Instance.AddListener(EVENT_TYPE.GOLD_CHANGE, OnEvent);
        init();
    }

    private void init()
    {
        curhasGold = GameData.Instance.players[1].coin;
        _itemDisplay.text = curhasGold.ToString();
    }

    // 추가될 아이템의 양
    public void AddItem(int quantity)
    {
        curhasGold += quantity;
		_itemDisplay.text = curhasGold.ToString();
        GameData.Instance.players[1].coin = curhasGold;

        Item_Anim(quantity);
    }

    public void Item_Anim(int quantity)
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", curhasGold, "to", curhasGold - quantity, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
        curhasGold -= quantity;
    }

    //-----TODO:개별적으로 작동하게 함
    void UpdateGoldDisplay(int curGold)
    {
        _itemDisplay.text = curhasGold.ToString();

      
    }


    public void decreaseItem(int quantity)
    {
        curhasGold -= quantity;
        _itemDisplay.text = curhasGold.ToString();
        GameData.Instance.players[1].coin = curhasGold;
      
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.GOLD_CHANGE:
                OnGoldChange(Sender, (int)Param);
                break;
        }
    }

    void OnGoldChange(Component purchaseCard, int gold)
    {
        if (this.GetInstanceID() != purchaseCard.GetInstanceID()) return;

        //골드 금액이 달라질때마다 호출을 한다.
         if(goldEvent != null)
            goldEvent();

    }

   
}
