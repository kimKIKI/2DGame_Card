using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinueTEST : MonoBehaviour {

    public GameObject poi;       //관심 지점
    public GameObject startObj;  //
    public float u = 0.3f;
    public Vector3 p0, p1, p01;


    private void Start()
    {
        StartCoroutine(Deactivation());
    }
    void Update()
    {
        //p0 = this.transform.position;
        //p1 = poi.transform.position;  //최종 가야 할곳 

        ////보간
        //p01 = (1 - u) * p0 + u * p1;      //u가 0이면 p1,u가 1이면 p0이 된다.
        //this.transform.position = p01;
    }


    float   currentSpeed = 0.8f;
    float   acceleration = 0.2f;


    private IEnumerator Deactivation()
    {
        // 현재 속도를 시작점으로 초기화 한다.
        float start = currentSpeed;
        // 0까지 선형보간 해주어야 하므로 끝점은 0이다.
        float end = 0f;
        // 보간에 사용할 t를 0으로 초기화 해준다.
        float t = 0f;

        while (t <= 1f)
        {
            // t 를 증가시켜주고
            t += acceleration * Time.deltaTime;
            // 현재 속도를 0까지 선형보간해준다.
            currentSpeed = Mathf.Lerp(start, end, t);
            // 캐릭터를 이동시켜준다.
            startObj.transform.position += startObj.transform.position * currentSpeed * Time.deltaTime;
            yield return null;
        }
    }
    }
