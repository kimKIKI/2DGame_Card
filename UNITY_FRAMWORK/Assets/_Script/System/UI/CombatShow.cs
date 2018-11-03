using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


enum eEff_state
{
 //발사,날라감,파괴,연기..등
 NON,
 FIRE,
 SHOOT,
 DESTROY,
}

enum eShock
{
  //발사 또는 공격에 따라 피격되는 타입
  LAZERDEST,
}

//전체 타격방법의 종류: 각각의공격에 필요한 오브젝트는 따로 설정한다.
public enum eAttackType
{
 //TODO: 혹시 연속 공격 또는 
 //특이한 형태의 공격이 있을시 표현해 주기 위해서 설정
  NON,
  GENERAL,
  LAZER,
  GROUND,
  BULLET,
}

public class CombatShow : MonoBehaviour
{
    //IList<AnimationClip> EffeAnimator = new List<AnimationClip>();
   

    public Transform panel;
    public GameObject powerEFF;   //공격이펙트
    public GameObject DestroyEFF; //피격이펙트

   

    public Image bg;
    public Transform playerKingTower;
    public Transform comKingTower;

    public Transform player;
    public Transform Com;
    public Transform playerArm;
    public Transform comArm;
    public Transform combatImg;
  


    delegate void AttackEffect(int id);
    AttackEffect eventSetEffect;

    //다야한 이펙트를 발생시키기 이해서 딕셔너리 필요함 .TODO : 
   // public readonly static int ANISTS_Idle = Animator.StringToHash("Base Layer.Power");

    
    //-------------------------------
    CombatCardSlot playerSlot;
    CombatCardSlot comSlot;
    CombatCardSlot KingPlyerSlot;
    CombatCardSlot KingComSlot;

    UIEffect       E_comArm;   //승리판정 오브젝트
    UIEffect       E_playerArm;
    SpriteAnim     ef_power;
    SpriteAnim     ef_destroy;
    Image          img_power;
    Image          img_destroy;
    Transform      KingPlayer; //player의 왕이미지
    Transform      KingCom;    //com의 왕이미지
    GameObject     EFFObject;  //생성되는 이펙트 오브젝트
    GameObject     EFFFire;    //발사 부분 이펙트 오브젝트
    Animator       anim_Power;
    Animator       anim_Destroy; //Effect Sprite animation을 교체하기 위해서 만든 스크립트
    Camera         MainCamera;
    Vector3        start;
    Vector3        endPos;
    Vector3        startY;  //킹타워가 출현하고  물러설 위치
    Vector3        endPosY;
    Vector3        firstPos;//변동이 없은 처음 시작좌표로 처음위치한 곳으로 되돌리기 위해서 사용
    Vector3        target;  //타워에서 발사될 이펙트가 도착할 좌표
    Transform       A;      //레이저 발사 시작(플레어편에서 볼때)
    Transform       B;      //레이저 도찾지점(플레이어편에서 볼때);
    int      curVicAndDef;  //현재 진행중인 애니가 player승리인지 컴승리인지 등록함
    eEff_state    effState      = eEff_state.NON;
    eShock        shock         = eShock.LAZERDEST;

    eAttackType   atkType       = eAttackType.NON;
    eAttackType   after;


    GameManager gm;
    int[] playerShow = new int[3];
    int[] comShow    = new int[3];
    int[] result     = new int[3];
    Card[] temp      = new Card[2];   //승리한 카드의 정보를 임시로 보관한다.

    float YPosArm;

    

