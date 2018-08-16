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

	// HiScore
	static int[] 	HiScoreInitData = new int[10] { 300000,100000,75000,50000,25000,10000,7500,5000,2500,1000 };
	public static int[] 	HiScore 		= new int[10] { 300000,100000,75000,50000,25000,10000,7500,5000,2500,1000 };
	
	// Option
	public static float 	SoundBGMVolume 	= 1.0f;
	public static float 	SoundSEVolume  	= 1.0f;
	#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
	public static bool		VRPadEnabled 	= true;
	#else
	public static bool		VRPadEnabled 	= false;
	#endif
	
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
				//playerData.Add ("Player_HPMax", PlayerController.nowHpMax);
				//playerData.Add ("Player_HP"	  , PlayerController.nowHp);
				//playerData.Add ("Player_Score", PlayerController.score);
				//playerData.Add ("Player_Coin",PlayerController.coin);
				
				//item name ,수량 저장 ------------------------------------------------
				//playerData.Add("Cannon_3Shooter",PlayerController.cannon3);
				//playerData.Add("Cannon_ShooterIce",PlayerController.cannonIce);
				//playerData.Add("Cannon_ShooterTim",PlayerController.cannonTim);
				//playerData.Add("Cannon_ShooterDivide",PlayerController.cannonDivide);
				//playerData.Add ("Player_LevelFort",PlayerLevelData.Instance.BoomFortLevel);
				//playerData.Add ("Player_LevelFortNUM",PlayerLevelData.Instance.FortStageLevel);
				
				//playerData.Add ("Player_checkPointEnabled"	, PlayerController.checkPointEnabled);
				//playerData.Add ("Player_checkPointSceneName", PlayerController.checkPointSceneName);
				//playerData.Add ("Player_checkPointLabelName", PlayerController.checkPointLabelName);
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
					//PlayerController.nowHpMax 			 = (float)playerData.GetData ("Player_HPMax");
					//PlayerController.nowHp 				 = (float)playerData.GetData ("Player_HP");
					//PlayerController.score 				 = (int)playerData.GetData ("Player_Score");
					//PlayerController.coin                = (int)playerData.GetData("Player_Coin");
					//PlayerController.cannon3             = (int)playerData.GetData("Cannon_3Shooter");
					//PlayerController.cannonIce           = (int)playerData.GetData("Cannon_ShooterIce");
					//PlayerController.cannonTim           = (int)playerData.GetData("Cannon_ShooterTim");
					//PlayerController.cannonDivide        = (int)playerData.GetData("Cannon_ShooterDivide");
					//PlayerLevelData.Instance.BoomFortLevel    = (int)playerData.GetData("Player_LevelFort");
					//PlayerLevelData.Instance.FortStageLevel   = (int)playerData.GetData("Player_LevelFortNUM");
					
					//PlayerController.checkPointEnabled 	 = (bool)playerData.GetData ("Player_checkPointEnabled");
					//PlayerController.checkPointSceneName = (string)playerData.GetData ("Player_checkPointSceneName");
					//PlayerController.checkPointLabelName = (string)playerData.GetData ("Player_checkPointLabelName");
					
					
				}
				// StageData
				if (PlayerPrefs.HasKey("StageData_" + currScene.name)) {
					zFoxDataPackString stageData = new zFoxDataPackString();
					stageData.DecodeDataPackString(stageData.PlayerPrefsGetStringUTF8 ("StageData_" + currScene.name));
					//Debug.Log(stageData.PlayerPrefsGetStringUTF8 ("StageData_" + Application.loadedLevelName));
					//zFoxUID[] uidList = GameObject.Find ("Stage").GetComponentsInChildren<zFoxUID> ();
					//foreach(zFoxUID uidItem in uidList)
     //               {
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
	
	// === 코드(최고 득점 데이터·저장, 불러오기) ================
	public static bool SaveHiScore(int playerScore) {
		
		LoadHiScore ();
		
		try {
			Debug.Log("SaveData.SaveHiScore : Start");
			// Hiscore Set & Sort
			newRecord = 0;
			int[] scoreList = new int [11];
			HiScore.CopyTo (scoreList, 0);
			scoreList[10] = playerScore;
			System.Array.Sort(scoreList);
			System.Array.Reverse(scoreList);
			for(int i = 0;i < 10;i ++) {
				HiScore[i] = scoreList[i];
				if (playerScore == HiScore[i]) {
					newRecord = i + 1;
				}
			}
			
			// Hiscore Save
			SaveDataHeader("player_HiScore");
			zFoxDataPackString hiscoreData = new zFoxDataPackString();
			for(int i = 0;i < 10;i ++) {
				hiscoreData.Add ("Rank" + (i + 1), HiScore[i]);
			}
			hiscoreData.PlayerPrefsSetStringUTF8 ("HiScoreData", hiscoreData.EncodeDataPackString ());
			
			PlayerPrefs.Save ();
			Debug.Log("SaveData.SaveHiScore : End");
			return true;
		} catch(System.Exception e) {
			Debug.LogWarning("SaveData.SaveHiScore : Failed (" + e.Message + ")");
		}
		
		return false;
	}
	
	public static bool LoadHiScore() {
		try {
			if (CheckSaveDataHeader("player_HiScore")) {
				Debug.Log("SaveData.LoadHiScore : Start");
				zFoxDataPackString hiscoreData = new zFoxDataPackString();
				hiscoreData.DecodeDataPackString(hiscoreData.PlayerPrefsGetStringUTF8 ("HiScoreData"));
				//Debug.Log(hiscoreData.PlayerPrefsGetStringUTF8 ("HiScoreData"));
				for(int i = 0;i < 10;i ++) {
					HiScore[i] = (int)hiscoreData.GetData ("Rank" + (i + 1));
				}
				Debug.Log("SaveData.LoadHiScore : End");
			}
			return true;
		} catch(System.Exception e) {
			Debug.LogWarning("SaveData.LoadHiScore : Failed (" + e.Message + ")");
		}
		return false;
	}
	
	// === 코드(옵션 데이터·저장, 불러오기) ================
	public static bool SaveOption() {
		try {
			Debug.Log("SaveData.SaveOption : Start");
			// 옵션 디폴트 설정  SaveDataheader --SDG_Option
			SaveDataHeader("SDG_Option");
			
			PlayerPrefs.SetFloat ("SoundBGMVolume", SoundBGMVolume);
			PlayerPrefs.SetFloat ("SoundSEVolume" , SoundSEVolume);
			PlayerPrefs.SetInt 	 ("VRPadEnabled"  , (VRPadEnabled ? 1 : 0));
			
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
				VRPadEnabled   = (PlayerPrefs.GetInt ("VRPadEnabled") > 0) ? true : false;
				
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
			
			#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
			VRPadEnabled 	= true;
			#else
			VRPadEnabled 	= false;
			#endif
			
			HiScoreInitData.CopyTo(HiScore,0);
		}
	}
}

// Windows
// (ComputerName)\HKEY_CURRENT_USR\Software\(CompanyName:DefaultCompany)\(AppName)
// http://docs-jp.unity3d.com/Documentation/ScriptReference/PlayerPrefs.html

