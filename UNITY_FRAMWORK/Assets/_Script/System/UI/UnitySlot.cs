using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UnitySlot : MonoBehaviour {

    public  void MoveTo(Vector3 to)
    {
        iTween.MoveTo(gameObject, iTween.Hash("islocal", true,
                                                                "position", to,
                                                                 "oncompletetarget", gameObject,
                                                                 "time", 0.5f,
                                                                 "easetype", "easeOutQuart")
                                                               );
    }

  
}
