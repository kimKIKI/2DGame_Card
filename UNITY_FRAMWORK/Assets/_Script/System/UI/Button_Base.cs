using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Button_Base : MonoBehaviour {

    Vector2 endScale;
    Vector2 OrigScale;
    RectTransform btn;
    float ScaleX;
    float ScaleY;
    Vector2 pos;
    public float DelayTime;

    private void Awake()
    {
        btn = gameObject.GetComponent<RectTransform>();
        OrigScale = new Vector2(btn.localScale.x, btn.localScale.y);
        pos = gameObject.transform.localPosition;
        Small();
       
    }

    private void Start()
    {
        StartCoroutine(coButtonEFF(DelayTime));
    }

    IEnumerator coButtonEFF(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnClick();
    }
    private void Small()
    {
        //Small -->Large 축소된 사이즈를 저장한다
        ScaleY = OrigScale.y/4f;
    }

    public void OnClick()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from",ScaleY, "to", OrigScale.y, "easetype", iTween.EaseType.easeOutBack, "onupdate", "ScaleButton", "time", .7));
        //움직임
        //iTween.MoveBy(gameObject, iTween.Hash("y", 90.0f, "time", 2.0f, "easetype", iTween.EaseType.easeInExpo));

        AppSound.instance.SE_MENU_OPEN.Play();
    }

  
    void ScaleButton(float size)
    {
        btn.localScale = new Vector3(OrigScale.x, size);
    }
}
