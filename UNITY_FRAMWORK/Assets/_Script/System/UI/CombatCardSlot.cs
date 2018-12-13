using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CombatCardSlot : MonoBehaviour {

    Image slotImage;        // 슬롯 이미지
    [HideInInspector]
    public Card cardInfo = null;
    [HideInInspector]
    public Image itemIcon;   // 아이템 아이콘(이미지)
   
    private void Awake()
    {
        slotImage = this.GetComponent<Image>();
        itemIcon  = transform.Find("ItemCard").GetComponentInChildren<Image>();
    }

    private void Start()
    {
        SetCardInfo(null);
    }

    public void SetCardInfo(Card newInfo)
    {
            if (newInfo != null)
            {
            itemIcon.enabled = true;
            cardInfo         = newInfo;
            string name      = cardInfo.IconName;
         
         
            itemIcon.sprite      = SpriteManager.GetSpriteByName("Sprite",name);
            //itemIcon.sprite    = SpriteManager.GetSpriteByName("Sprite", itemIcon.sprite.name);
            }
            else
            {
            cardInfo         = null;
            itemIcon.enabled = false;
           }
     }

    public Card GetUnityInfo()
    {
        if (cardInfo == null)
        {
            Debug.Log("Card 정보가 null입니다.");
        }
        return cardInfo;
    }

    public void  MoveTo(Vector3 to)
    {
        iTween.MoveTo(gameObject, iTween.Hash("islocal", true,
                                                                  "position", to,
                                                                   "oncompletetarget",gameObject,
                                                                   "time", 0.7f,
                                                                   "easetype", "easeOutQuart")
                                                                 );
    }
  

}