    private void Awake()
    {
        MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        playerSlot  = player.GetComponent<CombatCardSlot>();
        comSlot     = Com.GetComponent<CombatCardSlot>();
        E_comArm    = comArm.GetComponent<UIEffect>();
        E_playerArm = playerArm.GetComponent<UIEffect>();
        img_power   = powerEFF.GetComponentInChildren<Image>();
        img_destroy = DestroyEFF.GetComponentInChildren<Image>();
        A           = GameObject.Find("A").GetComponent<Transform>();
        B           = GameObject.Find("B").GetComponent<Transform>();

        anim_Power   = powerEFF.GetComponentInChildren<Animator>(); 
        anim_Destroy = DestroyEFF.GetComponentInChildren<Animator>();
        ef_power     = powerEFF.GetComponentInChildren<SpriteAnim>();
        ef_destroy   = DestroyEFF.GetComponentInChildren <SpriteAnim>();

        KingPlayer    = transform.Find("Panel/KingPlayer").GetComponent<Transform>();
        KingCom       = transform.Find("Panel/KingCom").GetComponent<Transform>();
        KingPlyerSlot = KingPlayer.GetComponent<CombatCardSlot>();
        KingComSlot   = KingCom.GetComponent<CombatCardSlot>();

        bg.enabled  = false;
        start       = player.localPosition;
        endPos      = new Vector3(-258, player.transform.localPosition.y, 1);

        startY      = playerKingTower.localPosition;
       
        endPosY     = new Vector3(0,-600 , 1);
        firstPos    = new Vector3(0, 1320, 0);

        YPosArm     = comArm.localPosition.y;
        gm          = GameObject.Find("@GM").GetComponent<GameManager>();
        after       = atkType;
        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //EffeAnimator = Resources.LoadAll("Animation", typeof(AnimationClip));
        //EffeAnimator = Resources.LoadAll("Animation", typeof(AnimationClip)).Cast<AnimationClip>().ToArray();
      
    }

  

    //카드별 차이가 발생할수 있으므로 세팅시 자동체크 slot[0][1][2] 인덱스
    //newCard - com, newCard2-player
    public void SetInfo(Card newCard, Card newCard2,int slot)
    {
        bg.enabled = true;
        comSlot.SetCardInfo(newCard);
        playerSlot.SetCardInfo(newCard2);

        StartCoroutine(AnimeStart(slot));
        //TODO:타입을 입력받는다.
        //Delegate;메소드로 이벤트 호출
        temp[0] = newCard;
        temp[1] = newCard2;
    }


    //Delegate연결
    void ATK_Object()
    {
       // Resources.
       // EffeAnimator
    }

    #region AnimtionClip 방식

    //폴더에서 애니메이션 클립을 리스트에 저장한다.
    void ImportAnimtionClip()
    {
        //EffeAnimator = Resources.LoadAll("Animation", typeof(AnimationClip)).Cast<AnimationClip>().ToArray();
        //foreach (var t in EffeAnimator)
        //{
        //    string animeName = t.name;
        //    Debug.Log("name"+animeName);
        //    //여기서 card의 
        //}
    }
    #endregion

    #region Resources Road 방식
    //오브젝트를 삭제하고 다시 생성해서 위치시킬때 방식 별로 안좋음
    void SwitchEffect_DELETE()
    {
        //이펙트가 있을때
        SpriteAnim[] childList = GetComponentsInChildren<SpriteAnim>(true);
        Transform[] chs = powerEFF.GetComponentsInChildren<Transform>(true);
        Transform[] chsD = DestroyEFF.GetComponentsInChildren<Transform>(true);


        if (childList != null)
        {
            //Transform의 chsD 의 개수는 2개이다. [0]은 부모 [1]이 자식으로 자식을 제거하기 위해서 
            if (chs[1] != transform)
                Destroy(chs[1].gameObject);


            if (chsD[1] != transform)
                Destroy(chsD[1].gameObject);

        }
    }
    //새로운 이펙트로 변경시 사용
    IEnumerator SetArm()
    {

        //프리팹을 미리 로드하고 찾아서 쓰는 방식..메니저 추가 요망..리소스메니저에서 가져오게 할것
        yield return new WaitForSeconds(0.2f);
        Object prefab = Resources.Load("RoadPrefabs/Image_blue");
        GameObject obj = Instantiate(prefab) as GameObject;
        obj.transform.parent = powerEFF.transform;

        Object prefabD = Resources.Load("RoadPrefabs/Image_destruct_blue");
        GameObject objD = Instantiate(prefab) as GameObject;
        obj.transform.parent = DestroyEFF.transform;
    }

