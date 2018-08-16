using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TT : MonoBehaviour {

    int next = 0;

    IList<UnityCard> testSlot;

    public RectTransform parent;

    private void Start()
    {
        testSlot = new List<UnityCard>();
        testSlot = parent.GetComponentsInChildren<UnityCard>();
    }


    public void SwitchButton()
    {
        next++;

        if (next > 3)
        {
            next = 1;
        }
        switch (next)
        {

            case 1:
                Button1();
                break;
            case 2:
                Button2();
                break;
            case 3:
                Button3();
                break;
            default:
                break;
        }
    }

    private void Button1()
    {
        print("Button 이 1 입닌다.");
    }


    private void Button2()
    {
        print("Button 이 2 입닌다.");
    }

    private void Button3()
    {
        print("Button 이 3 입닌다.");
    }


}