using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    //무기 미사일 발사 종류
    public enum BulletType
    {
        nono,
        Direct,       //Vector3 up 방향 직선
        Wave,         //Vector3 up 방향 웨이브 
        Line,         // 보간 사용  target
        Bizer,        //Bizer 사영 target 
        DirectTarget, //target 직선
        End,
    }

    // player ,와  Enemy를 선택할수 있게 타입을 선택
    public enum targetType
    {
        non,
        Enemy,
        Player,
        end,
    }

    public BulletType StateBullet = BulletType.nono;

    //bullet를 재사용을 하기 위해서 일단 enum 처리 나중에 스크립팅 처리를 요망
    public GameObject target;          //계속해서 위치 추적을 하는값
          
    public targetType StateTarget;

    float minRange;                    //앵글의 최소값
    float maxRange;                    //앵글의 최대값
    public float angleSpeed = 15.0f;   //회전하는데 걸리는 시간값 작을수록 큰호를 그림
    public float speed = 5f;           //프레임당 이동스피트

    float angle;                       //쿼터니언 도 ..Radian임
   

    Vector3 v_start0;                   //처음 한번만 저장되어야할 위치 
    Vector3 trTarget;                   //처음 한번만 저장되어야할 위치 
    Vector2 tempV2;                     //일직선으로 날아가게 하기 위해서 임시로 사용

    public targetType StateTARGET
    {
        get
        {
            return StateTarget;
        }

        set
        {
            StateTarget = value;
        }
    }

    private void Start()
    {
        
        angle = transform.rotation.z;
       // v_start0 = transform.position;
     
       
        target = new GameObject();
       
        bulletSetFirst(); //회전탄

    }

    private void OnEnable()
    {
       //자신의 위치 설정
        v_start0 = transform.position;
        target = GameObject.FindGameObjectWithTag("Player");
        trTarget = target.transform.position; //타겟의 위치 설정


    }

    private void Update()
    {


        switch (StateBullet)
        {
            case BulletType.nono:

                break;
            case BulletType.Direct:
                bulletDirectionUpdate2();
           

                break;
            case BulletType.Wave:
                bulletDirectionUpdate();
             

                break;
            case BulletType.Line:
              
               
               bulletLine(target.transform.position);
             
             
                break;

            case BulletType.Bizer:
                BulletBizer(target.transform.position);

                break;

            case BulletType.DirectTarget:

                BulletTarget(target.transform.position,false);
                break;
            case BulletType.End:

                break;
        }

        switch (StateTarget)
        {
            case targetType.Enemy:
                target = GameObject.FindGameObjectWithTag("Enemy").gameObject;
                break;

            case targetType.Player:
                target = GameObject.FindGameObjectWithTag("Player").gameObject;
                break;
        }

        //발사 당시의 부모의 로테이트를 기준으로 좌우 움직이게하기 우해
        //  Invoke("ReLoadObject", 3);

    }


    void bulletSetFirst()
    {
        angle    = this.transform.eulerAngles.z;
        minRange = this.transform.eulerAngles.z - 45.0f;
        maxRange = this.transform.eulerAngles.z + 45.0f;

    }

    //커브돌며 날아가는 탄
    void bulletDirectionUpdate()
    {
       
        angle += angleSpeed;
        //z축기준 좌우로 45,-45에 오면 방향을 반대로 주기 위해서 -1
      
        if (angle >= maxRange || angle <= minRange)
        {
            angleSpeed *= -1;
        }

      
        Vector3 pos = this.transform.position;

        //도함수를 라디안 표기법으로 적용따라서 sin,cos이 달라짐
        pos.y += Mathf.Cos(Mathf.PI / 180 * angle) * speed * Time.deltaTime;
        pos.x += -Mathf.Sin(Mathf.PI / 180 * angle) * speed * Time.deltaTime;
        //삼각함수를 미분할때  sin도함수를 cos세타
        //cos세타를  -sin세타로 표현할수 있다.
        this.transform.position = new Vector3(pos.x, pos.y, 0);
        //오브젝트의 좌우 로테이션 적용 날아가는 진행방향으로 기울게하기 
      
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    //직선으로 날아가는 방향탄
    //출발좌표와  세타를 알때
    void bulletDirectionUpdate2()
    {
        Vector3 pos = transform.position;
        float angSata = transform.localEulerAngles.z;
           if(angSata > 180)
        {
            angSata -= 360;
        }
        pos.y += Mathf.Cos(angSata * Mathf.PI / 180) * speed * Time.deltaTime;
        pos.x += -Mathf.Sin(angSata * Mathf.PI / 180) * speed * Time.deltaTime;
        this.transform.position = new Vector3(pos.x,pos.y, 0);
    }

    //보간 적용 미사일테스트 toPosition 은 적 타겟임
    void bulletLine(Vector3 toPosition)
    {
        Vector3 pos = transform.position;

        //TODO: t값을 점점빠르게 점점느리게 빨랐다 느려졌다 컨트롤 할수 있게 할것 
        float t = 0f; 
        t += Time.deltaTime / 1.0f;  //분모가 커질수록 점점 느려짐
       
        Vector3 dir = toPosition - transform.position;
        Vector3 Position = Vector3.zero;
        Utils_Proto.LinearInterpolation(out Position.x,out Position.y,transform.position.x,transform.position.y,toPosition.x,toPosition.y,t);
      
        //z값 특정변경시 주의 필요함
        transform.position = new Vector3(Position.x,Position.y,Position.z);
        Quaternion rotation = Quaternion.LookRotation(dir,Vector3.up);
        transform.rotation = rotation;

    }

    //Bizer적용 움직임테스트
    void BulletBizer(Vector3 target)
    {
        Vector3 pos = transform.position;
        //TODO: t값을 점점빠르게 점점느리게 빨랐다 느려졌다 컨트롤 할수 있게 할것 
        float t = 0.0f;
        t += Time.deltaTime / 1f;  //분모가 증가 할수록 전체속도가 느려짐 속도에 영향

        Vector3 dir = target - transform.position;
        Vector3 Position = Vector3.zero;
        float viaX = (target.x + pos.x) / 2; //곡선을 이룰 방향값
       
        //TODO: 목표물의  In , Out 을 구별해서 보다 정밀하게 할 필요 있음

        float viaY = target.y;
        Utils_Proto.BezierInterpolation2(out Position.x,out Position.y,pos.x,pos.y,viaX,viaY,target.x,target.y,t);
        transform.position = new Vector3(Position.x, Position.y, 0);
        //transform.forward = Vector3.Slerp(transform.forward, dir.normalized, Time.deltaTime);
        
    }

    //시간 갯수 
    void bulletDivide(float dir_angle,int num)
    {
        //일정한 각도로 분산시키는 탄 부채꼴
        //객체를 회전만 하게 만들었음 
        this.transform.localRotation = Quaternion.Euler(0, 0, dir_angle); //(x,y,z) z에 앵글값 
        //dir_angle기준으로 최소값과 최대값  주의 폭이 아니라 자신의 좌우회전이 최대 최소임
        angle = this.transform.eulerAngles.z;
        minRange = this.transform.eulerAngles.z - 45.0f;
        maxRange = this.transform.eulerAngles.z + 45.0f;
    }

    void BulletTarget(Vector3 target,bool One)
    {
        bool isOneDir = One;
        //한번만 위치값을 받아서 적용시키기
        Vector3 pos = transform.position;

        if (isOneDir)
        {
           
            Vector3 dir = target - pos;
            dir = dir.normalized;
            pos.x += dir.x * Time.deltaTime * speed;
            pos.y += dir.y * Time.deltaTime * speed;
            transform.position = new Vector3(pos.x, pos.y, 0);
        }
        else
        {
                         
                         //처음 target - 자기위치
            Vector3 dir = trTarget - v_start0;
                    dir = dir.normalized;
            tempV2.Set(dir.x,dir.y);
            tempV2 = tempV2 * speed;

           // pos.x +=  dir.x * Time.deltaTime * speed;
           // pos.y +=  dir.y * Time.deltaTime * speed;

           transform.GetComponent<Rigidbody>().velocity = tempV2;
            
        }
     

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           
           // Destroy(gameObject);
        }
    }

    void ReLoadObject()
    {

        switch (StateTarget)
        {
            case targetType.Enemy:
                target = GameObject.FindGameObjectWithTag("Enemy").gameObject;
                break;

            case targetType.Player:
                target = GameObject.FindGameObjectWithTag("Player").gameObject;
                break;
        }
    }

}





