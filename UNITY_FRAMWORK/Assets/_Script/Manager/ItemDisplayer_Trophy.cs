using UnityEngine;
using UnityEngine.UI;
using System.Collections;


 
public class ItemDisplayer_Trophy : MonoBehaviour {

	// Reference to the Text component displaying the item collected quantity
	[Tooltip("Reference to the Text component displaying the item collected quantity")]
	public Text _itemDisplay;
    int playerIndex = GameData.Instance.curPlayerIndex;

    // 현재 아이템의 양
    //재사용가능한 스크립트가 될수있도록 코드수정
   
    public delegate void TrophyCallBack();
    public static event TrophyCallBack trophyEvent;

    //외부에서 접근하면 바뀔수 있는value
    int curhasTrophy;       
    public static ItemDisplayer_Trophy  instance_ItemDisplayerTrophy;

    public int CurhasGold
    {
        get { return curhasTrophy; }
        set
        { 
            //플레이어의 골드가 바뀔때 마다 체크한다.
            curhasTrophy = Mathf.Clamp(value, 0, curhasTrophy);
        }
    }

    private void Start()
    {
        instance_ItemDisplayerTrophy = this;
        init();
    }

    private void init()
    {
        curhasTrophy        = GameData.Instance.players[playerIndex].trophy;
        _itemDisplay.text   = curhasTrophy.ToString();
    }

    // 추가될 아이템의 양 증가되는양
    public void AddItem(int quantity)
    {
                curhasTrophy += quantity;
                _itemDisplay.text = curhasTrophy.ToString();
                GameData.Instance.players[playerIndex].trophy = curhasTrophy;
                Item_Anim(quantity);
    }

    public void Item_Anim(int quantity)
    {
               iTween.ValueTo(gameObject, iTween.Hash("from", curhasTrophy, "to", curhasTrophy - quantity, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
               curhasTrophy += quantity;
    }

    //-----TODO:개별적으로 작동하게 함
    void UpdateGoldDisplay(int curGold)
    {
               _itemDisplay.text = curhasTrophy.ToString();
              GameData.Instance.players[playerIndex].trophy = curhasTrophy;
    }


    public void decreaseItem(int quantity)
    {
            curhasTrophy -= quantity;
            _itemDisplay.text = curhasTrophy.ToString();
            GameData.Instance.players[playerIndex].trophy = curhasTrophy;
    }

}
