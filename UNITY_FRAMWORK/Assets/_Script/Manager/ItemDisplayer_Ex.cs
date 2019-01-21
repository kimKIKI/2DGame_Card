using UnityEngine;
using UnityEngine.UI;
using System.Collections;


 
public class ItemDisplayer_Ex : MonoBehaviour {

	// Reference to the Text component displaying the item collected quantity
	[Tooltip("Reference to the Text component displaying the item collected quantity")]
	public Text _itemDisplay;
    public Text _level;        //update일때 증가 시키기 위해서 설정
    public Text _levelL;       //기존레벨업에 필요한 레벨카운트
    int playerIndex = GameData.Instance.curPlayerIndex;

    // 현재 아이템의 양
    //재사용가능한 스크립트가 될수있도록 코드수정
   
    public delegate void    ExCallBack();  //경험치
    public static   event   ExCallBack ExEvent;

    //외부에서 접근하면 바뀔수 있는value
    int   curhasEx;       
    public static ItemDisplayer_Ex  instance_ItemDisplayerEx;
                   //playerLevel[0]
    int[] playerLevel = { 500, 1000, 2000, 3000, 4000, 6000, 8000, 10000, 20000, 30000, 40000, 50000 };
                       
    public int CurhasGold
    {
        get { return curhasEx; }
        set
        { 
            //플레이어의 골드가 바뀔때 마다 체크한다.
            curhasEx = Mathf.Clamp(value, 0, curhasEx);
          
        }
    }

    private void Start()
    {
        instance_ItemDisplayerEx = this;
        init();
    }

    private void init()
    {
        int curLevel        = GameData.Instance.players[playerIndex].exp;      //현재 플레이어 레벨
        curhasEx            = GameData.Instance.players[playerIndex].expCount;
        _itemDisplay.text   = curhasEx.ToString();
        int LevelCount      = playerLevel[curLevel];
        _levelL.text        = string.Format("/{0}", LevelCount);
    }

    // 추가될 아이템의 양 증가되는양
    public void AddItem(int quantity)
    {
                curhasEx += quantity;
                _itemDisplay.text = curhasEx.ToString();
                GameData.Instance.players[playerIndex].expCount = curhasEx;
                Item_Anim(quantity);
               
    }

    public void Item_Anim(int quantity)
    {
               iTween.ValueTo(gameObject, iTween.Hash("from", curhasEx, "to", curhasEx - quantity, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
               curhasEx += quantity;
               ExCount();
    }

    //-----TODO:개별적으로 작동하게 함
    void UpdateGoldDisplay(int curGold)
    {
              _itemDisplay.text = curhasEx.ToString();
              GameData.Instance.players[playerIndex].expCount = curhasEx;
    }


    public void decreaseItem(int quantity)
    {
            curhasEx -= quantity;
            _itemDisplay.text = curhasEx.ToString();
            GameData.Instance.players[playerIndex].expCount = curhasEx;
    }

    public void ExCount()
    {
        int curLevel       = GameData.Instance.players[1].exp;      //현재 플레이어 레벨
        int curCount       = GameData.Instance.players[1].expCount; //현재 보유중인 카운트  
        int LevelCount     = playerLevel[curLevel];                 //500

        if (curCount >= LevelCount)
        {
            curLevel++;
            GameData.Instance.players[1].exp      = curLevel;
            curCount -= LevelCount;
            curhasEx = curCount;
            GameData.Instance.players[1].expCount = curCount;
            _level.text = string.Format("{0}", curLevel);

             int LevelCountP = playerLevel[curLevel];
            _levelL.text = string.Format("/{0}", LevelCountP);
        }
    }

 
}
