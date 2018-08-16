using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RPGItemTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RPGItem item = RPGItemDatabase.GetItem(1);
        if (item != null)
        {
            Debug.Log(string.Format("Item ID:{0},Item Name:{1},Item Desc{2}",item.ItemID,item.itemName,item.itemDesc));
        }
        item = RPGItemDatabase.GetItem(2);
        if (item != null)
        {
            Debug.Log(string.Format("Item ID:{0},Item Name:{1},Item Desc{2}", item.ItemID, item.itemName, item.itemDesc));
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
