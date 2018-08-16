using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// UI에서 tab Tab1 ,Tab2,Tab3..버튼 기능을 컨트롤한다.
public class UI_TabControl : MonoBehaviour {

    public List<RectTransform> tab        = new List<RectTransform>();   //버튼
    Dictionary<int, bool> tabEnable       = new Dictionary<int, bool>(); //tab 선택시 다른 선택되지않음 버튼의 선택유무 
    public List<RectTransform> ImgBtn     = new List<RectTransform>();   //tab 버튼이 선택됐을대 카드가 위치하게 될곳 
    public List<RectTransform> panelBtn   = new List<RectTransform>();   //선택정렬이 되는 slot
    public List<RectTransform> panelSwBtn = new List<RectTransform>();   //선택정렬이 되는 slot가 선택시 확대

    IList<RectTransform> lsSlots1 = new List<RectTransform>();  //slot의 위치 
    IList<RectTransform> lsSlots2 = new List<RectTransform>();  //slot의 위치 
    IList<RectTransform> lsSlots3 = new List<RectTransform>();  //slot의 위치

   
    IList<RectTransform> swSlots1 = new List<RectTransform>();  //switchSlots
    IList<RectTransform> swSlots2 = new List<RectTransform>();  //switchSlots
    IList<RectTransform> swSlots3 = new List<RectTransform>();  //switchSlots

    IList<RectTransform> lsSwtichSlots = new List<RectTransform>();  //swtichSlots

    public RectTransform choiceImg; //선택될때 표시될 이미지RectTransform

    [SerializeField]
    public RectTransform afterPos; //움직이기 이전의 위치
    List<RectTransform> tabPos = new List<RectTransform>(); //tab의 좌표
    public GameObject cardObjTop;      //복사될 버튼오브젝트 
    public GameObject cardObjFront;      
    public GameObject cardObjBack;     //선택시 활성화될 버튼
    int curTab;
    int curTabButton = 1;

    //전체 UnityCard데이터로 움직이게 변경한다.
    UnitySlot[] slots1;
    UnitySlot[] slots2;
    UnitySlot[] slots3;

  

    UnityCard[]  sSlots1Cards;
    UnityCard[]  sSlots2Cards;
    UnityCard[]  sSlots3Cards;

    UnityCard[]  swSlots1Cards;
    UnityCard[]  swSlots2Cards;
    UnityCard[]  swSlots3Cards;

    //Tab버튼 3개
    int[] cur = new int[3];

    Vector3 selectCurBox;            //현재 막스가 위치하는 위치
    Vector3 selectNextBox;           //다음 이동해야할 포지션

   

