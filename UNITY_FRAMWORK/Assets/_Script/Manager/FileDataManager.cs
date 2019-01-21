using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using LitJson;
using Newtonsoft;

//임시
public  class FileDataManager  {
    //필히 static 여야함
    static string UnitData       = "UnitData";
    static string PlayerJsonData = "PlayerJsonData";
    static string MarketJsonData = "MarketJsonData";
    

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

   //public  JsonData JsonFileLoad(string FileName )
   // {
   //     string defaultPath = "/Resources/Data/"+FileName+".json";
   //     JsonData jsondata = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + defaultPath));
   //     return jsondata;
   // }

    //UnityInfo 게임에서 사용되는 유닛카드의 정보 
    public void ParsingFirstUnitData()
    {   //JsonData는 내부에 설정
        JsonData jsonUnitydata;
        LoadJson(out jsonUnitydata, UnitData);

        //JsonData jsonUnitydata = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Resources/Data/UnitData.json"));

        for (int i = 0; i < jsonUnitydata.Count; i++)
        {
            // {"ID":0,"Name":"Ice Gollum ","ATK_Type":"Top","Kinds":"Unit","Coin":300,
            //  "Jew":10,"Elixir":2,"HP":400,"Speed":"Slow","Attack":4,"Atk_Zone":100,"BuildTime":1,
            //"Sp_atk":4,"Up_Hp":10,"Up_atk":10,"Life":0,"EA":1},
            UnityInfo unit = new UnityInfo();
            //유닛의 고유ID 와 맞출려고 +1함
            unit.Id       = (int) jsonUnitydata[i]["ID"];
            unit.Name     = jsonUnitydata[i]["Name"].ToString();
            unit.Atk_Type = jsonUnitydata[i]["ATK_Type"].ToString(); //유닛타입 지상,공중
            unit.Kinds    = jsonUnitydata[i]["Kinds"].ToString();
            unit.Coin     = (int) jsonUnitydata[i]["Coin"];
            unit.Jew      = (int) jsonUnitydata[i]["Jew"];
            unit.HP       = (int) jsonUnitydata[i]["HP"];
            unit.Speed    = (int) jsonUnitydata[i]["Speed"];
            unit.Attack   = (int) jsonUnitydata[i]["Attack"];
            unit.Atk_Zone = (int) jsonUnitydata[i]["Atk_Zone"];
            unit.Build    = (int) jsonUnitydata[i]["BuildTime"];
            unit.Life     = (int) jsonUnitydata[i]["Life"];
            unit.Up_atk   = (int) jsonUnitydata[i]["Up_atk"];
            unit.Up_Hp    = (int) jsonUnitydata[i]["Up_Hp"];
            unit.SpawnEA  = (int) jsonUnitydata[i]["EA"];
            unit.Elixir   = (int) jsonUnitydata[i]["Elixir"];
            unit.SpAble   = jsonUnitydata[i]["SpAble"].ToString();
            //파일의 내용을 딕셔너리에 저장한다.
            GameData.Instance.UnityDatas.Add(unit);
            GameData.Instance.infoCards = GameData.Instance.UnityDatas.Count;
        }
    }

   
                                                        
