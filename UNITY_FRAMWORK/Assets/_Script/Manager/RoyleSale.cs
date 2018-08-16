using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoyleSale : MonoBehaviour {

    [HideInInspector]
    public int rodomType;
   // [HideInInspector]
    public int priceJew;
    [HideInInspector]
    public string Name;
    [HideInInspector]
    public int caseIndex;

    public Image main;     //메인아이콘 이미지
    public Text  jew;      //gold 가격
    public Text  charName; //Item의 네임 ID 를 확인해서 얻어와햐 한다.
   

    private void Awake()
    {
      
        main.sprite = SpriteManager.GetSpriteByName("Sprite", name);
    }

   

    public int BoxName
    {
        get { return caseIndex; }
        set
        {
              if (value > 0)
              {
                caseIndex = value;
                StartCoroutine(SetItem());
              }
         }
    }

    IEnumerator SetItem()
    {
     
        yield return new WaitForSeconds(0.2f);

        jew.text      = string.Format("{0}", priceJew);
       // charName.text = string.Format("{0}", Name);

        switch (caseIndex)
            {
                case 1:
                name =  "rareCase";
                   main.sprite = SpriteManager.GetSpriteByName("Sprite",name);
                   charName.text = string.Format("{0}", "희귀상자");
                

                break;

                case 2:
                    name =  "giantCase";
                    main.sprite = SpriteManager.GetSpriteByName("Sprite",name);
                    charName.text = string.Format("{0}", "자이언트상자");
                break;

                case 3:
                    name =  "luckyCase";
                    main.sprite = SpriteManager.GetSpriteByName("Sprite",name);
                    charName.text = string.Format("{0}", "행운상자");
                break;

                case 4:
                name = "legendaryCase";
                    main.sprite = SpriteManager.GetSpriteByName("Sprite",name);
                    charName.text = string.Format("{0}", "전설킹상자");
                break;
            }
    }

    public void PurchaseSend()
    {

    }
    
}
