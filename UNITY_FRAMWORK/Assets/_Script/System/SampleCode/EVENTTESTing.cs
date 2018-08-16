using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class EVENTTESTing : MonoBehaviour,IDragHandler, IBeginDragHandler, IEndDragHandler {

    Vector3 curPos;
    bool isRed;

    public void MouseUp()
    {
        isRed = !isRed;
        if (isRed)
        {

            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("OnBeginDrag작동");
    }

    public void OnDrag(PointerEventData eventData)
    {
        curPos = eventData.position;
        print("OnDrag작동");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("OnEndDrag 작동");
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    isRed = !isRed;
    //    if (isRed)
    //    {

    //        GetComponent<Renderer>().material.color = Color.red;
    //    }
    //    else
    //    {
    //        GetComponent<Renderer>().material.color = Color.white;
    //    }
    //}
}