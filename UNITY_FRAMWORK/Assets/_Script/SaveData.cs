using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public static class SaveData {

    //ID SaveData 데이터 처리 
    // Info 저장데이터 스크립팅 버전
    //datagrounName를 playerName변경 
    //static  public  string   GameID;
    //static  public  string   playerName;

    const float  SaveDataVersion = 0.31f;
    static Scene currScene;
	
	// === 외부 파라미터 ======================================
	public static string 	SaveDate 		= "(non)";

	// Option
	public static float 	SoundBGMVolume 	= 1.0f;
	public static float 	SoundSEVolume  	= 1.0f;
	
	
	// Etc(Don't Save)
	public static bool 		continuePlay 	= false;
	public static int		newRecord		= -1;
	public static bool		debug_Invicible	= false;
	
	// === 코드(저장 데이터 검사) ========================
	static void SaveDataHeader(string dataGroupName) {
        //player의 버전으로 reg에 SaveDataVersion | SaveDataVersion
		PlayerPrefs.SetFloat ("SaveDataVersion"	, SaveDataVersion);
		SaveDate = System.DateTime.Now.ToString("G");
        //reg에 SaveDataData | 2018.12.12..
		PlayerPrefs.SetString ("SaveDataDate"	, SaveDate);
		PlayerPrefs.SetString (dataGroupName	, "on");
       

		//dataGrounName --- Version,3.0f,2016년 6월,on
	}
	
	static bool CheckSaveDataHeader(string dataGroupName) {
		if (!PlayerPrefs.HasKey ("SaveDataVersion")) {
			Debug.Log("SaveData.CheckData : No Save Data");
			return false;
		}
		if (PlayerPrefs.GetFloat ("SaveDataVersion") != SaveDataVersion) {
			Debug.Log("SaveData.CheckData : Version Error");
			return false;
		}
		if (!PlayerPrefs.HasKey (dataGroupName)) {
			Debug.Log("SaveData.CheckData : No Group Data");
			return false;
		}

        //if (!PlayerPrefs.HasKey(GameID))
        //{
        //    Debug.Log("SaveData.Check data : GameID");
        //}

        SaveDate = PlayerPrefs.GetString ("SaveDataDate");
		return true;
	}
	
	public static bool CheckGamePlayData() {
        //"dataGroupName"
        return CheckSaveDataHeader ("GUEST_GAMEPLAYER"); 
	}
	
	// === 코드(플레이 데이터·저장, 불러오기) ===================
    // SaveGamePlay() --saveDataHeader(playerName) -세이브시에 자동으로 playerName이 header로 저장
    //                                             -현재State가 LoadLevel이 저장
	public static bool SaveGamePlay() {
        currScene = SceneManager.GetActiveScene();
        try {
			Debug.Log("SaveData.SaveGamePlay : Start");
			// SaveDataInfo   "SDG_GamePlay"
			SaveDataHeader("GUEST_GAMEPLAYER");
			{ // PlayerData------------------------------------------------------------
				zFoxDataPackString playerData = new zFoxDataPackString();
                //GameData.,,GoldAmount 프로퍼티로 호출과 동시에 json에 저장된다.
                //같이 플레이어의 로컬 playerfabs에 저장시킨다.
				playerData.Add ("Player_COIN", GameData.Instance.GoldAmount);
				playerData.Add ("Player_JEW", GameData.Instance.JewAmount);
				playerData.Add ("Player_EXP", GameData.Instance.ExpLevel);
				playerData.Add ("Player_EXPCOUNT",GameData.Instance.ExpAmount);
				
				
				playerData.PlayerPrefsSetStringUTF8 ("PlayerData", playerData.EncodeDataPackString ());
				Debug.Log(playerData.EncodeDataPackString ());
			}
			{ // StageData
				zFoxDataPackString stageData = new zFoxDataPackString();
                //zFoxUID[] uidList = GameObject.Find ("Stage").GetComponentsInChildren<zFoxUID> ();
                //foreach(zFoxUID uidItem in uidList)
                //            {
                //	if (uidItem.uid != null && uidItem.uid != "(non)") { 
                //		stageData.Add (uidItem.uid,true);
                //	}
                //} 
                
				stageData.PlayerPrefsSetStringUTF8 ("StageData_" + currScene.name, stageData.EncodeDataPackString ());
				//Debug.Log(stageData.EncodeDataPackString ());
			}
			{ // EventData
				zFoxDataPackString eventData = new zFoxDataPackString();
				//eventData.Add ("Event_KeyItem_A", PlayerController.itemKeyA);
				//eventData.Add ("Event_KeyItem_B", PlayerController.itemKeyB);
				//eventData.Add ("Event_KeyItem_C", PlayerController.itemKeyC);
				eventData.PlayerPrefsSetStringUTF8 ("EventData", eventData.EncodeDataPackString ());
				//Debug.Log(playerData.EncodeDataPackString ());
			}
			// Save
			PlayerPrefs.Save ();
			
			Debug.Log("SaveData.SaveGamePlay : End");
			return true;
			
		} catch(System.Exception e) {
			Debug.LogWarning("SaveData.SaveGamePlay : Failed (" + e.Message + ")");
		}
		return false;
	}
	
	public static  bool LoadGamePlay(bool allData) {
		try {
			// SaveDataInfo
			if (CheckSaveDataHeader("GUEST_GAMEPLAYER")) {
				Debug.Log("SaveData.LoadGamePlay : Start");
				SaveDate = PlayerPrefs.GetString ("SaveDataDate");
				if (allData) { // PlayerData
					zFoxDataPackString playerData = new zFoxDataPackString();
					playerData.DecodeDataPackString(playerData.PlayerPrefsGetStringUTF8 ("PlayerData"));
					//Debug.Log(playerData.PlayerPrefsGetStringUTF8 ("PlayerData"));
					
					 GameData.Instance.GoldAmount             = (int)playerData.GetData("Player_Coin");
					 GameData.Instance.JewAmount              = (int)playerData.GetData("Player_JEW");
					 GameData.Instance.ExpLevel               = (int)playerData.GetData("Player_EXP");
					 GameData.Instance.ExpAmount              = (int)playerData.GetData("Player_EXPCOUNT");
						
					
				}
				// StageData
				if (PlayerPrefs.HasKey("StageData_" + currScene.name)) {
					zFoxDataPackString stageData = new zFoxDataPackString();
					stageData.DecodeDataPackString(stageData.PlayerPrefsGetStringUTF8 ("StageData_" + currScene.name));
					//Debug.Log(stageData.PlayerPrefsGetStringUTF8 ("StageData_" + Application.loadedLevelName));
					//zFoxUID[] uidList = GameObject.Find ("Stage").GetComponentsInChildren<zFoxUID> ();
					//foreach(zFoxUID uidItem in uidList)
                    //{
					//	if (uidItem.uid != null && uidItem.uid != "(non)") { 
					//		if (stageData.GetData (uidItem.uid) == null) {
					//			uidItem.gameObject.SetActive(false);
					//		}
					//	}
					//}
				}
				if (allData) { // EventData
					zFoxDataPackString eventData = new zFoxDataPackString();
					eventData.DecodeDataPackString(eventData.PlayerPrefsGetStringUTF8 ("EventData"));
					//Debug.Log(playerData.PlayerPrefsGetStringUTF8 ("PlayerData"));
					//PlayerController.itemKeyA 			= (bool)eventData.GetData ("Event_KeyItem_A");
					//PlayerController.itemKeyB 			= (bool)eventData.GetData ("Event_KeyItem_B");
					//PlayerController.itemKeyC 			= (bool)eventData.GetData ("Event_KeyItem_C");
				}
				Debug.Log("SaveData.LoadGamePlay : End");
				return true;
			}
		} catch(System.Exception e) {
			Debug.LogWarning("SaveData.LoadGamePlay : Failed (" + e.Message + ")");
		}
		return false;
	}
	
	public static string LoadContinueSceneName() {
		if (CheckSaveDataHeader("GUEST_GAMEPLAYER")) {
			zFoxDataPackString playerData = new zFoxDataPackString();
			playerData.DecodeDataPackString(playerData.PlayerPrefsGetStringUTF8 ("PlayerData"));
			return (string)playerData.GetData ("Player_checkPointSceneName");
		}
		continuePlay = false;
		//여기 필요 없을듯 
		return "BoombardaFort";
	}
	
	
	
	
	// === 코드(옵션 데이터·저장, 불러오기) ================
	public static bool SaveOption() {
		try {
			Debug.Log("SaveData.SaveOption : Start");
			// 옵션 디폴트 설정  SaveDataheader --SDG_Option
			SaveDataHeader("SDG_Option");
			PlayerPrefs.SetFloat ("SoundBGMVolume", SoundBGMVolume);
			PlayerPrefs.SetFloat ("SoundSEVolume" , SoundSEVolume);
	
			// Save
			PlayerPrefs.Save ();
			Debug.Log("SaveData.SaveOption : End");
			return true;
		} catch(System.Exception e) {
			Debug.LogWarning("SaveData.SaveOption : Failed (" + e.Message + ")");
		}
		return false;
	}
	
	public static bool LoadOption() {
		try {
			if (CheckSaveDataHeader("SDG_Option")) {
				Debug.Log("SaveData.LoadOption : Start");
				
				SoundBGMVolume = PlayerPrefs.GetFloat ("SoundBGMVolume");
				SoundSEVolume  = PlayerPrefs.GetFloat ("SoundSEVolume");
				Debug.Log("SaveData.LoadOption : End");
			}
		} catch(System.Exception e) {
			Debug.LogWarning("SaveData.LoadOption : Failed (" + e.Message + ")");
		}
		return false;
	}
	
	// === 코드(저장·블러오기 삭제·초기화) ================
	public static  void DeleteAndInit(bool init) {
		Debug.Log("SaveData.DeleteAndInit : DeleteAll");
		PlayerPrefs.DeleteAll ();
		
		if (init) {
			Debug.Log("SaveData.DeleteAndInit : Init");
			SaveDate 		= "(non)";
			SoundBGMVolume  = 1.0f;
			SoundSEVolume   = 1.0f;
			
		}
	}
}

// Windows
// (ComputerName)\HKEY_CURRENT_USR\Software\(CompanyName:DefaultCompany)\(AppName)
// http://docs-jp.unity3d.com/Documentation/ScriptReference/PlayerPrefs.html

