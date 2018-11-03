using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingTower : MonoBehaviour {

   
    //private Text manCount;              //수비병value
    public  int manNum;
    //외부에서 카드별로 Damage값을 변경할수 있어야 한다.
    //private int damage = -1;               //증가량
    //private int damageMan = -50;
    SliderController slicontrol;
    //SliderController slicontrolMan;
    public delegate void OneValue(int num);
    public static  event OneValue hp;

    //public int Damage
    //{
    //    get
    //    {
    //        return damage;
    //    }

    //    set
    //    {
    //        damage = value;
    //    }
    //}

   

    //델리게이트 이벤트 필요

    private void Awake()
    {
      
        //manCount      = transform.Find("Text_Man").GetComponentInChildren<Text>();
        slicontrol    = transform.Find("Slider").GetComponentInChildren<SliderController>();
        //slicontrolMan = transform.Find("SliderMan").GetComponentInChildren<SliderController>();
    }

    public void Init()
    {
        //TODO:현재 플레이어의 레벨을 적용시켜야 한다.
        //com과 player의 레벨적용..인공적으로 player보다 한단계 높거나 낮은 com배정한다.
        //com레벨롭 필요함
        //manCount.text  = string.Format("TOWER GARD : {0}", manNum);

    }


    //다양한 이펙트 효과가 필요하다.
    //레벨바 형태도 괜찬음
    //public void TextEFF()
    //{
    //    //iTween.ValueTo(gameObject, iTween.Hash("from", wallNum, "to", wallNum + damage, "time", .6, "onUpdate", "UpdateDefenceDisplay", "oncompletetarget", gameObject));
    //    iTween.ValueTo(gameObject, iTween.Hash("from", manNum, "to", manNum + damageMan, "time", .6, "onUpdate", "UpdateManDisplay", "oncompletetarget", gameObject));

    //    manNum = manNum + damageMan;
    //    //wallNum = wallNum + damage;
    //}

    //여기에서 커플링이 발생한다.
    public void postHp(int hp)
    {
        //hp를 SlidrController에 전달한다.
        slicontrol.Init(hp);
    }

    //public void postMan(int man)
    //{
    //    slicontrolMan.Init(man);
    //}

    public int GetHpBar()
    {
        return slicontrol.Hp;
    }

    public void Hit(int damage)
    {
        slicontrol.Hit(damage);
    }

    void UpdateDefenceDisplay(int Value)
    {
        //wallCount.text = string.Format("TOWER Defence : {0:0,0}", Value);
     
        //if (damage >= Value)
        //{

        ////iTween.Stop(wallCount.gameObject);
        //}
    }


    //void UpdateManDisplay(int Value)
    //{
    //    manCount.text = string.Format("TOWER     GARD : {0}", Value);

    //    if (damageMan >= Value)
    //    {

    //       // iTween.Stop(wallCount.gameObject);
    //    }
    //}





}