    IEnumerator SetArm2()
    {
       
        yield return new WaitForSeconds(0.2f);

        Object prefab = Resources.Load("RoadPrefabs/Image_red");
        GameObject obj = Instantiate(prefab) as GameObject;
        obj.transform.parent = powerEFF.transform;

        Object prefabD = Resources.Load("RoadPrefabs/Image_destruct_red");
        GameObject objD = Instantiate(prefab) as GameObject;
        obj.transform.parent = DestroyEFF.transform;
    }
    #endregion


    //승리했을때 이펙트의 타입을 받아와서 생성되게 한다.
   public IEnumerator  coAttackEffect(float t)
    {
       
        //여기서 타입이 결정되거나 분리 되어야 한다.
        yield return new WaitForSeconds(t);
       
        
        //파괴 이펙트에 타입을 전달한다.
        SpriteAnim.eAtk = this.atkType;

        switch (this.atkType)
        {
            case eAttackType.LAZER:
                //스프라이트 이미지가 보여지기 위해서 활성화 시킨다.
                StartCoroutine(ShotEffect(0.3f));
                img_power.enabled = true;
                anim_Power.SetBool("Lazer_blue", true);

                break;
            case eAttackType.GROUND:
                img_power.enabled = true;
                anim_Power.SetBool("Lazer_red", true);
                break;

            case eAttackType.BULLET:

                //발사체
                GameObject ef = AppUIEffect.instance.InstanceVFX(eEFFECT_NAME.BULLET);
                EFFObject = ef;
                //Fire
                GameObject fir = AppUIEffect.instance.InstanceFIRE(eEFFECT_NAME.BULLET);
                EFFFire = fir;
                //컴승리 프레이어패배일때
                if (curVicAndDef == 1)
                {
                    EFFFire.transform.position      = B.transform.position;
                 
                    EFFObject.transform.position    = B.transform.position;
                  
                    EFFObject.transform.localScale  = new Vector3(5, 5, 5);
                    target = new Vector3(A.transform.position.x,A.transform.position.y,1);
                    iTween.MoveTo(EFFObject, iTween.Hash("islocal", true, "position", target,
                                                      "oncompletetarget", gameObject, "time", .5f,
                                                       "easetype", "easeOutQuart"));

                }
                else if (curVicAndDef == 3)
                {
                    EFFFire.transform.position     = A.transform.position;
                    EFFObject.transform.position   = A.transform.position;
                    EFFObject.transform.localScale = new Vector3(5, 5, 5);
                    target = new Vector3(B.transform.position.x, B.transform.position.y,1);
                    iTween.MoveTo(EFFObject, iTween.Hash("islocal", true, "position", target,
                                                      "oncompletetarget", gameObject, "time", .5f,
                                                       "easetype", "easeOutQuart"));
                }
               
             

                break;
        }
        AppSound.instance.SE_COMBAT_LAZER2.Play();
    }

  

    //레이저가 발사되는 애니실행을 위햇서 스크립트를 활성화 시킨다.
    IEnumerator ShotEffect(float t)
    {
      
        yield return new WaitForSeconds(t);
        img_destroy.enabled = true;
        switch (this.atkType)
        {
            case eAttackType.LAZER:
                anim_Destroy.SetBool("Dest_blue", true);
                break;

            case eAttackType.GROUND:
                anim_Destroy.SetBool("Dest_red", true);
                break;
        }
       
    }

