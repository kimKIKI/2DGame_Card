using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnim : MonoBehaviour {


    Animator anime;
    Image img;
    public static eAttackType eAtk;

    //eEff_state eState = eEff_state.NON;

    private void Awake()
    {
        anime = GetComponent<Animator>();
        img   = GetComponent<Image>();
    }

    //스프라이트 이미지를 켜고 끌수 있게 한다.
    public void TurenOFf()
    {
        switch (eAtk)
        {
            case eAttackType.NON:

                break;
            case eAttackType.LAZER:

                anime.SetBool("Lazer_blue", false);
                break;

            case eAttackType.GROUND:
                anime.SetBool("Lazer_red", false);
                break;

              
        }
      
        img.enabled = false;
    }

    public void TurnOffDestroy()
    {
        switch (eAtk)
        {
            case eAttackType.NON:

                break;
            case eAttackType.LAZER:
                    anime.SetBool("Dest_blue", false);

                break;
            case eAttackType.GROUND:
                    anime.SetBool("Dest_red", false);
                break;
        }
        img.enabled = false;
    }

}
