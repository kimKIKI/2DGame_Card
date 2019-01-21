using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeOut : MonoBehaviour {

    public float animTime = 2f;
    public float delayStopTime = 3f;
    Image fadeImage;
   
    float start; 
    float end ;    
    float time ;
    //체크시 페이드 인이 작동하게 하기 위해서 
    public  bool isFadeIN    = false;
    public  bool isPlaying   = false;
    public  bool isDelaytime = false; //faseOut전에 대기 시간이 있어야 할경우 

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
       StartFadeAnim();
    }

   

    public void StartFadeAnim()
    {
        if (isPlaying == true)
        {
            return;
        }

        if (isFadeIN)
        {
            StartCoroutine(PlayFadeIn());
        }
        else if (isDelaytime)
        {
            StartCoroutine(DelayFadeOut(delayStopTime));
        }
        else 
        {
            StartCoroutine(PlayFadeOut());
        }
    }

    //서서히 사라지는 것
    IEnumerator PlayFadeOut()
    {

        start = 0f;    // fade IN 1
        end   = 1f;    //fade IN 0
        time  = 0f;

        isPlaying = true;

        Color color = fadeImage.color;
        time = 0f;
        color.a = Mathf.Lerp(start, end,time);
        while (color.a < 1f)
        {
            time += Time.deltaTime / animTime;
            color.a = Mathf.Lerp(start,end, time);
            fadeImage.color = color;
            if (color.a >= 1)
            {
                Destroy(gameObject);

            }
            yield return null;
            
        }
        isPlaying = false;
    }

    IEnumerator PlayFadeIn()
    {
        start = 1f; 
        end   = 0f;    
        time  = 0f;
        isPlaying = true;

        Color color = fadeImage.color;
        time = 0f;
        color.a = Mathf.Lerp(start, end, time);
        while (color.a > 0f)
        {
            time += Time.deltaTime / animTime;
            color.a = Mathf.Lerp(start,end,time);
            fadeImage.color = color;

            if(color.a <= 0)
            {
                Destroy(gameObject);
            }
            yield return null;
        }
        isPlaying = false;
    }

    //일정 딜레이 시간이후에 천천히 컴은 암texture가 없어지게 한다.
    //캔버스가 자유롭게 된다.
    IEnumerator DelayFadeOut(float delay)
    {
        start = 1f; // fade IN 1
        end = 0f;    //fade IN 0
        time = 0f;

       // isPlaying = true;

        Color color = fadeImage.color;
        time = 0f;
        color.a = Mathf.Lerp(start, end, time);
        yield return new WaitForSeconds(delay);

        while (color.a > 0f)
        {
            time += Time.deltaTime / animTime;
            color.a = Mathf.Lerp(start, end, time);
            fadeImage.color = color;
            if (color.a <= 0)
            {
                Destroy(gameObject);

            }
            yield return null;
        }
        isPlaying = false;
        
    }
}
