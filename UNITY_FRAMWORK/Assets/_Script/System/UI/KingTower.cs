using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingTower : MonoBehaviour {

    private Text wallCount;             //디펜스value
    private Text manCount;              //수비병value
    public  int wallNum;
    public  int manNum;
    //외부에서 카드별로 Damage값을 변경할수 있어야 한다.
    private int damage = -1;               //증가량
    private int damageMan = -50;

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    public int DamageMan
    {
        get
        {
            return damageMan;
        }

        set
        {
            damageMan = value;
        }
    }

    //델리게이트 이벤트 필요

    private void Awake()
    {
        wallCount = transform.Find("Text_Wall").GetComponentInChildren<Text>();
        manCount  = transform.Find("Text_Man").GetComponentInChildren<Text>();
    }

    public void Init()
    {
        //TODO:현재 플레이어의 레벨을 적용시켜야 한다.
        //com과 player의 레벨적용..인공적으로 player보다 한단계 높거나 낮은 com배정한다.
        //com레벨롭 필요함
      
        wallCount.text = string.Format("TOWER Defence: {0}", wallNum);
        manCount.text  = string.Format("TOWER GARD : {0}", manNum);

    }


    //다양한 이펙트 효과가 필요하다.
    //레벨바 형태도 괜찬음
    public void TextEFF()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", wallNum, "to", wallNum + damage, "time", .6, "onUpdate", "UpdateDefenceDisplay", "oncompletetarget", gameObject));
        iTween.ValueTo(gameObject, iTween.Hash("from", manNum, "to", manNum + damageMan, "time", .6, "onUpdate", "UpdateManDisplay", "oncompletetarget", gameObject));

        manNum = manNum + damageMan;
        wallNum = wallNum + damage;
    }


    void UpdateDefenceDisplay(int Value)
    {
        wallCount.text = string.Format("TOWER Defence : {0:0,0}", Value);
     
        if (damage >= Value)
        {

        iTween.Stop(wallCount.gameObject);
        }
    }


    void UpdateManDisplay(int Value)
    {
        manCount.text = string.Format("TOWER     GARD : {0}", Value);

        if (damageMan >= Value)
        {

            iTween.Stop(wallCount.gameObject);
        }
    }





}
