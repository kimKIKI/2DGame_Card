using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



 

public class CardEff_Result : MonoBehaviour {


    public  Transform PanelTop;  //Top의 화면 
    public  GameObject card;     //승리한 카드
    
    public  Transform gridGroup;    //패배한 카드요소
    public  Transform VicgridGroup; //승리한 카드의 위치
    int vic;                       //승리한 편

    Dictionary<int, IList<Card>> vicCard;
    Dictionary<int, IList<Card>> defCard;

   
    private void Awake()
    {
        //보이는 위치가 아니므로 gradGroup와 같이 한다.
       
    }

    private void Start()
    {


    }

    private void OnEnable()
    {
        //생성시 바로 alive 카드를 생성한다.
        //승리한 정보를 받아야 한다.
        //playerVic;
        //public Dictionary<int, IList<Card>> comVic;
        if (vic == 3)
        {
            vicCard = GameData.Instance.playerVic;
            defCard = GameData.Instance.comVic;
        }
        else if (vic == 1)
        {
            vicCard =  GameData.Instance.comVic;
            defCard =  GameData.Instance.playerVic;
        }
    }


    IEnumerator ResultAnimation()
    {

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(VictoryCardAnimation());
        yield return new WaitForSeconds(5f);
        StartCoroutine(DefectCardAnimation());


    }

    //승리한 데이터를 바탕으로 오브젝트를 생성한다.
    void FormationCard()
    {
           // vicCard.ContainsKey(i).Equals(vicCard.Values);
           //딕셔너리를 돌면서 id - card,card,card 를 배열시켜야 한다.
            foreach (KeyValuePair<int, IList<Card>> id in vicCard)
            {
                   //승리한 카드 
                   GameObject newCard0 = Instantiate(card, VicgridGroup);
                   Card vCardVic       = new Card();
                   vCardVic.ID = id.Key;
                   vCardVic.IconName = GameData.Instance.UnityDatas[id.Key].Name;
                   newCard0.GetComponent<VictoryCard>().SetCardInfo(vCardVic);
                   //패배한 카드를 담을 리스트를 여기서 생성시킨다.
                   newCard0.GetComponent<VictoryCard>().vicCards = new List<Card>();
                   newCard0.transform.localPosition = new Vector3(0, 0, 0);
                       

                    //승리한 카드가 패배시킨 카드들
                    IList<Card> cards = new List<Card>();
                    if (vicCard.TryGetValue(id.Key, out cards))
                    {
                        for(int d = cards.Count-1; d>=0;d--)
                        {
                            GameObject newCard = Instantiate(card, gridGroup);
                            Card vCard = new Card();
                            vCard.ID = cards[d].ID;
                            vCard.IconName = GameData.Instance.UnityDatas[cards[d].ID].Name;
                            newCard.GetComponent<VictoryCard>().SetCardInfo(vCard);
                            newCard.transform.localPosition = new Vector3(0, 0, 0);
                            newCard0.GetComponent<VictoryCard>().vicCards.Add(vCard);

                        }
                    }
            }
    }

    //승리한 카드 를 보여준다.
    IEnumerator VictoryCardAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        //group에서 카드의 정보를 받아서 검사한다.
        int vicCardNum = VicgridGroup.transform.childCount;
        //gridGroup에서의 순서를 위해서 
        //int sumNum = 0;
        float sideAmount = 300f;
       
        for (int i = 0; i < vicCardNum; i++)
        {
          
            VicgridGroup.transform.GetChild(i);

            Vector3 pos = new Vector3(2400+sideAmount, 200, 0);
            iTween.MoveTo(VicgridGroup.transform.GetChild(i).gameObject, iTween.Hash("islocal", true, "position", pos, "time", 0.6f,
                                                "easetype", "easeOutQuart"));
            //위치를정렬시킨다.
            //Grid를 코드로 구현한다.
            sideAmount -= 100;
            yield return new WaitForSeconds(1f);

           
            #region 연속애니실행시
            //int listCards = VicgridGroup.transform.GetChild(i).GetComponent<VictoryCard>().vicCards.Count;

            //    for (int j = 0; j < listCards; j++)
            //    {

            //        gridGroup.transform.GetChild(j + sumNum).gameObject.transform.localPosition = pos;
            //        //순서가 보장되는가?foreach에서 순서대로넣어서 가능할지 모름
            //        Vector3 pos2 = new Vector3(2400, 1000, 0);
            //        iTween.MoveTo(gridGroup.transform.GetChild(j+sumNum).gameObject, iTween.Hash("islocal", true, "position", pos2, "time", 0.5f,
            //                                            "easetype", "easeOutQuart"));

            //        yield return new WaitForSeconds(0.5f);
            //    }
            //sumNum += listCards;
            #endregion
        }
       
