using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    private UIProgressBar uiBar;
    public float maxHp = 100.0f;

    private float currHp;
    private UILabel hpLabel;

    private float healAmount;       // 힐량
    private float damageAmount;     // 데미지 량
    private bool isHealing = false;
    private bool isDamaging = false;

    private void Awake()
    {
        uiBar = this.GetComponent<UIProgressBar>();
        uiBar.foregroundWidget.color = Color.green; // 프로그레스 바의 색상 값 변경
        currHp = maxHp;

        hpLabel = this.GetComponentInChildren<UILabel>();
        hpLabel.text = "현재 체력 : " + currHp.ToString();
        hpLabel.color = Color.white;
    }

    private void Update()
    {
        // 체력 범위 제한
        currHp = (currHp >= maxHp) ? maxHp : currHp;
        currHp = (currHp <= 0.0f) ? 0.0f : currHp;

        ColorChange();
        uiBar.value = currHp / maxHp;
        hpLabel.text = "현재 체력 : " + ((int)currHp).ToString();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isHealing = true;
            healAmount = 25.0f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isDamaging = true;
            damageAmount += 10.0f;
        }

        if (isHealing) GetHeal();      // 힐 적용
        if (isDamaging) GetDamage();    // 데미지 적용
    }

    private void GetHeal()
    {
        float healHp = Time.deltaTime * 10.0f; // 한 프레임 당 회복량 (초당 10회복)

        // 현재 남은 회복량이 한프레임 회복량 보다 작은 경우
        if (healAmount < healHp)
        {
            currHp += healAmount;   // 남은 회복량 만큼만 회복
            isHealing = false;
        }
        else
            currHp += healHp;       // 한프레임 당 회복량 만큼 회복


        healAmount -= healHp;
    }

    private void GetDamage()
    {
        float damage = Time.deltaTime * 20.0f; // 한 프레임 당 데미지량 (초당 20)

        if (damageAmount < damage)
        {
            currHp -= damageAmount;
            isDamaging = false;
        }
        else
            currHp -= damage;


        damageAmount -= damage;
    }

    private void ColorChange()
    {
        Color color = Color.Lerp(Color.red, Color.green, currHp / maxHp);
        uiBar.foregroundWidget.color = color;
    }
}
