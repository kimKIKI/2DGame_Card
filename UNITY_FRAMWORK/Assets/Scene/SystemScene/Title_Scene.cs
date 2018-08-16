using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title_Scene : MonoBehaviour {
    public static string nextScene;

    [SerializeField]
     Slider slider;

    FadeOut fadeOut;
	void Start ()
    {
        nextScene = "2_Main_Scene";
        fadeOut = GameObject.Find("FadeOut").GetComponent<FadeOut>();
		StartCoroutine ("LogoWork");
        StartCoroutine(LoadScene());
	}

	IEnumerator LogoWork() {

        fadeOut.isFadeIN = true;
        fadeOut.StartFadeAnim();
      
        yield return new WaitForSeconds (3.0f);
        fadeOut.isFadeIN = false;
        fadeOut.StartFadeAnim();
        yield return new WaitForSeconds (1.2f);
		//SceneManager.LoadScene ("2_Main_Scene");
	}

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress >= 0.9f)
            {
                slider.value = Mathf.Lerp(slider.value, 1f,timer);
                if (slider.value >= 0.95f)
                {
                    op.allowSceneActivation = true;
                }
                else
                {
                    slider.value= Mathf.Lerp(slider.value,op.progress,timer);
                    timer = 0f;
                }
            }
        }
    }
}