        yield return new WaitForSeconds(0.7f);

     

       VicgridGroup.GetComponent<GridLayoutGroup>().enabled = true;
        float sellX     = VicgridGroup.GetComponent<GridLayoutGroup>().cellSize.x;
        float spacingX  = VicgridGroup.GetComponent<GridLayoutGroup>().spacing.x;
        float widthX    =  vicCardNum*sellX  +(vicCardNum - 1) * spacingX;
        float startX    = widthX * 0.5f * -1f;
        VicgridGroup.GetComponent<RectTransform>().localScale    = new Vector3(0.7f, 0.7f, 0);
        VicgridGroup.GetComponent<RectTransform>().localPosition = new Vector3(startX, 140, 0);

    }

    IEnumerator DefectCardAnimation( )
    {
        int vicCardNum = VicgridGroup.transform.childCount;
        int sumNum = 0;
        for (int i = 0; i < vicCardNum; i++)
        {
            int listCards = VicgridGroup.transform.GetChild(i).GetComponent<VictoryCard>().vicCards.Count;
            Vector3 pos   = VicgridGroup.GetChild(i).transform.position;

            for (int j = 0; j < listCards; j++)
            {
                VicgridGroup.transform.GetChild(i).GetComponent<VictoryCard>().AnimeClick();
                //움직임을 시작할 시작위치
                gridGroup.transform.GetChild(j + sumNum).gameObject.transform.position = pos;
                //순서가 보장되는가?foreach에서 순서대로넣어서 가능할지 모름
                //마무리되는 위치 
                Vector3 pos2 = new Vector3(2400, 1000, 0);
              
                iTween.MoveTo(gridGroup.transform.GetChild(j + sumNum).gameObject, iTween.Hash("islocal", true, "position", pos2, "time", 0.5f,
                                                    "easetype", "easeOutQuart"));

                yield return new WaitForSeconds(0.5f);
            }
            sumNum += listCards;
            yield return new WaitForSeconds(1.3f);
            //TODO: 마지막 카드가 사라지면 특별한 이벤트가 발생해야 한다.
            //카드가 움직임이 발생하면 특정한 이벤트발생

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
         {

            //임시로 카드를 넣고 테스트 하기 위해서 작성
            int[] temp = { 2, 5, 4, 7 }; //일단 같은 카드가 없도록 한다.                     
            int[] temp2 = { 9, 8 };
            int[] temp3 = { 1, 6 };
            int[] victemp = { 3, 4, 1 };    //최종 승리한 카드 

            vicCard = new Dictionary<int, IList<Card>>();

           
                IList<Card> newList = new List<Card>();

                for (int i = 0; i < temp.Length; i++)
                {
                Card newCard = new Card();
                newCard.ID = GameData.Instance.UnityDatas[temp[i]].Id;
                newList.Add(newCard);
                }
                vicCard.Add(victemp[0], newList);

            IList<Card> newList2 = new List<Card>();
            for (int i = 0; i < temp2.Length; i++)
            {
                Card newCard = new Card();
                newCard.ID = GameData.Instance.UnityDatas[temp2[i]].Id;
                newList2.Add(newCard);
            }
            vicCard.Add(victemp[1], newList2);

            IList<Card> newList3 = new List<Card>();
            for (int i = 0; i < temp3.Length; i++)
            {
                Card newCard = new Card();
                newCard.ID = GameData.Instance.UnityDatas[temp3[i]].Id;
                newList3.Add(newCard);
            }
            vicCard.Add(victemp[2], newList3);



        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            FormationCard();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(ResultAnimation());
        }
    }
}



