using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SaveDataPlayer : MonoBehaviour {

    const float SaveDataVersion = 0.1f;
    public static string SaveDate = "(non)";

    // Option
    public static float SoundBGMVolume = 1.0f;
    public static float SoundSEVolume = 1.0f;

    static void SaveDataHeader(string dataGroupName)
    {
        //player의 버전으로 reg에 SaveDataVersion | SaveDataVersion
        PlayerPrefs.SetFloat("SaveDataVersion", SaveDataVersion);
        SaveDate = System.DateTime.Now.ToString("G");
        //reg에 SaveDataData | 2018.12.12..
        //json 파일에 저장
        PlayerPrefs.SetString("SaveDataDate", SaveDate);
        PlayerPrefs.SetString(dataGroupName, "on");


        //dataGrounName --- Version,3.0f,2016년 6월,on
    }


}
