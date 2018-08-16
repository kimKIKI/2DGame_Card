using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AppSound : MonoBehaviour {
	
	// === 외부 파라미터 ======================================
	public static AppSound instance = null;
    Scene currSceneName;
	// 배경음
	[System.NonSerialized] public zFoxSoundManager fm;

	[System.NonSerialized] public AudioSource BGM_LOGO;
    [System.NonSerialized] public AudioSource BGM_MAIN;
    [System.NonSerialized] public AudioSource BGM_TITLE;
	[System.NonSerialized] public AudioSource BGM_HISCORE;
	[System.NonSerialized] public AudioSource BGM_STAGEA;
	[System.NonSerialized] public AudioSource BGM_STAGEB;
	[System.NonSerialized] public AudioSource BGM_BOSSA;
	[System.NonSerialized] public AudioSource BGM_ENDING;
	
	// 효과음
	[System.NonSerialized] public AudioSource SE_MENU_OK;
	[System.NonSerialized] public AudioSource SE_MENU_CANCEL;
    [System.NonSerialized] public AudioSource SE_MENU_OPEN;
    [System.NonSerialized] public AudioSource SE_MENU_ITEMBOXOPEN;
    [System.NonSerialized] public AudioSource SE_MENU_ITEMCOUNTGOLD;


    [System.NonSerialized] public AudioSource SE_ATK_A1;
	[System.NonSerialized] public AudioSource SE_HIT_A1;
	
	[System.NonSerialized] public AudioSource SE_MOV_JUMP;
	[System.NonSerialized] public AudioSource SE_ITEM_KEY;
	[System.NonSerialized] public AudioSource SE_ITEM_COIN;
	
	[System.NonSerialized] public AudioSource SE_OBJ_EXIT;
	[System.NonSerialized] public AudioSource SE_OBJ_OPENDOOR;
	[System.NonSerialized] public AudioSource SE_OBJ_SWITCH;
	
	
	[System.NonSerialized] public AudioSource SE_CHECKPOINT;
	[System.NonSerialized] public AudioSource SE_EXPLOSION;
	
	
	
	// === 내부 파라미터 ======================================
	string sceneName = "non";
	
	// === 코드 =============================================
	void Awake () {
		// 사운드
		fm = GameObject.Find("zFoxSoundManager").GetComponent<zFoxSoundManager>();
		
		// 배경음
		fm.CreateGroup("BGM");
		fm.SoundFolder = "Sounds/BGM/";
		BGM_LOGO 				= fm.LoadResourcesSound("BGM","LOGO");
        BGM_MAIN               = fm.LoadResourcesSound("BGM", "MAIN");
        BGM_TITLE 				= fm.LoadResourcesSound("BGM","Title");
		BGM_HISCORE 			= fm.LoadResourcesSound("BGM","HiScore");
		
		BGM_STAGEA 				= fm.LoadResourcesSound("BGM","PLAY");
		BGM_STAGEB 				= fm.LoadResourcesSound("BGM","StageB");
		
		BGM_BOSSA 				= fm.LoadResourcesSound("BGM","BossA");
		
		BGM_ENDING				= fm.LoadResourcesSound("BGM","Ending");
		
		
		// 효과음
		fm.CreateGroup("SE");
		fm.SoundFolder = "Sounds/SE/";
		SE_MENU_OK 				= fm.LoadResourcesSound("SE","SE_Menu_Ok");
		SE_MENU_CANCEL  		= fm.LoadResourcesSound("SE","SE_Menu_Cancel");
        SE_MENU_OPEN            = fm.LoadResourcesSound("SE", "SE_OpenCard");
        SE_MENU_ITEMBOXOPEN     = fm.LoadResourcesSound("SE","SE_ItemBoxOpen");
        SE_MENU_ITEMCOUNTGOLD   = fm.LoadResourcesSound("SE", "SE_ItemCountGold");


        SE_ATK_A1  				= fm.LoadResourcesSound("SE","SE_ATK_A1");
		SE_HIT_A1	  			= fm.LoadResourcesSound("SE","SE_HIT_A1");
		
		#if xxx
		SE_HIT_B1	  			= fm.LoadResourcesSound("SE","SE_HIT_B1");
		SE_HIT_B2	  			= fm.LoadResourcesSound("SE","SE_HIT_B2");
		SE_HIT_B3	  			= fm.LoadResourcesSound("SE","SE_HIT_B3");
		#endif

		//SE_HIT_B1 = SE_HIT_A1;
		//SE_HIT_B2 = SE_HIT_A2;
		//SE_HIT_B3 = SE_HIT_A3;
		
		SE_MOV_JUMP  			= fm.LoadResourcesSound("SE","SE_MOV_Jump");
		
		
		SE_ITEM_KEY				= fm.LoadResourcesSound("SE","SE_Item_Key");
		SE_ITEM_COIN            = fm.LoadResourcesSound("SE","SE_ITEM_COIN");
		
		SE_OBJ_EXIT				= fm.LoadResourcesSound("SE","SE_OBJ_Exit");
		SE_OBJ_OPENDOOR			= fm.LoadResourcesSound("SE","SE_OBJ_OpenDoor");
		SE_OBJ_SWITCH			= fm.LoadResourcesSound("SE","SE_OBJ_Switch");
		
		
		SE_CHECKPOINT			= fm.LoadResourcesSound("SE","SE_CheckPoint");
		SE_EXPLOSION			= fm.LoadResourcesSound("SE","SE_Explosion");
		
		instance = this;
	}
	
	void Update() {
        // 씬이 바뀌었는지 검사
        currSceneName =SceneManager.GetActiveScene();

        if (sceneName != currSceneName.name) {
			sceneName = currSceneName.name;
			
			//음량 설정
			//fm.SetVolume("BGM",SaveData.SoundBGMVolume);
			//fm.SetVolume("SE",SaveData.SoundSEVolume);
			
			// BGM 재생
			if (sceneName == "0_Title_Scene") {
				BGM_LOGO.Play();
              

			} else
			if (sceneName == "2_Main_Scene") {
				if (!BGM_MAIN.isPlaying) {
					fm.Stop ("BGM");
                    //BGM_MAIN.Play();
					fm.FadeInVolume(BGM_MAIN, 1.0f,1.0f,true);
				}
			} else
				if (sceneName == "Menu_Option"  ||
				    sceneName == "Menu_HiScore" ||
				   sceneName == "Menu_Option") {
			} else
			if (sceneName == "3_Game_Scene") {
				fm.Stop ("BGM");
				fm.FadeOutVolumeGroup("BGM",BGM_STAGEA,0.0f,1.0f,false);
				fm.FadeInVolume(BGM_TITLE,1.0f,1.0f,true);
				BGM_STAGEA.loop = true;
				BGM_STAGEA.Play();
			} else
			if (sceneName == "DungeonA") {
				fm.Stop ("BGM");
				//fm.FadeOutVolumeGroup("BGM",BGM_DungeonA,0.0f,1.0f,false);
				fm.FadeInVolume(BGM_TITLE,1.0f,1.0f,true);
				//BGM_DungeonA.loop = true;
				//BGM_DungeonA.Play();
			}
			
			
			
			else
			if (sceneName == "StageB_Room") {
				fm.Stop ("BGM");
				//BGM_STAGEB_ROOMSAKURA.loop = true;
				//BGM_STAGEB_ROOMSAKURA.Play();
			} else
				if (sceneName == "StageB_Room_A" ||
				    sceneName == "StageB_Room_B" ||
				   sceneName == "StageB_Room_C") {
				fm.Stop ("BGM");
				BGM_BOSSA.loop = true;
				BGM_BOSSA.Play();
			} else
			if (sceneName == "StageB_Boss") {
				fm.Stop ("BGM");
				//BGM_BOSSB.loop = true;
				//BGM_BOSSB.Play();
			} else
			if (sceneName == "StageZ_Ending") {
				fm.Stop ("BGM");
				BGM_ENDING.Play();
			} else {
				if (!BGM_STAGEB.isPlaying) {
					fm.Stop ("BGM");
					BGM_STAGEB.loop = true;
					BGM_STAGEB.Play();
				}
			}
		}
	}
}