    void EFFECTEX()
    {
        ////스폰될 갯수 이다.
        //CollectingEffectController._instance.CollectItem(amount);
        ////스폰될 갯수의 개당 value;
        //CollectingEffectController._instance.Amount = 100;
        AppUIEffect.instance.InstanceVFX(eEFFECT_NAME.GOLD);
        //코인애니메이션실행
    }

    //com에서 이펙트 실행할때 coAttackEffectCom을 반대로 돌림 스프라이트 이펙트 경우 
    IEnumerator coAttackEffectCom(float t)
    {
        yield return new WaitForSeconds(t);
        powerEFF.transform.localRotation = Quaternion.AngleAxis(180, Vector3.back);
        StartCoroutine(coAttackEffect(0));
        float ti = t + 0.1f;

        StartCoroutine(coReturnEffectCom(ti));
    }

    //이펙트의 방향을 원래대로 돌려 놓는다.
    IEnumerator coReturnEffectCom(float t)
    {
        yield return new WaitForSeconds(t);
        powerEFF.transform.localRotation = Quaternion.AngleAxis(0, Vector3.back);
    }

    //승패를 보여주는 애니메이션 실행
    public IEnumerator AnimeStart(int cur)
    {
      
               //카드 출현 사운드
                AppSound.instance.SE_COMBAT_OUT.Play();
                Vector3 to = endPos;
                Vector3 to2 = new Vector3(endPos.x * -1, endPos.y, 1);
                playerSlot.MoveTo(to);
                comSlot.MoveTo(to2);

                //PlayerKing가 애니실행시 옴겨져야할 위치
                Vector3 to_k = endPosY; 
                //comKing가 옴겨져야할 위치
                Vector3 to_k2 = new Vector3(0, -1*endPosY.y, 1);
                KingPlyerSlot.MoveTo(to_k);
                KingComSlot.MoveTo(to_k2);
                yield return new WaitForSeconds(0.8f);

                Vector3 to3 = new Vector3(endPos.x, YPosArm, 1);
                Vector3 to4 = new Vector3(endPos.x * -1, YPosArm, 1);
                AppSound.instance.SE_COMBAT_IMPACT.Play();
                E_playerArm.MoveTo(to3);
                E_comArm.MoveTo(to4);

                Vector3 to3_k = new Vector3(0, startY.y, 1);
                Vector3 to4_k = new Vector3(0 , -1*startY.y, 1);
                KingPlyerSlot.MoveTo(to3_k);
                KingComSlot.MoveTo(to4_k);
                yield return new WaitForSeconds(.3f);
                //가위바위보 표시 
                OpenCard(cur);
                //가위바위보 충돌 짠하는 이펙트
                yield return new WaitForSeconds(0.5f);
               
                //승리한 판정을 바탕으로 애니메이션을 실행한다.
                StartCoroutine(VictoryAni(result[cur]));
    }

    //리스너... 적용할 델리게이트를 등록한다.
    void OnEnable()
    {
        GameManager.sendEventRPS += InPutRPS;
        GameManager.evnt_Victory += VictoryRPS;
    }

    private void OnDisable()
    {
        GameManager.sendEventRPS -= InPutRPS;
        GameManager.evnt_Victory -= VictoryRPS;

        AppSound.instance.SE_COMBAT_START.Stop();
    }

    //델리게이트로 받은 가위바위보 데이터를 현재에 담는다.
    //가위바위보 정보
    void InPutRPS(int[] player, int[] com)
    {
        playerShow = player;
        comShow    = com;
    }

    //델리게이트로 등록이 되어 있음. 호출시 배열이 갱신된다.인자로 데이터값을 받는 경우가 됨
    //승패정보
    void VictoryRPS(int[] index)
    {
        result = index;
    }

     //가위바위보를 표시한다.
    public void OpenCard(int index)
    {
        E_playerArm.itemIcon.enabled = true;
        E_comArm.itemIcon.enabled = true;

        E_playerArm.Rock_paperName(playerShow[index]);
        E_comArm.Rock_paperName(comShow[index]);
       
        //itweenScale  애니실행
        //중간 볼이 멈춰지면 각자의 가위바위보 로 sprite를 바꿔준다.
    }

