﻿using System.Collections;
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
                //TODO:여기서 카드의 베이스 폼을 정할수 있게된다.
                formation2.FormationCardsArcCom(eGameCardSize.BASE, eBelong.COM,eCardType.SLOT);
                //formation2.FormationCards(eGameCardSize.BASE);
                yield return new WaitForSeconds(0.5f);
                AppSound.instance.SE_CardOpenWidth.Play();
                formation.FormationCardsArc(eGameCardSize.BASE, eBelong.PLAYER,eCardType.SLOT);
                //formation.FormationCards(eGameCardSize.BASE);
                gameState = eGameState.COM_CARDMOVE;

            }
            else if (gameState == eGameState.COM_CARDMOVE)
            {  //com의 카드가 이동완료후 가위바위보 설정
                eText.text = String.Format("{0} :", "COM_CARDMOVE");
                eveCardIn();
                //TODO:간헐적으로 에러발생
                StartCoroutine(CardMove(1f));
                yield return new WaitForSeconds(5f);

                //컴퓨터의 랜덤한 숫자할당
                InputRPS();
                //컴가위바위보 수정
                AppSound.instance.SE_CARD_COM.Play();
                yield return new WaitForSeconds(1f);
                gameState      = eGameState.PLAYER_CARDDISTRIBUTION;

            }
            else if (gameState == eGameState.PLAYER_CARDDISTRIBUTION)
            {   //player가 주먹가위보 선택------------
                //만약 선택없이 대기 시간이 길어진다면 알림메세지 발동한다.

                if(!throwMessagePlay)
                StartCoroutine(checkMessageTime(10f));

                if (throwMessage)
                {
                    Debug.Log("안내를 시작합니다.");
                    //TODO:안내가 끝나면 throwMessagPlay 를 다시  fale로 놓아야 한다.
                    throwMessage = false;
                }

                //turn버튼이 클릭 가능한 상태일때 player에게 확실히 알려주어야 한다.
                //player의 카드가없을때도 턴이 가능하다.따라서 슬롯이 빈상태일때 검사는 안됨..
                // 컴퓨터가 배치되고 난후 일정시간이후에 turn 활성화시킨다.
                eText.text     = String.Format("{0} :", "PLAYER_CARDDISTRIBUTION");
                //이펙트추가 필요: 중앙부터 버튼까지 이동
                //누룰수 있게 표시  한번만 실행
                if (IsTurn)
                {
                    if (centerEffect.gameObject != null)
                        centerEffect.gameObject.SetActive(true);

                    //SOUND 생성   AppSound.instance.SE_COMBAT_OUT.Play();
                    if (EffTurn.gameObject != null)
                    {
                        //Debug.Log("비어 있지 않습니다.");
                        EffTurn.gameObject.SetActive(true);
                        Vector3 pos = buttonTurn.position;
                        iTween.MoveTo(EffTurn.gameObject, iTween.Hash("position", pos,"easetype", iTween.EaseType.easeInOutQuint, "time", 0.3f));
                    }
                    imgbtnColor = Color.white;
                    //color을 컴포넌트에 적용
                    btnImg.color = imgbtnColor;
                    IsTurn = false;
                }

                if (tem)
                {
                    //플레이어가 결정을 완료했다면 JUDGEMENT로 이동
                    //버튼에서 정보를 받는다.
                    PlayerRPS();
                    yield return new WaitForSeconds(1f);
                    tem          = false;

                    //버튼칼라변경
                    imgbtnColor  = Color.black;
                    btnImg.color = imgbtnColor;
                    gameState    = eGameState.JUDGEMENT;

                    EffTurn.gameObject.SetActive(false);
                    EffTurn.gameObject.transform.position = new Vector3(0, 0, 0);
                    //새로운 턴이 발생시 버튼이펙트가 작동되게 한다.
                    IsTurn       = true;
                }
                yield return null;
            }
            else if (gameState == eGameState.JUDGEMENT)
            {
                eText.text      = String.Format("{0} :","JUDGEMENT");
                //보류 :TODO 별로임
                //MoviesEffect(0);
                //슬롯의 가위바위보정보를 델리게이트가 호출되는 곳에 전달한다.
                SendRPS(rock_paper_scissorsPlayer, rock_paper_scissors);
                //승리한 결과값을 애니메이션 하기 위해서 CombatShow로 보낸다.델리게이트로 
                //연결이 되어 있으므로 value만 넘겨지게 된다.
                SendRPSResult();

                yield return new WaitForSeconds(1f);
                //애니실행
                gameState      = eGameState.JUDGEMENT_ANI;
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
                    float delayAdd = 0;
                    //TODO: 무승부가 아닐경우 이펙트 시간을 늘려줘야 한다.
                    //무승부일때2일때 추가시간0,승패애니있을때 1추가 
                    if (onlyVictoryResult1(i) == 2)
                    {
                        delayAdd = 0f;
                    }
                    else
                    {
                        delayAdd = 3f;
                    }

                    //1이 끝날때 까지 어떻게 기다리게 하는가?
                    yield return new WaitForSeconds(5.5f+delayAdd);

                    //슬롯이 비어있지 안다면 판정및 삭제애니 실행됨 센터자리별 판단
                    Victory_Result(Victory_pntEMPT(i));

                    //판별중 게임이 승부가 나오면 게임을 중지한다.
                    //전체 카드 판단
                    //TODO: true일때 모무 비었다는 뜻
                    bool playerEMPT = EMP(1); //player
                    bool comEMPT    = EMP(2); //com

                    if (playerEMPT)
                    {//player가 카드가 없을때
                     //1:컴승리,2: ,3:player승리
                     //  GameData.Instance.vicResult = stageVictory;
                     //GameData.Instance.vicResult = 1;
                        stageVictory = 1;
                       
                        StartCoroutine(VICTORY_OR_DEFEAT());
                        break;
                    }
                    else if (comEMPT)
                    {//com카드가 비었을때
                     //GameData.Instance.vicResult = 3;
                        stageVictory = 3;
                        StartCoroutine(VICTORY_OR_DEFEAT());
                        break;
                    }
                    else if (playerEMPT && comEMPT)
                    {//모두 비었을때

                    }
                    else if (!playerEMPT && !comEMPT)
                    {//모두 있을때
                     //계속 게임진행
                    }


                    //kingTower의 Hp,숫자 판단
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
                //TODO: 잘 작동하는지 의심스러움..
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
