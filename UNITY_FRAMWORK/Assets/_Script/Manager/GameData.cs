﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System;

//전체 게임실행에 필요한 데이터 
public sealed class GameData
{

    // 싱글톤 인스턴스를 저장.
    private static volatile GameData uniqueInstance;
    private static object _lock = new System.Object();


    // 생성자.
    private GameData() { }

    // 외부에서 접근할 수 있도록 함.
    public static GameData Instance
    {
        get
        {
            if (uniqueInstance == null)
            {
                //lock으로 지정된 블록안의 코드를 하나의 쓰레드만 접근하도록 한다.
                lock (_lock)
                {
                    if (uniqueInstance == null)
                        uniqueInstance = new GameData();
                }
            }
            return uniqueInstance;
        }
    }

    public int CurSlotID
    {
        get
        {
            return curSlotID;
        }

        set
        {
            if (curSlotID != value)
            {
                afterSlotID = curSlotID;
                curSlotID = value;
            }
            afterSlotEve = value;
        }
    }

    public int ShowID
    {
        get
        {
            return showID;
        }

        set
        {
            // 새로 입력된 값이 다른때만 갱신하게 한다.
            if (showID != value)
            {
                afterShowID = showID;
                showID      = value;
            }
        }
    }

    public int CurTab
    {
        get
        {
            return curTab;
        }

        set
        {
            if (curTab != value)
            {
                afterTab = curTab;
                curTab = value;
            }
            afterTabEve = value;
        }
    }

    public int PanelItem
    {
        get { return panelItem; }
        set
        {
            if (panelItem != value)
            {
                panelItemAfter = panelItem;
                panelItem      = value;
            }
        }
    }

  
    public int[] Level = { 0, 2, 4, 10, 20, 50, 100, 200, 400, 1000, 2000, 4000, 5000 };
    //유닛 고유의 데이터 ..이데이터를 바탕으로 player의 데이터와 
    //FileDataManager에서부터 데이터가 들어온다.
    public IList<UnityInfo> UnityDatas       = new List<UnityInfo>();

    //현재 선택된 tab의 카드중 선택되어있지 않지만 정렬탭에 보여지는 카드 
    public IList<UnityCard> panelSlots       = new List<UnityCard>();
    //가려져 있는 슬롯 정렬의 슬록시 움직이기 때문에 필요하다.
    public IList<UnityCard> panelBackSlots   = new List<UnityCard>();
          
    public Dictionary<int, Card> hasCard = new Dictionary<int, Card>(); // ID , Card
    public PlayerInfo player = new PlayerInfo();
    public Dictionary<int, PlayerInfo> players = new Dictionary<int, PlayerInfo>();
    //덱의 선택에 필요한 정보를 담는다.
    public Dictionary<int, int[]> playerSelectDecks = new Dictionary<int, int[]>();
    public Dictionary<int, MarketInfo> dailys = new Dictionary<int, MarketInfo>();
    //판매가격및 아이템카드의 오브젝트를 세팅한다.

    public Dictionary<int, MarketRoyle> royles = new Dictionary<int, MarketRoyle>();
    //아이템이 구매되어 열렸을때의 내용물을 세팅한다.
    public Dictionary<int, MarketItemBoxInfo> itemBoxs = new Dictionary<int, MarketItemBoxInfo>();
    //상품별 수량을 기록하기 위해서 저장
    public Dictionary<string, ItemBoxInfo> dic_SetItems = new Dictionary<string, ItemBoxInfo>();
    //TODO :리소스 메니져가 구성되어야 할부분
    //REesourcs 의 오브젝트를 가져오기 위해 생성한다.                    
    public IList<GameObject> prefabs = new List<GameObject>();

    //승리한카드의 경험치및 보사을 판단하기 위해서 저장한다.
    public Dictionary<int, IList<Card>> playerVic;
    public Dictionary<int, IList<Card>> comVic;
    // player vs  com 중 승리한 것을 받는다 . 3 :player 승리 ,2:무승부 ,1 :com 승리
    public int vicResult = 0;

    public eGameState eGamestate = eGameState.NONE; // GameManager에 상태를 전달하기 위해서 설정한다.

    public int TempNUM;
    public int hasCards;          //현재 가지고 있는 카드의 종류수
    public int infoCards;

    private int curTab = 0;       //default 텝에 선택된 인덱스를 저장한다.

    private int showID;           //현재 선택되어 보여지고 있는 카드의 ID
    public int afterShowID;       //이전에 눌린키 하나를 기록하게 한다.
    public int afterSlotID;
    public int afterTab = 0;

    public int afterTabEve;        //두번째 선택아이템 카드가 선택됐을때 다시 Hide해주기 위해서 저장함
    public int afterSlotEve;

    public int toTabSlot;         //Tab의 위치에서 array정렬위치로 이동하기 위한slot 

    int panelItem;                //UI의 bottom 목록
    public int panelItemAfter;

    public Vector3 toTopPos;      //Tab으로 이동하게될 위치 좌표      

    public bool isShowID;         //하나만 보여져야한다.다른 클릭이 이루어진다면 전부 안보이게 해야 한다.

    public int curSwitchCard;     //현재 교체하기 위해서 선택된 카드<------- 
    public int fromSwitchSlot;    //움직일 슬롯<--------

    private int curSlotID;         //현재 선택된 아이템의 slot 인덱스 번호
    public int curSlotIndex;       //Tab 에서 선택됐을때의 Tab내의 인덱스 번호
    public int fromSwitchId;       //이동할 id 현재가 아니라 미리 저장해 놓아야 한다.
    public int curAddGold;         // 스코어의 애니를 위해서 증가분의 금액을 저장한다.

    public int fromSwitchCard;      //이동할 선택된 카드 
    public Vector3 fromSwitchPos;   //움직일 좌표
    public int toSwitchCard;
    public Vector3 toSwitchPos;
    public bool isSwitch;            //현재 바꿀수 있는 상태인지 판단

    public eGameState gameState = eGameState.NONE; 

    // temp.GetComponent<UnityCard>().mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", "Sample_UI_1");
    //전체 테두리
    //  temp.GetComponent<Image>().sprite = SpriteManager.GetSpriteByName("Sprite", "Sample_UI_1");

    void RandomAddCards(int genCardNum)
    {
        //일반카드가 많을때 2개의 종류로 나누어 줄수 있다.
        if (genCardNum > 45)
        {
            int[] numCard = new int[2];
            
            for (int i = 0; i < numCard.Length; i++)
            {
                int ID = UnityEngine.Random.Range(1, 12);
                 //6
                if (numCard[i] != numCard[i+1])
                {
                    numCard[i] = ID;
                }
            }
        }

        //int randomID = UnityEngine.Random.Range(1, 12);
        //if (hasCard.ContainsKey(randomID))
        //{
        //    var pair = hasCard[randomID];
        //}
    }
     
}


