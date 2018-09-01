using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_counter : MonoBehaviour {

    Image bgGround;
    Image bgGroundTop;
    Text  numText;
    Text  numText2;
    int   gold;

    private void Awake()
    {
         bgGround    = transform.Find("Image_bg").GetComponentInChildren<Image>();
         bgGroundTop = transform.Find("Image_bgTop").GetComponentInChildren<Image>();
         numText     = transform.Find("Image_TextValue").GetComponentInChildren<Text>();

        if(numText != null)
        {
          numText2    = transform.Find("Image_LevelNum").GetComponentInChildren<Text>();
        }



    }

    void Start ()
    {
		//EventManager.
	}

    public int Gold
    {
        get { return gold; }
        set
        {
           
            gold = Mathf.Clamp(value, 0, 2147483647);
            EventManager.Instance.PostNotification(EVENT_TYPE.GOLD_CHANGE, this, gold);
        }
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        //Detect event type
        switch (Event_Type)
        {
            case EVENT_TYPE.HEALTH_CHANGE:
                OnGoldChange(Sender, (int)Param);
                break;
        }
    }

    void OnGoldChange(Component show, int Gold)
    {
        if (this.GetInstanceID() != show.GetInstanceID()) return;
        numText.text = string.Format("{0}",gold);

    }


}
