using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//이펙트 오브젝트와 이름을 연결시켜서  외부에서 인스턴스에 쉽게 접근할수 있게 한다.
// 여기서 네임과 1 : 1 로 대입한다.
//Eff의 오브젝트 네임을 쉽게 전달하기 위해서 enum 처리
public enum eEFFECT_NAME
{
    NONE,
    GOLD,         //GOLD 
    JEW,
    EXPERIENCE,   //경험치
    Destroy_Card, //카드가 사라질때 나오는 이펙트
    BULLET,       //bullet
}

public class AppUIEffect : MonoBehaviour {

    //다른 클래스에서 이곳의 오브젝트에 델리게이트 방식으로 접근할수 있도록한다.
    //모든 이펙트를 리스트에 담고 찾아서 생성해 주는 역할을 한다.
    //코드만 가지고있고 외부에서 호출을 한다.

    //하이라키에서 드레그 해주어야 한다.
        public GameObject[] effs;
                   string[] curEffName;

    //외부에서 이펙트에 접근할수 인스턴스 생성
    public static AppUIEffect instance = null;

    private void Awake()
        {
            //effs = transform.GetComponents<GameObject>();
            curEffName = new string[effs.Length];
            instance = this;
        }


        void Start()
        {
            //핸들러타이밍에 자신의 메소드를  등록한다.
            //CardEff_Result.onUnitySpawn += this.NewEffCreated;
            //CardEff_Result.onUnitySpawn += this.EffCreateDestroy;
            //패배한 카드가 사라질때 생성될 이펙트 
          

       }

        void OnDisable()
        {
           // CardEff_Result.onUnitySpawn -= this.NewEffCreated;
           // CardEff_Result.onUnitySpawn -= this.EffCreateLong;
           // CardEff_Result.onUnitySpawn -= this.EffCreateDestroy;
        }

          void EffCreateLong()
         {
            EffCreated("MagicSparkles");
         }

        void EffCreateDestroy()
         {
            EffCreated("CuteDeath");
         }

         void EffCreated(string name)
         {
                    for (int i = 0; i < effs.Length; i++)
                    {
                        if (effs[i].name == name)
                        {
                           effs[i].SetActive(true);
                           break;
                        }
                    }
          }

    //복제 
    GameObject CreatetEffObj(string name)
    {
        GameObject effObj = null;
        for (int i = 0; i < effs.Length; i++)
        {
            if (effs[i].name == name)
            {
                effObj = Instantiate(effs[i]);
                effObj.SetActive(true);
                //복제되는 이펙트는 활성화후 파괴된다.
                effObj.GetComponent<AutoDestructShuriken>().OnlyDeactivate = false;
                break;
            }
        }
        return effObj;
    }

       //네입별로 찾아서 인스턴스를 생성하게한다.
       //복제한다.
      public GameObject InstanceVFX(eEFFECT_NAME vfx)
        {
        GameObject vfxIntance = null;
            switch (vfx)
            {
            case eEFFECT_NAME.GOLD:

                vfxIntance = CreatetEffObj("NovaGOLD");
               
                break;
            case eEFFECT_NAME.JEW:

                vfxIntance = CreatetEffObj("NovaGOLD");
                break;

            case eEFFECT_NAME.EXPERIENCE:
                vfxIntance = CreatetEffObj("NovaGOLD");
                break;

            case eEFFECT_NAME.Destroy_Card:
                vfxIntance = CreatetEffObj("CuteDeath");
                break;
            case eEFFECT_NAME.BULLET:
                vfxIntance = CreatetEffObj("BulletSmallFire");
                break;
        }

        return vfxIntance;
       }



    //네입별로 발사할 부분의 이펙트
    public GameObject InstanceFIRE(eEFFECT_NAME vfx)
    {
        GameObject vfxIntance = null;
        switch (vfx)
        {
            case eEFFECT_NAME.GOLD:

                vfxIntance = CreatetEffObj("CartoonyFightAction2");

                break;
            case eEFFECT_NAME.JEW:

                vfxIntance = CreatetEffObj("CartoonyFightAction2");
                break;

            case eEFFECT_NAME.EXPERIENCE:
                vfxIntance = CreatetEffObj("CartoonyFightAction2");
                break;

            case eEFFECT_NAME.Destroy_Card:
                vfxIntance = CreatetEffObj("CartoonyFightAction2");
                break;
            case eEFFECT_NAME.BULLET:
                vfxIntance = CreatetEffObj("CartoonyFightAction2");
                break;
        }

        return vfxIntance;
    }










    //GameData에서 설정된 enum 을 받아서  현재 가지고있는 것을 활성화 시킨다.
    //활성화
    //public void NewEffCreated()
    //{
    //    eEFFECT_NAME effName = GameData.Instance.gameEffect;
    //    switch (effName)
    //    {
    //        case eEFFECT_NAME.GOLD:

    //            curEffName[0] = "NovaGOLD";
    //            EffCreated(curEffName[0]);

    //            break;
    //        case eEFFECT_NAME.JEW:
    //            curEffName[0] = "NovaGOLD";
    //            EffCreated(curEffName[0]);

    //            break;
    //        case eEFFECT_NAME.EXPERIENCE:
    //            curEffName[0] = "NovaGOLD";
    //            EffCreated(curEffName[0]);

    //            break;
    //    }

    //}

}

