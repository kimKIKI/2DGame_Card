using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardEff_Open : MonoBehaviour {

    public   delegate   void    CardEff();
    public   static     event   CardEff  eff;
    public   static     event   CardEff  cardUp;

    enum eState
    {
        NONE,
        SHOW,
        OPEN,
        ITEMCLICK,
        CLOSE,
    }

    //open된것중 어떤 타입의 아이템이 선택됐는지 판단해서 오브젝트를 바꾸어주어야 한다.
    enum eOpen
    {
        OPEN,
        GOLD,
        JEW,
        UNITY,
        RARES,
        HEROS,
        LEGENDS,
        CLOSE,
    }

    //----드레깅-----------------------
    public Transform    Image;
    public GameObject   iconImgGM;
    public GameObject   ImgOpen;
    public GameObject   ImgClose;
    public Transform    title;       //카드의 이름
    public GameObject   arrowLevel;  //card amount 증가
    public GameObject   arrowGold;   //골드 증가
    public GameObject   arrowJew;    //보석 증가
    public GameObject   arrow;       //화살표의 위로올라가는 애니
    public GameObject   cardNum;     //상자가 열렸을때  몇개의 상품항목이 있는지 보여줄 것
    public GameObject   FadeOut;     //fadeOut
    public Transform    panel;       //활성화시 꺼놓은 것을 활성화 시키기 위해서 사용
  
    
    float        openImgScAmount = 5f;//크기를 조절할수있게 
    Vector3      openImglocalScale;   //openImage원래 크기 

    Image        iconImg;                    //바뀌어 보이는 이미지
    Image        opImg;
    Image        closeImg;
    // Use this for initialization
    Image        icon;                      //카드의 케릭터 이미지 
    Image        cardbase;                  //카드의 테두리 이미지 
    int          Gold;
    int          index;                     //현제 선택된 카드의 인덱스 Datas -1되어 있음
    RectTransform rtImg;             //default  width:200  height:240
   // Vector2 rtImg;
    Vector3      initScale;
    float        height;
    float        width;
    Button       btn;

    //public Transform[] path;

    int    curGold;
    int    curJew;                 //현재 까지저장된 jew
    int    cuItemID;               //현재 선택된 카드 ID

    int    curItemNum;             //현재 서택된 카드의 수량

    int    baseLevel;              //레벨숫자를 공통으로 사용하게 한다.

    Text   txGold;
    Text   txJew;
    Text   txLevel;
    Text   itemNum;                //상자내 표시 카운트 
    Text[] titleNames;
    bool   isOpen = false;         //처음 실행인지 판단하기 위해서 
                                   //임시로 이벤트발생시 아이템의 목록을 기입한다.
    bool isReOpen = false;         //상품을 바꾸어 줄때 이전상품을 hide하게 한다.
   
    public static string keyName = "";

    int productNum;                //전체 상품수량
                                   //보석상자에서 입력받을 증가시킬value 
    int Addgold;
    int AddJew;
    int Addunits;
    int Addrarts;
    int Addheros;
    int Addheros1;
    int Addheros2;
    int Addlengends;

    string legOpenImg   = "legendaryCase_bottom";
    string legCloseImg  = "legendaryCase";
    string luckOpenImg  = "luckyCase_bottom";
    string luckCloseImg = "luckyCase";
    string heroOpenImg  = "HeroCase";
    string heroCloseImg = "HeroCase";

    [SerializeField]
     eOpen curOpen = eOpen.OPEN;

    IList<UnityInfo> units     = new List<UnityInfo>();
    IList<UnityInfo> rairtys   = new List<UnityInfo>();
    IList<UnityInfo> heros     = new List<UnityInfo>();
    IList<UnityInfo> legenards = new List<UnityInfo>();
    IList<UnityInfo> unitsSuffle = new List<UnityInfo>();
    IList<UnityInfo> rairtysSuffle = new List<UnityInfo>();
    IList<UnityInfo> herosSuffle = new List<UnityInfo>();
    IList<UnityInfo> legenardsSuffle = new List<UnityInfo>();

    //-----------------------------------------------------------
    private void Awake()
    {
        btn       = GetComponent<Button>();
        icon      = Image.GetComponentInChildren<Image>(); //보여질 아이콘 이미지
        cardbase  = Image.GetComponent<Image>();            //카드이미지 
        height    = Image.GetComponent<RectTransform>().rect.height;
        width     = Image.GetComponent<RectTransform>().rect.width;
        rtImg     = Image.GetComponent<RectTransform>();
        initScale = Image.localScale;
        txGold    = arrowGold.GetComponentInChildren<Text>();
        txJew     = arrowJew.GetComponentInChildren<Text>();
        txLevel   = arrowLevel.GetComponentInChildren<Text>();
        //titleName[0] Title [1] Kinds  [2] Level
        titleNames = title.GetComponentsInChildren<Text>();
        iconImg   = iconImgGM.GetComponentInChildren<Image>();
        opImg     = ImgOpen.GetComponent<Image>();
        closeImg  = ImgClose.GetComponent<Image>();
                  
        itemNum  = cardNum.GetComponentInChildren<Text>();
      
        openImglocalScale = ImgOpen.transform.localScale;
        rtImg.sizeDelta = new Vector2(100, height);

        btn.onClick.AddListener(delegate 
        {
            ClickCount();
           
        });


    }

    //string name = GameData.Instance.UnityDatas[ID - 1].Name;
    //main.sprite = SpriteManager.GetSpriteByName("Sprite", name);

    private void OnEnable()
    {
        Create_FADEOUT(FadeOut, transform.parent.parent);
        StartCoroutine(SetCardOpen());

        opImg.enabled    = false;
        closeImg.enabled = true;
        //데이터가 연동되어야할 UnityCard,Daily그룹에 자신의 이벤트를 등록시킨다.
        for (int i = 0; i < GameData.Instance.days.Count; i++)
        {
            GameData.Instance.days[i].GetComponent<DailySale>().CardEff_Event();
        }

        for (int i = 0; i < GameData.Instance.uCards.Count; i++)
        {
            GameData.Instance.uCards[i].GetComponent<UnityCard>().OnEventUpdate();
        }
    }


    //상품선택시 업데이트된 내용을 다른 카드들에 전달한다.
    void updateCard()
    {
            switch (curOpen)
            {
                case eOpen.OPEN:


                    break;
                case eOpen.GOLD:
                  
                    break;
               
                case eOpen.UNITY:

                    //ID 번호로 hasCard의 값을 변경시킨다.
                    if (GameData.Instance.hasCard.ContainsKey(index))
                    {     //key
                        GameData.Instance.hasCard[index].hasCard += Addunits;
                    }

                    break;
                case eOpen.RARES:
                    
                    if (GameData.Instance.hasCard.ContainsKey(index))
                    {     //key
                        GameData.Instance.hasCard[index].hasCard += Addrarts;
                        
                    }

                    break;
                case eOpen.HEROS:

                  
                    if (GameData.Instance.hasCard.ContainsKey(index))
                    {     //key
                        GameData.Instance.hasCard[index].hasCard += Addheros;
                    }

                    break;

                case eOpen.LEGENDS:

                  
                    if (GameData.Instance.hasCard.ContainsKey(index))
                    {     //key
                        GameData.Instance.hasCard[index].hasCard += Addlengends;
                    }
                    break;

                case eOpen.CLOSE:

                    break;
            }
    }

    //TODO:제너릭으로 바꿔야 할부분
    public IList<UnityInfo> shuffle(IList<UnityInfo> source)
    {
        IList<UnityInfo> target = new List<UnityInfo>();
        int currentCount = 0;
        while (0 < source.Count)
        {
            currentCount = Random.Range(0, source.Count);
            target.Add(source[currentCount]);
            source.Remove(source[currentCount]);
        }
        return target;
    }

    IEnumerator SetCardOpen()
    {
        yield return new WaitForSeconds(0.4f);

        panel.gameObject.SetActive(true);

        //dic_SetItems
        if (GameData.Instance.dic_SetItems.ContainsKey(keyName))
        {
            if (keyName == "lunchyCase")
            {
                // ItemBoxInfo 
                var caseBox = GameData.Instance.dic_SetItems["lunchyCase"];

                //AddJew        = caseBox.Jew;
                Addunits        = caseBox.generalNum;
                Addrarts        = caseBox.RareNum;
                Addheros        = caseBox.HeroNum;
                Addlengends     = caseBox.LengendaryNum;
                this.productNum = caseBox.productNum;

               
                //상자가 아직 안열렸을때 이미지
                closeImg.sprite = SpriteManager.GetSpriteByName("Sprite",luckCloseImg);
                //열렸을때 이미지 
                opImg.sprite = SpriteManager.GetSpriteByName("Sprite", luckOpenImg);

            }
            else if (keyName == "legendaryCase")
            {
                var caseBox = GameData.Instance.dic_SetItems["legendaryCase"];

                //AddJew          = caseBox.Jew;
                Addlengends     = caseBox.LengendaryNum;
                this.productNum = caseBox.productNum;

                //상자가 아직 안열렸을때 이미지
                closeImg.sprite = SpriteManager.GetSpriteByName("Sprite", legCloseImg);
                //열렸을때 이미지 
                opImg.sprite = SpriteManager.GetSpriteByName("Sprite", legOpenImg);

            }
            else if (keyName == "HeroCase")
            {
                var caseBox = GameData.Instance.dic_SetItems["HeroCase"];
                //Hero카드가 나오는 수량 종류별로 나와야 해서 int 3,4,3 정함
                Addheros  = caseBox.HeroNum;
                Addheros1 = caseBox.HeroNum2;
                Addheros2 = caseBox.HeroNum3;
                this.productNum = caseBox.productNum;

                //상자가 아직 안열렸을때 이미지
                closeImg.sprite = SpriteManager.GetSpriteByName("Sprite", heroCloseImg);
                //열렸을때 이미지 
                opImg.sprite = SpriteManager.GetSpriteByName("Sprite", heroOpenImg);
            }
        }


        //데이터에서 분류함 시작과 동시에 나올카드를 랜덤하게 정해야 된다.
        //datas 이므로 아직 가지지 않은 카드도 포함하고 있다.
        foreach (UnityInfo cardKind in GameData.Instance.UnityDatas)
        {
            if (cardKind.Kinds == "Unit")
            {
                //일반
                units.Add(cardKind);
            }
            else if (cardKind.Kinds == "Rarity")
            {
                //희귀
                rairtys.Add(cardKind);
            }
            else if (cardKind.Kinds == "Hero")
            {
                //영웅
                heros.Add(cardKind);
            }
            else if (cardKind.Kinds == "Legend")
            {
                //전설
                legenards.Add(cardKind);
            }
        }

        //상자 열림과 동시에 보여줘야할 카드 카운트량
        itemNum.text = string.Format("{0}", productNum);
        //시작시 상자외에는 보여줘서는 안된다.
        SetActiviteTitleAllOFF();
        //카드 카운트는 따로 관리되어져야 한다.
        cardNum.SetActive(false);

        //TODO: 여기에 셔플넣기..세팅된뒤에 넣어야 함
        //순서대로 분리한 카드정보를 석는다.
        StartCoroutine(Suffle(0.3f));

    }

    //분리된 CardInfo의 순서를 랜덤하게 섞는다.
    IEnumerator Suffle(float t)
    {
        yield return new WaitForSeconds(t);
        if (GameData.Instance.dic_SetItems.ContainsKey(keyName))
        {
            if (keyName == "lunchyCase")
            {
              
                unitsSuffle     = shuffle(units);
                rairtysSuffle   = shuffle(rairtys);
                herosSuffle     = shuffle(heros);
                legenardsSuffle = shuffle(legenards);
                
            }
            else if (keyName == "legendaryCase")
            {
                legenardsSuffle = shuffle(legenards);
            }
            else if (keyName == "HeroCase")
            {
                herosSuffle =  shuffle(heros);
            }
        }
    }

    

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //    {
    //        SetCard();
    //        SetActiviteTitleAllOFF();
    //    }

    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        SetActiviteTitle(false);
    //        curGold = GameData.Instance.players[1].coin;
    //        curJew = GameData.Instance.players[1].jew;
    //    }

    //}

    int RandomRange(int max)
     {
        int random = Random.Range(1, max);
        return random;
     }

    //호출부분
    void OnDisable()
    {
        panel.gameObject.SetActive(false);
        //fadeIN,Out실행이벤트
        eff();
        UnityCard.evReScroll -= updateCard;

        for (int i = 0; i < GameData.Instance.days.Count; i++)
        {
            GameData.Instance.days[i].GetComponent<DailySale>().CardEff_EventDis();
        }
    }

    //Title 의 내용물
    void SetActiviteTitleAllOFF()
    {
        title.gameObject.SetActive(false);
        //cardNum.SetActive(false);
        arrowLevel.SetActive(false);
        arrowGold.SetActive(false);
        arrowJew.SetActive(false);
        // 한동작의 애니가 끝났을때 false로 바꾼다.
        
    }

     void SetActiviteTitle(bool isEnable)
     {
        
        switch (curOpen)
        {
            case eOpen.OPEN:
                title.gameObject.SetActive(isEnable);
                cardNum.SetActive(isEnable);

                break;
            case eOpen.GOLD:
                title.gameObject.SetActive(isEnable);
                cardNum.SetActive(isEnable);
                arrowGold.SetActive(isEnable);

                break;

            //case eOpen.JEW:
            //    title.gameObject.SetActive(isEnable);
            //    cardNum.SetActive(isEnable);
            //    arrowJew.SetActive(isEnable);
            // break;

            case eOpen.UNITY:
                title.gameObject.SetActive(isEnable);
                cardNum.SetActive(isEnable);
                arrowLevel.SetActive(isEnable);

                break;
            case eOpen.RARES:
                title.gameObject.SetActive(isEnable);
                cardNum.SetActive(isEnable);
                arrowLevel.SetActive(isEnable);

                break;
            case eOpen.HEROS:
                title.gameObject.SetActive(isEnable);
                cardNum.SetActive(isEnable);
                arrowLevel.SetActive(isEnable);

                break;
            case eOpen.LEGENDS:
                title.gameObject.SetActive(isEnable);
                cardNum.SetActive(isEnable);
                arrowLevel.SetActive(isEnable);

                break;

            default:
                title.gameObject.SetActive(false);
                //cardNum.SetActive(false);
                arrowLevel.SetActive(false);
                arrowGold.SetActive(false);
                arrowJew.SetActive(false);
                break;
        }
    }

    //상자가 클릭될때 마다 실행되게 한다. 중앙의 카드의 애니메이션 
    public void OpenCase()
    {
        //애니가 실행중 클릭이 되지 않게 한다.
        isClick = true;
        //TODO: 플레이어의 가지고 있는 정보를 담는다.
        curGold = GameData.Instance.players[1].coin;
        //curJew  = GameData.Instance.players[1].jew;

        opImg.enabled    = true;
        closeImg.enabled = false;

        CaseCardShow(true);
        float dur = 0.8f;

        //TODO: 이곳에 카드를 열리게 한다.

        if (!isOpen) 
        {    //상자가 열림 애니 
            iTween.ScaleFrom(ImgOpen,iTween.Hash("x",openImglocalScale.x + 0.3f, "y",openImglocalScale.y + 0.3f, "oncomplete", "IdleCase", "oncompletetarget", this.gameObject, "time", 0.3f));
            isOpen = true;
            //상자에서 나온카드가 보여지게 된다.
            ShowCard();
            //처음시작시 현재의증가할 정보를 담는다. 여기 왜 0이 나오는지 알수 없음?혹시 초기값문제인지 확인?
        }
       else
        {
                SetActiviteTitleAllOFF(); //전체 Imag를 끈다. 
                SetCard();                //중앙의 카드를 다시 처음비율로 세팅 
                PullCardBase();           //상자가 위로올라가는 애니
        }
    }

    //상자에서 카드가 나오는 애니
    void PullCardBase()
    {
        if (isReOpen)
        {
            //두번 클릭됐을때 위로 점프하면서 카드를 위로 올릴때 상자 애니
            Vector3 a = new Vector3(ImgOpen.transform.localPosition.x, ImgOpen.transform.localPosition.y + 100, 0);
            //점프 또는 상하로 움직이는 
            iTween.MoveFrom(ImgOpen, iTween.Hash("position", a, "time", .5f, "islocal", true, "easetype", iTween.EaseType.easeInOutQuint));
            ShowCard();
        }
    }


    bool isClick = false;

    public  void ClickCount()
    {
        //상품이 나오는 순서를 정한다.
        //터치에 따른 인터페이스 설정을 가능하게 한다.
        //애니 중간에 작동하지 않게 하거나 속도를 빠르게 조절가능할수 있게 한다.
        if (isClick != true)
        {

                if (keyName == "lunchyCase")
                {
                    switch (productNum)
                    {
               
                        //case 5:
                        //    curOpen = eOpen.JEW;
                        //    OpenCase();
                        //    isReOpen = true;
                        //    break;
                        case 4:
                            curOpen = eOpen.UNITY;
                            OpenCase();
                            isReOpen = true;
                            break;
                        case 3:
                            curOpen = eOpen.RARES;
                            OpenCase();
                            isReOpen = true;
                            break;
                        case 2:
                            curOpen = eOpen.HEROS;
                            OpenCase();
                            isReOpen = true;
                            break;
                        case 1:
                            curOpen = eOpen.LEGENDS;
                            OpenCase();
                            isReOpen = true;
                            break;
                        case 0:
                            curOpen = eOpen.CLOSE;
                            isReOpen = false; //title작동되지 않음
                            //다시 클릭됐을때를 위해서 카드이미지가 안보이게 세팅한다.
                            iconImg.enabled = false;
                            cardbase.enabled = false;
                            isOpen = false;
                            transform.root.gameObject.SetActive(false);
                   
                            break;

                    }
                }
                else if (keyName == "legendaryCase")
                {
           
                    switch (productNum)
                    {
                        case 1:
                            curOpen = eOpen.LEGENDS;
                            OpenCase();
                            isReOpen = true;
                            break;

                        case 0:
                            curOpen = eOpen.CLOSE;
                            isReOpen = false; //title작동되지 않음
                            //다시 클릭됐을때를 위해서 카드이미지가 안보이게 세팅한다.
                            iconImg.enabled = false;
                            cardbase.enabled = false;
                            isOpen = false;
                            transform.root.gameObject.SetActive(false);
                   
                            break;

                    }
                }
                else if (keyName == "HeroCase")
                {

                    switch (productNum)
                    {
               
                        case 3:
                            curOpen = eOpen.HEROS;
                            OpenCase();
                            isReOpen = true;
                            break;
                        case 2:
                            curOpen = eOpen.HEROS;
                            OpenCase();
                            isReOpen = true;

                            break;
                        case 1:
                            curOpen = eOpen.HEROS;
                            OpenCase();
                            isReOpen = true;
                            break;

                        case 0:
                            curOpen = eOpen.CLOSE;
                            isReOpen = false; //title작동되지 않음
                            //다시 클릭됐을때를 위해서 카드이미지가 안보이게 세팅한다.
                            iconImg.enabled = false;
                            cardbase.enabled = false;
                            isOpen = false;
                            transform.root.gameObject.SetActive(false);
                   
                            break;

                    }
                }
        }// isClik END
    }

    void IdleCase()
    {
        //상자 아이들링 ani 
        iTween.MoveAdd(ImgOpen, iTween.Hash("y", -10, "time", .6, "looptype",iTween.LoopType.pingPong,"easetype",iTween.EaseType.easeOutBounce));
        
    }

    void IdleObject(GameObject obj)
    {
        iTween.MoveAdd(obj, iTween.Hash("x", -8, "time", .2, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.easeOutExpo));

    }
   

    void CaseCardShow(bool isEnable)
    {
        cardNum.SetActive(isEnable);
        itemNum.text = string.Format("{0}", productNum);
    }

    //text 와 이동애니메이션 실행
    void OpenCards()
    {
        SetActiviteTitle(true);
        switch (curOpen)
        {
            case eOpen.OPEN:


                break;
            case eOpen.GOLD:
                //Gold증가
                iTween.ValueTo(gameObject, iTween.Hash("from", curGold, "to", curGold + Addgold, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
                titleNames[0].text = "GOLD";
                titleNames[1].text = "광물자원";
                titleNames[2].text = "내골드";
                productNum--;
                itemNum.text = string.Format("{0}", productNum);
                iconImg.sprite = SpriteManager.GetSpriteByName("Sprite", "UI_coin");
                isClick = false;
                break;
            //case eOpen.JEW:                                    
            //    iTween.ValueTo(gameObject, iTween.Hash("from", curJew, "to", curJew + AddJew, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
            //    titleNames[0].text = "JEW";
            //    titleNames[1].text = "광물자원";
            //    titleNames[2].text = "내보석";
            //    productNum--;
               
            //    iconImg.sprite = SpriteManager.GetSpriteByName("Sprite", "UI_Icon_0");
            //    isClick = false;
            //    break;
            case eOpen.UNITY:
                                           
                int[] level1   = new int[2];
                level1         = ShowTitle();
                int cur1       = level1[0];
                baseLevel      = level1[1];  //level Amount


              
                iTween.ValueTo(gameObject, iTween.Hash("from", cur1, "to", cur1 + Addunits, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
                productNum--;
                itemNum.text = string.Format("{0}", productNum);
                string name = GameData.Instance.UnityDatas[index-1].Name;
                Debug.Log("name :"+name +" == index" +index);
                iconImg.sprite = SpriteManager.GetSpriteByName("Sprite", name);

                //ID 번호로 hasCard의 값을 변경시킨다.
                if (GameData.Instance.hasCard.ContainsKey(index))
                {     //key
                    GameData.Instance.hasCard[index].hasCard += Addunits;
                    cardUp();
                }


                isClick = false;
                break;
            case eOpen.RARES:
                int[] level2 = new int[2];
                level2       = ShowTitle();
                int cur2     = level2[0];
                baseLevel    = level2[1];  //level Amount


                //cardUp();
                //ID 번호로 hasCard의 값을 변경시킨다.
                if (GameData.Instance.hasCard.ContainsKey(index))
                {     //key
                   GameData.Instance.hasCard[index].hasCard += Addrarts;
                   cardUp();
                }

                iTween.ValueTo(gameObject, iTween.Hash("from", cur2, "to", cur2 + Addrarts, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
                productNum--;
                itemNum.text = string.Format("{0}", productNum);
                string name2 = GameData.Instance.UnityDatas[index-1].Name;
                iconImg.sprite = SpriteManager.GetSpriteByName("Sprite", name2);

                isClick = false;
                break;
            case eOpen.HEROS:

                int[] level3 = new int[2];
                level3       = ShowTitle();
                int cur3     = level3[0];
                baseLevel    = level3[1];  //level Amount

                //cardUp();
                if (GameData.Instance.hasCard.ContainsKey(index))
                {     //key
                    GameData.Instance.hasCard[index].hasCard += Addheros;
                    cardUp();
                }
           

                iTween.ValueTo(gameObject, iTween.Hash("from", cur3, "to", cur3 + Addheros, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
                productNum--;
                itemNum.text = string.Format("{0}", productNum);
                isClick = false;

                string name3   = GameData.Instance.UnityDatas[index-1].Name;
                iconImg.sprite = SpriteManager.GetSpriteByName("Sprite", name3);
                break;

            case eOpen.LEGENDS:

                int[] level4 = new int[2]; //level Amount
                level4       = ShowTitle();
                int cur4     = level4[0];
                baseLevel    = level4[1];  //level Amount

                //cardUp();
                if (GameData.Instance.hasCard.ContainsKey(index))
                {     //key
                    GameData.Instance.hasCard[index].hasCard += Addlengends;
                    cardUp();

                }

             
                iTween.ValueTo(gameObject, iTween.Hash("from", cur4, "to", cur4 + Addlengends, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
                productNum--;
                itemNum.text = string.Format("{0}", productNum);
                //다시 버튼이 클릭될수 있도록한다.

                string name4 = GameData.Instance.UnityDatas[index-1].Name;
                iconImg.sprite = SpriteManager.GetSpriteByName("Sprite", name4);
                isClick = false;
                break;

            case eOpen.CLOSE:

                break;
        }
    }

    //메인의그림카드 
    void ShowCard()
    {
        float dur = 1f;
        cardbase.enabled = true;

     
        //이미지가 작은 사이즈에서 원래 사이즈로 돌아오게한다.
        iTween.MoveFrom(Image.gameObject, iTween.Hash("y", 260, "easetype", iTween.EaseType.easeInOutCirc, "time", dur / 2, "oncomplete", "ReForm", "oncompletetarget", this.gameObject));
        //이미지가 한바퀴 도는 애니 발생시킨다.
        iTween.ScaleFrom(Image.gameObject, iTween.Hash("x", -initScale.x, "easetype", iTween.EaseType.easeInOutElastic, "time", dur
                                                        , "oncomplete", "ShowImg", "oncompletetarget", this.gameObject));
       
    }
  
    void SetCard()
    {
        cardbase.enabled = false;
        rtImg.sizeDelta = new Vector2(100, height);
        iconImg.enabled = false;
       
    }

     void Show()
    {
        iTween.ScaleFrom(arrowGold, iTween.Hash("x", 1.2f, "easetype", iTween.EaseType.easeInOutCirc, "time", .2f
                                                            ,"looptype", iTween.LoopType.loop));
        iTween.ScaleFrom(arrowLevel, iTween.Hash("x", 1.1f, "easetype", iTween.EaseType.easeInOutCirc, "time", .2f
                                                           , "looptype", iTween.LoopType.loop));
        iTween.ScaleFrom(arrowJew, iTween.Hash("x", 1.1f, "easetype", iTween.EaseType.easeInOutCirc, "time", .2f
                                                          , "looptype", iTween.LoopType.loop));
        iTween.ScaleFrom(arrow, iTween.Hash("y", 1.1f, "easetype", iTween.EaseType.easeInOutCirc, "time", .5f
                                                          , "looptype", iTween.LoopType.loop));
      
    }

    //value 가 증가할때 가지 애니실행한다. 
    void UpdateGoldDisplay(int Value)//Gold:200 ~ 1200:증가 되며 계속호출됨
    {
        AppSound.instance.SE_MENU_ITEMCOUNTGOLD.Play();
        //if (AppSound.instance.SE_MENU_ITEMCOUNTGOLD.isPlaying)
        //{
        //    AppSound.instance.SE_MENU_ITEMCOUNTGOLD.Stop();
        //}

        switch (curOpen)
        {
            case eOpen.OPEN:

                break;
            case eOpen.GOLD:
                //스크린에 text가 보이게 한다.
                txGold.text = string.Format("{0:0,0}",Value);
                if (Addgold <= Value)
                {
                    iTween.Stop(arrowGold);
                }

                break;
            case eOpen.JEW:
              
                txJew.text = string.Format("{0}",Value);
                if (AddJew <= Gold)
                {
                    iTween.Stop(arrowJew);
                }

                break;
            case eOpen.UNITY:
               
                txLevel.text = string.Format("{0} / {1}",Value,baseLevel);
                break;
            case eOpen.RARES:
               
                txLevel.text = string.Format("{0} / {1}", Value, baseLevel);
                break;
            case eOpen.HEROS:
                
                txLevel.text = string.Format("{0} / {1}", Value, baseLevel);
                break;
            case eOpen.LEGENDS:
               
                txLevel.text = string.Format("{0} / {1}", Value, baseLevel);
                break;
        }
    }


    void ReForm()
    {
       //작게 시작해서 원래 크기로 돌아오게 
        rtImg.sizeDelta = new Vector2(width, height);
    }

    void ShowImg()
    {
        //감춰진 이미지가 보여지게 
        iconImg.enabled = true;
        //TODO  : 이곳에 넣으면 이미지 후 나온다.
       // string name = GameData.Instance.UnityDatas[index].Name;
        //iconImg.sprite = SpriteManager.GetSpriteByName("Sprite", name);
        OpenCards();
    }

    //TODO;Random한 카드 구하는 방식이 이상함
    int[] ShowTitle()
    {
        index = 0;
        switch (curOpen)
        {
            case eOpen.OPEN:

                break;
            case eOpen.GOLD:
               
                break;
            case eOpen.JEW:
               
                break;
            case eOpen.UNITY:
                //상품케이스가 열렸을때 한번만 유닛카드가 선택되므로 그냥 첫번째걸로 정함
                index = unitsSuffle[productNum - 1].Id;
              
                break;
            case eOpen.RARES:
                //상품케이스가 열렸을때 한번만 유닛카드가 선택되므로 그냥 첫번째걸로 정함
                index = rairtysSuffle[productNum - 1].Id;
              
                break;
            case eOpen.HEROS:
                //뒤에서 부터 하나씩 출력 2,1,0 3번선택받는다.
               index =  herosSuffle[productNum - 1].Id;
            

                break;

            case eOpen.LEGENDS:
                //상품케이스가 열렸을때 한번만 유닛카드가 선택되므로 그냥 첫번째걸로 정함
                index = legenardsSuffle[productNum-1].Id;
             
                break;

            case eOpen.CLOSE:

                break;
        }

      
        string titleName   = GameData.Instance.UnityDatas[index-1].Name;
        string kinds       = GameData.Instance.UnityDatas[index-1].Kinds;

        titleNames[0].text = titleName;
        titleNames[1].text = string.Format("{0}", kinds);
        int[] leve = new int[2];
        // 만약 내가 기지고 있는 카드라면 레벨을 찾아서 적용하고 그렇지 않다면
        // 레벨을 0으로 적용한다. hasCard
        if (GameData.Instance.hasCard.ContainsKey(index))
        {
            int level       = GameData.Instance.hasCard[index].level;
            int hasCardNums = GameData.Instance.hasCard[index].hasCard;
            int levelremain = GameData.Instance.Level[level];

            titleNames[2].text = string.Format("{0}", level);
            leve[0]            = hasCardNums; //가지고 있는 카드
            leve[1]            = levelremain; //업레벨까지의 카드량 
                                              //클릭됐을때 카드의 업데이트를 알린다.
           
        }
        else
        {
            titleNames[2].text = string.Format("{0}", 0);
        }

        return leve;
    }


    //순간적으로 빨리 켜지고 꺼지는 효과 
    //IEnumerator FADEOUT_FLASH()
    //{
    //    fadeOut.isFadeIN = true;
    //    fadeOut.StartFadeAnim();
    //    yield return new WaitForSeconds(0.1f);
    //}
    //public void FADEOUT_FLASHClick()
    //{
    //    fadeOut.isFadeIN = true;
    //    fadeOut.StartFadeAnim();
    //}
    //상자가 열리자 마자 바로 같이 표시되어야 하는 카드카운트



    void Create_FADEOUT(GameObject ob,Transform t)
    {
        GameObject fadeObj         = Instantiate(ob, t);
        fadeObj.GetComponent<FadeOut>().animTime = 0.5f;
        ob.transform.localScale    = new Vector3(1, 1, 1);
        ob.transform.localPosition = new Vector3(0, 0, 0);
    }

}