    private void Start()
    {
        //시작과 동시에 tab버튼의 좌표를 확인한다.
        for (int i = 0; i < tab.Count; i++)
        {
            RectTransform tmp = tab[i];
            tabPos.Add(tmp);
            tabEnable.Add(i, false);
        }

        //lsSlots는 Slots의 좌표
        lsSlots1 = panelBtn[0].GetComponent<UI_GridGroup>().lsrcTransforms;
        lsSlots2 = panelBtn[1].GetComponent<UI_GridGroup>().lsrcTransforms;
        lsSlots3 = panelBtn[2].GetComponent<UI_GridGroup>().lsrcTransforms;

       
   
        //정렬버튼이 선택시 선택됐을때 
        swSlots1 = panelSwBtn[0].GetComponent<UI_GridGroup>().lsrcTransforms;
        swSlots2 = panelSwBtn[1].GetComponent<UI_GridGroup>().lsrcTransforms;
        swSlots3 = panelSwBtn[2].GetComponent<UI_GridGroup>().lsrcTransforms;
        //처음 시작시0번의 위치에 있어야 하지만 나중에 playerData로 저장해서 기록이 남아있게 해야한다.
        choiceImg.anchoredPosition = tab[0].anchoredPosition;
        //이동하기 전위치 
        selectCurBox = Vector3.zero;

       
        slots1 = ImgBtn[0].gameObject.GetComponentsInChildren<UnitySlot>();
        slots2 = ImgBtn[1].gameObject.GetComponentsInChildren<UnitySlot>();
        slots3 = ImgBtn[2].gameObject.GetComponentsInChildren<UnitySlot>();

       

        sSlots1Cards = panelBtn[0].GetComponentsInChildren<UnityCard>();
        sSlots2Cards = panelBtn[1].GetComponentsInChildren<UnityCard>();
        sSlots3Cards = panelBtn[2].GetComponentsInChildren<UnityCard>();



        //정렬버튼이 선택시 선택됐을때    UnityCards
        swSlots1Cards = panelSwBtn[0].GetComponentsInChildren<UnityCard>();
        swSlots2Cards = panelSwBtn[1].GetComponentsInChildren<UnityCard>();
        swSlots3Cards = panelSwBtn[2].GetComponentsInChildren<UnityCard>();


        SelectDeckMade();
        //처음 시작시 정렬탭이 선택되게 하기 위해서 
        ButtonTab1();
        //isSelectDeck();
        //tabEnable[0] = true; 중에 선택된 것이 없다면 
        //Slots 처음에는 전부 보이지 않게 한다.
        //lsSlots는  UnityCards

        //Object  Set 
        ImgBtn[0].gameObject.SetActive(true);
        ImgBtn[1].gameObject.SetActive(false);
        ImgBtn[2].gameObject.SetActive(false);

        panelBtn[0].gameObject.SetActive(true);
        panelBtn[1].gameObject.SetActive(false);
        panelBtn[2].gameObject.SetActive(false);

        panelSwBtn[0].gameObject.SetActive(true);
        panelSwBtn[1].gameObject.SetActive(false);
        panelSwBtn[2].gameObject.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            HideSelectInt(1);
            HideSelectInt(2);
            HideSelectInt(3);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {

                // 현재 어떤 tab 버튼이 선택됐는지 판단해서  tem 리스트를 만든다.
                foreach (KeyValuePair<int, int[]> pair in GameData.Instance.playerSelectDecks)
                {
                    if (pair.Key == 0)
                    {
                         int []  a = pair.Value;
                        for (int i = 0; i < a.Length; i++)
                        {
                            Debug.Log("000000--"+i+":"+a[i]);
                        }
                    }

                    if (pair.Key == 1)
                    {
                        int[] a = pair.Value;
                        for (int i = 0; i < a.Length; i++)
                        {
                            Debug.Log("1111111--" + i + ":" + a[i]);
                        }
                    }

                    if (pair.Key == 2)
                    {
                        int[] a = pair.Value;
                        for (int i = 0; i < a.Length; i++)
                        {
                            Debug.Log("2222222---" + i + ":" + a[i]);
                        }
                    }
                }

        }

    }

    //탭버튼 시스템
    void SelectDeckMade()
    {
     //Tab의 위치에 카드가생성되게 한다.
       tabPosition(1);
       tabPosition(2);
       tabPosition(3);

    }

