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
    [System.NonSerialized] public AudioSource SE_DESTROY;
    [System.NonSerialized] public AudioSource SE_OpenCard2;
    [System.NonSerialized] public AudioSource SE_CardOpenWidth;
    [System.NonSerialized] public AudioSource SE_Card_PickUp;
    [System.NonSerialized] public AudioSource SE_Card_Down;
    [System.NonSerialized] public AudioSource SE_START;
    [System.NonSerialized] public AudioSource SE_CARD_MOVE;
    [System.NonSerialized] public AudioSource SE_START_SPR;
    [System.NonSerialized] public AudioSource SE_CARD_COM;      //com의 가위바위보세팅
    [System.NonSerialized] public AudioSource SE_COMBAT_LAZER2; //레이저 발사
    [System.NonSerialized] public AudioSource SE_COMBAT_START;  //전투씬시작
    [System.NonSerialized] public AudioSource SE_COMBAT_DEFECT;  //전투패배음
    [System.NonSerialized] public AudioSource SE_COMBAT_IMPACT;  //전투카드 충돌음
    [System.NonSerialized] public AudioSource SE_COMBAT_OUT;     //전투카드 출현음
    [System.NonSerialized] public AudioSource SE_SLOT_CHOICE;     //슬롯 선택음
    [System.NonSerialized] public AudioSource SE_CARD_APPERMOVE;  //카드가 첫등장이동한다.
    [System.NonSerialized] public AudioSource SE_RESULT_START;    //승패후 상품결과창시작음
    [System.NonSerialized] public AudioSource SE_CARD_PUMP;       //패배한 카드 출현
    [System.NonSerialized] public AudioSource SE_CARD_DEFECTSHOOT;       //패배한 카드 출현
    [System.NonSerialized] public AudioSource SE_MENU_OPEN_D;     //상품카드오픈시 
    [System.NonSerialized] public AudioSource SE_CARD_CHOICE;     //카드 선택에서 선택시 음
    [System.NonSerialized] public AudioSource SE_CARD_BTN;       //카드 선택에서 업데이트및 사용음
    [System.NonSerialized] public AudioSource SE_CASE_CLOSE;     // 선택창을 닫을때음
    [System.NonSerialized] public AudioSource SE_SHIP_HIT;     // 선택창을 닫을때음





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
		//BGM_HISCORE 			= fm.LoadResourcesSound("BGM","HiScore");
		
		BGM_STAGEA 				= fm.LoadResourcesSound("BGM","PLAY");
		//BGM_STAGEB 				= fm.LoadResourcesSound("BGM","StageB");
		
		//BGM_BOSSA 				= fm.LoadResourcesSound("BGM","BossA");
		
		BGM_ENDING				= fm.LoadResourcesSound("BGM","Ending");
		
		
		// 효과음
		fm.CreateGroup("SE");
		fm.SoundFolder = "Sounds/SE/";
		SE_MENU_OK 				= fm.LoadResourcesSound("SE","SE_Menu_Ok");
		SE_MENU_CANCEL  		= fm.LoadResourcesSound("SE","SE_Menu_Cancel");
        SE_MENU_OPEN            = fm.LoadResourcesSound("SE","SE_OpenCard");
        SE_MENU_OPEN_D          = fm.LoadResourcesSound("SE", "SE_OpenCardDaily");
        SE_CARD_CHOICE         = fm.LoadResourcesSound("SE", "SE_CARD_CHOICE");
        //SE_OpenCardDaily
        SE_MENU_ITEMBOXOPEN = fm.LoadResourcesSound("SE","SE_ItemBoxOpen");
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
        SE_DESTROY              = fm.LoadResourcesSound("SE","SE_Explosion");
        SE_OpenCard2            = fm.LoadResourcesSound("SE","SE_OpenCard2");
        SE_CardOpenWidth        = fm.LoadResourcesSound("SE","SE_CardOpenWidth");
        SE_Card_PickUp          = fm.LoadResourcesSound("SE","SE_Card_PickUp");
        SE_Card_Down            = fm.LoadResourcesSound("SE", "SE_Card_Down");
        SE_START                = fm.LoadResourcesSound("SE", "SE_Start");
        SE_START_SPR            = fm.LoadResourcesSound("SE", "SE_Start_SPR");
        SE_CARD_MOVE            = fm.LoadResourcesSound("SE", "SE_CARD_MOVE");
        SE_CARD_COM             = fm.LoadResourcesSound("SE", "SE_CARD_COM");
        SE_COMBAT_LAZER2        = fm.LoadResourcesSound("SE", "SE_COMBAT_LAZER2");
        SE_COMBAT_START         = fm.LoadResourcesSound("SE", "SE_COMBAT_START");
        SE_COMBAT_OUT           = fm.LoadResourcesSound("SE", "SE_COMBAT_OUT");
        SE_COMBAT_DEFECT        = fm.LoadResourcesSound("SE", "SE_COMBAT_DEFECT");
        SE_COMBAT_IMPACT        = fm.LoadResourcesSound("SE", "SE_COMBAT_IMPACT");
        SE_SLOT_CHOICE          = fm.LoadResourcesSound("SE", "SE_SLOT_CHOICE");
        SE_CARD_APPERMOVE       = fm.LoadResourcesSound("SE", "SE_CARD_APPERMOVE");
        SE_RESULT_START         = fm.LoadResourcesSound("SE", "Angry_Bird");
        SE_CARD_PUMP            = fm.LoadResourcesSound("SE", "SE_CARD_PUMP");
        SE_CARD_DEFECTSHOOT     = fm.LoadResourcesSound("SE", "SE_CARD_DEFECTSHOOT");
        SE_CARD_BTN             = fm.LoadResourcesSound("SE", "SE_CARD_BTN");
        SE_CASE_CLOSE           = fm.LoadResourcesSound("SE", "SE_CASE_CLOSE");
         SE_SHIP_HIT            = fm.LoadResourcesSound("SE", "SE_SHIP_HIT"); ;     

        instance = this;
	}
	
	void Update() {
        // 씬이 바뀌었는지 검사
        currSceneName =SceneManager.GetActiveScene();

        if (sceneName != currSceneName.name) {
			sceneName = currSceneName.name;
			
			//음량 설정
			fm.SetVolume("BGM",SaveData.SoundBGMVolume);
			fm.SetVolume("SE",SaveData.SoundSEVolume);

            // BGM 재생
            if (sceneName == "0_Title_Scene")
            {
                BGM_LOGO.Play();
            }
            else
            if (sceneName == "2_Main_Scene")
            {
                //if (!BGM_MAIN.isPlaying)
                //{
                //    fm.Stop("BGM");
                //    BGM_MAIN.Play();
                //    fm.FadeInVolume(BGM_MAIN, 1.0f, 1.0f, true);
                //}
                BGM_MAIN.Play();
                fm.FadeInVolume(BGM_MAIN, .3f, 0.2f, true);
            }
            else
            if (sceneName == "3_Game_Scene")
            {
                fm.Stop("BGM");
                fm.FadeOutVolumeGroup("BGM", BGM_STAGEA, 0.0f, 1.0f, false);
                fm.FadeInVolume(BGM_TITLE, 1.0f, 1.0f, true);
                BGM_STAGEA.loop = true;
                BGM_STAGEA.Play();
            }
           // else
           // if (sceneName == "DungeonA")
           // {
           //     fm.Stop("BGM");
           //     //fm.FadeOutVolumeGroup("BGM",BGM_DungeonA,0.0f,1.0f,false);
           //     fm.FadeInVolume(BGM_TITLE, 1.0f, 1.0f, true);
           //     //BGM_DungeonA.loop = true;
           //     //BGM_DungeonA.Play();
           // }
           
           //else
           //if (sceneName == "StageB_Room_A" ||
           //         sceneName == "StageB_Room_B" ||
           //        sceneName == "StageB_Room_C")
           //     {
           //     fm.Stop("BGM");
           //     BGM_BOSSA.loop = true;
           //     BGM_BOSSA.Play();
           // }
            else
            {
                //if (!BGM_STAGEB.isPlaying)
                //{
                //    fm.Stop("BGM");
                //    BGM_STAGEB.loop = true;
                //    BGM_STAGEB.Play();
                //}
            }
		}
	}
}
