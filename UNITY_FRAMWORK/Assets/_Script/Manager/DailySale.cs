using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailySale : MonoBehaviour {

    [HideInInspector]
   public int itemId;
   [HideInInspector]
   public  int priceCoin;
    [HideInInspector]
   public  int priceJew;
    [HideInInspector]
   public  int ea;

    //임시로 값이 들어 왔는지확인하기 위해서 


    public Image main;       //메인아이콘 이미지
    public Text  dollar ;    //gold 가격
    public Text  LevelCount; //해당 카드의 업데이트까지의 숫자/전체숫자 *
    public Text  TimeCount;  //시간 표시될때 보여줘야할 text
    public Text  itemNum;    //클릭시 습득될 카드숫자
   
    public float timeCost  ;  //시간별 줄어들 량
    public float _coolTime ;  //실제로 카운트 되는 시간
    public Text  charName;    //Item의 네임 ID 를 확인해서 얻어와햐 한다.
    public Image arrow;       //화살표
    public Image icon;        //coinImage
    public Image imgTime;     //구매후 타임을 보여주기 위한 타이밍
    public Image imgTimeFill; //구매후타임을 보여주기 위한 타이머 이미지
    public Image btnImg;      //구매할수 없을때 색깔변경을 위해서 
    public GameObject set;    //구매됐을때 가려줘야할 부분들
           bool  isPurchase; //한번 구매했는지 판단


  
    int     curhasGold;        //현재 플레이어가 가지고 있는 코인량
    Button  button;         
    Color   imgbtnColor;
    Slider  slider;
    private int playerGold;

    //이미 오브젝트가 생성된후 데이터 대입방법이므로 프로퍼티화 한다.
    private void Awake()
    {
            button = GetComponent<Button>();
            button.onClick.AddListener(delegate
            {
                //Main_Scene.OPenPurchaseBox;
                CanvasForm.Instance.purchaseBox.SetActive(true);
                CanvasForm.Instance.Set(itemId, priceCoin, ea,this.gameObject);
            });

        slider = GetComponentInChildren<Slider>();
    }

    public int ID
    {
        get { return itemId; }
        set
        {
            if (value > 0)
            {
                itemId = value;
               //딕셔너리의 ID 는 1부터 시작하므로 인덱스 0번째로 맞추기 위해서 -1
                charName.text = GameData.Instance.UnityDatas[value-1].Name;
                StartCoroutine(SetItem());
            }
        }
    }

    public int PlayerGold
    {
        get
        {
            return playerGold;
        }

        set
        {
            playerGold = value;
            if (priceCoin > curhasGold)
            {   //event Manager이 작동안함.
                EventManager.Instance.PostNotification(EVENT_TYPE.GOLD_CHANGE, this, playerGold);
            }
               
        }
    }

    private void Start()
    {
        set.SetActive(true);
        StartCoroutine(coLateSet());
        CheckAmount();
    }

    //TODO: ERROR 작동안함 ..
    public void StartManager()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.GOLD_CHANGE, OnEvent);
    }

    IEnumerator coLateSet()
    {
        yield return new WaitForSeconds(0.3f);
        curhasGold              = GameData.Instance.players[1].coin;
        ItemDisplayer.goldEvent += ReCollBackGold;
    }


    void ReCollBackGold()
    {  //바뀐값을 콜백으로 GameData에서 부터 받도록한다.
        curhasGold = GameData.Instance.players[1].coin;
        CheckAmount();
    }

    void CheckAmount()
    {
        curhasGold = GameData.Instance.players[1].coin;
        //Debug.Log("curhasGold:" + curhasGold + "  priceCoin:" + priceCoin);

        if (priceCoin > curhasGold)
        {
            //구매가 되지 않도록한다.
            button.enabled = false;
            //color를 설정
            btnImg.color = Color.black;
        }
    }

    ////TODO: 현재 카드가 가지고 있는 수량확인
    IEnumerator SetItem()
    {
        yield return new WaitForSeconds(0.2f);
        dollar.text     = priceCoin.ToString();
        itemNum.text    = string.Format("X {0}", ea);
        //이미지 세팅 
    }

   

    public void PurchaseSend()
    {
        if (GameData.Instance.hasCard.ContainsKey(itemId))
        {
            //가지고 있는 카드라면 찾아서 추가 한다.
            var reMode                         = GameData.Instance.hasCard[itemId];
            reMode.hasCard                    += ea;
            GameData.Instance.curAddGold       = priceCoin;
            GameData.Instance.players[1].coin -= priceCoin;
            //TODO:전체 레벨업을 위한 계산식 필요함
        }
        else
        {
            GameData.Instance.curAddGold       = priceCoin;
            GameData.Instance.players[1].coin -= priceCoin;
            //가지고 있는 카드가 아니라면 새로운 확보카드에 새로 추가한다.
            Card newInfo                       = new Card();
            newInfo.level                      = CardLevel(ea);
            newInfo.hasCard                    = ea;
            newInfo.ID                         = itemId; //고유아이디 
            GameData.Instance.hasCard.Add(itemId, newInfo);
        }
    }

    ////구매후 대기 상태의 아이템 표시 
    void SetItemDelay()
    {
        imgTime.gameObject.SetActive(true);
        imgTimeFill.gameObject.SetActive(true);
        isPurchase = true;
        StartCoroutine(MatketTime(1f));
    }

    public void OnClick()
    {
        //AppSound.instance.SE_MENU_OPEN.Play();
        //다른 클래스에서 변경됐을때 그 값을 적용받기 위해서 
        //한번더 값을 받아 온다. 
        if (curhasGold >= priceCoin)
        {   //UI topGold에서 차감후  현재 돈이 더 많을때
            curhasGold = GameData.Instance.players[1].coin;
            ItemDisplayer.instance_ItemDisplayer.decreaseItem(priceCoin);

        }
        else
        {
            //혹시 몰라서 돈없을 못접근하긴 하는데...
        }
            
    }

    //Main_Scene에 있는데 중복되 코드 델리게이트로 or 인터페이스화 실습
    int CardLevel(int cardNum)
    {
        int n = 0;
        for (int i = 0; i < GameData.Instance.Level.Length; i++)
        {
            if (cardNum - GameData.Instance.Level[i] >= 0)
            {
                continue;
            }
            else
            {
                n = i + 1;
                break;
            }
        }
        return n;
    }

    public void PurchaseEnd()
    {
        //구매가 완료됐을때의 표시 
        //코인표시,갯수 표시 이미지 변경, 버튼 효과 false;
        charName.text    = string.Format("{0}", "구매완료");
        set.SetActive(false);
        SetItemDelay();
    }


    public  void Slider(int ID)
    {
        //level 에 필요한 모든 카드의 합
        int remainCards = LevelUpRemain(ID);
        int level       = GameData.Instance.hasCard[ID].level;

        //next level의 전체 카드 value 
        int nextRemainCards = GameData.Instance.Level[level];
        //현재까지의 레벨로 레벨에 필요한카드숫자로 변환 ex) 5 & 50
        int curLevel = GameData.Instance.Level[level-1];
        //현재레벨에서 다음레벨까지  필요한 카드의 value
        int curRemainCards = nextRemainCards - curLevel;
       
        LevelCount.text = string.Format("{0} / {1}", remainCards, nextRemainCards);
        //value 카드단위당 증가할 vale설정 백분율
        float uni = 1f / nextRemainCards;
        //최대값이 1이므로 1실수율로 변환
        float value = remainCards * uni;
        //구매시 애니가 작동되어야 한다.
        slider.value = value;
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
            curLevel = GameData.Instance.Level[i];
            hasCards -= curLevel;
        }

        return hasCards;
    }

    IEnumerator MatketTime(float t)
    {
        //fillAmount 감소될량 10번 호출된다.
        TimeCount.gameObject.SetActive(true);
        float reUseTime            = 10f;
        float amount               = 1 / reUseTime * t;
        int hours, minute, second;
        int count = 0; 
        while (reUseTime >= 0)
        {
            count++;
            reUseTime              -= Time.deltaTime;
            imgTimeFill.fillAmount -= amount;
            float data              = imgTimeFill.fillAmount * 100; //소수2번째 자리 없애기 위해곱함
            data                    = Mathf.Ceil(data);               //정수만 남긴다.
            int dataInt             = Mathf.RoundToInt(data);

            hours                   = dataInt / 3600;    //시 공식
            minute                  = dataInt % 3600 / 60;//분을 구하기위해서 입력되고 남은값에서 또 60을 나눈다.
            second                  = dataInt % 3600 % 60;//마지막 남은 시간

            TimeCount.text             = string.Format(" {0} : {1} : {2}", hours, minute, second);

            if ( count >= reUseTime)
            {
                NewItem();
              
                break;
            }
            yield return new WaitForSeconds(t);
        }
    }

    //자신의 프로퍼티값을 호출하지 못한다.따라서 메니저에서대신호출해서 보내주는 역할을함
    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.LEVEL_CHANGE:
                OnGoldChange(Sender, (int)Param);
                break;
        }
    }

    //EventManager이 작동안함 이유  징그럽게 모르겠다.
    void OnGoldChange(Component purchaseCard, int gold)
    {
        string aa = purchaseCard.name;
        Debug.Log("why  why why  : " + aa);
        CheckAmount();
        if (this.GetInstanceID() != purchaseCard.GetInstanceID()) return;
    }

    void NewItem()
    {
        charName.text = string.Format("{0}", "구매완료");
        set.SetActive(true);
        //현재 가지고있는카드중 하나를 랜덤하게 받는다.
        int length = GameData.Instance.hasCard.Count;
        int rnd = UnityEngine.Random.Range(0, length);
        ID = GameData.Instance.hasCard[rnd].ID;
        string name = GameData.Instance.UnityDatas[ID - 1].Name;
        main.sprite = SpriteManager.GetSpriteByName("Sprite", name);
        imgTimeFill.fillAmount = 1;
        imgTime.gameObject.SetActive(false);
        imgTimeFill.gameObject.SetActive(false);
        TimeCount.gameObject.SetActive(false);

    Slider(ID);
    }

   
}