    //텝별로 playerSelectDecks에서 직접불러옴
    void tabPosition(int btnPos)
    {      //btn [1,2,3]   default = 0    
     
        if (btnPos != GameData.Instance.CurTab)
        {
            int[] tempbtn = new int[4];
            IList<int> hasTemp = new List<int>(); //tob1의 카드를 빼기 위해서 임시로 생성
            int[] tem = new int[4];               //확인필요 tab 당 원소는4 에 선택된 카드
                                
            //현재 player가 가지고 있는 카드 리스트를 만든다.
            foreach (KeyValuePair<int, Card> pair in GameData.Instance.hasCard)
            {       
                hasTemp.Add(pair.Value.ID);
            }

            // 현재 어떤 tab 버튼이 선택됐는지 판단해서  tem 리스트를 만든다.
            foreach (KeyValuePair<int, int[]> pair in GameData.Instance.playerSelectDecks)
            {
                if (pair.Key == ( btnPos -1))
                {
                    tem = pair.Value;
                }
            }

            for (int i = 0; i < tem.Length; i++)
            {
                GameObject temp = Instantiate(cardObjTop);
                // i인덱스 
                int id = tem[i]; 
                //tab의 이미지가 위치해야될 곳임 1  ImgBtn[btnPos];
                if (btnPos == 1)
                {
                    temp.transform.SetParent(slots1[i].transform,false);
                }
                else if (btnPos == 2)
                {
                    temp.transform.SetParent(slots2[i].transform, false);
                }
                else if (btnPos == 3)
                {
                    temp.transform.SetParent(slots3[i].transform, false);
                }

                //부모에 따라서 상대위치가 달라지므로 다시 스케일 세팅해야됨
                temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                // x,y는 영향을 주지 못함
                temp.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

                temp.GetComponent<UnityCard>().Elixer    = GameData.Instance.UnityDatas[id].Elixir;
                temp.GetComponent<UnityCard>().Icon_name = GameData.Instance.UnityDatas[id].Name;
                temp.GetComponent<UnityCard>().ID        = GameData.Instance.UnityDatas[id].Id;
                temp.GetComponent<UnityCard>().SlotIndex = i;
                //enum 으로 리스트를 위한 number을 얻는다.
                temp.GetComponent<UnityCard>().Kinds = GameData.Instance.UnityDatas[id].Kinds;
                //TODO: 리소스를 불러와서 교체하는 구간임 kind타입에 따라 아이콘의 모양이 달라지게 한다.

                //if (temp.GetComponent<UnityCard>().kindNum >= 1 && temp.GetComponent<UnityCard>().kindNum < 3)
                //{
                //    temp.GetComponent<UnityCard>().mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", "Sample_UI_1");
                //    //전체 테두리
                //    temp.GetComponent<Image>().sprite = SpriteManager.GetSpriteByName("Sprite", "Sample_UI_1");
                //}
            } //for End;

            for (int i = 0; i < GameData.Instance.hasCard.Count; i++)
            {
                for (int j = 0; j < tem.Length; j++)
                {
                    if (hasTemp[i] == tem[j])
                    {
                        //Tab 이외에 정렬된 ItemCard
                        hasTemp.RemoveAt(i);
                        hasTemp.Insert(i, 0);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            int switchIndex = 0;
            for (int i = 0; i < hasTemp.Count; i++)
            {
                if (hasTemp[i] > 0)
                {
                    //false일때 생성
                    GameObject sample1 = Instantiate(cardObjFront);                        
                    GameObject sample2 = Instantiate(cardObjBack);
                    switch (btnPos)
                    {
                        case 1:
                          
                            sample1.transform.SetParent(lsSlots1[switchIndex].transform, false);
                            sample1.GetComponent<UnityCard>().rootIndex = switchIndex;
                            sample1.GetComponent<UnityCard>().SlotIndex = switchIndex;

                          
                            sample2.transform.SetParent(swSlots1[switchIndex].transform, false);
                            sample2.GetComponent<UnityCard>().rootIndex = switchIndex;
                            sample2.GetComponent<UnityCard>().SlotIndex = switchIndex;

                            break;
                        case 2:
                           
                            sample1.transform.SetParent(lsSlots2[switchIndex].transform, false);
                            sample1.GetComponent<UnityCard>().rootIndex = switchIndex;
                            sample1.GetComponent<UnityCard>().SlotIndex = switchIndex;

                          
                            sample2.transform.SetParent(swSlots2[switchIndex].transform, false);
                            sample2.GetComponent<UnityCard>().rootIndex = switchIndex;
                            sample2.GetComponent<UnityCard>().SlotIndex = switchIndex;

                            break;
                        case 3:
                           
                            sample1.transform.SetParent(lsSlots3[switchIndex].transform, false);
                            sample1.GetComponent<UnityCard>().rootIndex = switchIndex;
                            sample1.GetComponent<UnityCard>().SlotIndex = switchIndex;

                           
                            sample2.transform.SetParent(swSlots3[switchIndex].transform, false);
                            sample2.GetComponent<UnityCard>().rootIndex = switchIndex;
                            sample2.GetComponent<UnityCard>().SlotIndex = switchIndex;

                            break;
                        default:
                            break;
                    }


                    ////sloat의 크기를 입력해준다.
                    sample1.GetComponent<UnityCard>().width = 260f;
                    sample1.GetComponent<UnityCard>().height = 400f;
                    sample1.GetComponent<UnityCard>().ID = hasTemp[i]; //키와 같다면 id를 입력한다.
                   
                    sample1.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);//부모의 위치 세팅
                    sample1.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);


                    ////sloat의 크기를 입력해준다.
                    sample2.GetComponent<UnityCard>().width = 260f;
                    sample2.GetComponent<UnityCard>().height = 400f;
                    sample2.GetComponent<UnityCard>().ID = hasTemp[i]; //키와 같다면 id를 입력한다.
                   
                    sample2.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);//부모의 위치 세팅
                    sample2.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    switchIndex++;
                } //if END

            } //for end
        } //if END 
     
    }