    // player 의 ATK_Type 타입을 이펙트 타입에 적용한다.
    void SetATKType(int num)
    {
        if (num == 3)
        { //player승리
          //카드의 정보를 받아서 이펙트를 변경시켜준다.
          //temp 0:player,1:com 슬롯
            if (temp[0]  == null)
            {
                Debug.Log(" 카드가 없을때 작동합니다.");
            }
           else if (temp[0].atk_type == "Ground")
            {
                atkType = eAttackType.GROUND;
            }
            else if (temp[0].atk_type == "Lazer")
            {
                atkType = eAttackType.LAZER;
            }
            else if (temp[0].atk_type == "the air")
            {
                atkType = eAttackType.LAZER;
            }
            else if (temp[0].atk_type == "Top")
            {
                atkType = eAttackType.GROUND;
            }
            else if (temp[1].atk_type == "bullet")
            {
                atkType = eAttackType.BULLET;
            }
        }
        else if (num == 1)
        {//com 승리
         //카드의 정보를 받아서 이펙트를 변경시켜준다.
            if (temp[1] == null)
            {
                Debug.Log(" 카드가 없을때 작동합니다.");
            }
            else if (temp[1].atk_type == "Ground")
            {
                atkType = eAttackType.GROUND;
            }
            else if (temp[1].atk_type == "Lazer")
            {
                atkType = eAttackType.LAZER;
            }
            else if (temp[1].atk_type == "the air")
            {
                atkType = eAttackType.LAZER;
            }
            else if (temp[1].atk_type == "Top")
            {
                atkType = eAttackType.GROUND;
            }
            else if (temp[1].atk_type == "bullet")
            {
                atkType = eAttackType.BULLET;
            }
        }
        else if (num == 2)
        { //무승부
          //카드의 정보를 받아서 이펙트를 변경시켜준다.
            //if (temp[1] == null)
            //{
            //    Debug.Log(" 카드가 없을때 작동합니다.");
            //}
            //else if (temp[1].atk_type == "Ground")
            //{
            //    atkType = eAttackType.GROUND;
            //}
            //else if (temp[1].atk_type == "Lazer")
            //{
            //    atkType = eAttackType.LAZER;
            //}
            //else if (temp[1].atk_type == "the air")
            //{
            //    atkType = eAttackType.LAZER;
            //}
            //else if (temp[1].atk_type == "Top")
            //{
            //    atkType = eAttackType.GROUND;
            //}
        }
       
    }


