using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour {

   public   Animator anim;

    void Start () {
        
    }
	
	
	void Update () {
        anim.SetBool("isActive", true);
    }
}
