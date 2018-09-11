using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StrollVertical : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler//, IPointerUpHandler, IPointerClickHandler
{

    float  basePosX;    //처음배치localpos
    float  basePosY;

    float  curPosX;
    float  curPosY;
    bool   isDragging  = false;
    bool   isDragOn    = false;
    RectTransform myRect;

    public float CurPosX
    {
        get
        {
            return curPosX;
        }

        set
        {
            curPosX = value;
        }
    }

    public float CurPosY
    {
        get
        {
            return curPosY;
        }

        set
        {
            curPosY = value;
        }
    }

    private void Awake()
    {
         myRect = GetComponent<RectTransform>();
        
    }

    private void Start()
    {
        Init();
    }



    void Init()
    {
        basePosX = myRect.localPosition.x;
        basePosY = myRect.localPosition.y;
        CurPosX  = myRect.localPosition.x;
        CurPosY  = myRect.localPosition.y;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       /// throw new System.NotImplementedException();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if(!isDragging)
          isDragOn = true;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (isDragOn)
        {
        isDragging = true;
        isDragOn = false;

        }
        curPosY = myRect.localPosition.y;

        if (curPosY <= -200f)
        {
            //더이상 밑으로 드레깅되지 않게 한다.
            curPosY = -200f;
            StartCoroutine(coMove(myRect, curPosY, 0, 5f));
            return;
        }
        print("OnBeginDrag : curPosY :"+curPosY);
    }



    IEnumerator coMove(RectTransform obj, float start, float end, float speed)
    {
        float t = 0;
        while (t <= 1)
        {
            //현재 이동중일때
            t += Time.deltaTime * speed;
            float move = Mathf.Lerp(start, end, t);
            obj.localPosition = new Vector3(obj.localPosition.x, move, obj.localPosition.z);
            yield return null;
        }
        yield break;
     }

    public void SwitchTopPosition()
    {
        //화면이 바뀌수 있는 위치로 이동
        myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, 300f);
    }
}



    //void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    //{
    //    print("OnDrag");
    //}

    //void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    //{
    //    print("OnPointerUp");
    //}



