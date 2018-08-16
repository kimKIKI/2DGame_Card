using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollViewController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IEventSystemHandler, IPointerUpHandler
{


    public RectTransform [] panelRECT; //컨트롤할 패널을 드레깅
    int iPanelNum;                     //전체패널의 숫자
    float[] rects;                     //패널들의 상단좌측 좌표를 입력받는다.


    //  static GameObject obj;
    // Image PanelImage;
    public Vector3 firstPos;
    public Vector3 MidPos;
    public Vector3 EndPos;
    public bool bCanvasClick = false; //캔버스가 클릭 됐는지 판단
    public bool bCanvasDrag = false;
    public bool bVertical = false; // scroll vertical 
    public bool bHorizontal = false; // scroll horizontal
    bool bRight = false; //좌방향인지 판단
    bool bLeft = false; //우방향인지 판단

    //일단 영역별 판단
    bool isA = false;
    bool isB = false;
    bool isC = false;
    bool isD = false;
    bool isE = false;
    bool isF = false;


    float nWidth  = 1080f; //스크린 의 panel 크기
    float nHeight = 1920f;

    public bool bDetermineVH = false; //UI가 Horizontal,Vertical방향으로 움직일지 결정
                                      //public bool curUImove  현재의 어느것이 선택됐는지 알필요가 있다.

    int temp = 0;
    ScrollRect sR;         //자신의 scroll을 이벤트로 컨트롤한다.
    RectTransform rect;    //자신의 Rect에 제한을 두기위해서 

    float currentSpeed = 0.5f;
    float acceleration = 0.4f;

    private void Awake()
    {
        iPanelNum = panelRECT.Length;

        sR = GetComponent<ScrollRect>();
        rect = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        //처음한번만 컴포넌트에 직접접근해서 처리 
        sR.horizontal = false;
        sR.vertical = false;
    }


    //public bool _Vertiacl
    //{
    //    get { return bVertical; }
    //    set
    //    {
    //        bVertical = _Vertiacl;
    //        sR.vertical = true;
    //    }
    //}

    //public bool _Horizontal
    //{
    //    get { return bHorizontal; }
    //    set
    //    {
    //        bHorizontal  = _Horizontal;
    //        sR.horizontal = true;
    //    }
    //}


    public void OnBeginDrag(PointerEventData eventData)
    {
        bCanvasClick = true;
        if (bCanvasClick && !bDetermineVH)
        {
            //판단하기 위해서 동시에 켜져야 한다.
            sR.horizontal = true;
            sR.vertical = true;

            firstPos = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        bCanvasDrag = true;
        MidPos = eventData.position;
        //print("nDrag" + bCanvasClick + MidPos);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bCanvasClick = false;
        bCanvasDrag = false;
        bDetermineVH = false;
        EndPos = eventData.position;
        print("OnEndDrag" + bCanvasClick + EndPos);
        LIMIT();
        StartCoroutine(coDirection());
        LlimitMove();
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        // print("OnPointerClick" + pos);
    }


    public void OnPointerUp(PointerEventData eventData)
    {

        Vector3 pos = eventData.position;
        // print("OnPointerUp" + pos);
    }

    IEnumerator coDirection()
    {

        yield return new WaitForSeconds(0.1f);
        //x,y방향중 어느쪽으로 이동했는지 판별한다.
        Vector3 dir = MidPos - firstPos;

        if (firstPos.x >= MidPos.x)
        {
            //--->방향
            bRight = true;
        }
        else
        {
            //<---방향
            bLeft = true;
        }

        dir = dir.normalized;
        float x = Mathf.Abs(dir.x);
        float y = Mathf.Abs(dir.y);

        if (x >= y)
        {
            sR.vertical = false;
            // _Horizontal = true;
            //위치에 도달하게 되면 false;로 바뀌게 한다.
            bDetermineVH = true;     //방향이 결정된경우

        }
        else if (x < y)
        {
            sR.horizontal = false;
            // _Vertiacl = true;
            //위치에 도달하게 되면 false;로 바뀌게 한다.
            bDetermineVH = true;     //방향이 결정된경우

        }

        yield return new WaitForSeconds(0.1f);
    }

    // 드레그 이벤트가 시작시 위치하는Panel의 위치 를 얻는다. 
    void LIMIT()
    {
        float recX = rect.position.x;
        float recY = rect.position.y;
        if (recX >= 0 && recX < 1080)
        {
            print("A위치에 위치합니다.");
            //거꾸로 넘어간 상황
            // aa위치에서  0으로 이동해야함
        }
        else if (0 > recX && -1080 <= recX)
        {
            isA = false;
            isB = true; //B
            isC = false;
            isD = false;
            isE = false;
            isF = false;

        }
        else if (-1080 > recX && -2160 <= recX)
        {
            isA = false;
            isB = false;
            isC = true;//C
            isD = false;
            isE = false;
            isF = false;

        }
        else if (-2160 > recX && -3240 <= recX)
        {
            isA = false;
            isB = false;
            isC = false;
            isD = true; //D
            isE = false;
            isF = false;

        }
        else if (-3240 > recX && -4260 <= recX)
        {
            isA = false;
            isB = false;
            isC = false;
            isD = false;
            isE = true; //E
            isF = false;

        }
        else if (-4260 > recX)
        {
            //화면을 벗어난 경우
            isA = false;
            isB = false;
            isC = false;
            isD = false;
            isE = false;
            isF = true;
        }

    }

    //드레그된 위치를 바탕으로 움직일 좌표를 얻는다.
    void LlimitMove()
    {
        Vector3 curPos = rect.position;
        Vector3 next;
        StartCoroutine(coMove());

        if (isA && bRight)
        {
            //b영역으로 이동
            next = new Vector3(0, rect.position.y, rect.position.z);
        }


        if (isB && bRight)
        {
            //C영역으로 이동
            next = new Vector3(-1080, rect.position.y, rect.position.z);
        }

        if (isC && bRight)
        {
            next = new Vector3(-2160, rect.position.y, rect.position.z);
            //D영역으로 이동
        }

        if (isD && bRight)
        {
            //E영역으로
            next = new Vector3(-3240, rect.position.y, rect.position.z);
        }
        if (isE && bRight)
        {
            next = new Vector3(-4260, rect.position.y, rect.position.z);
            //E영역에 머물게 한다.
        }
    }

    IEnumerator coMove()
    {
        float start = currentSpeed;
        //float end = 0;
        float startPos = rect.position.x;
        float endPos = -3240;
        float t = 0;

        //EaseOut u2 = 1 - (1 - u) * (1 - u);        //끝이 가까워 지면 느려짐    
        // pos start,pos end
        //             시작P  종료P
        // p01 = (1-u)*p0 + u*p1;

        while (t <= 1f)
        {
            //t값이 0 ~ 1까지 증가 
            t += acceleration * Time.deltaTime;
            startPos = Mathf.Lerp(startPos, endPos, t);
            //float movX = rect.position.x;
            // movX += movX * currentSpeed * Time.deltaTime;
            //rect.position = new Vector3(movX, rect.position.y, rect.position.z);
            print("currentSpeed:" + currentSpeed);
            print(" startPos:" + startPos);
        }
        //프레임마다 리턴해서 속도를 
        yield return null;
    }

    public void NextUIRight()
    {
        //sR.horizontal = false;
        //float fFirst = rect.localPosition.x;
        //float fNext = rect.localPosition.x - 1080f;
        //rect.localPosition = new Vector3(fNext, rect.localPosition.y, rect.localPosition.z);
        //보간 적용이동
        StartCoroutine(coNextRight());

    }

    public IEnumerator coNextRight()
    {
        //현재 위치가 어디인지 판단해야 된다.
        float duration = 0.3f;
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float t = 0;
        Vector3 fStart = rect.localPosition;
        float fNext = rect.localPosition.x - 1080f;
        Vector3 end = new Vector3(fNext, rect.localPosition.y, rect.localPosition.z);

        while (t < duration)
        {
            t += Time.deltaTime;
            rect.localPosition = Vector3.Lerp(fStart,end,t/duration);
            yield return  wait;
        }
        rect.localPosition = end;
    }

    public IEnumerator coNextLeft()
    {
        float duration = 0.3f;
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float t = 0;
        Vector3 fStart = rect.localPosition;
        float fNext = rect.localPosition.x + 1080f;
        Vector3 end = new Vector3(fNext, rect.localPosition.y, rect.localPosition.z);

        while (t < duration)
        {
            t += Time.deltaTime;
            rect.localPosition = Vector3.Lerp(fStart, end, t / duration);
            yield return wait;
        }
        rect.localPosition = end;
    }

    public void NextUILeft()
    {
        StartCoroutine(coNextLeft());
    }


}
