using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MainScrollView : MonoBehaviour //,IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler, IPointerUpHandler, IPointerClickHandler
{
    
    public RectTransform[] panelRECT;       // 컨트롤할 패널OBJ을 저장한다.
    public RectTransform[] bottomPanelRect; //메뉴의 하단 버튼
   
    public RectTransform bottomMoveRect;    //버튼이 클릭 됐는지 알기 위한 이펙트 효과
    public Transform buttonBackGround;      //버튼이 이동시 뒤 이펙트 이미지 
   
    PanelNUM[] stPanels;                    //구조체 Panel데이터
    int iPanelNum;                          //전체패널의 숫자
    public Canvas mainCanvas;               //raycast
    GraphicRaycaster gr;
    PointerEventData ped;
    ScrollRect scRect1;
    ScrollRect scRect2;
    ScrollRect scRect3;
    Vector3 startPos;
    Vector3 destPos;
    Vector3 v3movePanel;
    Vector3 fromScale;                    //UI bottom의 뒤배경 그라운드 크기 조절
    public bool bIsMoveing = false;       //현재 이동중일때 새로운 좌표가 설정되면 안된다.
    bool bIsRight = false;                //left 와 right에 따라 move의 부호가 달라짐.주의

    int curPanel = 0;                     //처음 시작 패널의 값을 저장한다.
    int afterPanel = 0;                   //새로울 패널을 전달 받았을때 이동하기 위해서 
    float initY = -860;
    struct PanelNUM
    {  //PanelRect로 드레그된 정보를 컨트롤할수 있게 구조체배열로 저장한다.
       public int    index;
       public float  Width;
       public float  posX;                //원래의 위치를
       public float  curPosX;             //현재 이동좌표
       public float  nextPos;             //다음 이동좌표
       public float  posY;
       public float  curPosY;
       public bool   bIsActive;          //현재 작업중인지 판단.
       
    } 

    private void Awake()
    {
        iPanelNum = panelRECT.Length;
        stPanels = new PanelNUM[iPanelNum];
        scRect1 = panelRECT[0].GetComponent<ScrollRect>();
        scRect2 = panelRECT[1].GetComponent<ScrollRect>();
        scRect3 = panelRECT[2].GetComponent<ScrollRect>();
        InitPanel();//처음시작시 panelRect 오브젝트의 정보를 panel구조체에 저장한다.
    }

    private void Start()
    {
        gr = mainCanvas.GetComponent<GraphicRaycaster>();
        //그래픽레이캐스터에서의 마우스 포인터 위치를 나타낸다.
        ped = new PointerEventData(null);
        //Debug발생으로 처리한 코드부분 만약 한칸 건너서 없은곳 보일때 다시수정필요
        // StartCoroutine(coReConFirm());
        StrollVertical.eveVerticalMove += PanelMove;
     
    }

    void PanelMove()
    {
        PanleMove();
    }


  


    private void Update()
    {
      ////이벤트화 하기 전에 일단 입력받게함
      //  stPanels[0].curPosY = panelRECT[0].localPosition.y;

      //  if (Input.GetKeyDown(KeyCode.K))
      //  {
      //      startPos = destPos = this.transform.position;
      //   Vector3 tempPos = ped.position;
      // }


        //그래픽 레이케이스로 이미지의 네임을 지속적으로 판단하고 있다.
        //계속 해서 리스트에 등록하고 있으므로 가비지나 리셋해야됨....
        //
        //TODO:꼭 수정해야됨
        //SetStrollRectID();
        //지속적으로 x,y의 위치를 판단하게 한다.
        //CurPanel();
       
        //Touch//===================
        // Touch[] touchs = Input.touches;

    }


    public void RightMove()
    {
        bIsRight = true;
        //이동이 끝까지 왔다면 더이상 이동시키지 않은다. 배열명3 
        if (stPanels[iPanelNum - 1].curPosX <= 0 || bIsMoveing)
            return;

       
        //현재의 설정된 width,의 값과 인덱스 정보를 알아온다.
        for (int i = 0; i < iPanelNum; ++i)
        {
            stPanels[i].nextPos = stPanels[i].curPosX - stPanels[i].Width;
            //코루틴으로 이동시킬오브젝트,시작점,도착지점,스피드전달
            StartCoroutine(coMove(panelRECT[i], stPanels[i].curPosX, stPanels[i].nextPos, 3f));
        }
    }

   


    //이동
    IEnumerator coMove(RectTransform obj,float start,float end,float speed)
    {
        bIsMoveing = true;
        float t = 0;
        while (t <= 1)
        {
            //현재 이동중일때
            t += Time.deltaTime * speed;
            float move = Mathf.Lerp(start,end,t);
            obj.localPosition = new Vector3(move, obj.localPosition.y, obj.localPosition.z);

            if (bIsRight)
            {
                if (end >= move)
                {
                    bIsMoveing = false;
                    CurPanel(); //버튼이 눌리면 바로 페널위치를 초기화 저장한다.
                }
            }
            else if (!bIsRight)
            {
                if (end <= move)
                {
                    bIsMoveing = false;
                    CurPanel(); //버튼이 눌리면 바로 페널위치를 초기화 저장한다.
                }
            }
            yield return null;
        }
    }

 

    public void LeftMove()
    {
        bIsRight = false;
        //이동이 끝까지 왔다면 더이상 이동시키지 않은다.
        if (stPanels[0].curPosX >= 0 || bIsMoveing)
        { 
            return;
        }
        
        //현재의 설정된 width,의 값과 인덱스 정보를 알아온다.
        for (int i = 0; i < iPanelNum; ++i)
        {
            float moveX = stPanels[i].curPosX + stPanels[i].Width;
            StartCoroutine(coMove(panelRECT[i], stPanels[i].curPosX, moveX, 3f));
        }
    }

    public void PanleMove()
    {
        //panelItem의 Id를 얻어서 이동시킬 좌표를 구한다. endx좌표
        //start좌표는 어떻게 구할것인가?
        //after 
        int width = 1080;
        int hight = 1920;
      
        int curID = GameData.Instance.PanelItem;
        // panelRECT[]


        switch (curID)
        {
            case 0:
                v3movePanel = new Vector3((width - width * 0.5f) * -1, 0, 0);
                break;
            case 1:
              
                v3movePanel = new Vector3((width - width * 0.5f) * -1, 0, 0);
                //content 의 y값 초기값이  -1000

                break;
            case 2:
                
                //v3movePanel = new Vector3((width * 2 - width * 0.5f) * -1, hight * 0.5f, 0);
                v3movePanel = new Vector3((width * 2 - width * 0.5f) * -1, 0, 0);
               

                break;
            case 3:
               
                //content[2].transform.localPosition = new Vector3(0, -1000, 0);
                v3movePanel = new Vector3((width * 3 - width * 0.5f) * -1, 0, 0);
          

                break;
            case 4:
                v3movePanel = new Vector3((width * 4 - width * 0.5f) * -1, 0, 0);
                break;
        }
      
       
            iTween.MoveTo(gameObject, iTween.Hash("islocal",true,
                                                  "position", v3movePanel,
                                                  "oncomplete", "afterChange",
                                                  "easetype", "easeOutQuart",
                                                  "time", .7f));
        Debug.Log("v3movePanel :" + v3movePanel);
        Debug.Log("curId:" + curID);
    }

    void InitPanel()
    {
        for (int i = 0; i < iPanelNum; i++)
        {
            stPanels[i].index   = i;
            stPanels[i].Width   = panelRECT[i].rect.width;
            stPanels[i].posX    = panelRECT[i].localPosition.x;
            stPanels[i].curPosX = panelRECT[i].localPosition.x;
            stPanels[i].posY    = panelRECT[i].localPosition.y;
            stPanels[i].curPosY = panelRECT[i].localPosition.y;
        }
    }

    void CurPanel()
    {
        for (int i = 0; i < iPanelNum; i++)
        {
            //이벤트 발생시 계속 위치를 변경한다.
            stPanels[i].curPosX = panelRECT[i].localPosition.x;
            stPanels[i].curPosY = panelRECT[i].localPosition.y;
        }
    }

    public int CCurPanel
    {
        get { return curPanel; }
        set
        {
            if (curPanel != afterPanel)
            {
                afterPanel = curPanel;
                curPanel = value;
            }
        }
    }


    void ScaleButton(Vector2 size)
    {
        fromScale.x = size.x;
        fromScale.y = size.y;
        fromScale.x = 2;
        fromScale.y = 1;

    }

     Vector2 fromSize()
    {
        return new Vector2(buttonBackGround.localScale.x, buttonBackGround.localScale.y);
    }

    //ButtonEvent------------------------
    public void SetCurPanelID_1()
    {
        curPanel = 1;
        //이벤트 시스템에서는 인수를 넘길수없다

        //뒤의 사각형이 천천히 크게 선택되는 모양 이펙트 
        buttonBackGround.transform.localPosition = new Vector3(bottomPanelRect[0].anchoredPosition.x, initY, 0);
       
        Vector3 toScale = new Vector3(1,.4f,0);
      
            //iTween.ValueTo(buttonBackGround.gameObject, iTween.Hash("islocal", true,
            //                                                 "from", fromSize(),
            //                                                 "to", toScale,
            //                                                 "easetype", iTween.EaseType.easeOutExpo,
            //                                                 "onupdate", "ScaleButton",
            //                                                 "time", .5f));
       
           
        // 클릭된 버튼의 위치로 박스가 이동하기 위해서 next설정
        Vector3 selectNextBox = new Vector3(bottomPanelRect[0].anchoredPosition.x, initY, 0);
        
            if (curPanel != afterPanel)
            {
                iTween.MoveTo(bottomMoveRect.gameObject, iTween.Hash("islocal", true,
                                                                "position", selectNextBox,
                                                                "oncomplete", "afterChange",
                                                                "easetype", "easeOutQuart",
                                                                "time", .7f));
            }
        }

    public void SetCurPanelID_2()
    {
        curPanel = 2;
        // 클릭된 버튼의 위치로 박스가 이동하기 위해서 next설정
        Vector3 selectNextBox = new Vector3(bottomPanelRect[1].anchoredPosition.x, initY, 0);
        buttonBackGround.transform.localPosition = new Vector3(bottomPanelRect[1].anchoredPosition.x, initY, 0);

        if (curPanel != afterPanel)
        {
            iTween.MoveTo(bottomMoveRect.gameObject, iTween.Hash("islocal", true,
                                                            "position", selectNextBox,
                                                            "oncomplete", "afterChange",
                                                            "easetype", "easeOutQuart",
                                                            "time", .7f));
        }
    }

    public void SetCurPanelID_3()
    {
       curPanel = 3;
        // 클릭된 버튼의 위치로 박스가 이동하기 위해서 next설정
        Vector3 selectNextBox = new Vector3(bottomPanelRect[2].anchoredPosition.x, initY, 0);

        buttonBackGround.transform.localPosition = new Vector3(bottomPanelRect[2].anchoredPosition.x, initY, 0);

        if (curPanel != afterPanel)
        {
            iTween.MoveTo(bottomMoveRect.gameObject, iTween.Hash("islocal", true,
                                                            "position", selectNextBox,
                                                            "oncomplete", "afterChange",
                                                            "easetype", "easeOutQuart",
                                                            "time", .7f));
        }
    }

    public void SetCurPanelID_4()
    {
        curPanel = 4;
        // 클릭된 버튼의 위치로 박스가 이동하기 위해서 next설정
        Vector3 selectNextBox = new Vector3(bottomPanelRect[3].anchoredPosition.x, initY, 0);

        buttonBackGround.transform.localPosition = new Vector3(bottomPanelRect[3].anchoredPosition.x, initY, 0);

        if (curPanel != afterPanel)
        {
            iTween.MoveTo(bottomMoveRect.gameObject, iTween.Hash("islocal", true,
                                                            "position", selectNextBox,
                                                            "oncomplete", "afterChange",
                                                            "easetype", "easeOutQuart",
                                                            "time", .7f));
        }
    }

    void afterChange()
    {
        afterPanel = curPanel;
        //scRect1.content.localPosition = new Vector3(0, -1000, 0);
        //scRect2.content.localPosition = new Vector3(0, -1000, 0);
    }

    //-------------------------------------

    //void LimitVertical(float limitY)
    //{
    //    //현재 패널이 제한된 vertical 을 넘지 않게 한다.
    //    //현재 패널의 id를 확인한다.
    //    //패널의 제한 위치를 파악한다.
    //    //현재패널의 좌표를 구한다.
    //    //이동한 좌표를 파악할수 있는가?
    //    for (int i = 0; i < iPanelNum; i++)
    //    {
    //        if (stPanels[i].bIsActive)
    //        {
    //            if (stPanels[i].curPosY <= limitY)
    //            {
    //                //일단 밑으로 내려가지 않게 한다.
    //                print("오우 노~~~~끝이여");
    //                //밑으로 내려가지 않게한다.
    //                panelRECT[i].localPosition = new Vector3(panelRECT[i].localPosition.x,0,panelRECT[i].localPosition.z);
    //            }
    //        }
    //    }
    //}


    //void OnBeginDrag(PointerEventData eventData)
    //{

    //    //y가 0보다 밑으로 내려가지 않게 한다.
    //    //그러나 드레그시 그대로 밑으로 내려감 형식적 위치
    //    LimitVertical(-100f);


    //    print("OnBeginDrag 작동함");

    //}
    //public void OnDrag(PointerEventData eventData)
    //{
    //    LimitVertical(-200f);
    //    print("OnDrag 작동함" + "ped.posiiton");
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    print("OnDrag 작동함" + "ped.posiiton");
    //}
    void SetStrollRectID()
    {
        ped.position = Input.mousePosition;
        //모든 레이캐스터의 위치를 생성
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        //현재 충돌된 레이캐스터를 찾는다.
        gr.Raycast(ped, results);

        if (EventSystem.current.IsPointerOverGameObject())
        {
            for (int i = 0; i < results.Count;i++)
            {
                GameObject temp = results[i].gameObject;
                if (temp.name == "PanelScroll_Market")
                {
                    //print("야호 찾았다panelscroll_market");
                    stPanels[0].bIsActive = true;
                    stPanels[1].bIsActive = false;
                    stPanels[2].bIsActive = false;

                }
                else if (temp.name == "PanelScroll_UnityChoice")
                {
                    //print("야호 찾았다PanelScroll_UnityChoice");
                    stPanels[0].bIsActive = false;
                    stPanels[1].bIsActive = true;
                    stPanels[2].bIsActive = false;
                }
                else if (temp.name == "PanelScroll_GameChoice")
                {
                    //print("야호 찾았다PanelScroll_GameChoice");
                    stPanels[0].bIsActive = false;
                    stPanels[1].bIsActive = false;
                    stPanels[2].bIsActive = true;
                }
            }
        }
    }



    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    Vector3 tempPos = ped.position;
    //    print("OnPointerClick 작동함" );
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    Vector3 tempPos = ped.position;
    //    print("OnPointerUp 작동함" );
    //}


    // Debug-----일정시간마다 체크해서 판넬의 위치가 올바른지 검사 한다.---
    IEnumerator coReConFirm()
    {
        ReConFirm();
        yield return new WaitForSeconds(1f);
      yield  return StartCoroutine("coReConFirm");
    }

   public  void ReConFirm()
    {
        //처음과 마지막 판넬의 위치로 판단후 초기화 시킴
        if (stPanels[0].curPosX > 0)
        {
            for (int i = 0; i < iPanelNum; i++)
            {
            // panelRECT[i].localPosition = new Vector3(stPanels[0].posX ,panelRECT[i].localPosition.y, panelRECT[i].localPosition.z);
               panelRECT[i].localPosition = new Vector3(stPanels[0].posX,-1000f, panelRECT[i].localPosition.z);
            }
            InitPanel();
        }

        if (stPanels[iPanelNum-1].curPosX < 0 )
        {
            for (int i = 0; i < iPanelNum; i++)
            {
               // panelRECT[i].localPosition = new Vector3(stPanels[0].posX, panelRECT[i].localPosition.y, panelRECT[i].localPosition.z);
                panelRECT[i].localPosition = new Vector3(stPanels[0].posX, -1000f, panelRECT[i].localPosition.z);
            }
            InitPanel();
        }
    }

   


    
}
