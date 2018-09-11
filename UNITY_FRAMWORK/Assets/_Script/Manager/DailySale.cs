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
    public Text  levelNum;   //해당 카드의 업데이트까지의 숫자/전체숫자 *
    public Text  itemNum;    //클릭시 습득될 카드숫자
   
    public float timeCost  ;  //시간별 줄어들 량
    public float _coolTime ;  //실제로 카운트 되는 시간
    public Text  charName;    //Item의 네임 ID 를 확인해서 얻어와햐 한다.
    public Image bar;         //바탕
    public Image arrow;       //화살표
    public Image icon;        //coinImage
    public Image imgTime;     //구매후 타임을 보여주기 위한 타이밍
    public Image imgTimeFill; //구매후타임을 보여주기 위한 타
    public Image btnImg;      //구매할수 없을때 색깔변경을 위해서 

           bool  isPurchase; //한번 구매했는지 판단

   
   
    int curhasGold;        //현재 플레이어가 가지고 있는 코인량
    Button button;         
    Color imgbtnColor;

    //이미 오브젝트가 생성된후 데이터 대입방법이므로 프로퍼티화 한다.
    private void Awake()
    {
       button = GetComponent<Button>();
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
                charName.text = GameData.Instance.UnityDatas[ID-1].Name;
                StartCoroutine(SetItem());
            }
        }
    }

    private void Start()
    {
        StartCoroutine(coLateSet());
    }

    IEnumerator coLateSet()
    {
        yield return new WaitForSeconds(0.3f);
        curhasGold = GameData.Instance.players[1].coin;
        ItemDisplayer.goldEvent += ReCollBackGold;
        Debug.Log("DailySale -coLateSet" + priceCoin);
    }


    void ReCollBackGold()
    {  //바뀐값을 콜백으로 GameData에서 부터 받도록한다.
        curhasGold = GameData.Instance.players[1].coin;
        CheckAmount();
    }

    void CheckAmount()
    {

        curhasGold = GameData.Instance.players[1].coin;
        Debug.Log("curhasGold:" + curhasGold + "  priceCoin:" + priceCoin);
        if (priceCoin > curhasGold)
        {
            //구매가 되지 않도록한다.
            button.enabled = false;
            //color를 설정
            btnImg.color = Color.black;
        }
        //else
        //{
        //    // btnImg.color = imgbtnColor;
        //}
    }

    ////TODO: 현재 카드가 가지고 있는 수량확인
    IEnumerator SetItem()
    {
        yield return new WaitForSeconds(0.2f);
        dollar.text = priceCoin.ToString();
        itemNum.text = string.Format("X {0}", ea);
        //이미지 세팅 
    }

    public void PurchaseSend()
    {

        if (GameData.Instance.hasCard.ContainsKey(itemId))
        {
            //가지고 있는 카드라면 찾아서 추가 한다.
            var reMode = GameData.Instance.hasCard[itemId];
            reMode.hasCard += ea;
            GameData.Instance.curAddGold = priceCoin;
            GameData.Instance.players[1].coin -= priceCoin;
            //TODO:전체 레벨업을 위한 계산식 필요함
        }
        else
        {
            GameData.Instance.curAddGold = priceCoin;
            GameData.Instance.players[1].coin -= priceCoin;
            //가지고 있는 카드가 아니라면 새로운 확보카드에 새로 추가한다.
            Card newInfo = new Card();
            newInfo.level = CardLevel(ea);
            newInfo.hasCard = ea;
            newInfo.ID = itemId; //고유아이디 
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
        {   //UI topGold에서 차감후 Ga
            curhasGold = GameData.Instance.players[1].coin;

            ItemDisplayer.instance_ItemDisplayer.decreaseItem(priceCoin);
        }

        //TODO: 완전삭제 가능
        //GameData.Instance.curAddGold = priceCoin;
        //GameData.Instance.players[1].coin -= priceCoin;
        //curhasGold -= priceCoin;
        //Debug.Log("DailySale:" + curhasGold);
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
        charName.text = string.Format("{0}", "구매완료");

        button.enabled   = false;      //버튼컴포넌트
        dollar.enabled   = false;      //text
        levelNum.enabled = false;    //text
        bar.enabled      = false;         //img
        arrow.enabled    = false;       //img
        icon.enabled     = false;        //img
        //itemNum.enabled  = false;  //시간표시와겸용할수 있다.

        SetItemDelay();
    }



    IEnumerator MatketTime(float t)
    {
        //fillAmount 감소될량

        float reUseTime = 100f;
        float amount = 1 / reUseTime * t;
        int hours, minute, second;

        while (reUseTime > 0)
        {
            reUseTime -= Time.deltaTime;
            imgTimeFill.fillAmount -= amount;
            float data = imgTimeFill.fillAmount * 100; //소수2번째 자리 없애기 위해곱함
            data = Mathf.Ceil(data);               //정수만 남긴다.
            int dataInt = Mathf.RoundToInt(data);

            hours = dataInt / 3600;    //시 공식
            minute = dataInt % 3600 / 60;//분을 구하기위해서 입력되고 남은값에서 또 60을 나눈다.
            second = dataInt % 3600 % 60;//마지막 남은 시간

            itemNum.text = string.Format(" {0} : {1} : {2}", hours, minute, second);
            yield return new WaitForSeconds(t);

        }
    }

    //자신의 프로퍼티값을 호출하지 못한다.따라서 메니저에서대신호출해서 보내주는 역할을함
    void OnGoldChange(Component purchaseCard, int gold)
    {

        if (this.GetInstanceID() != purchaseCard.GetInstanceID()) return;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {

            Debug.Log("itemId:" + itemId);
        }
    }

}
