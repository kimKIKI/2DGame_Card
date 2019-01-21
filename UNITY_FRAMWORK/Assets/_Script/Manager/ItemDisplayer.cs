using UnityEngine;
using UnityEngine.UI;
using System.Collections;


 
public class ItemDisplayer : MonoBehaviour {

	// Reference to the Text component displaying the item collected quantity
	[Tooltip("Reference to the Text component displaying the item collected quantity")]
	public Text _itemDisplay;
    int playerIndex = GameData.Instance.curPlayerIndex;

    // 현재 아이템의 양
    //재사용가능한 스크립트가 될수있도록 코드수정
    public delegate void    GoldCallBack();
    public static   event   GoldCallBack     goldEvent;

    
    //외부에서 접근하면 바뀔수 있는value
    int curhasGold;       
    public static ItemDisplayer instance_ItemDisplayer;

    public int CurhasGold
    {
        get { return curhasGold; }
        set
        { 
            //플레이어의 골드가 바뀔때 마다 체크한다.
            curhasGold = Mathf.Clamp(value, 0, curhasGold);
        }
    }



    private void Start()
    {
         instance_ItemDisplayer = this;
         init();
    }

    private void init()
    {
        curhasGold        = GameData.Instance.players[playerIndex].coin;
        _itemDisplay.text = curhasGold.ToString();
    }

    // 추가될 아이템의 양 증가되는양
    public void AddItem(int quantity)
    {
                curhasGold += quantity;
                _itemDisplay.text = curhasGold.ToString();
                GameData.Instance.players[playerIndex].coin = curhasGold;
                Item_Anim(quantity);
    }

    public void Item_Anim(int quantity)
    {
               iTween.ValueTo(gameObject, iTween.Hash("from", curhasGold, "to", curhasGold - quantity, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
               curhasGold += quantity;
    }

    //-----TODO:개별적으로 작동하게 함
    void UpdateGoldDisplay(int curGold)
    {
               _itemDisplay.text = curhasGold.ToString();
              GameData.Instance.players[playerIndex].coin = curhasGold;
    }


    public void decreaseItem(int quantity)
    {
            curhasGold -= quantity;
            _itemDisplay.text = curhasGold.ToString();
            GameData.Instance.players[playerIndex].coin = curhasGold;
    }

}
