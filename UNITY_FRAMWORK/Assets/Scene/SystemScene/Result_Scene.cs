using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Result_Scene : MonoBehaviour {

    public static string nextScene;
    string jumpSceneName;
    FadeOut fadeOut;

   

    void SceneJump()
    {
        Debug.Log(string.Format("Start Game : {0}", jumpSceneName));
        SceneManager.LoadScene(jumpSceneName);
    }

    //계속 반복되는 코드 발생:Main_Scene,Game_Secne .
    //따로 쓰는 이유는 버튼OnClick에서 스크립트명과 오브젝트명이 같아야 되는 문제 때문임
    public void SceneChange()
    {
        SceneManager.LoadScene("2_Main_Scene");
        //nextScene = "2_Main_Scene";
    }

    //순간적으로 빨리 켜지고 꺼지는 효과 
    IEnumerator FADEOUT_FLASH()
    {
        fadeOut.isFadeIN = true;
        fadeOut.StartFadeAnim();
        yield return new WaitForSeconds(0.1f);
    }
}
