using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FloorFormation : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    //드레그 상태에서 카드가 올라 왔을때 
    //좌우를 판정해서 위치를 정렬 시키 도록 한다. 
    //카드의 수자가 많을 때 작게 한다. 

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("드레그 된것이 올라 왔네요");
    }

    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("박스안에 마우스 가 다운됐네요");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("박스안에 마우스 가  UP 됐네요");
    }
}
