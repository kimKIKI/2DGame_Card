using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillMode : MonoBehaviour
{
    private UISprite uiSp;

    private void Awake()
    {
        uiSp = this.GetComponent<UISprite>();
        uiSp.type = UIBasicSprite.Type.Filled;  // 스프라이트 타입을 필모드로 바꾼다.

        uiSp.fillAmount = 1.0f; // 시작 값
    }

    private void Update()
    {
        // 10초의 쿨다운을 가지도록 한다.
        if (uiSp.fillAmount < 1.0f) 
            uiSp.fillAmount += Time.deltaTime / 10.0f;
    }

    public void UseSkill()
    {
        // 쿨다운이 아직 안끝난 경우
        if (uiSp.fillAmount < 1.0f) return;

        uiSp.fillAmount = 0.0f; // 스킬이 시전 된 경우
    }
}
