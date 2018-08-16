using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTest : MonoBehaviour
{
    public GameObject gEffect;
    public Transform tfSkillPos;

    private float fCooldown = 0.0f;

    private void Update()
    {
        if (fCooldown != 0.0f)
        {
            fCooldown -= Time.deltaTime;

            if (fCooldown < 0.0f)
                fCooldown = 0.0f;
        }
    }

    public void UseSkill()
    {
        if (gEffect && fCooldown == 0.0f)
        {
            fCooldown = 10.0f;

            GameObject gObject = Instantiate(gEffect, tfSkillPos.position, tfSkillPos.rotation);
            Destroy(gObject, 7.0f);
        }
    }
}
