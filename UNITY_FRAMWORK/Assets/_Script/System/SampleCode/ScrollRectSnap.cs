using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour {

    public RectTransform panel;
    public Button[] bttn;
    public RectTransform center; //세터와 각각의 버튼 까지의 거리를 비교한다.

    float[] distance;             //center에서 의 거리 
    public float[] disReposition;
    public int startButton = 1;
    bool dragging = false;
    int bttnDistance;
    int minButttonNum;
    int bttnLenght;
    bool messageSend = false;
    float lerpSpeed = 5f;
    bool targetNearestButton = true; //가까이 있는 버튼이 true일때
    private void Start()
    {
        bttnLenght = bttn.Length;
        distance = new float[bttnLenght];
        disReposition = new float[bttnLenght];

        //처음시작 거리값
        bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);
        panel.anchoredPosition = new Vector2((startButton-1)*-300f,0f);

    }

    private void Update()
    {
        for (int i = 0; i < bttn.Length; i++)
        {
            disReposition[i] = center.GetComponent<RectTransform>().position.x - bttn[i].GetComponent<RectTransform>().position.x;
            distance[i] = Mathf.Abs(disReposition[i]);
            //distance[i] = Mathf.Abs(center.transform.position.x - bttn[i].transform.position.x);

            if (disReposition[i] > 1000)
            {
                float curX = bttn[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = bttn[i].GetComponent<RectTransform>().anchoredPosition.y;
                Vector2 newAnchoredPos = new Vector2(curX + (bttnLenght * bttnDistance), curY);
                bttn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
            }
            if (disReposition[i] < -1000)
            {
                float curX = bttn[i].GetComponent<RectTransform>().anchoredPosition.x;
                float curY = bttn[i].GetComponent<RectTransform>().anchoredPosition.y;
                Vector2 newAnchoredPos = new Vector2(curX - (bttnLenght * bttnDistance), curY);
                bttn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
            }
        }
        if (targetNearestButton)
        {
            float minDistance = Mathf.Min(distance);
            for (int a = 0; a < bttn.Length; a++)
            {
                if (minDistance == distance[a])
                {
                    minButttonNum = a;
                    //Debug.Log(bttn[minButttonNum].name);
                }
            }
        }


        if (!dragging)
        {
            //LerpToBttn(minButttonNum * -bttnDistance);
            //마지막 순서가 바뀔때
            LerpToBttn(-bttn[minButttonNum].GetComponent<RectTransform>().anchoredPosition.x);
        }
    }

    void LerpToBttn(float position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x,position,Time.deltaTime* lerpSpeed);

        if (Mathf.Abs(position - newX) < 3f)
        {
             newX = position;
           // lerpSpeed = 100f;
        }


        if (Mathf.Abs(newX) >= Mathf.Abs(position) -4f && Mathf.Abs(newX) <= Mathf.Abs(position) + 4f && !messageSend)
        {
            messageSend = true;
            SendMessageFromButton(minButttonNum);
           // Debug.Log("messageSend:"+bttn[minButttonNum].name);
        }
        Vector2 newPosition = new Vector2(newX,panel.anchoredPosition.y);
        panel.anchoredPosition = newPosition;

    }

    void SendMessageFromButton(int bttnIndex)
    {
        if (bttnIndex-1 == 3)
        {
            Debug.Log("Message Send");
        }
    }
    public void StartDrag()
    {
        messageSend = false;
        dragging = true;
        lerpSpeed = 5f;
        targetNearestButton = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }

    public void GoToButton(int buttonIndex)
    {
        targetNearestButton = false;
        minButttonNum = buttonIndex -1;
    }
}