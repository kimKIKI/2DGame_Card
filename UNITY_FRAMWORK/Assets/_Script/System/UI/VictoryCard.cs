using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Animations;
public class VictoryCard : MonoBehaviour {

    public IList<Card> vicCards = new List<Card>();      //승리한 카드르 리스트로 가진다.
    Card cardInfo = null;
    
    //Shape
    public     Image       levelImg;       //레벨 뒤화면 이미지
    public     Text        txLevelNum;     //현재 카드의 엘릭서 숫자
    public     Text        txTempLevelNum; //level과 num을 비교하기 위해서 설정
    public     Text        txOwnCards;     //레벨업 될때까지 남은 카드의 숫자를 표시 한다.
    public     Image       mainICon;       //메인이미지
    public     Slider      slider;         //카드의 숫자를 받아 바 크기를 나타내 준다.
    public     Text        txElixerNum;    //엘릭서 Num
    public     GameObject  arrow;          //자동으로 컨트롤하기 위해서 
    public     Transform   hideCard;       //리스트에서 동작이 끝난후 보이지 않게 하기 위해서
    
   
    Vector3 localScale;
   
    int xPadding = 10;
    int yPadding = 40;
    RectTransform rt;

    public Text iconName;   //card의 이름

    int hasCardNum;        //전체 카드 숫자
    int levelCardNum;      //현재레벨업된 카드숫자
    int currLevel;
    int elixer;
    int cardNum;
    int cardID;

    Animator animes;

    private void Awake()
    {
        animes = this.GetComponent<Animator>();
    }

    void Start()
    {
      

        if (arrow != null)
        {
            localScale = arrow.transform.localScale;
        }
        rt = gameObject.GetComponent<RectTransform>();
    }

    public int CardID
    {
        get
        {
            return cardID;
        }
        set
        {
            cardID = value;
            if (cardInfo == null)
            {
                foreach (KeyValuePair<int,Card> ids in GameData.Instance.hasCard)
                {
                    if (ids.Key == value)
                    {
                        cardInfo = ids.Value;
                       
                        SetCardInfo(cardInfo);
                    }
                }
               
            }
        }
    } //PROPERTY END

    public int Elixer
    {
        get
        {
            return elixer;
        }

        set
        {
            elixer = value;
            //int형을  text에 넣기 위해서 변환함
            txElixerNum.text = string.Format("{0}", value);
        }
    }

    public int CardNum
    {
        get
        {
            return cardNum;
        }

        set
        {
            cardNum = value;
        }
    }

    //전체카드의 숫자를 입력받는다.
    public int HasCardNum
    {
        get
        {
            return hasCardNum;
        }
        set
        {
            hasCardNum = value;
        }
    }

    public void AnimeClick()
    {
        animes.SetBool("bClickOn",true);
    }

    public void AnimeOffClick()
    {
        animes.SetBool("bClickOn", false);
    }
  

    public void SetCardInfo(Card newCard)
    {
        if (cardInfo == null)
        {
        cardInfo = newCard;
        iconName.text = string.Format("{0}", newCard.IconName);
        mainICon.sprite.name = newCard.IconName;
        mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", mainICon.sprite.name);
        mainICon.enabled = true;
           
        }
        else
        {
            cardInfo = null;
           
        }
        //TODO: 카드의 특별한 능력을 추가할 것인가?

    }

    //패배한 유닛을 뽑을때 
    public void PumpUnity()
    {
        AppSound.instance.SE_CARD_PUMP.Play();
    }
	
	
}
