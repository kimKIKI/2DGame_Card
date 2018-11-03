using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterFormation : MonoBehaviour {


   
    public GameCardSlot[] cardsLength;
    public eBelong eFormation = eBelong.NON;
    IList<Card> lsCards;
    GridLayoutGroup       gr;
    int startNum;

    bool Pick = false;
    int  IconCount;

    private void Awake()
    {
      cardsLength = transform.GetComponentsInChildren<GameCardSlot>();
        gr        = transform.GetComponent<GridLayoutGroup>();
        startNum  = gr.constraintCount/2;
    }

   

    //현재 슬롯에 위치한 card들을 담는다.
    void enableIcon()
    {
        lsCards = new List<Card>(); 
                                   
        for (int i = 0; i < cardsLength.Length; i++)
            {
                if (cardsLength[i].cardInfo != null)
                {
                lsCards.Add(cardsLength[i].cardInfo);
                }
            }
       
    }

    //저장후 현재 카드슬롯을 초기화 한다.
    IEnumerator IconAllClear()
    {
        yield return new WaitForSeconds(0.1f);
        //활성화된 card를 확인한다.
        for (int i = 0; i < cardsLength.Length; i++)
        {
            cardsLength[i].SetCardInfo(null,eBelong.NON,eCardType.SLOT);
        }
    }


    IEnumerator Move()
    {
        eBelong eT;
        yield return new WaitForSeconds(0.2f);
        if (lsCards.Count > 0)
        {
            for (int i = 0; i < lsCards.Count; i++)
            {
                cardsLength[startNum + i].SetCardInfo(lsCards[i],eCardType.SLOT);
            }
        }
    }

    //슬롯의 위치를 중앙으로 재 정렬하게 한다.----------???
    public void TurnMove()
    {
        enableIcon();
        StartCoroutine(IconAllClear());
        StartCoroutine(Move());
    }


}
