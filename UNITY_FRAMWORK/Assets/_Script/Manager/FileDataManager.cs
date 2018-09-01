using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

//임시
public  class FileDataManager  {

    static string UnitData = "UnitData";


    // 싱글톤 인스턴스를 저장.
    private static  FileDataManager uniqueInstance;
    private static object _lock = new System.Object();
    //GameData에서 FileDataManager에 접근하기 위해서 사용한다.
    public GameData GameData;

    // 생성자.
    public FileDataManager() { }

    // 외부에서 접근할 수 있도록 함.
    public static FileDataManager Instance
    {
        get
        {
            if (uniqueInstance == null)
            {
                // lock으로 지정된 블록안의 코드를 하나의 쓰레드만 접근하도록 한다.
                lock (_lock)
                {
                    if (uniqueInstance == null)
                        uniqueInstance = new FileDataManager();
                }
            }
            return uniqueInstance;
        }
    }

    //파일메니저에서 할일 파일 경로에 있는 파일을 있나 없나 파악하고 있다면 실행하고 없다면 에러를 발생시킨다.
    //각기 다른 파일을 관리할수 있도록 한다.
    //파싱할수 있도록 한다.

   public  JsonData JsonFileLoad(string FileName )
    {
        string defaultPath = "/Resources/Data/"+FileName+".json";
        JsonData jsondata = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + defaultPath));
        return jsondata;
    }

    //UnityInfo 게임에서 사용되는 유닛카드의 정보 
    public void ParsingFirstUnitData()
    {

        JsonData jsonUnitydata = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Resources/Data/UnitData.json"));
   
        for (int i = 0; i < jsonUnitydata.Count; i++)
        {
            // {"ID":0,"Name":"Ice Gollum ","ATK_Type":"Top","Kinds":"Unit","Coin":300,
            //  "Jew":10,"Elixir":2,"HP":400,"Speed":"Slow","Attack":4,"Atk_Zone":100,"BuildTime":1,
            //"Sp_atk":4,"Up_Hp":10,"Up_atk":10,"Life":0,"EA":1},
            UnityInfo unit = new UnityInfo();
            unit.Id       = i;
            unit.Name     = jsonUnitydata[i]["Name"].ToString();
            unit.Atk_Type = jsonUnitydata[i]["ATK_Type"].ToString(); //유닛타입 지상,공중
            unit.Kinds    = jsonUnitydata[i]["Kinds"].ToString();
            unit.Coin     = (int)jsonUnitydata[i]["Coin"];
            unit.Jew      = (int)jsonUnitydata[i]["Jew"];
            unit.HP       = (int)jsonUnitydata[i]["HP"];
            unit.Speed    = (int)jsonUnitydata[i]["Speed"];
            unit.Attack   = (int) jsonUnitydata[i]["Attack"];
            unit.Atk_Zone = (int) jsonUnitydata[i]["Atk_Zone"];
            unit.Build    = (int) jsonUnitydata[i]["BuildTime"];
            unit.Life     = (int) jsonUnitydata[i]["Life"];
            unit.Up_atk   = (int) jsonUnitydata[i]["Up_atk"];
            unit.SpawnEA  = (int) jsonUnitydata[i]["EA"];
            unit.Elixir   = (int)jsonUnitydata[i]["Elixir"];
            unit.SpAble   = jsonUnitydata[i]["SpAble"].ToString();
            //파일의 내용을 딕셔너리에 저장한다.
            GameData.Instance.UnityDatas.Add(unit);
            GameData.Instance.infoCards = GameData.Instance.UnityDatas.Count;
        }
    }

    public void ParsingPlayerData()
    {   //현재 플레이거가 가지고 있는 카드의 상태
        JsonData jsonUnitydata = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Resources/Data/PlayerJsonData.json"));
        #region tempData
        ////파싱하기전 개별 데이터를 입시로 작성한다.
        //int[] card    = { 1,   2, 5,    8, 9,  11, 12 };
        //int[] cardNum = { 12, 34, 12, 106, 20, 10, 70 };
        //// 키와 값을 딕서너리로 저장
        //Dictionary<int, int> hasCard = new Dictionary<int, int> {
        //                                { 1, 12}, { 2, 34}, { 5, 12}, { 8, 106},
        //                                { 9, 20}, { 11, 10}, { 12,70} };

        #endregion

        //ex)string TempID = jsonUnit["PlayerInfo"]["SelectDek"][3]["item-id"].ToString();
        //파일안의 목록의 수자 만큼 불러와야 되는데?
        int num = jsonUnitydata["PlayerInfo"]["hasCard"].Count;
        GameData.Instance.hasCards = num;

        for (int i = 0; i < num; i++)
        {
            //Dictoinary 에 저장하는 구간
            //public Dictionary<int, Dictionary<int, int>> hasCard = 
            //new Dictionary<int, Dictionary<int, int>>();
            Card newInfo    = new Card();
            newInfo.level   = (int)jsonUnitydata["PlayerInfo"]["hasCard"][i]["hasLevel"];
            newInfo.hasCard = (int)jsonUnitydata["PlayerInfo"]["hasCard"][i]["hasNum"];
            newInfo.ID      = (int)jsonUnitydata["PlayerInfo"]["hasCard"][i]["item-id"];

            GameData.Instance.hasCard.Add(newInfo.ID, newInfo);
              
        }

        int  selDeckNum = jsonUnitydata["PlayerInfo"]["SelectDek"].Count;
       
        //전체3줄
        for (int i = 0; i < selDeckNum; i++) 
        {   //json에 저장된 덱을 찾아서 GameData에 저장한다.
            // Dictionary<int, int[]> selectCards = new Dictionary<int, int[]>();
                int[] temp = new int[4];
                temp[0] = (int)jsonUnitydata["PlayerInfo"]["SelectDek"][i]["item-id1"];
                temp[1] = (int)jsonUnitydata["PlayerInfo"]["SelectDek"][i]["item-id2"];
                temp[2] = (int)jsonUnitydata["PlayerInfo"]["SelectDek"][i]["item-id3"];
                temp[3] = (int)jsonUnitydata["PlayerInfo"]["SelectDek"][i]["item-id4"];
            GameData.Instance.playerSelectDecks.Add(i, temp);
        }
         //tab에서 선택된것이 있는가 판단, 0 :없음   1,2,3 선택번호
         GameData.Instance.CurTab = (int)jsonUnitydata["PlayerInfo"]["SelectTab"][0]["Tab"];


        //플레이어가 가지고있는 코인, 경험치, TODO: 일단 풀어서 이벤트 음향효과 테스트후 하나의 세이브파이로
        //묶어야 한다.

        int JoinDay = (int)jsonUnitydata["PlayerInfo"]["Info"][0]["Join"];
        string Id = (string)jsonUnitydata["PlayerInfo"]["Info"][0]["ID"];
        string IdName = jsonUnitydata["PlayerInfo"]["Info"][0]["IdName"].ToString();
        string PlayerName = jsonUnitydata["PlayerInfo"]["Info"][0]["PlayerName"].ToString();
       
        PlayerInfo playerLoad = new PlayerInfo(Id, IdName, PlayerName, JoinDay);
        //플레이어 재정상태
        playerLoad.cardHas = (int)jsonUnitydata["PlayerInfo"]["has"][0]["hasNum"];
        playerLoad.coin    = (int)jsonUnitydata["PlayerInfo"]["has"][0]["coin"];
        playerLoad.jew     = (int)jsonUnitydata["PlayerInfo"]["has"][0]["Jew"];

       
        playerLoad.telephone   = (int)jsonUnitydata["PlayerInfo"]["Info"][0]["telPhone"];
        playerLoad.exp         = (int)jsonUnitydata["PlayerInfo"]["Info"][0]["Exprince Num"];
        playerLoad.expCount    = (int)jsonUnitydata["PlayerInfo"]["Info"][0]["Exprince count"];

        //GameData.Instance.player = playerLoad;
        GameData.Instance.players.Add(1, playerLoad);
    }

    public void ParsingMarket()
    {
        JsonData jsonUnitydata = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Resources/Data/MarketJsonData.json"));
        int length =  (int)jsonUnitydata["Market"]["DaySale"].Count;
        for (int i = 0; i < length;i++)
        {
            MarketInfo day = new MarketInfo();
        
             day.ID    = (int)jsonUnitydata["Market"]["DaySale"][i]["item-id1"];
             day.Gold  = (int)jsonUnitydata["Market"]["DaySale"][i]["PriceCoin"];
             day.Jew   = (int)jsonUnitydata["Market"]["DaySale"][i]["PriceJew"];
             day.EA    = (int)jsonUnitydata["Market"]["DaySale"][i]["EA"];

            GameData.Instance.dailys.Add(i, day);
        }

        int length1 = (int)jsonUnitydata["Market"]["RoyleSale"].Count;

        for (int i = 0; i < length1; i++)
        {
            MarketRoyle royle = new MarketRoyle();
           
            royle.Jew        = (int)jsonUnitydata["Market"]["RoyleSale"][i]["PriceJew"];
            royle.CaseName   = (string)jsonUnitydata["Market"]["RoyleSale"][i]["BoxName"];
            royle.CaseIndex  = (int)jsonUnitydata["Market"]["RoyleSale"][i]["case"];
            GameData.Instance.royles.Add(i, royle);
        }

        int length2 = (int)jsonUnitydata["Market"]["ItemBox"].Count;
        for (int i = 0; i < length2; i++)
        {
            MarketItemBoxInfo itemBoxs = new MarketItemBoxInfo();
            itemBoxs.Type    =  (string) jsonUnitydata["Market"]["ItemBox"][i]["Type"];
            itemBoxs.PGold   = (int) jsonUnitydata["Market"]["ItemBox"][i]["presentCoin"];
            itemBoxs.General = (int)jsonUnitydata["Market"]["ItemBox"][i]["generalUnit"];
            itemBoxs.Rare    = (int)jsonUnitydata["Market"]["ItemBox"][i]["rareUnit"];
            itemBoxs.Hero    = (int)jsonUnitydata["Market"]["ItemBox"][i]["heroUnit"];
            itemBoxs.Legendary = (int)jsonUnitydata["Market"]["ItemBox"][i]["legendaryUnit"];

            GameData.Instance.itemBoxs.Add(i, itemBoxs);
        }
        // "lunchyCase":
        // {"Gold":1800,"Jew":2,"generalNum":192,"rareNum":50,"heroNum":8,"lengendaryNum":1}

        int length3 = (int)jsonUnitydata["Market"]["lunchyCase"].Count;

        ItemBoxInfo newItemBox = new ItemBoxInfo();
        newItemBox.boxName     = "lunchyCase";
        newItemBox.Gold        =  (int)jsonUnitydata["Market"]["lunchyCase"][0]["Gold"];
        newItemBox.Jew         = (int)jsonUnitydata["Market"]["lunchyCase"][0]["Jew"];
        newItemBox.generalNum  = (int)jsonUnitydata["Market"]["lunchyCase"][0]["generalNum"];
        newItemBox.RareNum     = (int)jsonUnitydata["Market"]["lunchyCase"][0]["rareNum"];
        newItemBox.HeroNum     = (int)jsonUnitydata["Market"]["lunchyCase"][0]["heroNum"];
        newItemBox.LengendaryNum = (int)jsonUnitydata["Market"]["lunchyCase"][0]["lengendaryNum"];
        newItemBox.productNum = (int)jsonUnitydata["Market"]["lunchyCase"][0]["product"]; //상품품목의 갯수

        //여기 문제 
        GameData.Instance.dic_SetItems.Add("lunchyCase", newItemBox);
    }

}
