using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;


public   class EsSaveDataManager :MonoBehaviour{

    //플레이어의 현재 설정상태를 저장해야 된다.
    //값자기 접속이 끈겼을때 복구가능한 데이터가 포함되어야 한다.

    //const float SaveDataVersion = 0.01f;
    //static Scene currScene;

    //// === 외부 파라미터 ======================================
    //public static string SaveDate = "(non)";
    //// Option
    //public static float SoundBGMVolume = 1.0f;
    //public static float SoundSEVolume = 1.0f;
    ////저장되어야 할 데이터
    ////player  카드량
    ////전체 게임상태?
    ////레벨.
    ////증가된 레벨
    ////코인량,보석량,경험치량
    public int Gold = 11111;
    public int Jew = 555;



   //public  void AutoSave()
   // {

   //     //ES2.Save(123, Application.dataPath + "/Resources/myFile.bytes?tag=myInt");

      
   //     //ES2.Save(GameData.Instance.GoldAmount, "GoldAmount");
   //     //ES2.Save(GameData.Instance.JewAmount, "JewAmount");
   //     //ES2.Save(GameData.Instance.ExpLevel, "ExpLevel");
   //     //ES2.Save(GameData.Instance.ExpAmount, "ExpAmount");

   //     //JsonData jsonUnitydata = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Resources/Data/UnitData.json"));
   //     ////jons파일에 접근해서 개별 플레이어 파일을 수정해야함
   //     //JsonData saveAmount = JsonMapper.ToJson(GameData.Instance.GoldAmount);

   //     //jsonUnitydata["PlayerInfo"]["Info"][0]["Exprince Num"] = GameData.Instance.ExpLevel;
   //     //jsonUnitydata["PlayerInfo"]["Info"][0]["Exprince count"] = GameData.Instance.ExpAmount;
   //     //jsonUnitydata["PlayerInfo"]["has"][0]["coin"] = GameData.Instance.GoldAmount;
   //     //jsonUnitydata["PlayerInfo"]["has"][0]["Jew"] = GameData.Instance.JewAmount;
   //     //변경된 내용 저장?

   // }

    //parsing 이 이루어진다음에 발생할 부분임
    //인터넷에서 체킹은 어제 이루어 지는가?
    //public  void  StartLoad()
    //{
    //    if (GameData.Instance.players.ContainsKey(1))
    //    {
    //        GameData.Instance.ExpLevel = GameData.Instance.players[1].exp;
    //        GameData.Instance.ExpAmount = GameData.Instance.players[1].expCount;
    //        //string defaultExp = playerLevel[GameData.Instance.players[1].exp].ToString();
    //        GameData.Instance.GoldAmount = GameData.Instance.players[1].coin;
    //        GameData.Instance.JewAmount= GameData.Instance.players[1].jew;
    //    }
    //}

   

     
            //ES2.Exists("C:/Users/kimdaehwan/MTest");
            //ES2.Save<int>("myKey", Application.dataPath + "/Resources/myFile.bytes");
            //ES2.Save<int>("myKey", 123, Application.dataPath + "/Resources/myFile.bytes");
            //AssetDatabase.Refresh();
            // File.WriteAllText(Application.dataPath + "/Resources/User.Json", JCD.ToString());
        
   

}
