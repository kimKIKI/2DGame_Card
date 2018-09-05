using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public partial class GameManager : MonoBehaviour {

    IEnumerator AutoStep()
    {
       

        yield return new WaitForSeconds(2f);
        while (true)
        {

            if (gameState == eGameState.STARTMOTION)
            {
                //시작음향
                AppSound.instance.SE_START.Play();
                //시작이미지
                eText.text = String.Format("{0} :", "STARTMOTION");
                //시작시 보여지는 이펙트 
                StartLabelEff(startLabel);
                yield return new WaitForSeconds(1.3f);
                //시작이미지 사라지는 음향
                AppSound.instance.SE_START_SPR.Play();
                yield return new WaitForSeconds(1f);
                gameState = eGameState.START;
            }
            else if (gameState == eGameState.START)
            {
                eText.text = String.Format("{0} :", "START");
                //배치가 완료된후 잠시 모든 카드의 상황과 배치 
                //선공 후공을 랜덤으로 결정한다.
                StartLabelEffClose(startLabel);
                //TODO:kingT 레벨적용
                //player의 저장 순번 1 = key
                //player & com 타워레벨 데이터 설정
                LevelSet();
                //타워의 초기 체력값을 적용한다.
                kingT2.Init();
                kingT.Init();

                yield return new WaitForSeconds(2f);
                gameState = eGameState.CARDDISTRIBUTION;
            }
            else if (gameState == eGameState.CARDDISTRIBUTION)
            {    //카드분배
                eText.text = String.Format("{0} :", "CARDDISTRIBUTION");
                //처음 시작시 com이 랜덤하게 주먹,가위,보 결정하게 한다.

                //플레이어 카드정보를 받아온다
                PlayerCardInfoData();
                ComCardInfoData();
                AppSound.instance.SE_OpenCard2.Play();
                //Player 자리 배치
                for (int i = 0; i < CardNumSet; i++)
                {
                    InputCard(i);
                    InputCardP2(i);
                    yield return new WaitForSeconds(0.12f);
                }

                AppSound.instance.SE_CardOpenWidth.Play();
                formation2.FormationCardsArcCom(eGameCardSize.BASE, eBelong.COM);

                yield return new WaitForSeconds(0.5f);
                AppSound.instance.SE_CardOpenWidth.Play();
                formation.FormationCardsArc(eGameCardSize.BASE, eBelong.PLAYER);

                gameState = eGameState.COM_CARDMOVE;

            }
            else if (gameState == eGameState.COM_CARDMOVE)
            {  //com의 카드가 이동완료후 가위바위보 설정
                eText.text = String.Format("{0} :", "COM_CARDMOVE");

                //TODO:간헐적으로 에러발생
                StartCoroutine(CardMove(1f));
                yield return new WaitForSeconds(5f);

                //컴퓨터의 랜덤한 숫자할당
                InputRPS();
                //컴가위바위보 수정
                AppSound.instance.SE_CARD_COM.Play();
                yield return new WaitForSeconds(1f);
                gameState = eGameState.PLAYER_CARDDISTRIBUTION;

            }
            else if (gameState == eGameState.PLAYER_CARDDISTRIBUTION)
            {   //player가 주먹가위보 선택------------
                eText.text = String.Format("{0} :", "PLAYER_CARDDISTRIBUTION");

                //누룰수 있게 표시 
                imgbtnColor = Color.white;
                //color을 컴포넌트에 적용
                btnImg.color = imgbtnColor;

                if (tem)
                {
                    //플레이어가 결정을 완료했다면 JUDGEMENT로 이동
                    //버튼에서 정보를 받는다.
                    PlayerRPS();
                    yield return new WaitForSeconds(1f);
                    tem = false;

                    //버튼칼라변경
                    imgbtnColor = Color.black;
                    btnImg.color = imgbtnColor;

                    gameState = eGameState.JUDGEMENT;

                }
                yield return null;

            }
            else if (gameState == eGameState.JUDGEMENT)
            {
                eText.text = String.Format("{0} :", "JUDGEMENT");
                //보류 :TODO 별로임
                //MoviesEffect(0);
                //슬롯의 가위바위보정보를 델리게이트가 호출되는 곳에 전달한다.
                SendRPS(rock_paper_scissorsPlayer, rock_paper_scissors);
                //승리한 결과값을 애니메이션 하기 위해서 CombatShow로 보낸다.델리게이트로 
                //연결이 되어 있으므로 value만 넘겨지게 된다.
                SendRPSResult();

                yield return new WaitForSeconds(1f);
                //애니실행
                gameState = eGameState.JUDGEMENT_ANI;
            }
            else if (gameState == eGameState.JUDGEMENT_ANI)
            {
                bool isDecision = false;
                //전투씬의 긴박한 음악시작
                AppSound.instance.SE_COMBAT_START.Play();
                //슬롯에 카드의 정보가 있을때 카드전투 애니실행
                for (int i = 0; i < 3; i++)
                {
                    ResultAniStart(i);
                    //1이 끝날때 까지 어떻게 기다리게 하는가?
                    yield return new WaitForSeconds(5.5f);
                    //슬롯이 비어있지 안다면 판정및 삭제애니 실행됨
                    // TODO : 타워의 인원이나 피가 다 됐는지 판단한다.

                    Victory_Result(Victory_pntEMPT(i));

                    //판별중 게임이 승부가 나오면 게임을 중지한다.
                    if (isDecision != KingTowerCheck())
                    {
                        StartCoroutine(VICTORY_OR_DEFEAT());
                        yield break;
                    }
                    yield return new WaitForSeconds(1f);

                    //TODO : 여기서 중간 판정을 해서 gameState를 변경해야된다.
                    //그러나그 변경값이적용되지 않고 있다.
                }
                gameState = eGameState.JUDGEMENT_RESULT;

            }
            else if (gameState == eGameState.JUDGEMENT_RESULT)
            {
                AppSound.instance.SE_COMBAT_START.Stop();
                yield return new WaitForSeconds(1f);

                gameState = eGameState.JUDGEMENT_NEXT;

            }
            else if (gameState == eGameState.JUDGEMENT_NEXT)
            {
                //승리조건 한턴후 승리조건판단한다.
                eText.text = String.Format("{0} :", "JUDGEMENT_NEXT");
                //버튼부모 이미지의 칼라를 변경한다.버튼 칼라가 클릭후 적용되지 안아서 적용
                imgbtnColor = Color.black;
                btnImg.color = imgbtnColor;


                // com의 베이스와 센터에 카드가 남아 있는지 판단한다.
                if (formation2.CardFindsNumCom() > 0 || ComRemainCenterCard() > 0 || formation.CardFindsNum() > 0 || RemainCenterCard() > 0)
                {  //   com의 Base 숫자      com의  slot
                   //플레이어 클릭세팅 판정요구
                    gameState = eGameState.COM_CARDMOVE;
                }

                yield return null;
            }
            else if (gameState == eGameState.COM_DISTRIBUTION)
            {
                eText.text = String.Format("{0} :", "COM_DISTRIBUTION");

                yield return new WaitForSeconds(0.2f);
                //com의 자동으로 가위바위보 입력
                InputRPS();
                gameState = eGameState.PLAYER_CARDDISTRIBUTION;
            }
            else if (TempCorStop)
            {
                yield  break;
            }
        }
    }
}
