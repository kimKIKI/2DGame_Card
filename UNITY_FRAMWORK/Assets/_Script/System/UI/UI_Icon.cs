using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Icon : MonoBehaviour {


    public GameObject hideImageLeft;
    public GameObject hideImageRight;
    public RectTransform icon;
    public int PanelItem;            //4개의 항목, gameScene,MarketScene,Button_CardScene,Button_E,


    private void OnEnable()
    {
        //UnityCard.evReScroll += PanelReTrue;
    }
     
    public void SetActivateShowA()
    {
        hideImageLeft.SetActive(true);
        hideImageRight.SetActive(true);
        icon.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        //Vector3  move = new Vector3(0, 20, 0);
       
        iTween.MoveTo(icon.gameObject, iTween.Hash("islocal", true,
                                                          "y", 20,
                                                          "easetype", "easeOutQuart",
                                                          "time", 1f));

        iTween.MoveTo(hideImageLeft.gameObject, iTween.Hash("islocal", true,
                                                          "x", -60,
                                                          "easetype", "easeOutElastic",
                                                          "time", 1f));
        iTween.MoveTo(hideImageRight.gameObject, iTween.Hash("islocal", true,
                                                         "x", 60,
                                                         "easetype", "easeOutElastic",
                                                         "time", 1f));
    }

    public void SetActivateHideA()
    {
       
        icon.localScale = new Vector3(.8f, .8f, .8f);
        //icon.localPosition = new Vector3(0, 0, 0);
        iTween.MoveTo(icon.gameObject, iTween.Hash("islocal", true,
                                                        "y", 0,
                                                        "easetype", "easeOutQuart",
                                                        "time", 1f));
        hideImageLeft.transform.localPosition = new Vector3(-30, 0, 0);
        hideImageRight.transform.localPosition = new Vector3(30, 0, 0);
        hideImageLeft.SetActive(false);
        hideImageRight.SetActive(false);
    }

    public void SendPanelItem()
    {
        GameData.Instance.PanelItem = PanelItem;
       
    }

    public void IconEff()
    {
        //선택시 옛날 값과 같지 않을때 활성화 
        if (PanelItem != GameData.Instance.panelItemAfter)
        {
            hideImageLeft.SetActive(true);
            hideImageRight.SetActive(true);

        }
    }


	
}
