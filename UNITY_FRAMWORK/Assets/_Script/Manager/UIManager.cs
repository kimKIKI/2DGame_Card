using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

//UI에 구성에 필요한데이터
//cards, item, UI 점수 ,라벨
public class UIManager : MonoBehaviour {

    private static UIManager Item_Instance;
    private static object m_pLock = new object();

   
    public static UIManager INSTANCE
    {
        get
        {
            lock (m_pLock)
            {
                if (Item_Instance == null)
                {
                    Item_Instance = (UIManager)FindObjectOfType(typeof(UIManager));

                    if (FindObjectsOfType(typeof(UIManager)).Length > 1)
                    {
                        return Item_Instance;
                    }

                    if (Item_Instance == null)
                    {
                        GameObject singleton = new GameObject();
                        Item_Instance = singleton.AddComponent<UIManager>();
                        singleton.name = typeof(UIManager).ToString();
                        DontDestroyOnLoad(singleton);
                    }
                }
            }
            return Item_Instance;
        }
    }

    // 파싱한 정보를 다 저장할꺼에요.
    Dictionary<int, UnityInfo> dicUnityData = new Dictionary<int,UnityInfo>();
    public int CurrentID;//상품 선택시 중간  잠시 저장할 필드 

    //this 인덱서 
    public UnityInfo this[int index]
    {
        get { return dicUnityData[index]; }
        set { dicUnityData[index] = value; }
    }
    
    private void Start()
    {
        //시작과 동시에 파일메니저에서 json데이터를 찾아 가져온다.

    }

    
 
    public void JsonParsing(string FileName="PlayerJsonData")
    {
        JsonData  jsonUnit = FileDataManager.Instance.JsonFileLoad(FileName);
        //여기서 파싱 테스트 함
        string TempID = jsonUnit["PlayerInfo"]["SelectDek"][3]["item-id"].ToString();
        print(TempID);
    }


    //UnitData 파싱
    public void JsonParsingUnit()
    {
        string FileName = "UnitData";
        JsonData jsonUnit = FileDataManager.Instance.JsonFileLoad(FileName);
        //여기서 파싱 테스트 함
        int num = jsonUnit.Count; //11
                                  //string TempID = jsonUnit["PlayerInfo"]["SelectDek"][3]["item-id"].ToString();
                           
        for (int i = 0; i < jsonUnit.Count; i++)
        {
            // {"ID":0,"Name":"Ice Gollum ","ATK_Type":"Top","Kinds":"Unit","Coin":300,
            //  "Jew":10,"Elixir":2,"HP":400,"Speed":"Slow","Attack":4,"Atk_Zone":100,"BuildTime":1,
            //"Sp_atk":4,"Up_Hp":10,"Up_atk":10,"Life":0,"EA":1},
            UnityInfo unit = new UnityInfo();
            unit.Id = i;
            unit.Name = jsonUnit[i]["Name"].ToString();
            unit.Atk_Type = jsonUnit[i]["ATK_Type"].ToString(); //유닛타입 지상,공중
            unit.Kinds = jsonUnit[i]["Kinds"].ToString();
            unit.Coin = (int)jsonUnit[i]["Coin"];
            unit.Jew = (int)jsonUnit[i]["Jew"];
            unit.HP = (int)jsonUnit[i]["HP"];
            unit.Speed = (int)jsonUnit[i]["Speed"];
            unit.Attack =(int)jsonUnit[i]["Attack"];
            unit.Atk_Zone = (int)jsonUnit[i]["Atk_Zone"];
            unit.Build = (int)jsonUnit[i]["BuildTime"];
            unit.Life = (int)jsonUnit[i]["Life"];
            unit.Up_atk = (int)jsonUnit[i]["Up_atk"];
            unit.SpawnEA = (int)jsonUnit[i]["EA"];
           //파일의 내용을 딕셔너리에 저장한다.
            dicUnityData.Add(i,unit);
        }
    }

    // 아이템 추가.
    public void AddItem(UnityInfo _cInfo)
    {
        // 아이템은 고유해야 되니까, 먼저 체크!
        if (dicUnityData.ContainsKey(_cInfo.Id)) return;
        // 이제 아이템을 추가.
        dicUnityData.Add(_cInfo.Id, _cInfo);
    }


    // 전체 갯수 얻기
    public int GetItemsCount()
    {
        return dicUnityData.Count;
    }

    // 전체 리스트 얻기
    public Dictionary<int, UnityInfo> GetAllItems()
    {
        return dicUnityData;
    }

}