    public void ParsingPlayerData()
    {   //현재 플레이거가 가지고 있는 카드의 상태
        // JsonData jsonUnitydata = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Resources/Data/PlayerJsonData.json"));
        //#region tempData
        ////파싱하기전 개별 데이터를 입시로 작성한다.
        //int[] card    = { 1,   2, 5,    8, 9,  11, 12 };
        //int[] cardNum = { 12, 34, 12, 106, 20, 10, 70 };
        //// 키와 값을 딕서너리로 저장
        //Dictionary<int, int> hasCard = new Dictionary<int, int> {
        //                                { 1, 12}, { 2, 34}, { 5, 12}, { 8, 106},
        //                                { 9, 20}, { 11, 10}, { 12,70} };

        //#endregion


        JsonData jsonUnitydata;
        LoadJson(out jsonUnitydata,PlayerJsonData);

        //플레이어가 가지고있는 코인, 경험치, TODO: 일단 풀어서 이벤트 음향효과 테스트후 하나의 세이브파이로
        //묶어야 한다.

        int JoinDay       = (int)jsonUnitydata["Info"][0]["Join"];
        int exp           = (int)jsonUnitydata["Info"][0]["ExprinceNum"];
        int expCount      = (int)jsonUnitydata["Info"][0]["ExprinceCount"];
        int hasCards      = (int)jsonUnitydata["Info"][0]["hasNum"];
        int Gold          = (int)jsonUnitydata["Info"][0]["coin"];
        int Jew           = (int)jsonUnitydata["Info"][0]["Jew"];
        int Trophy        = (int)jsonUnitydata["Info"][0]["Trophy"];

        string Id         = (string)jsonUnitydata["Info"][0]["ID"];
        string IdName     = jsonUnitydata["Info"][0]["IdName"].ToString();
        string PlayerName = jsonUnitydata["Info"][0]["PlayerName"].ToString();
       
        PlayerInfo playerLoad = new PlayerInfo(Id, IdName, PlayerName, JoinDay, hasCards, Gold, Jew, exp, expCount,Trophy);
        //PlayerInfo playerLoad = new PlayerInfo(Id, IdName, PlayerName, JoinDay, hasCards, Gold, Jew, exp, expCount);
        //GameData.Instance.player = playerLoad;
        GameData.Instance.players.Add(1, playerLoad);


        //ex)string TempID = jsonUnit["PlayerInfo"]["SelectDek"][3]["item-id"].ToString();
        //파일안의 목록의 수자 만큼 불러와야 되는데?
        int num = jsonUnitydata["hasCard"].Count;
        GameData.Instance.hasCards = num;

        for (int i = 0; i < num; i++)
         {
            //Dictoinary 에 저장하는 구간
            //public Dictionary<int, Dictionary<int, int>> hasCard = new Dictionary<int, Dictionary<int, int>>();
            Card newInfo    = new Card();
            newInfo.level   = (int)jsonUnitydata["hasCard"][i]["hasLevel"];
            newInfo.hasCard = (int)jsonUnitydata["hasCard"][i]["hasNum"];
            newInfo.ID      = (int)jsonUnitydata["hasCard"][i]["item_id"];

            GameData.Instance.hasCard.Add(newInfo.ID, newInfo);
              
         }

        int  selDeckNum = jsonUnitydata["SelectDek"].Count;
       
        //전체3줄
        for (int i = 0; i < selDeckNum; i++) 
        {   //json에 저장된 덱을 찾아서 GameData에 저장한다.
            // Dictionary<int, int[]> selectCards = new Dictionary<int, int[]>();
                int[] temp = new int[8];
                temp[0] = (int)jsonUnitydata["SelectDek"][i]["item_id1"];
                temp[1] = (int)jsonUnitydata["SelectDek"][i]["item_id2"];
                temp[2] = (int)jsonUnitydata["SelectDek"][i]["item_id3"];
                temp[3] = (int)jsonUnitydata["SelectDek"][i]["item_id4"];
                temp[4] = (int)jsonUnitydata["SelectDek"][i]["item_id5"];
                temp[5] = (int)jsonUnitydata["SelectDek"][i]["item_id6"];
                temp[6] = (int)jsonUnitydata["SelectDek"][i]["item_id7"];
                temp[7] = (int)jsonUnitydata["SelectDek"][i]["item_id8"];
             
            GameData.Instance.playerSelectDecks.Add(i, temp);
        }
         //tab에서 선택된것이 있는가 판단, 0 :없음   1,2,3 선택번호
         GameData.Instance.CurTab = (int)jsonUnitydata["SelectTab"][0]["Tab"];
    }

    //public void SaveJsonData()
    //{
    //    JsonData jsonUnitydata;
    //    LoadJson(out jsonUnitydata, PlayerJsonData);
    //    int num = GameData.Instance.hasCards;
    //    int loof = 0;
    //    //주의 key 값이 ID와 일치함.순서 0,1,2가 아님
    //        foreach (KeyValuePair<int, Card> pair in GameData.Instance.hasCard)
    //        {
    //            do
    //            {
    //            jsonUnitydata["hasCard"][loof]["item_id"] = pair.Value.ID;
    //            jsonUnitydata["hasCard"][loof]["hasNum"] = pair.Value.hasCard;
    //            jsonUnitydata["hasCard"][loof]["hasLevel"] = pair.Value.level;

