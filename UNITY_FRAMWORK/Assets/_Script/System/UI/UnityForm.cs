using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//기본적인 UI 에서 cards 을 구성하는 필드의 세팅 
public class UnityForm : Singleton<UnityForm>
{

    //private static UnityForm sInstance;
    //public static UnityForm Instance
    //{
    //    get {
    //        return sInstance; }
    //}

    Text     ownCards;     //레벨업 될때까지 남은 카드의 숫자를 표시 한다.
    Image    mainICon;     //메인이미지
    Image    exitImg;      //UpdateShow에서 쇼가끝나고 클릭이 가능하게해서 빠져나가게한다.
    Slider   slider;       //카드의 숫자를 받아 바 크기를 나타내 준다.
    Text     cost;         //판매될 금액
    Text     LevelCount;   //카드의 다음 증가량을 보여준다. 
    Text     name;
    Transform buttonE;     //UpdateShow가 끝나고 보여질 문구를 위해서 잠시커둘버튼
    int      Count;        //count를 위해서 설정
    int      CountCards;   //Cards의 증가량 설정
    int      nextCardAmount;
    float    CountSlider;    //증가될 수치
    float    CountEa;        //계산된 증가량
    int      remainCards;    //남은 카드량
    int      nextRemainCards;//다음레벨의 업그레이드 카드량
    int      resultCard;     //남은 카드량에서 업그레이드된 카드를 뺀값
    int      ID;             //입력받은 id

    //외부에서 전달받지 않아도 자체적으로 오브젝트 구조를 구성하는 요소들 
    protected override void Awake( )
    {
        base.Awake();


        //sInstance    = this;

        LevelCount   = transform.Find("tx_LevelCount").GetComponentInChildren<Text>();
        ownCards     = transform.Find("tx_Amount").GetComponentInChildren<Text>();    
        mainICon     = transform.Find("Icon").GetComponentInChildren<Image>();
        slider       = GetComponentInChildren<Slider>();
        cost         = transform.Find("Button/tx_Coin").GetComponentInChildren<Text>();
        name         = transform.Find("txIcon_Name").GetComponentInChildren<Text>();
        exitImg      = transform.Find("Button_Close").GetComponent<Image>();
        buttonE      = transform.Find("Button").GetComponent<Transform>();
       
    }

    //타입에 맞게 데이터를 적용해서 출력한다.
   public  virtual  void   Set(int id,int gold,int amount)
    {
           ID = id;
       
        if (GameData.Instance.hasCard.ContainsKey(ID))
        {   //가지고 있는 카드의 tatal value
            int hasCards        = GameData.Instance.hasCard[ID].hasCard;
            //현재 카드가 레벨업된value 
            int level           = GameData.Instance.hasCard[ID].level;
            //현재까지의 레벨로 총카드숫자로 변환
            int curLevel        = GameData.Instance.Level[level];
            //업데이트하고 남은 카드의 value
            int remainCards     = hasCards - curLevel;
            //next level의 전체 카드 value 
            int nextRemainCards = GameData.Instance.Level[level + 1];
            //현재레벨에서 다음레벨까지  필요한 카드의 value
            int curRemainCards  = nextRemainCards - curLevel;

            LevelCount.text      = string.Format("{0} / {1}", remainCards, nextRemainCards);
            //value 카드단위당 증가할 vale설정 백분율
            float uni            = 1f/nextRemainCards;
            //최대값이 1이므로 1실수율로 변환
            float value          = remainCards * uni;
            //구매시 애니가 작동되어야 한다.
            slider.value         = value;
            //외부에서 리스너로 접근할 경우
            // slider.onValueChanged.AddListener(delegate { OnAmountChanged(); });

            //다음 레벨에  업글가능한 카드가 존재할때
            if (remainCards >= curRemainCards)
            {
                //업그레이드를 유도하는 애니이펙트 실행 화살표 움직임
            }
           // 고유아이디 1 은 UnityDatas[0]번째에 있음 따라서 -1                 
            string cardName = GameData.Instance.UnityDatas[ID-1].Name;
            mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", cardName);
            cost.text       = string.Format("{0}", gold);
            ownCards.text   = string.Format(" X {0}", amount);
            name.text       = string.Format("{0} X ", cardName);
            Count           = amount;
            CountCards      = remainCards;
            nextCardAmount  = nextRemainCards;
            CountSlider     = value;     //증가될 수치
            CountEa         = uni;       //계산된 증가량
        }

    }

