using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Z))
        {
        //iTween.RotateFrom(gameObject, iTween.Hash("y", 90.0f, "time", 2.0f, "easetype", iTween.EaseType.easeInExpo));
         iTween.MoveBy(gameObject, iTween.Hash("y", 90.0f, "time", 2.0f, "easetype", iTween.EaseType.easeInExpo));
        }
    }
}