    //            loof++;
    //                break;
    //            }
    //            while ( loof < num);
    //        }

    //    //선택창의 숫자는 정해져 있어서 그냥씀
    //    int selDeckNum = jsonUnitydata["SelectDek"].Count;
       
    //    foreach (KeyValuePair<int, int[]> pair in GameData.Instance.playerSelectDecks)
    //    {
    //        for (int i = 0; i < selDeckNum; i++)
    //        {
    //            if (pair.Key == i)
    //            {
    //                jsonUnitydata["SelectDek"][i]["item_id1"] = pair.Value[0];
    //                jsonUnitydata["SelectDek"][i]["item_id2"] = pair.Value[1];
    //                jsonUnitydata["SelectDek"][i]["item_id3"] = pair.Value[2];
    //                jsonUnitydata["SelectDek"][i]["item_id4"] = pair.Value[3];
    //                jsonUnitydata["SelectDek"][i]["item_id5"] = pair.Value[4];
    //                jsonUnitydata["SelectDek"][i]["item_id6"] = pair.Value[5];
    //                jsonUnitydata["SelectDek"][i]["item_id7"] = pair.Value[6];
    //                jsonUnitydata["SelectDek"][i]["item_id8"] = pair.Value[7];
    //            }
    //        }
           
    //    }
    //    JsonData whitedata = JsonMapper.ToJson(jsonUnitydata);
    //    File.WriteAllText(Application.streamingAssetsPath +"/"+ PlayerJsonData+".json", whitedata.ToString());
       
    //}

    public void ParsingMarket()
    {
        // JsonData jsonUnitydata = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Resources/Data/MarketJsonData.json"));
        JsonData jsonUnitydata;
        LoadJson(out jsonUnitydata,MarketJsonData);

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


        int length4 = (int)jsonUnitydata["Market"]["NorSale"].Count;

        for (int i = 0; i < length4; i++)
        {   //버튼의또는 케이스의 세팅을위한 Data
            NorInfo newNor = new NorInfo();
            newNor.caseName = (string)jsonUnitydata["Market"]["NorSale"][i]["Type"];
            newNor.PriceGold = (int)jsonUnitydata["Market"]["NorSale"][i]["PriceGold"];
            GameData.Instance.nors.Add(i, newNor);
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
            itemBoxs.Type      = (string) jsonUnitydata["Market"]["ItemBox"][i]["Type"];
            itemBoxs.PGold     = (int) jsonUnitydata["Market"]["ItemBox"][i]["presentCoin"];
            itemBoxs.General   = (int)jsonUnitydata["Market"]["ItemBox"][i]["generalUnit"];
            itemBoxs.Rare      = (int)jsonUnitydata["Market"]["ItemBox"][i]["rareUnit"];
            itemBoxs.Hero      = (int)jsonUnitydata["Market"]["ItemBox"][i]["heroUnit"];
            itemBoxs.Legendary = (int)jsonUnitydata["Market"]["ItemBox"][i]["legendaryUnit"];

            GameData.Instance.itemBoxs.Add(i, itemBoxs);
        }
        // "lunchyCase":
        // {"Gold":1800,"Jew":2,"generalNum":192,"rareNum":50,"heroNum":8,"lengendaryNum":1}

    
        ItemBoxInfo newItemBox    = new ItemBoxInfo();
        newItemBox.boxName     = "lunchyCase";
      
        //newItemBox.Jew            = (int)jsonUnitydata["Market"]["lunchyCase"][0]["Jew"];
        newItemBox.generalNum     = (int)jsonUnitydata["Market"]["lunchyCase"][0]["generalNum"];
        newItemBox.RareNum        = (int)jsonUnitydata["Market"]["lunchyCase"][0]["rareNum"];
        newItemBox.HeroNum        = (int)jsonUnitydata["Market"]["lunchyCase"][0]["heroNum"];
        newItemBox.LengendaryNum  = (int)jsonUnitydata["Market"]["lunchyCase"][0]["lengendaryNum"];
        newItemBox.productNum     = (int)jsonUnitydata["Market"]["lunchyCase"][0]["product"]; //상품품목의 갯수

        //여기 문제 
        GameData.Instance.dic_SetItems.Add("lunchyCase", newItemBox);

        ItemBoxInfo newItemBox2   = new ItemBoxInfo();
        newItemBox2.boxName       = "HeroCase";
        newItemBox2.HeroNum       = (int)jsonUnitydata["Market"]["HeroCase"][0]["heroNum1"];
        newItemBox2.HeroNum       = (int)jsonUnitydata["Market"]["HeroCase"][0]["heroNum2"];
        newItemBox2.HeroNum       = (int)jsonUnitydata["Market"]["HeroCase"][0]["heroNum3"];
        newItemBox2.productNum    = (int)jsonUnitydata["Market"]["HeroCase"][0]["product"]; 
        GameData.Instance.dic_SetItems.Add("HeroCase", newItemBox2);

        ItemBoxInfo newItemBox3    = new ItemBoxInfo();
        newItemBox3.boxName        = "legendaryCase";
        newItemBox3.LengendaryNum  = (int)jsonUnitydata["Market"]["legendaryCase"][0]["legendaryNum"];
        newItemBox3.productNum     = (int)jsonUnitydata["Market"]["legendaryCase"][0]["product"];
        GameData.Instance.dic_SetItems.Add("legendaryCase", newItemBox3);

    }

