using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StrollVertical : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler//, IPointerUpHandler, IPointerClickHandler
{
    public delegate void Strol();
    public static event Strol eveVerticalMove; //이동이벤트 발생
    public static event Strol stopMoveTrue;     //정지시켜야할 이벤트 발생
    public static event Strol CloseCards;       //열린카드를 닫아주는 이벤트 발생
    public static event Strol moveUp;           //터치이동후 툴바 버튼의 화살표를 재설정한다.


    float basePosX;    //처음배치localpos
    float basePosY;

    float startMouseX;
    float startMouseY;

    float endMouseX;
    float endMouseY;

    float curPosX;
    float curPosY;
    bool isDragging = false;
    bool isDragOn = false;
    bool isLeft;
    bool isChoice = false;
    float disX;
    float disY;
    float startAnchorPos = -1000f;  //panel 의 처음 맨위 Y값
    RectTransform myRect;
    ScrollRect myScrol;



    int tempAfter = 0;              //현재의 curItem

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


        DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        Init();
    }



    void Init()
    {
        basePosX = myRect.localPosition.x;
        basePosY = myRect.localPosition.y;
        CurPosX = myRect.localPosition.x;
        CurPosY = myRect.localPosition.y;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endMouseX = Input.mousePosition.x;
        endMouseY = Input.mousePosition.y;
        //float distanceX = Mathf.distance
        Debug.Log("end:" + new Vector2(endMouseX, endMouseY));
        //스톱되어 있는 스트롤을 재시작하기 위해서 설정에서 좌우이동이 되지 안게한다.
        if (GameData.Instance.isStopScroview)
            stopMoveTrue();

        //업데이트또는 선택을 위해 버튼이 열린상태일때 좌우이동이되지 않게
        if (GameData.Instance.IsShowCard)
        {
            stopMoveTrue();
            //GameData.Instance.IsShowCard = false;
        }


        if (startMouseX - endMouseX >= 0)
        {
            //Debug.Log("왼쪽으로 갔네요");
            isLeft = true;
            disX = startMouseX - endMouseX;
        }
        else if (startMouseX - endMouseX < 0)
        {
            //Debug.Log("오른쪽으로 갔네요");
            disX = endMouseX - startMouseX;
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
                    InitPosition();
                }
                else if (curItem == 1)
                {
                    GameData.Instance.PanelItem = 2;
                    InitPosition();
                }
                else if (curItem == 2)
                {
                    GameData.Instance.PanelItem = 3;
                    InitPosition();
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
                    InitPosition();
                }
                else if (curItem == 2)
                {
                    GameData.Instance.PanelItem = 1;
                    InitPosition();
                }
                else if (curItem == 3)
                {
                    GameData.Instance.PanelItem = 2;
                    InitPosition();
                }
               
            }

            if (eveVerticalMove != null)
            {
                eveVerticalMove();
            }
            else
            {
                Debug.Log("eveVertical 없네요 없어 ");
            }

           
        } //Debug.Log("x 가 길어요"); END
        else if (disX < disY)
        {
            // Debug.Log("Y 가 길어요");
        }

        moveUp();
    }

   

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (!isDragging)
            isDragOn = true;

        startMouseX = Input.mousePosition.x;
        startMouseY = Input.mousePosition.y;
        // Debug.Log("onBeginDrag---->>"+startMouseX+"--"+ startMouseY);

      
        //선택카드가 열리고 좌우 스트롤이 고정됐을때 모두 해제한다.
        //선택했다가 이동중간에 그만둔 상태인경우
        if (!GameData.Instance.isStopScroview  && GameData.Instance.IsShowCard)
        { //화면이 정지가 되지 안고 열리기만 한 상태
            GameData.Instance.isStopScroview = false;
            GameData.Instance.IsShowCard     = false;
        }
        else if(GameData.Instance.isStopScroview || GameData.Instance.IsShowCard)
        { //화면이 정지이고 , show카드가 열렸을때
            CloseCards();
        }
        else if (!GameData.Instance.isStopScroview && !GameData.Instance.IsShowCard)
        {//화면이 정지가 아니고  열리지도 안은상태
            CloseCards();
        }


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
        //화면이 바뀔수 있는 위치로 이동
        myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x, 0);
        myScrol.content.anchoredPosition = new Vector2(0, startAnchorPos);

        //Debug.Log("myRect.anchorPostion" + myRect.anchoredPosition);
     
    }

    public void InitPosition()
    {
        StartCoroutine(ReturnTop(0));
    }

    IEnumerator ReturnTop(float ti)
    {
        yield return new  WaitForSeconds(ti);
        float t      = 0;
        float speed  = 2.0f;
      
        float ypos = myRect.anchoredPosition.y;
       
        if (gameObject.transform.name == "PanelScroll_GameChoice")
         { yield break; }

      
       
        while (t < 1.0f)
        {   //보간을 적용해서 pos------->_itemDisplayer의 위치로 이동하게 한다.
             t += Time.deltaTime * speed;
             myScrol.content.anchoredPosition = new Vector3(0, Mathf.Lerp(ypos,- 1000f, t), 0);
         
            yield return new WaitForFixedUpdate();
        }
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