    public virtual void SetUpdate(int id)
    {
           ID = id;

        if (GameData.Instance.hasCard.ContainsKey(ID))
        {   //가지고 있는 카드의 tatal value
            int hasCards        = GameData.Instance.hasCard[ID].hasCard;
            //현재 카드가 레벨업된value 
            int level           = GameData.Instance.hasCard[ID].level;
            //현재까지의 레벨로 총카드숫자로 변환
            int curLevel        = GameData.Instance.Level[level];
            //업데이트하고 남은 카드의 value
            int remainCards     = hasCards - curLevel;
            //next level의 전체 카드 value 
            int nextRemainCards = GameData.Instance.Level[level + 1];
            int nextUpGold      = GameData.Instance.LevelCost[level-1];
            //현재레벨에서 다음레벨까지  필요한 카드의 value
            int curRemainCards  = nextRemainCards - curLevel;

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
            CountSlider     = value;     //증가될 수치
            CountEa         = uni;       //계산된 증가량
        }
    }



    public virtual void SetUpdateShow(int id,ref int UpLevel,GameObject obj)
    {
        ID = id;

        if (GameData.Instance.hasCard.ContainsKey(ID))
        {   //가지고 있는 카드의 tatal value
            int hasCards        = GameData.Instance.hasCard[ID].hasCard;
            //현재 카드가 레벨업된value 
            int level           = GameData.Instance.hasCard[ID].level;
            UpLevel = level + 1;
            //현재까지의 레벨로 총카드숫자로 변환
            int curLevel        = GameData.Instance.Level[level];
            //업데이트하고 남은 카드의 value
            remainCards         = hasCards - curLevel;
            //next level의 전체 카드 value 
            nextRemainCards     = GameData.Instance.Level[level + 1];
            int nextUpGold      = GameData.Instance.LevelCost[level - 1];
            //현재레벨에서 다음레벨까지  필요한 카드의 value
            int curRemainCards  = nextRemainCards - curLevel;

            LevelCount.text     = string.Format("{0} / {1}", nextRemainCards, nextRemainCards);
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
          
            ownCards.text   = string.Format("{0}", level);
            name.text       = string.Format("{0} X ", cardName);
            nextCardAmount  = nextRemainCards;
            CountSlider     = value;         //증가될 수치
            CountEa         = uni;           //계산된 증가량

            resultCard      =  remainCards - nextRemainCards;
        }
        StartCoroutine(LevelCountAnim());
    }

    IEnumerator LevelCountAnim()
    {
        yield return new WaitForSeconds(1f);
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
    public  void CountAnime()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", CountCards, "to", CountCards + Count, "time", .6, "onUpdate", "UpdateCardDisplay", "oncompletetarget", gameObject));
               CountCards++;
        //Gold증가
        iTween.ValueTo(gameObject, iTween.Hash("from", Count, "to", 0, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
                Count--;

        iTween.ValueTo(gameObject, iTween.Hash("from", CountSlider, "to", 1, "time", .6, "onUpdate", "UpdateSliderDisplay", "oncompletetarget", gameObject));
        CountSlider += CountEa;
       
    }


    //value 가 증가할때 가지 애니실행한다. 
    void UpdateCardDisplay2(int Value)
    {               
        AppSound.instance.SE_MENU_ITEMCOUNTGOLD.Play(); 
        LevelCount.text = string.Format("{0} / {1}", Value, nextRemainCards);

        if (Value <= 0)
        {
            //현재 카드가 레벨업된value 
            int level       = GameData.Instance.hasCard[ID].level;
            // 업그레이드후 앞으로업에 필요한 레벨카드
            int curLevel    = GameData.Instance.Level[level+2];
            LevelCount.text = string.Format("{0} / {1}", Value, curLevel);
            //업그레이드된후 레벨
            ownCards.fontSize = 70;
            ownCards.text   = string.Format("{0}", level+1);
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
    void UpdateGoldDisplay(int Value)//Gold:200 ~ 1200:증가 되며 계속호출됨
    {
        AppSound.instance.SE_MENU_ITEMCOUNTGOLD.Play();
        ownCards.text = string.Format(" X {0}", Value);
    }

    void UpdateCardDisplay(int Value)//Gold:200 ~ 1200:증가 되며 계속호출됨
    {
        LevelCount.text = string.Format("{0} / {1}", Value, nextCardAmount);
    }

    void UpdateSliderDisplay(float Value)
    {
        slider.value = Value;
    }

    void OnAmountChanged()
    {

    }

}