    //승리했을때의 이동 & 승리한 카드의 ID 를 바탕으로 이펙트가 변경되게 한다.
    IEnumerator VictoryAni(int num)
    {
        //이펙트 출현시 방향이 달라져야 하므로 num을 통해서 어디가 승리인지 판단한다.
        curVicAndDef = num;
        SetATKType(num);
        switch (num)
        {
            case 1:
                //player패배________________________
                //이펙트의 state를 설정한다.
                yield return new WaitForSeconds(0.2f);

                Vector3 to3 = new Vector3(start.x, endPos.y, endPos.z);
                playerSlot.MoveTo(to3);

                //승리한 컴카드는 중앙으로 오고
                Vector3 to4 = new Vector3(0, start.y, 1);
                comSlot.MoveTo(to4);
                //작게 움직이는 중앙 아이콘 충돌애니실행
                Vector3 toPlayerArmL = new Vector3(start.x, YPosArm, 1);
                E_playerArm.MoveTo(toPlayerArmL);
                Vector3 toComArmL = new Vector3(0, YPosArm, 1);
                E_comArm.MoveTo(toComArmL);
               
                //이펙트의 타입을 받아와서 생성되게 한다. 스프라이트 이펙트가 섞여 있어서 2개 다 적어줌
                StartCoroutine(coAttackEffect(1f));
                DestroyEFF.transform.position = playerKingTower.transform.position;
               
                StartCoroutine(EndShow(1.6f));
                //카드가 소진되거나 킹타워가 Hp가 모두 소진됐다면 다음 애니가 실행되지 않고
                //게임이 끝나게 한다.

                break;
            case 2:
                //무승부
                Debug.Log("무승부 애니가 실현되었습니다.");
                //원래 위치로돌아 가는 애니실행
                StartCoroutine(ReturnSlot(0.6f));

                //출발하기 전의 값으로 다시 세팅
                StartCoroutine(EndShow(1.3f));
                // StartCoroutine(coAttackEffectCom(1f));
                
                break;

            case 3:
                //player승리_____________________________________

                //TODO: 승리한 카드의 정보를 얻거나 전달할수 있는 위치
                yield return new WaitForSeconds(0.2f);

                Vector3 to = new Vector3(0, endPos.y, endPos.z);
                playerSlot.MoveTo(to);

                Vector3 to1 = new Vector3(start.x * -1, start.y, 1);
                //패배한 것은 바깥으로 사라지게 한다.
                comSlot.MoveTo(to1);

                Vector3 toPlayerArm = new Vector3(0, YPosArm, 1);
                E_playerArm.MoveTo(toPlayerArm);

                Vector3 toComArm    = new Vector3(start.x, YPosArm, 1);
                E_comArm.MoveTo(toComArm);

                //Vector3 to6 = new Vector3(start.x, endPos.y, endPos.z);
                //playerSlot.MoveTo(to6);


                StartCoroutine(coAttackEffectCom(1f)); //--->>coAttackEffect
                                                       //이펙트 실행부분
                DestroyEFF.transform.position = comKingTower.transform.position;
              
                StartCoroutine(EndShow(1.5f));
                //카드가 소진되거나 킹타워가 Hp가 모두 소진됐다면 다음 애니가 실행되지 않고
                //게임이 끝나게 한다.


                break;
        }
    }

    //초기의 값으로 다시 세팅함(한턴이 끝나는 애니에이션임 3턴돔)
     IEnumerator EndShow(float t)
    {
        yield return new WaitForSeconds(t);
        //처음의 위치로 되돌린다.
        player.transform.localPosition     = start;
        Com.transform.localPosition        = new Vector3(start.x * -1, start.y, 1);
        playerArm.transform.localPosition  = new Vector3(start.x, YPosArm, 1);
        comArm.transform.localPosition     = new Vector3(start.x * -1, YPosArm, 1);
        E_playerArm.itemIcon.enabled       = false;
        E_comArm.itemIcon.enabled          = false;
        bg.enabled                         = false;

        //상하가 조금 늦게 움직이게 한다.
        StartCoroutine(DelayKingplayerSlot(0.55f));

    }

    //playerKing상하로 움직이는 애니메이션작동
    IEnumerator DelayKingplayerSlot(float t)
    {
        yield return new WaitForSeconds(t);
        Vector3 to3_k = new Vector3(0, -1 * firstPos.y, 1);
        Vector3 to4_k = new Vector3(0, firstPos.y, 1);
        KingPlyerSlot.MoveTo(to3_k);
        KingComSlot.MoveTo(to4_k);
    }

    IEnumerator ReturnSlot(float t)
    {
        yield return new WaitForSeconds(t);
        Vector3 tostart                   = start;
        playerSlot.MoveTo(tostart);

        Vector3 tostartCom                = new Vector3(start.x * -1, start.y, 1);
        comSlot.MoveTo(tostartCom);

        playerArm.transform.localPosition = new Vector3(start.x, YPosArm, 1);
        comArm.transform.localPosition    = new Vector3(start.x * -1, YPosArm, 1);
    }

  


}

         
