using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestructShuriken : MonoBehaviour
{
	public bool OnlyDeactivate;
    //roof일때 일정시간 이후에 자동삭제 되게 한다.
    public bool RoofClose;
   

    void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}
	
	IEnumerator CheckIfAlive ()
	{
        int LifeCount = 0;
        while (true)
		{
            // 0.3초 간격으로 파티클이 살아 있는지 체크합니다.
         
			yield return new WaitForSeconds(0.3f);
            LifeCount++;
            if (!GetComponent<ParticleSystem>().IsAlive(true))
            {
                if (OnlyDeactivate)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                   
                   GameObject.Destroy(this.gameObject);
                    break;
                }
            }
            else
            {
                if (RoofClose)
                {
                    //GetComponent<ParticleSystem>().IsAlive(true)
                    if (LifeCount >= 4)
                    {
                        GameObject.Destroy(this.gameObject);
                    }
                }

            }
        }
	}

   

    //private void OnDestroy()
    //{
    //    //혹시 다른 이펙트
    //    Debug.Log("autoDes OnDesteoy작동");
    //}
}
