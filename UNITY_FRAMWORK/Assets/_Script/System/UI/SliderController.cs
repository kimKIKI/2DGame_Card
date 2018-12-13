using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderController : MonoBehaviour {


    public Transform parent; //정보를 받아오거나 외부에서 접근할수 있는 객체
  
    [HideInInspector]
    public float  sliderValue;
    [HideInInspector]
    private int hp;


    //-----------내부-------------------
    public Text tx_Hp;
    Slider slider;
    float uni;

    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
            tx_Hp.text = string.Format("{0}", value);
        }
    }

    private void Awake()
    {   //Awak에서는 찾을것을 명시적으로 처음부터 하자
        slider = GetComponentInChildren<Slider>();
        //tx_Hp  = transform.Find("Text_Wall").GetComponentInChildren<Text>();
    }

    public  void Init(int fHp)
    {
                Hp = fHp;
        var level  = fHp;
             uni   = 1f / level;
        var  value = 1f;
        //구매시 애니가 작동되어야 한다.
        slider.value = value;
        tx_Hp.text = string.Format("{0}", fHp);
    }

    //슬라이더적용
    public  void Hit(int damage)
    {
        var curHp = Hp - damage;
        var value = curHp * uni; //float
        slider.value = value;
        //tx_Hp.text = string.Format("{0}", curHp);
        //자기 자신의 프로퍼티늘 불러올수 없는데 ... 한번시도 그냥
        Hp = curHp;
    }



}