    public void LoadJson(out JsonData json, string fileName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            //string fullpath = Path.Combine("jar:file://" + Application.dataPath + "!/assets", PlayerJsonData);
            //Application.persistentDataPath로 변경시 안드로이드에서 데이터를 못불러옴
            string fullpath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName + ".json");
            WWW reader = new WWW(fullpath);

            while (!reader.isDone) { }
            string realpath = Application.persistentDataPath + "/" + fileName;
            //realpath로 reader의데이터를 복사해준다.
            System.IO.File.WriteAllBytes(realpath, reader.bytes);
            json = JsonMapper.ToObject(File.ReadAllText(realpath));
        }
        else
        {
            json = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/" + fileName + ".json"));
        }
    }
    //SaveES 사용시
    public void savees(int exp, int expcount, int gold,int trophy)
    {

        //JsonData jsonUnitydata;
        //LoadJson(out jsonUnitydata, MarketJsonData);
        //int length = (int)jsonUnitydata["Market"]["DaySale"].Count;
        
        //현재 저장되어있는 Json파일로드
        JsonData jsonDataA = new JsonData();
        LoadJson(out jsonDataA,@PlayerJsonData); //ToObject
      
         //출력된Json에 새로운 value 적용
        jsonDataA["Info"][0]["ExprinceNum"]   = exp;
        jsonDataA["Info"][0]["ExprinceCount"] = expcount;
        jsonDataA["Info"][0]["coin"]          = gold;
        jsonDataA["Info"][0]["Trophy"]        = trophy;

                 //적용된 객체를 json변형           
        JsonData whitedata = JsonMapper.ToJson(jsonDataA);
        //Streaming경로의 파일이름
        string fullpath = System.IO.Path.Combine(Application.streamingAssetsPath, PlayerJsonData + ".json");
        //testing -----
        string path1 = Application.persistentDataPath;
        path1 = path1.Substring(0, path1.LastIndexOf('/'));
        string realpath1 = Path.Combine(path1, PlayerJsonData + ".json");
        Debug.Log("ffff : "+ path1);
        //-------------
         //플렛폼 안드로이드 일때 저장
        if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            string realpath = Path.Combine(path,PlayerJsonData + ".json");
            //string realpath = Application.persistentDataPath + "/" + PlayerJsonData;
            WWW reader = new WWW(realpath);

            while (!reader.isDone) { }

            File.WriteAllText(realpath, whitedata.ToString());
        }
        else
        {
            File.WriteAllText(fullpath, whitedata.ToString());                                 
        }
    }
        //Path로 했을때 맨마지막에 자동으로 '\'가 추가된다.이걸삭제하고 '/'로 바꾸어야 한다.
        //TODO:이거 적용안됨
        public string pathForDocumentsFile(string filename)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(Path.Combine(path, "Documents"), filename);
            }

            else if (Application.platform == RuntimePlatform.Android)
            {
                string path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, filename);
            }

            else
            {
                string path = Application.dataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, filename);
            }
        }

   

    }
