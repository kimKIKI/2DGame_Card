using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItweenTest : MonoBehaviour {


    public GameObject sphere;

	void Start () {
        //iTween.MoveTo(gameObject, iTween.Hash("y", -3, "oncomplete", "OnCom", "easeType", iTween.EaseType.easeInCubic));
        iTween.MoveTo(sphere, iTween.Hash("y", -3, "oncomplete", "OnCom", "oncompletetarget", gameObject, "easeType", iTween.EaseType.easeInCubic));
        //iTween.MoveTo(sphere, iTween.Hash("y", -3, "onupdate", "OnCom", "onupdateparams","rese","onupdatetarget", gameObject, "easeType", iTween.EaseType.easeInCubic));
    }

    void OnCom(string bb)
    {

        //iTween.MoveTo(gameObject, iTween.Hash("x", -3, "easeType", iTween.EaseType.easeInOutBack));
        AppSound.instance.SE_CARD_MOVE.Play();
        Debug.Log("--" + bb);
    }
}
