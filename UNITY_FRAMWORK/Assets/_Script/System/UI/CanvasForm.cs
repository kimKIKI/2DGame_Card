using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasForm : MonoBehaviour
{
            public static CanvasForm Instance;
            Text     ownCards;         //레벨업 될때까지 남은 카드의 숫자를 표시 한다.
            Image    mainICon;         //메인이미지
            Image    exitImg;          //UpdateShow에서 쇼가끝나고 클릭이 가능하게해서 빠져나가게한다.
            Slider   slider;           //카드의 숫자를 받아 바 크기를 나타내 준다.
            Text     cost;             //판매될 금액
            Text     LevelCount;       //카드의 다음 증가량을 보여준다. 
            Text     name;
            Transform buttonE;         //UpdateShow가 끝나고 보여질 문구를 위해서 잠시커둘버튼
            int      Count;            //count를 위해서 설정
            int      CountCards;       //Cards의 증가량 설정
            int      nextCardAmount;
            float    CountSlider;      //증가될 수치
            float    CountEa;          //계산된 증가량
            float    toFullCount;      //그래프에서 증가될 총한계
            int      remainCards;      //남은 카드량
            int      nextRemainCards;  //다음레벨의 업그레이드 카드량
            int      resultCard;       //남은 카드량에서 업그레이드된 카드를 뺀값
            int      ID;               //입력받은 id
          
            public GameObject purchaseBox; //구매확인
            public GameObject updateBox;   //구매
            public GameObject updateShow;  //구매이펙트
     
            //-------------외부접근------------------
            [HideInInspector]
            public bool IsTop;            //tab 의 탭안의 카드가 업데이트 될때
                                          //현재의 카드의 타입을 직접 선택하는 것으로바꿈
           //--------------------------------------------
              [HideInInspector]
            public GameObject frontObj;    //레벨업을 표시해주기 우해서front받음
            public int        curLevel;    //Update되기전 카드의 레벨
            Button updateBoxBtn;           //이벤트 리스너 설정
            Button updateShowBtn;
            Button UpdateShowCloseBtn;
            Button puchaseBtn;             //코인결제버튼
            Button purchaseCloseBtn;       //코인결제 버튼
            public UnityCard card;
            bool   isOne;
            bool   isPurchase;             //구매가 이루어 졌는지 판단
           
            int    nextUpGold;              //레벨업에 필요한 골드량
            GameObject SaleObj;             //여기서 호출한 곳의 메소드를 실행시키기 위해서 설정
            public   delegate  void  collBackLeveUp(); //레벨업이나 구매가 이루어 졌을때다른카드를업데이트한다.
            public   static event    collBackLeveUp  eveLevelUp; //Resting()
            //public   static event    collBackLeveUp  collLeveUp; //LevelUp() UnityCard 의 Lavel만 업

            void Awake()
            {
                Instance = this;
               // base.Awake();
                //purchaseBox 에 붙어 있는 updatebox을 열기위한 버튼임
                updateBoxBtn = updateBox.GetComponentInChildren<Button>();
                updateBoxBtn.onClick.AddListener(delegate
                {
                    //돈이 업데이트금액보다 만을때
                    if (nextUpGold < ItemDisplayer.instance_ItemDisplayer.CurhasGold )
                    {
                        //활성화 되면 자동으로 애니실행
                        updateShow.SetActive(true);
                        updateBox.SetActive(false);
                       
                       
                        //ItemSampleTop의 타입을 직접 확인
                        if (card.eCardType == CARDOBJTYPE.TabSlotCard)
                        {
                            SetUpdateShowOne(ID, card.gameObject, updateShow);
                           
                        }
                        else
                        {
                            SetUpdateShow(ID, card.gameObject, frontObj, updateShow);
                        }
                        
                    }
                    else
                    {
                        //TODO:ToolBox로 표현해  주어야 한다.
                        print("금액이 부족합니다.");
                    }
                });

                UpdateShowCloseBtn = updateShow.transform.Find("Button_Close").GetComponent<Button>();
                UpdateShowCloseBtn.onClick.AddListener(delegate
                {
                        updateShow.SetActive(false);
                        GameData.Instance.isStopScroview = false;
                        GameData.Instance.IsShowCard     = false;
                        //TODO: 여기서 활성화된 backCard를 다시 하이드 해줘야 한다.
                        //card.GetComponent<UnityCard>().transform.Find("Panel").gameObject.SetActive(false);
                        //가격전달 메소드
                });

                puchaseBtn = purchaseBox.transform.Find("Button").GetComponent<Button>();
                puchaseBtn.onClick.AddListener(delegate
                {
                    if(!isOne)
                    CountAnime();

                    SaleObj.GetComponent<DailySale>().PurchaseSend();
                    SaleObj.GetComponent<DailySale>().OnClick();
                    isPurchase = true;

                    eveLevelUp();

                });

                purchaseCloseBtn = purchaseBox.transform.Find("Button_Close").GetComponent<Button>();
                purchaseCloseBtn.onClick.AddListener(delegate
                 {
                     if (isPurchase)
                     {
                         SaleObj.GetComponent<DailySale>().PurchaseEnd();
                         isPurchase = false;
                     }
                     purchaseBox.SetActive(false);
                 });
               }

            //타입에 맞게 데이터를 적용해서 출력한다.
            public virtual void Set(int id, int gold, int amount,GameObject obj)
            {
                //상품구매시 호출때 한번실행하기 위해서 설정
                isOne       = false;
                SaleObj     = obj;
                ID          = id;

                LevelCount  = purchaseBox.transform.Find("tx_LevelCount").GetComponentInChildren<Text>();
                ownCards    = purchaseBox.transform.Find("tx_Amount").GetComponentInChildren<Text>();
                mainICon    = purchaseBox.transform.Find("Icon").GetComponentInChildren<Image>();
                slider      = purchaseBox.GetComponentInChildren<Slider>();
                cost        = purchaseBox.transform.Find("Button/tx_Coin").GetComponentInChildren<Text>();
                name        = purchaseBox.transform.Find("txIcon_Name").GetComponentInChildren<Text>();
                exitImg     = purchaseBox.transform.Find("Button_Close").GetComponent<Image>();
                buttonE     = purchaseBox.transform.Find("Button").GetComponent<Transform>();
             

            if (GameData.Instance.hasCard.ContainsKey(ID))
                    {   //가지고 있는 카드의 tatal value
                        int hasCards        = GameData.Instance.hasCard[ID].hasCard;
                        //현재 카드가 레벨업된value 
                        int level           = GameData.Instance.hasCard[ID].level;
                        //현재까지의 레벨로 총카드숫자로 변환
                        int curLevel        = GameData.Instance.Level[level-1];
                        //업데이트하고 남은 카드의 value
                        int remainCards     = LevelUpRemain(ID);
                        //next level의 전체 카드 value 
                        int nextRemainCards = GameData.Instance.Level[level];
                        //현재레벨에서 다음레벨까지  필요한 카드의 value
                        int curRemainCards  = nextRemainCards - curLevel;

                        LevelCount.text     = string.Format("{0} / {1}", remainCards, nextRemainCards);
                        //value 카드단위당 증가할 vale설정 백분율
                        float uni           = 1f / nextRemainCards;
                        //최대값이 1이므로 1실수율로 변환
                        float value         = remainCards * uni;
                      
                                                 //구매시 애니가 작동되어야 한다.
                         slider.value        = value;
                        //외부에서 리스너로 접근할 경우
                        // slider.onValueChanged.AddListener(delegate { OnAmountChanged(); });

                        //다음 레벨에  업글가능한 카드가 존재할때
                        if (remainCards >= curRemainCards)
                        {
                            //업그레이드를 유도하는 애니이펙트 실행 화살표 움직임
                        }
                        // 고유아이디 1 은 UnityDatas[0]번째에 있음 따라서 -1                 
                        string cardName = GameData.Instance.UnityDatas[ID - 1].Name;
                        mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", cardName);
                        cost.text       = string.Format("{0}", gold);
                        ownCards.text   = string.Format(" X {0}", amount);
                        name.text       = string.Format("{0} X ", cardName);
                        Count           = amount;
                        CountCards      = remainCards;   //증가될 카드량
                       float countFloat = CountCards * uni; //증가될 카드량 
                        nextCardAmount  = nextRemainCards;
                        CountSlider     = value;         //증가될 수치
                        CountEa         = uni;           //계산된 증가량
                        toFullCount     = countFloat + CountSlider;
                     }

                }

           

            public virtual void SetUpdate(int id,GameObject obj)
            {
                ID         = id;
                LevelCount = obj.transform.Find("tx_LevelCount").GetComponentInChildren<Text>();
                ownCards   = obj.transform.Find("tx_Amount").GetComponentInChildren<Text>();
                mainICon   = obj.transform.Find("Icon").GetComponentInChildren<Image>();
                slider     = obj.GetComponentInChildren<Slider>();
                cost       = obj.transform.Find("Button/tx_Coin").GetComponentInChildren<Text>();
                name       = obj.transform.Find("txIcon_Name").GetComponentInChildren<Text>();
                exitImg    = obj.transform.Find("Button_Close").GetComponent<Image>();
                buttonE    = obj.transform.Find("Button").GetComponent<Transform>();

                if (GameData.Instance.hasCard.ContainsKey(ID))
                        {   //가지고 있는 카드의 tatal value
                            int hasCards        = GameData.Instance.hasCard[ID].hasCard;
                            //현재 카드가 레벨업된value 
                            int level           = GameData.Instance.hasCard[ID].level;
                            //현재까지의 레벨로 총카드숫자로 변환
                            int curLevel        = GameData.Instance.Level[level-1];
                            //업데이트하고 남은 카드의 value
                            int remainCards     = LevelUpRemain(ID);
                            //next level의 전체 카드 value 
                            int nextRemainCards = GameData.Instance.Level[level];
                            nextUpGold          = GameData.Instance.LevelCost[level - 1];
                            //현재레벨에서 다음레벨까지  필요한 카드의 value
                           int curRemainCards = nextRemainCards - curLevel;

                            LevelCount.text     = string.Format("{0} / {1}", remainCards, nextRemainCards);
                            //value 카드단위당 증가할 vale설정 백분율
                            float uni           = 1f / nextRemainCards;
                            //최대값이 1이므로 1실수율로 변환
                            float value         = remainCards * uni;
                           
                            //구매시 애니가 작동되어야 한다.
                            slider.value        = value;

                            //다음 레벨에  업글가능한 카드가 존재할때
                            if (remainCards >= curRemainCards)
                            {
                                //업그레이드를 유도하는 애니이펙트 실행 화살표 움직임
                                
                            }
                           
                            string cardName = GameData.Instance.UnityDatas[ID - 1].Name;
                            mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", cardName);
                            cost.text       = string.Format("{0}", nextUpGold);
                            ownCards.text   = string.Format("{0}", level);
                            name.text       = string.Format("{0} X ", cardName);
                            nextCardAmount  = nextRemainCards;
                            CountSlider     = value;     //증가될 수치(현재그래프레서 가지고value)
                            CountEa         = uni;       //계산된 증가량
                           
                           
                        }
            }

            //현재 ID의 카드가 레베업되고 남은 카드량
            int LevelUpRemain(int ID)
            {

                int hasCards = GameData.Instance.hasCard[ID].hasCard;
                //현재 카드가 레벨업된value 
                int level    = GameData.Instance.hasCard[ID].level;
                //레벨에 따른 카드의 수량배열
                int curLevel;
      
                for (int i = 0; i < level; ++i)
                {
                    curLevel  = GameData.Instance.Level[i];
                    hasCards -= curLevel;
                }

                return hasCards;
            }


            public virtual void SetUpdateShow(int id, GameObject obj,GameObject front,GameObject form)
            {
                
                ID               = id;
                card             = obj.GetComponent<UnityCard>();
               UnityCard frontO = front.GetComponent<UnityCard>();
              
                LevelCount       = form.transform.Find("tx_LevelCount").GetComponentInChildren<Text>();
                ownCards         = form.transform.Find("tx_Amount").GetComponentInChildren<Text>();
                mainICon         = form.transform.Find("Icon").GetComponentInChildren<Image>();
                slider           = form.GetComponentInChildren<Slider>();
                cost             = form.transform.Find("Button/tx_Coin").GetComponentInChildren<Text>();
                name             = form.transform.Find("txIcon_Name").GetComponentInChildren<Text>();
                exitImg          = form.transform.Find("Button_Close").GetComponent<Image>();
                buttonE          = form.transform.Find("Button").GetComponent<Transform>();

            if (GameData.Instance.hasCard.ContainsKey(ID))
            {   //가지고 있는 카드의 tatal value
                    int hasCards      = GameData.Instance.hasCard[ID].hasCard;
                    //현재 카드가 레벨업된value 
                    int level          = GameData.Instance.hasCard[ID].level;
                    //현재까지의 레벨로 총카드숫자로 변환
                    int curLevel       = GameData.Instance.Level[level-1];
                    //업데이트하고 남은 카드의 value
                    remainCards        = hasCards - curLevel;
                    //next level의 전체 카드 value 
                    nextRemainCards    = GameData.Instance.Level[level];
                    nextUpGold         = GameData.Instance.LevelCost[level - 1];
                    //현재레벨에서 다음레벨까지  필요한 카드의 value
                    int curRemainCards = nextRemainCards - curLevel;

                    LevelCount.text    = string.Format("{0} / {1}", nextRemainCards, nextRemainCards);
                    //value 카드단위당 증가할 vale설정 백분율
                    float uni          = 1f / nextRemainCards;
                    //최대값이 1이므로 1실수율로 변환
                    float value        = remainCards * uni;
                    //구매시 애니가 작동되어야 한다.
                    slider.value       = value;

                    //다음 레벨에  업글가능한 카드가 존재할때
                    if (remainCards >= curRemainCards)
                    {
                        //업그레이드를 유도하는 애니이펙트 실행 화살표 움직임
                    }
                    string cardName = GameData.Instance.UnityDatas[ID - 1].Name;
                    mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", cardName);
                   //TODO 애니에서 다른 애니가 생성됨..보류
                    ownCards.text   = string.Format("{0}", level);

                    name.text       = string.Format("{0} X ", cardName);
                    nextCardAmount  = nextRemainCards;
                    CountSlider     = value;         //증가될 수치
                    CountEa         = uni;               //계산된 증가량
                    resultCard      = remainCards - nextRemainCards;

                    StartCoroutine(LevelCountAnim());
                    //증가된 레벨업value 를 콜백해야 한다.
                    int levelUp     = level + 1;
                    ItemDisplayer.instance_ItemDisplayer.decreaseItem(nextUpGold);

                    //TODO: 이벤트 메니저에서 호출이 잘되지 않아서 여기서 집접입력함
                    frontO.Level          = levelUp;
                    //GameData.Instance.hasCard[ID].hasCard
                    card.LevelNum.text    = string.Format(" 레벨 {0}", levelUp);
                    frontO.LevelNum.text  = string.Format(" 레벨 {0}", levelUp);
                    GameData.Instance.hasCard[ID].level = levelUp;
                    //collLeveUp();
                    eveLevelUp();
                    card.ReSeting();
                    frontO.ReSeting();
                  }
     
          }



    public virtual void SetUpdateShowOne(int id, GameObject obj, GameObject form)
    {

        ID = id;
        card = obj.GetComponent<UnityCard>();

        LevelCount = form.transform.Find("tx_LevelCount").GetComponentInChildren<Text>();
        ownCards   = form.transform.Find("tx_Amount").GetComponentInChildren<Text>();
        mainICon   = form.transform.Find("Icon").GetComponentInChildren<Image>();
        slider     = form.GetComponentInChildren<Slider>();
        cost       =   form.transform.Find("Button/tx_Coin").GetComponentInChildren<Text>();
        name       = form.transform.Find("txIcon_Name").GetComponentInChildren<Text>();
        exitImg    = form.transform.Find("Button_Close").GetComponent<Image>();
        buttonE    = form.transform.Find("Button").GetComponent<Transform>();

        if (GameData.Instance.hasCard.ContainsKey(ID))
        {   //가지고 있는 카드의 tatal value
            int hasCards = GameData.Instance.hasCard[ID].hasCard;
            //현재 카드가 레벨업된value 
            int level = GameData.Instance.hasCard[ID].level;
            //현재까지의 레벨로 총카드숫자로 변환
            int curLevel = GameData.Instance.Level[level-1];
            //업데이트하고 남은 카드의 value
            remainCards = LevelUpRemain(ID);
            //next level의 전체 카드 value 
            nextRemainCards = GameData.Instance.Level[level];
            nextUpGold = GameData.Instance.LevelCost[level - 1];
            //현재레벨에서 다음레벨까지  필요한 카드의 value
            int curRemainCards = nextRemainCards - curLevel;

            LevelCount.text = string.Format("{0} / {1}", nextRemainCards, nextRemainCards);
            //value 카드단위당 증가할 vale설정 백분율
            float uni = 1f / nextRemainCards;
            //최대값이 1이므로 1실수율로 변환
            float value = remainCards * uni;
            //구매시 애니가 작동되어야 한다.
            slider.value = value;

            //다음 레벨에  업글가능한 카드가 존재할때
            if (remainCards >= curRemainCards)
            {
                //업그레이드를 유도하는 애니이펙트 실행 화살표 움직임
            }
            string cardName = GameData.Instance.UnityDatas[ID - 1].Name;
            mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", cardName);
            //TODO 애니에서 다른 애니가 생성됨..보류
            ownCards.text   = string.Format("{0}", level);

            name.text       = string.Format("{0} X ", cardName);
            nextCardAmount  = nextRemainCards;
            CountSlider     = value;         //증가될 수치
            CountEa         = uni;               //계산된 증가량
            resultCard      = remainCards - nextRemainCards;

            StartCoroutine(LevelCountAnim());
            //증가된 레벨업value 를 콜백해야 한다.
            int levelUp = level + 1;
            ItemDisplayer.instance_ItemDisplayer.decreaseItem(nextUpGold);

            //TODO: 이벤트 메니저에서 호출이 잘되지 않아서 여기서 집접입력함
        
            //GameData.Instance.hasCard[ID].hasCard
            card.LevelNum.text = string.Format(" 레벨 {0}", levelUp);

            //레벨업 이벤트 필요
            GameData.Instance.hasCard[ID].level = levelUp;
           // collLeveUp();
            eveLevelUp();
            card.ReSeting();
            //TODO: 아이디를 찾아서 바꿔주는 기능이 필요함
            //어디에 존재하는가?Tab 여기서 이벤트 발생
            //여기 UnityCard.cs와 중복확률
            if (IsTop)
            {
                card.updateBtn.gameObject.SetActive(false);
                IsTop = false;
            }
        }
    }

            IEnumerator LevelCountAnim()
            {
                yield return new WaitForSeconds(1f);
                //TODO:증가된 레벨을 GameData에 보낸다.그래야 다른UnityCard들도 같게 변경된다.

                CountAnimeLevel();
                //업데이트된 것을 카드에 전달해서 적용한다.
            }

            //UpdateShow 가 발생시이루어져야할 애니 
            public void CountAnimeLevel()
            {
                
                iTween.ValueTo(gameObject, iTween.Hash("from", nextCardAmount, "to", 0, "time", .6, "onUpdate", "UpdateCardDisplay2", "oncompletetarget", gameObject));
                nextCardAmount--;

                iTween.ValueTo(gameObject, iTween.Hash("from", CountSlider, "to", 0, "time", 1, "onUpdate", "UpdateSliderDisplay", "oncompletetarget", gameObject));
                CountSlider -= CountEa;

           }

            //중앙의 카드가 습득되는 애니 실행
            public void CountAnime()
            {
                iTween.ValueTo(gameObject, iTween.Hash("from", CountCards, "to", CountCards + Count, "time", .6, "onUpdate", "UpdateCardDisplay", "oncompletetarget", gameObject));
                CountCards++;
                //Gold증가
                iTween.ValueTo(gameObject, iTween.Hash("from", Count, "to", 0, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
                Count--;

                iTween.ValueTo(gameObject, iTween.Hash("from", CountSlider, "to", toFullCount, "time", .6, "onUpdate", "UpdateSliderDisplay", "oncompletetarget", gameObject));
                CountSlider += CountEa;
                
            }


            //value 가 증가할때 가지 애니실행한다. 업데이트 displayer
            void UpdateCardDisplay2(int Value)
            {
                AppSound.instance.SE_MENU_ITEMCOUNTGOLD.Play();
                LevelCount.text = string.Format("{0} / {1}", Value, nextRemainCards);

                if (Value <= 0)
                {
                    //현재 카드가 레벨업된value 
                    int level = GameData.Instance.hasCard[ID].level;
                    // 업그레이드후 앞으로업에 필요한 레벨카드
                    int curLevel = GameData.Instance.Level[level + 1];
                    LevelCount.text = string.Format("{0} / {1}", Value, curLevel);
                    //업그레이드된후 레벨
                    ownCards.fontSize = 70;
                    ownCards.text = string.Format("{0}", level);
                    //저장되고 크릭시 종료되게 버튼 활성화 시킨다.

                    //이미지를 켜서 버튼이 클릭가능하게 한다.이미지로 완료된것을 나타나게한다.
                    exitImg.enabled = true;
                    //버튼을켜서 cost문구가 보이게 한다.
                    buttonE.gameObject.SetActive(true);
                    cost.text = string.Format("{0}", "구매완료");
                    //좌우 스트롤이동이 가능하게 한다.
                    GameData.Instance.isStopScroview = false;
                    GameData.Instance.IsShowCard     = false;

                }
            }

            //value 가 증가할때 가지 애니실행한다. 
            void UpdateGoldDisplay(int Value)//증가한 레벨업
            {
                AppSound.instance.SE_MENU_ITEMCOUNTGOLD.Play();
                ownCards.text = string.Format(" X {0}", Value);
                if (Value <= 0)
                {
                  isOne = true;
                }
            }

            void UpdateCardDisplay(int Value)//Gold:200 ~ 1200:증가 되며 계속호출됨
            {
                LevelCount.text = string.Format("{0} / {1}", Value, nextCardAmount);
            }

            void UpdateSliderDisplay(float Value)
            {
                slider.value = Value;
            }



}
