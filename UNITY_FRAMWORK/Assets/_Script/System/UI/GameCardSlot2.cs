using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameCardSlot2 : MonoBehaviour {

    private Image slotImage;        // 슬롯 이미지
    private Image itemIcon;         // 아이템 아이콘(이미지)
    private int   rpkNumber;       //가위바위보를 숫자로 출력
    private Text  rpk;             //주먹,가위,보의 문자출력



    void Awake()
    {
        slotImage = this.GetComponent<Image>();
        itemIcon = transform.Find("ItemCard").GetComponentInChildren<Image>();
        rpk = transform.GetComponentInChildren<Text>();
        rpk.gameObject.SetActive(false);

    }

    public int RpkNumber
    {
        get
        {
            return rpkNumber;
        }

        set
        {
            if (value < 0)
            {
                rpk.gameObject.SetActive(false);
            }
            else if (value > 0)
            {
                rpk.gameObject.SetActive(true);
                switch (value)
                {
                    case 1:
                        rpk.text = string.Format("{0}", "주먹");
                        break;

                    case 2:
                        rpk.text = string.Format("{0}", "가위");
                        break;

                    case 3:
                        rpk.text = string.Format("{0}", " 보");
                        break;
                }
            }
            rpkNumber = value;

            //TODO: 이곳에이펙트가 가능한가 설정하기
            Vector3 rpkScale = rpk.transform.localScale;
            iTween.ScaleFrom(rpk.gameObject, iTween.Hash("x", rpkScale.x + 0.5f, "y", rpkScale.y + 0.5f, "oncomplete", "AddEffect", "oncompletetarget", this.gameObject, "time", 0.3f));

        }
    }

         void  AddEffect()
         {
                 //TODO: 이펙트 완료후 추가 이펙트나 다른설정하기
         }
    ////슬롯에 오브젝트 카드를 위치 시킨다.
    public void SetCardInfo(Card newInfo)
    {
        //GameCardSlot1과 내용이 달라서 준비중임

        //if (newInfo != null)
        //{
        //    cardInfo = newInfo;
        //    itemIcon.enabled = true;

        //}
        //else
        //{
        //    cardInfo = null;
        //    itemIcon.enabled = false;
        //}
    }


    public void TEST()
    {
        gameObject.transform.localScale = new Vector3(3, 3, 1);
    }

}
