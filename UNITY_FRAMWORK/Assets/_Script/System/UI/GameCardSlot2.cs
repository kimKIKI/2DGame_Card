using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameCardSlot2 : MonoBehaviour {

    private Image slotImage;        // 슬롯 이미지
    private Image itemIcon;         // 아이템 아이콘(이미지)
    private int   rpkNumber;       //가위바위보를 숫자로 출력
    private Text  rpk;             //주먹,가위,보의 문자출력
            bool  IsBase;          //기본상태
            bool  IsCard;           //카드배분상태
    Camera maincamera;

    void Awake()
    {
        slotImage = this.GetComponent<Image>();
        itemIcon = transform.Find("ItemCard").GetComponentInChildren<Image>();
        rpk = transform.GetComponentInChildren<Text>();
        rpk.gameObject.SetActive(false);
        maincamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    private void OnEnable()
    {
        GameManager.eveCardIn += ChildCount;
    }
    void OnDisable()
    {
        GameManager.eveCardIn -= ChildCount;
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
    public void ChildCount()
    {
       int num = transform.childCount;
        if (num > 2)
        {
            //새로운 오브젝트가 포함된 상태(카드배분)
            IsBase = true;
            string str = transform.GetChild(2).name;
            transform.Find(str).transform.localScale = new Vector3(0.5f, 0.5f, 1);
            transform.Find(str).transform.localRotation = Quaternion.identity;
        

            //시간차로 커지게 한다.
            StartCoroutine(coChildCount(0.3f));
        }
    }

    IEnumerator coChildCount(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.transform.localScale = new Vector3(2, 2, 2);
    }

    void  AddEffect()
    {
     //TODO: 이펙트 완료후 추가 이펙트나 다른설정하기
    }

    ////슬롯에 오브젝트 카드를 위치 시킨다.
    public void SetCardInfo(Card newInfo)
    {
      
    }


    public void TEST()
    {
        gameObject.transform.localScale = new Vector3(3, 3, 1);
    }

}
