using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemDisplayer : MonoBehaviour {

	// Reference to the Text component displaying the item collected quantity
	[Tooltip("Reference to the Text component displaying the item collected quantity")]
	public Text _itemDisplay;

    // 현재 아이템의 양
    private int _itemQuantity = 0;

    private void Start()
    {
        _itemQuantity = GameData.Instance.players[1].coin;
    }
    // 추가될 아이템의 양
    public void AddItem(int quantity) {
		_itemQuantity += quantity;
		_itemDisplay.text = _itemQuantity.ToString();
	}
}