    //여기아직 사용하면 문제 발생 
   void tabShow()
    {
        //  딕셔너리 목록을 검사해서 teue일때 활성화 false일때 보이지 않게 비활성화 시킨다.
        foreach (KeyValuePair<int,bool> pair in tabEnable)
        {
            if (pair.Value == true)
            {
                ImgBtn[pair.Key].gameObject.SetActive(true);
            }
            if(pair.Value == false)
            { 
                ImgBtn[pair.Key].gameObject.SetActive(false);
            }
        }
    }

   
    //이벤트 시스템에서는 인수를 넘길수없다
    public void ButtonTab1()
    {
        // 클릭된 버튼의 위치로 박스가 이동하기 위해서 next설정
        selectNextBox = tab[0].anchoredPosition;

        if (selectCurBox != selectNextBox)
        {
         iTween.MoveTo(choiceImg.gameObject, iTween.Hash("islocal", true,
                                                         "position", selectNextBox,
                                                         "oncomplete","CurNextChange",
                                                         "easetype", "easeOutQuart",
                                                         "time", .5f));
        }
          ImgBtn[0].gameObject.SetActive(true);
          ImgBtn[1].gameObject.SetActive(false);
          ImgBtn[2].gameObject.SetActive(false);

          panelBtn[0].gameObject.SetActive(true);
          panelBtn[1].gameObject.SetActive(false);
          panelBtn[2].gameObject.SetActive(false);

          panelSwBtn[0].gameObject.SetActive(true);
          panelSwBtn[1].gameObject.SetActive(false);
          panelSwBtn[2].gameObject.SetActive(false);


        GameData.Instance.CurTab = 1;
        GameData.Instance.panelSlots = null;
                                     //바뀐 panelBtn 의 정보를 담기위해서 GetComponent 필요
        GameData.Instance.panelSlots = panelBtn[0].GetComponentsInChildren<UnityCard>();

        GameData.Instance.panelBackSlots = null;
        GameData.Instance.panelBackSlots = panelSwBtn[0].GetComponentsInChildren<UnityCard>();


    }

    public void ButtonTab2()
    {
        selectNextBox = tab[1].anchoredPosition;
        if (selectCurBox != selectNextBox)
        {
                iTween.MoveTo(choiceImg.gameObject, iTween.Hash("islocal", true,
                                                                 "position", selectNextBox,
                                                                 "oncomplete", "CurNextChange",
                                                                 "easetype", "easeOutQuart",
                                                                 "time", .5f));
        }
         ImgBtn[0].gameObject.SetActive(false);
         ImgBtn[1].gameObject.SetActive(true);
         ImgBtn[2].gameObject.SetActive(false);

         panelBtn[0].gameObject.SetActive(false);
         panelBtn[1].gameObject.SetActive(true);
         panelBtn[2].gameObject.SetActive(false);

        panelSwBtn[0].gameObject.SetActive(false);
        panelSwBtn[1].gameObject.SetActive(true);
        panelSwBtn[2].gameObject.SetActive(false);

        GameData.Instance.CurTab = 2;
        //선택된 카드중 정렬탭의 카드를 정렬 시키기 위해서 데이터화 한다.
        GameData.Instance.panelSlots = null;
        GameData.Instance.panelSlots = panelBtn[1].GetComponentsInChildren<UnityCard>();
        GameData.Instance.panelBackSlots = null;
        GameData.Instance.panelBackSlots = panelSwBtn[1].GetComponentsInChildren<UnityCard>();
    }

