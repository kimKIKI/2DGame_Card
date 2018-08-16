using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingTower : MonoBehaviour {

    private Text wallCount;             //디펜스value
    private Text manCount;              //수비병value
    int wallNum;
    int manNum;
    int wallAdd = -100;               //증가량
    int manAdd = -50;

    private void Awake()
    {
        wallCount = transform.Find("Text_Wall").GetComponentInChildren<Text>();
        manCount  = transform.Find("Text_Man").GetComponentInChildren<Text>();
    }

    public void Init()
    {
        wallNum = 6000;
        manNum  = 1000;

        wallCount.text = string.Format("TOWER Defence: {0}", wallNum);
        manCount.text  = string.Format("TOWER GARD : {0}", manNum);

    }

    public void TextEFF()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", wallNum, "to", wallNum + wallAdd, "time", .6, "onUpdate", "UpdateDefenceDisplay", "oncompletetarget", gameObject));
        iTween.ValueTo(gameObject, iTween.Hash("from", manNum, "to", manNum + manAdd, "time", .6, "onUpdate", "UpdateManDisplay", "oncompletetarget", gameObject));
        manNum = manNum + manAdd;
        wallNum = wallNum + wallAdd;
    }


    void UpdateDefenceDisplay(int Value)
    {
        wallCount.text = string.Format("TOWER Defence : {0:0,0}", Value);
     
        if (wallAdd >= Value)
        {

        iTween.Stop(wallCount.gameObject);
        }
    }


    void UpdateManDisplay(int Value)
    {
        manCount.text = string.Format("TOWER     GARD : {0}", Value);

        if (manAdd >= Value)
        {

            iTween.Stop(wallCount.gameObject);
        }
    }





}
