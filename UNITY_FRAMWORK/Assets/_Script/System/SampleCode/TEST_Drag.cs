using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TEST_Drag : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler
{


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        print("OnBeginDrag");
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        print("OnBeginDrag");
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        print("OnDrag");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        print("OnPointerUp");
    }
}
