using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITWEEN_TEST : MonoBehaviour {

	public GameObject  cube;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Z))
        {


            //iTween.MoveTo(
            //               cube,
            //               iTween.Hash(
            //                “time”, 2.0f,
            //                “islocal”, true,
            //                “position”, this.transform.localPosition + new Vector3(0.0f, 1.0f, 0.0f),
            //               “easetype”, iTween.EaseType.easeOutElastic,
            //               “oncomplete”, “onCompleteiTween”,
            //                “oncompletetarget”, this.gameObject
            //               )
            //              );

           // iTween.MoveTo(this.gameObject,iTween.Hash("x",Screen.width/1.1,"easeType","easeOutBounce","time",0.5f,"oneComplate","aftergoToB"));

        }
    }
}
