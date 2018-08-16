using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeOut : MonoBehaviour {

    public float animTime = 2f;
    Image fadeImage;

    float start; 
    float end ;    
    float time ;
    //체크시 페이드 인이 작동하게 하기 위해서 
    public  bool isFadeIN = false;
    bool isPlaying = false;

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
    }

    public void StartFadeAnim()
    {
        if (isPlaying == true)
        {
            return;
        }

        if (!isFadeIN)
        {
            StartCoroutine("PlayFadeOut");
        }
        else
        {
            StartCoroutine(PlayFadeIn());
        }
    }

    IEnumerator PlayFadeOut()
    {

        start = 0f; // fade IN 1
        end = 1f;    //fade IN 0
        time = 0f;

        isPlaying = true;

        Color color = fadeImage.color;
        time = 0f;
        color.a = Mathf.Lerp(start, end,time);
        while (color.a < 1f)
        {
            time += Time.deltaTime / animTime;
            color.a = Mathf.Lerp(start,end, time);
            fadeImage.color = color;
            yield return null;
        }
        isPlaying = false;
    }

    IEnumerator PlayFadeIn()
    {

        start = 1f; 
        end = 0f;    
        time = 0f;

        isPlaying = true;

        Color color = fadeImage.color;
        time = 0f;
        color.a = Mathf.Lerp(start, end, time);
        while (color.a > 0f)
        {
            time += Time.deltaTime / animTime;
            color.a = Mathf.Lerp(start,end,time);
            fadeImage.color = color;
            yield return null;
        }
        isPlaying = false;
    }
}
