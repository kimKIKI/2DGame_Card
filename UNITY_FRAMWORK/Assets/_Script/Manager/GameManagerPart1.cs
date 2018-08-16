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
                eText.text = String.Format("{0} :", "STARTMOTION");
                //시작시 보여지는 이펙트 
                StartLabelEff(startLabel);
                yield return new WaitForSeconds(1f);
                gameState = eGameState.START;
            }
            else if (gameState == eGameState.START)
            {
                eText.text = String.Format("{0} :", "START");
                //배치가 완료된후 잠시 모든 카드의 상황과 배치 
                //선공 후공을 랜덤으로 결정한다.
                StartLabelEffClose(startLabel);
                kingT2.Init();
                kingT.Init(); //타워의 초기 체력값을 적용한다.

                yield return new WaitForSeconds(2f);
                gameState = eGameState.CARDDISTRIBUTION;
            }
            else if (gameState == eGameState.CARDDISTRIBUTION)
            {
                eText.text = String.Format("{0} :", "CARDDISTRIBUTION");
                //처음 시작시 com이 랜덤하게 주먹,가위,보 결정하게 한다.

                //플레이어 가드정보를 받아온다
                PlayerCardInfoData();
                ComCardInfoData();

                //Player 자리 배치
                for (int i = 0; i < 9; i++)
                {
                    InputCard(i);
                    InputCardP2(i);
                    yield return new WaitForSeconds(0.12f);
                }

                formation2.FormationCardsArcCom(eGameCardSize.BASE, eBelong.COM);
                yield return new WaitForSeconds(0.5f);
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
                yield return new WaitForSeconds(1f);
                gameState = eGameState.PLAYER_CARDDISTRIBUTION;

            }
            else if (gameState == eGameState.PLAYER_CARDDISTRIBUTION)
            {       //player가 주먹가위보 선택
                eText.text = String.Format("{0} :", "PLAYER_CARDDISTRIBUTION");

                //누룰수 있게 표시 
                imgbtnColor = Color.white;
                //color을 컴포넌트에 적용
                btnImg.color = imgbtnColor;

                if (tem)
                {
                    //플레이어가 결정을 완료했다면 JUDGEMENT로 이동
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
                //쑈타임 스테이트가 필요함.
                //슬롯에 카드의 정보가 있을때 카드전투 애니실행
                for (int i = 0; i < 3; i++)
                {
                    ResultAniStart(i);
                    //1이 끝날때 까지 어떻게 기다리게 하는가?
                    yield return new WaitForSeconds(5.5f);
                    Victory_Result(Victory_pntEMPT(i));
                    yield return new WaitForSeconds(1f);

                }
               
            
              
                gameState = eGameState.JUDGEMENT_RESULT;

            }
            else if (gameState == eGameState.JUDGEMENT_RESULT)
            {
                yield return new WaitForSeconds(1f);
                //실제 카드슬롯의 카드제거 결과 data 실행
                Victory_Result(Victory_pntEMPT(0));
                yield return new WaitForSeconds(1f);
               // gameState = eGameState.JUDGEMENT_ANI;
               // yield return new WaitForSeconds(10f);
                gameState = eGameState.JUDGEMENT_NEXT;

            }
            else if (gameState == eGameState.JUDGEMENT_NEXT)
            {

                //승리조건 판단 
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
                //com의 카드가 모두 파괴된 상태일때
                if (formation2.CardFindsNumCom() <= 0 && ComRemainCenterCard() <= 0)
                {   //   com의 Base 숫자      com의  slot
                    stageVictory = 3;
                    yield return new WaitForSeconds(0.5f);
                    gameState = eGameState.VICTORY_OR_DEFEAT;
                }

                if (formation.CardFindsNum() <= 0 && RemainCenterCard() <= 0)
                {
                    //Player의 카드 숫자를 판단한다. CardInfo가 있는지 없는지 
                    stageVictory = 1;
                    yield return new WaitForSeconds(0.5f);
                    gameState = eGameState.VICTORY_OR_DEFEAT;
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

            else if (gameState == eGameState.PLAYER_CARDCHOICE)
            {


            }
            else if (gameState == eGameState.VICTORY_OR_DEFEAT)
            {
                eText.text = String.Format("{0} :", "VICTORY_OR_DEFEAT");

                switch (stageVictory)
                {
                    case 1:
                        //  패배 
                        startLabel.SetActive(true);
                        labelText.text = string.Format("{0}", " DEFEATED ");
                        StartLabelEff(startLabel);

                        gameState = eGameState.GAMERETREAT;

                        break;
                    case 2:

                        break;
                    case 3:
                        //승리
                        startLabel.SetActive(true);
                        labelText.text = string.Format("{0}", " VICTORY");
                        StartLabelEff(startLabel);

                        gameState = eGameState.GAMERETREAT;

                        break;

                }
                //최후 마지막 
                yield break;
            }
        }
    }
}
