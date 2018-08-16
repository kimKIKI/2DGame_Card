using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class CombatShow : MonoBehaviour
{
    public Transform panel;
    public Image bg;
    public Transform player;
    public Transform Com;
    public Transform playerArm;
    public Transform comArm;
    public Transform combatImg;

    //-------------------------------
    CombatCardSlot playerSlot;
    CombatCardSlot comSlot;
    UIEffect E_comArm;
    UIEffect E_playerArm;
    Vector3 start;
    Vector3 endPos;
   

    GameManager gm;
    int[] playerShow = new int[3];
    int[] comShow    = new int[3];
    int[] result     = new int[3];
    float YPosArm;

  

    private void Awake()
    {
       
        playerSlot = player.GetComponent<CombatCardSlot>();
        comSlot = Com.GetComponent<CombatCardSlot>();
        E_comArm = comArm.GetComponent<UIEffect>();
        E_playerArm = playerArm.GetComponent<UIEffect>();
        bg.enabled = false;
        start = player.localPosition;
        endPos = new Vector3(-258, player.transform.localPosition.y, 1);
        YPosArm = comArm.localPosition.y;
        gm = GameObject.Find("@GM").GetComponent<GameManager>();
    }

    public void SetInfo(Card newCard, Card newCard2,int slot)
    {
        bg.enabled = true;
        playerSlot.SetCardInfo(newCard);
        comSlot.SetCardInfo(newCard2);

        StartCoroutine(AnimeStart(slot));

    }

    public IEnumerator AnimeStart(int cur)
    {

                Vector3 to = endPos;
                Vector3 to2 = new Vector3(endPos.x * -1, endPos.y, 1);
                playerSlot.MoveTo(to);
                comSlot.MoveTo(to2);

                yield return new WaitForSeconds(0.8f);

                Vector3 to3 = new Vector3(endPos.x, YPosArm, 1);
                Vector3 to4 = new Vector3(endPos.x * -1, YPosArm, 1);
                E_playerArm.MoveTo(to3);
                E_comArm.MoveTo(to4);
                yield return new WaitForSeconds(.3f);
                //가위바위보 표시 
                OpenCard(cur);
                //짠하는 이펙트 출현
                yield return new WaitForSeconds(0.5f);
                VictoryAni(result[cur]);
             
        
          
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
    }

    //델리게이트로 받은 가위바위보 데이터를 현재에 담는다.
    //가위바위보 정보
    void InPutRPS(int[] player, int[] com)
    {
        playerShow = player;
        comShow     = com;
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

  

    public void VictoryAni(int num)
    {

        switch (num)
        {
            case 1:
                //player패배

                Vector3 to3 = new Vector3(start.x , endPos.y, endPos.z);
                playerSlot.MoveTo(to3);

                Vector3 to4 = new Vector3(0, start.y, 1);
                //패배한 것은 바깥으로 사라지게 한다.
                comSlot.MoveTo(to4);

                Vector3 toPlayerArmL = new Vector3(start.x, YPosArm, 1);
                E_playerArm.MoveTo(toPlayerArmL);

                Vector3 toComArmL = new Vector3(0, YPosArm, 1);
                E_comArm.MoveTo(toComArmL);


                StartCoroutine(EndShow());

                break;
            case 2:
                //무승부
                Debug.Log("무승부 애니가 실현되었습니다.");
                //원래 위치로돌아 가는 애니실행
                StartCoroutine(ReturnSlot(0.6f));
                //출발하기 전의 값으로 다시 세팅
                StartCoroutine(EndShow());
                break;
            case 3:
                //player승리한 것음 중심으로이동시킨다.
             
                Vector3 to = new Vector3(0, endPos.y, endPos.z);
                playerSlot.MoveTo(to);

                Vector3 to1 = new Vector3(start.x * -1, start.y, 1);
                //패배한 것은 바깥으로 사라지게 한다.
                comSlot.MoveTo(to1);

                Vector3 toPlayerArm = new Vector3(0, YPosArm, 1);
                E_playerArm.MoveTo(toPlayerArm);

                Vector3 toComArm    = new Vector3(start.x, YPosArm, 1);
                E_comArm.MoveTo(toComArm);

           
                StartCoroutine(EndShow());
                break;
        }
    }

    //초기의 값으로 다시 세팅함
     IEnumerator EndShow()
    {
        yield return new WaitForSeconds(2f);
        //처음의 위치로 되돌린다.
        player.transform.localPosition = start;
        Com.transform.localPosition = new Vector3(start.x * -1, start.y, 1);
        playerArm.transform.localPosition = new Vector3(start.x, YPosArm, 1);
        comArm.transform.localPosition = new Vector3(start.x * -1, YPosArm, 1);
        E_playerArm.itemIcon.enabled = false;
        E_comArm.itemIcon.enabled    = false;
        bg.enabled = false;
    }

    IEnumerator ReturnSlot(float t)
    {
        yield return new WaitForSeconds(t);

        Vector3 tostart = start;
        playerSlot.MoveTo(tostart);

        Vector3 tostartCom = new Vector3(start.x * -1, start.y, 1);
        comSlot.MoveTo(tostartCom);

        playerArm.transform.localPosition = new Vector3(start.x, YPosArm, 1);
        comArm.transform.localPosition = new Vector3(start.x * -1, YPosArm, 1);
    }
}

         
