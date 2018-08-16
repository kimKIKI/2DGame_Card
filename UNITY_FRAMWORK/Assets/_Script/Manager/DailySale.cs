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
  

    public Image main;     //메인아이콘 이미지
    public Text  dollar ;  //gold 가격
    public Text  levelNum; //해당 카드의 업데이트까지의 숫자/전체숫자 *
    public Text  itemNum;  //클릭시 습득될 카드숫자
    public Text  charName; //Item의 네임 ID 를 확인해서 얻어와햐 한다.
    public Image bar;      //바탕
    public Image arrow;    //화살표
    public Image icon;     //coinImage
    Button btn;

    //이미 오브젝트가 생성된후 데이터 대입방법이므로 프로퍼티화 한다.
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    public int ID
    {
        get { return itemId; }
        set
        {
            if (value > 0)
            {
                itemId = value;
                charName.text = GameData.Instance.UnityDatas[ID-1].Name;
                StartCoroutine(SetItem());
            }
        }
    }

     //TODO: 현재 카드가 가지고 있는 수량확인
     IEnumerator SetItem()
    {
        yield return new WaitForSeconds(0.2f);
        dollar.text  = priceCoin.ToString();
        itemNum.text = string.Format("X {0}",ea);
        //이미지 세팅 
    }

    public void PurchaseSend()
    {
        //itemId;priceCoin;ea;
        // hasCard
        // newInfo.hasCard = (int)jsonUnitydata["PlayerInfo"]["hasCard"][i]["hasNum"];
       
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
            newInfo.ID      = itemId; //고유아이디 
            GameData.Instance.hasCard.Add(itemId, newInfo);
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
        charName.text = string.Format("{0}", "구매완료");
      
        btn.enabled   = false;    //버튼컴포넌트
        dollar.enabled = false;   //text
        levelNum.enabled = false; //text
        bar.enabled = false;      //img
        arrow.enabled = false;    //img
        icon.enabled = false;     //img
        itemNum.enabled = false;       
    }

}
