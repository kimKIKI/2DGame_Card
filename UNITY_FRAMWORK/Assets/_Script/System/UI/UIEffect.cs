using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffect : MonoBehaviour
{
    Image slotImage;         // 슬롯 이미지
    [HideInInspector]
    public Image itemIcon;   // 아이템 아이콘(이미지)

    private void Awake()
    {
        slotImage = this.GetComponent<Image>();
        itemIcon = transform.Find("RPSCard").GetComponentInChildren<Image>();

    }


    public void MoveTo(Vector3 to)
    {
        iTween.MoveTo(gameObject, iTween.Hash("islocal", true,
                                              "position", to,
                                               "oncompletetarget", gameObject,
                                               "time", 0.8f,
                                               "easetype", "easeOutElastic")
                                                               );
    }

    public void ScaleFrom()
    {

        iTween.ScaleFrom(gameObject, iTween.Hash("x", gameObject.transform.localScale.x + 0.3f, "y", gameObject.transform.localScale.y + 0.3f, "oncomplete", "ChangeImg", "oncompletetarget", this.gameObject, "time", 0.3f));

    }

    //가위바위보의 이미지 적용
    public void Rock_paperName(int rock_paper)
    {

        //가위1 or 바위2 or 보3
        switch (rock_paper)
        {
            case 1:
                itemIcon.sprite.name = "rps_0";
                itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", itemIcon.sprite.name);
                break;

            case 2:
                itemIcon.sprite.name = "rps_1";
                itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", itemIcon.sprite.name);
                break;

            case 3:
                itemIcon.sprite.name = "rps_2";
                itemIcon.sprite = SpriteManager.GetSpriteByName("Sprite", itemIcon.sprite.name);
                break;

        }
     
    }

   
}


