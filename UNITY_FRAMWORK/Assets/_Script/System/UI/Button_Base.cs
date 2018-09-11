using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Button_Base : MonoBehaviour {

    Vector2 endScale;
    Vector2 OrigScale;
    RectTransform btn;
    float ScaleX;
    float ScaleY;
    Vector2 pos;
   
    [HideInInspector]
    public string Name;
    [HideInInspector]
    public int Price;
    [HideInInspector]
    public string CaseName;
    public Text txLabel;  //가격
    public Text txLabel2; //상품명
    public Image btnImg;
    NorInfo info = null;

    public Image main;     //메인아이콘 이미지
    string keyName = "";
    int costGold;
    int curhasGold;        //현재 플레이어가 가지고 있는 코인량
    Button button;         //구매할수없을때 표시해 주기 위해서 
    Color imgbtnColor;

   
    //플레이어가 구매할수 없을때 비활성화 시켜야 한다.
    //그래서 플레이어의 코인의 변화를 체크한다.
    public int CurhasGold
    {
        get { return curhasGold; }
        set
        {
           //플레이어의 골드가 바뀔때 마다 체크한다.
            curhasGold = Mathf.Clamp(value, 0, curhasGold);
            EventManager.Instance.PostNotification(EVENT_TYPE.GOLD_CHANGE, this, curhasGold);
        }
    }

    

    private void Awake()
    {
        btn        = gameObject.GetComponent<RectTransform>();
        OrigScale  = new Vector2(btn.localScale.x, btn.localScale.y);
        pos        = gameObject.transform.localPosition;
        button     = GetComponentInChildren<Button>();
        Small();
        //btnImg = buttonTurn.GetComponent<Image>();
        imgbtnColor = btnImg.color;
    }

  

    private void Start()
    {   //기본적으로 처음 시작시에 한번 코인량을 받고
        curhasGold = GameData.Instance.players[1].coin;
       
        EventManager.Instance.AddListener(EVENT_TYPE.GOLD_CHANGE, OnEvent);
        //TODO: 여기 없으면 다른곳의 ClickCount가 작동하지 않음
        //시작과 동시에 button의 세팅을 적용한다.
        ItemDisplayer.goldEvent += ReCollBackGold;
        CheckAmount();
    }

   
    //player의 골드값이 바뀌었으로 바뀐값을 받아온다.
    //계속 플레임별로 호출이 되므로 한번만 호출되는것은 코딩하면 안됨
    void ReCollBackGold()
    {  //바뀐값을 콜백으로 GameData에서 부터 받도록한다.
        curhasGold = GameData.Instance.players[1].coin;
        CheckAmount();
    }

    public void ButtonClick()
    {   //버튼클릭시 자신의 가격을 전달한다.
        if (curhasGold >= costGold)
        {   //UI topGold에서 차감후 GameData에 차감한 값을 전달하게 된다.
            ItemDisplayer.instance_ItemDisplayer.decreaseItem(costGold);
            OnSetCase();
        }
    }

    //스스로 변경되게 델리게이트 이벤트의 이벤트 시스템이용
    void CheckAmount()
    {
        curhasGold = GameData.Instance.players[1].coin;
        //Debug.Log("costGold :"+costGold + "- curhasGold :"+curhasGold);
        if (CurhasGold < costGold)
        {
            //구매가 되지 않도록한다.
            button.enabled = false;
            //color를 설정
            btnImg.color = Color.black;
        }
        //else
        //{   //TODO: 에러발생요인 칼라가 조건에 안맞을때도 변경을 되는 문제 발생
        //    btnImg.color = imgbtnColor;
        //}
    }
    //case 의 Item객체를세팅한다.
    public  void SetInfo(NorInfo info)
    {
       txLabel.text  = string.Format("{0}", info.PriceGold); 
       txLabel2.text = string.Format("{0}", info.caseName);
       costGold      = info.PriceGold;
        if (info.caseName == "LunchyCase")
        {
            Debug.Log("역기 문제 없나요 LunchyCase");
            //sprite이름대입
            name             = "luckyCase";
            main.sprite.name = name;
            main.sprite      = SpriteManager.GetSpriteByName("Sprite", main.sprite.name);
            //GameData.Instance.dic_SetItems의 키값
            keyName = "lunchyCase";
        }
        else if (info.caseName == "legendaryCase")
        {   //sprite네임
            Debug.Log("역기 문제 없나요 legendaryCase");
            name             = "legendaryCase";
            main.sprite.name = name;
            main.sprite      = SpriteManager.GetSpriteByName("Sprite", main.sprite.name);
            keyName          = "legendaryCase";
        }
        else if (info.caseName == "HeroCase")
        {   //sprite네임
            Debug.Log("역기 문제 없나요 HeroCase");
            name             ="giantCase";
            main.sprite.name = name;
            main.sprite      = SpriteManager.GetSpriteByName("Sprite", main.sprite.name);
            keyName          = "HeroCase";
        }
    }

  

    private void Small()
    {
        //Small -->Large 축소된 사이즈를 저장한다
        ScaleY = OrigScale.y/4f;
    }

    //시작과 동시에 player의 데이터를 받는다.
    public void OnSetCase()
    {
        CardEff_Open.keyName = keyName;
        iTween.ValueTo(gameObject, iTween.Hash("from",ScaleY, "to", OrigScale.y, "easetype", iTween.EaseType.easeOutBack, "onupdate", "ScaleButton", "time", .7));
        //움직임
     
        AppSound.instance.SE_MENU_OPEN.Play();
        //다른 클래스에서 변경됐을때 그 값을 적용받기 위해서 
        //한번더 값을 받아 온다.
        curhasGold                         = GameData.Instance.players[1].coin;
        GameData.Instance.curAddGold       = costGold;
        
    }

  
    void ScaleButton(float size)
    {
        btn.localScale = new Vector3(OrigScale.x, size);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.GOLD_CHANGE:
                OnGoldChange(Sender, (int)Param);
                break;
        }
    }

    void OnGoldChange(Component purchaseCard, int gold)
    {
       
        if (this.GetInstanceID() != purchaseCard.GetInstanceID()) return;
       
        //label.text = string.Format("{0}", _health);
        //Debug.Log("Object: " + gameObject.name + " Health is: " + NewHealth.ToString());
    }
}
