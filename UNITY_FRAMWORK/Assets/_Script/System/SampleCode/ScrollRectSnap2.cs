using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollRectSnap2 : MonoBehaviour {

    public RectTransform panel;
    public Button[] bttn;
    public RectTransform center; //세터와 각각의 버튼 까지의 거리를 비교한다.

    float[] distance;             //center에서 의 거리 
    
    
    bool dragging = false;
    int bttnDistance;
    int minButttonNum;
    int bttnLenght;
    bool messageSend = false;
   
    

    private void Start()
    {
        bttnLenght = bttn.Length;
        distance = new float[bttnLenght];
       

        //처음시작 거리값
        bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);
      

    }

    private void Update()
    {
        for (int i = 0; i < bttn.Length; i++)
        {
           // distance[i] = Mathf.Abs(center.transform.position.x - bttn[i].transform.position.x);

        }
    }
}
