using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StrollVertical : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler//, IPointerUpHandler, IPointerClickHandler
{
    public  delegate void  Strol();
    public  static   event Strol eveVerticalMove;
   

    float   basePosX;    //처음배치localpos
    float   basePosY;

    float   startMouseX;  
    float   startMouseY;

    float   endMouseX;
    float   endMouseY;

    float   curPosX;
    float   curPosY;
    bool    isDragging  = false;
    bool    isDragOn    = false;
    bool    isLeft;
    float   disX;
    float   disY;
    float startAnchorPos = -1000f;  //panel 의 처음 맨위 Y값
    RectTransform myRect;
    ScrollRect    myScrol;

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
         myScrol = GetComponent<ScrollRect>();
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
        endMouseX = Input.mousePosition.x;
        endMouseY = Input.mousePosition.y;
        //float distanceX = Mathf.distance
        Debug.Log("end:"+new Vector2(endMouseX,endMouseY));

       

        if (startMouseX - endMouseX >= 0)
        {
            //Debug.Log("왼쪽으로 갔네요");
            isLeft = true;
             disX = startMouseX - endMouseX;
        }
        else if (startMouseX - endMouseX < 0)
        {
            //Debug.Log("오른쪽으로 갔네요");
             disX = endMouseX - startMouseX ;
            isLeft = false;
        }


        if (startMouseY - endMouseY >= 0)
        {
            //Debug.Log("아래쪽으로 갔네요");
            disY = startMouseY - endMouseY;
        }
        else if (startMouseY - endMouseY < 0)
        {
            //Debug.Log("위쪽으로 갔네요");
            disY = endMouseY - startMouseY;
        }

        if (disX >= disY)
        {
           
            //Debug.Log("x 가 길어요");
            if (isLeft)
            {
                int curItem = GameData.Instance.PanelItem;
                //TODO:MainScrollView의 curPanel ++오른쪽

                if (curItem == 0)
                {
                    GameData.Instance.PanelItem = 1;
                }
                else if (curItem == 1)
                {
                    GameData.Instance.PanelItem = 2;
                }
                else if (curItem == 2)
                {
                    GameData.Instance.PanelItem = 3;
                }

            }
            else
            {
                int curItem = GameData.Instance.PanelItem;
                Debug.Log("curItem :" + curItem);
                if (curItem == 0)
                {
                    //맨왼쪽이라 더이상 왼쪽으로 못감
                }
                else if (curItem == 1)
                {
                    GameData.Instance.PanelItem = 0;
                }
                else if (curItem == 2)
                {
                    GameData.Instance.PanelItem = 1;
                }
                else if (curItem == 3)
                {
                    GameData.Instance.PanelItem = 2;
                }
            }
            eveVerticalMove();
        }
        else if (disX < disY)
        {
           // Debug.Log("Y 가 길어요");
        }

    }

  

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if(!isDragging)
          isDragOn = true;

       startMouseX =  Input.mousePosition.x;
       startMouseY = Input.mousePosition.y;
       // Debug.Log("onBeginDrag---->>"+startMouseX+"--"+ startMouseY);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {

        endMouseX = Input.mousePosition.x;
        endMouseY = Input.mousePosition.y;

      
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
        myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, 0);
        myScrol.content.anchoredPosition = new Vector2(0,startAnchorPos);
        Debug.Log("myRect.anchorPostion" + myRect.anchoredPosition);
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