    public void ButtonTab3()
    {
        // tabEnable[2] = true;
        selectNextBox = tab[2].anchoredPosition;
        if (selectCurBox != selectNextBox)
        { 
            iTween.MoveTo(choiceImg.gameObject, iTween.Hash("islocal", true,
                                                             "position", selectNextBox,
                                                             "oncomplete", "CurNextChange",
                                                             "easetype", "easeOutQuart",
                                                             "time", .5f));
        }
        ImgBtn[0].gameObject.SetActive(false);
        ImgBtn[1].gameObject.SetActive(false);
        ImgBtn[2].gameObject.SetActive(true);

        panelBtn[0].gameObject.SetActive(false);
        panelBtn[1].gameObject.SetActive(false);
        panelBtn[2].gameObject.SetActive(true);

        panelSwBtn[0].gameObject.SetActive(false);
        panelSwBtn[1].gameObject.SetActive(false);
        panelSwBtn[2].gameObject.SetActive(true);

        GameData.Instance.CurTab = 3;
        GameData.Instance.panelSlots = null;
        GameData.Instance.panelSlots = panelBtn[2].GetComponentsInChildren<UnityCard>();
        GameData.Instance.panelBackSlots = null;
        GameData.Instance.panelBackSlots = panelSwBtn[2].GetComponentsInChildren<UnityCard>();
    }

    void CurNextChange()
    {
        //이동완료후 next 를 cur로 바꿔줘야 한다.
        selectCurBox = selectNextBox;
    }

    #region What?

    ////Tab 에 아이템을 만든다.
    //void isSelectDeck()
    //{
    //    // is Show버튼 숫자 tab의 3중 
    //    bool[] isShow = new bool[cur.Length];

    //    foreach (KeyValuePair<int, int[]> pair in GameData.Instance.playerSelectDecks)
    //    {   //시작시 selectDeck을 전부 가지고 있나 확인,비어있는지 판단
    //        for (int i = 0; i < cur.Length; i++)
    //        {                 //cur  3
    //            if (pair.Key == i)
    //            {  
    //                int[] tem = pair.Value;
    //                if (0 < tem.Length)
    //                {  // 0,true,  1,true  2,true로 선택된 것이 전부있다.
    //                    isShow[i] = true;
    //                }
    //            }
    //        }
             
    //        for (int i = 0; i < isShow.Length; i++)
    //        {
    //            //모두 있을때 true,true,true 이므로 isTrue 3
    //            int isFalse = 0;
    //            int isTrue = 0;

    //            if (isShow[i] == false)
    //            {
    //                isFalse++;
    //            }
    //            else
    //            {
    //                isTrue++;
    //            }
    //            // 3                3             3
    //            if (isTrue > 1 && isTrue <= isShow.Length)
    //            {
    //                //2,3     //1,2,3
    //                //GameData에서 저장된 tab를 가져와서 현재의 버튼저장
    //                curTabButton = GameData.Instance.curTab;
    //                //tab버튼을 SetActivate(true)
    //            }
    //            else if (isTrue == 1)
    //            {
    //                for (int j = 0; j < isShow.Length; j++)
    //                {
    //                    if (isShow[j] == true)
    //                    {    //tab 버튼 인덱스가 1부터라서 +1
    //                        curTabButton = j + 1;
    //                        //게임데이터에도 설정해 준다.
    //                        GameData.Instance.curTab = j + 1;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}//Tab END
    #endregion

    //버튼이 선택됐을때 다른 특수 버튼이 보일수있게 맨앞에 위치하게 한다.
    public  void showSelectItem()
    {
        //현재 활성화된 탭
        int curTabb = GameData.Instance.CurTab;
        int curId   = GameData.Instance.curSwitchCard; //여기 계속해서 0인지 판단한다.
        int curSlot = GameData.Instance.CurSlotID;

        switch (curTabb)
        {
            case 1:
                swSlots1[curSlot].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(true);
                break;
            case 2:
                swSlots2[curSlot].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(true);
                break;
            case 3:
                swSlots3[curSlot].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(true);
                break;
        }
    }



