using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Scene : MonoBehaviour {

    FadeOut fadeOut;
    void Start()
    {
        // !!! 가비지 컬렉션 강제 실행 !!!
        //System.GC.Collect();
        // !!!!!!!!!!!!!!!!!!!!!
        fadeOut = GameObject.Find("FadeOut").GetComponent<FadeOut>();
        StartCoroutine(FADEOUT_FLASH());
        GameData.Instance.PanelItem = 3;
    }

     void Awake()
    {
        //Game_Scene.Instance
        //base.Awake();
    }

    public void SceneChange()
    {
        //빌더에 있는 씬네임
        SceneManager.LoadScene("3_Game_Scene");
    }

    //순간적으로 빨리 켜지고 꺼지는 효과 
    IEnumerator FADEOUT_FLASH()
    {
        yield return new WaitForSeconds(3f);
        fadeOut.isFadeIN = true;
        fadeOut.StartFadeAnim();
        yield return new WaitForSeconds(0.1f);
    }

}
