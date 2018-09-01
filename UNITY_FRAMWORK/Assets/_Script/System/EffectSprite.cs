using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSprite : MonoBehaviour {

    //Prefab로 일시적으로 이펙트를 발생시킨다.
    Image image;
    Animator anim;

    private void Awake()
    {
        image = GetComponent<Image>();
        anim = GetComponent<Animator>();

    }


    private void OnEnable()
    {
        anim.SetBool("isStart",true);
    }

    public void animationStop()
    {
        Destroy(this.gameObject);
    }

   
}