    public void HideSelectItem()
    {

        int curTabb = GameData.Instance.CurTab;
        int num     = GameData.Instance.panelSlots.Count;
       
        switch (curTabb)
        {
            case 1:
                for (int i = 0; i < num; i++)
                {
                    swSlots1[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(false);
                }
               
                break;
            case 2:

                for (int i = 0; i < num; i++)
                {
                    swSlots2[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(false);
                }

                break;
            case 3:
                for (int i = 0; i < num; i++)
                {
                    swSlots3[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(false);
                }
                break;

        }
    }

      //num 1,2,3
    void HideSelectInt(int Num)
    {

        int num = GameData.Instance.panelSlots.Count;
        switch (Num)
        {
            case 1:
                for (int i = 0; i < num; i++)
                {
                    swSlots1[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(false);
                }

                break;
            case 2:

                for (int i = 0; i < num; i++)
                {
                    swSlots2[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(false);
                }

                break;
            case 3:
                for (int i = 0; i < num; i++)
                {
                    swSlots3[i].GetComponentInChildren<UnityCard>().hideButton.gameObject.SetActive(false);
                }
                break;
        }
    }

    public void SwitchSlot()
    {
        //from을 GameData.Instance로 부터 얻어 온다.
        //from  tab1 , slot , id
        int curId        = GameData.Instance.curSwitchCard;
        //복사되어진 인덱스 의 위치 
        int curSlot      = GameData.Instance.fromSwitchSlot;
        int curTab       = GameData.Instance.CurTab;
        int curSlotIndex = GameData.Instance.CurSlotID;

        switch (curTab)
        {
            case 1:
                //ERROR 코드 발생 :  첫줄만 실해되고 다음줄은 실행되지 않는다
                //sampleBack  정렬탭에 교체 되어야할 ID //GetComponent없애기
                //sSlots1Cards[curSlot].ID = curId;
                //swSlots1Cards[curSlot].ID = curId;

                lsSlots1[curSlot].GetComponentInChildren<UnityCard>().ID = curId;
                swSlots1[curSlot].GetComponentInChildren<UnityCard>().ID = curId;
               //sampleFront

                // 현재 어떤 tab 버튼이 선택됐는지 판단해서  tem /리스트를 만든다.
                foreach (KeyValuePair<int, int[]> pair in GameData.Instance.playerSelectDecks)
                    {
                    if (pair.Key == 0)
                        {
                        //이동하기 위해 복사되어 있는 카드 의 ID
                        int val = GameData.Instance.fromSwitchId;
                         pair.Value[curSlotIndex] = val;
                   
                        }
                    }
               
              
                break;
            case 2:

                //sSlots2Cards[curSlot].ID = curId;
                //swSlots2Cards[curSlot].ID = curId;

                lsSlots2[curSlot].GetComponentInChildren<UnityCard>().ID = curId;
                swSlots2[curSlot].GetComponentInChildren<UnityCard>().ID = curId;
               

                // 현재 어떤 tab 버튼이 선택됐는지 판단해서  tem 리스트를 만든다.
                foreach (KeyValuePair<int, int[]> pair in GameData.Instance.playerSelectDecks)
                {
                    if (pair.Key == 1)
                    {
                        int val = GameData.Instance.fromSwitchId;
                        //이동하기 위해 복사되어 있는 카드 의 ID
                        pair.Value[curSlotIndex] = val;
                    }
                }
                break;

            case 3:
            

                lsSlots3[curSlot].GetComponentInChildren<UnityCard>().ID = curId;
                swSlots3[curSlot].GetComponentInChildren<UnityCard>().ID = curId;
               
                // 현재 어떤 tab 버튼이 선택됐는지 판단해서  tem 리스트를 만든다.
                foreach (KeyValuePair<int, int[]> pair in GameData.Instance.playerSelectDecks)
                {
                    if (pair.Key == 2)
                    {
                        //이동하기 위해 복사되어 있는 카드 의 ID
                        int val = GameData.Instance.fromSwitchId;
                        pair.Value[curSlotIndex] = val;
                    }
                }
                break;

            default:
                break;

        } //Switch END

    }

    #region Sample
    //void UnFindCardSet()
    //{

    //    IList<int> hasId = new List<int>();
    //    IList<int> datasId = new List<int>();

    //    foreach (KeyValuePair<int, Card> pair in GameData.Instance.hasCard)
    //    {
    //        hasId.Add(pair.Key); //ID ,
    //    }

    //    for (int i = 0; i < GameData.Instance.infoCards; i++)
    //    {
    //        datasId.Add(GameData.Instance.UnityDatas[i].Id);
    //    }

    //    for (int i = 0; i < GameData.Instance.infoCards; i++)
    //    {
    //        for (int j = 0; j < hasId.Count; j++)
    //        {
    //            if (datasId[i] == hasId[j])
    //            {
                 
    //                datasId.Remove(i);
    //                datasId.Insert(i, 0);
    //                break;
    //            }
    //            else 
    //            {
    //                continue;
    //            }
    //        }
    //    }

    //    foreach (var i in datasId)
    //    {
    //        if (i > 0)
    //        {

    //        }
    //    }
    //}
    #endregion




}
