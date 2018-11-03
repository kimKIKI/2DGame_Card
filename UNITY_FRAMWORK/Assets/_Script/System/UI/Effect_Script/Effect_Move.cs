using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Move : MonoBehaviour {

   public float t;

    public void MoveTo(Vector3 to)
    {
        iTween.MoveTo(gameObject, iTween.Hash("islocal", true,
                                                                  "position", to,
                                                                   "oncompletetarget", gameObject,
                                                                   "time", t,
                                                                   "easetype", "easeOutQuart")
                                                                 );
    }

}
