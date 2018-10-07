using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityUpdate : UnityForm
{


    protected override void Awake()
    {
        base.Awake();
    }

    public  void Set(int id, int gold, int amount)
    {
       
    //    int ID = id;

    //    if (GameData.Instance.hasCard.ContainsKey(ID))
    //    {   //가지고 있는 카드의 tatal value
    //        int hasCards = GameData.Instance.hasCard[ID].hasCard;
    //        //현재 카드가 레벨업된value 
    //        int level = GameData.Instance.hasCard[ID].level;
    //        //현재까지의 레벨로 총카드숫자로 변환
    //        int curLevel = GameData.Instance.Level[level];
    //        //업데이트하고 남은 카드의 value
    //        int remainCards = hasCards - curLevel;
    //        //next level의 전체 카드 value 
    //        int nextRemainCards = GameData.Instance.Level[level + 1];
    //        //현재레벨에서 다음레벨까지  필요한 카드의 value
    //        int curRemainCards = nextRemainCards - curLevel;

    //        LevelCount.text = string.Format("{0} / {1}", remainCards, nextRemainCards);
    //        //value 카드단위당 증가할 vale설정 백분율
    //        float uni = 1f / nextRemainCards;
    //        //최대값이 1이므로 1실수율로 변환
    //        float value = remainCards * uni;
    //        //구매시 애니가 작동되어야 한다.
    //        slider.value = value;
    //        //외부에서 리스너로 접근할 경우
    //        // slider.onValueChanged.AddListener(delegate { OnAmountChanged(); });

    //        //다음 레벨에  업글가능한 카드가 존재할때
    //        if (remainCards >= curRemainCards)
    //        {
    //            //업그레이드를 유도하는 애니이펙트 실행 화살표 움직임
    //        }
    //        // 고유아이디 1 은 UnityDatas[0]번째에 있음 따라서 -1                 
    //        string cardName = GameData.Instance.UnityDatas[ID - 1].Name;
    //        mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", cardName);
    //        cost.text = string.Format("{0}", gold);
    //        ownCards.text = string.Format(" X {0}", amount);
    //        name.text = string.Format("{0} X ", cardName);
    //        Count = amount;
    //        CountCards = remainCards;
    //        nextCardAmount = nextRemainCards;
    //        CountSlider = value;     //증가될 수치
    //        CountEa = uni;       //계산된 증가량
    //    }


    }


    ////중앙의 카드가 습득되는 애니 실행
    //public void CountAnime()
    //{
    //    iTween.ValueTo(gameObject, iTween.Hash("from", CountCards, "to", CountCards + Count, "time", .6, "onUpdate", "UpdateCardDisplay", "oncompletetarget", gameObject));
    //    CountCards++;
    //    //Gold증가
    //    iTween.ValueTo(gameObject, iTween.Hash("from", Count, "to", 0, "time", .6, "onUpdate", "UpdateGoldDisplay", "oncompletetarget", gameObject));
    //    Count--;

    //    iTween.ValueTo(gameObject, iTween.Hash("from", CountSlider, "to", 1, "time", .6, "onUpdate", "UpdateSliderDisplay", "oncompletetarget", gameObject));
    //    CountSlider += CountEa;

    //}


    ////value 가 증가할때 가지 애니실행한다. 
    //void UpdateGoldDisplay(int Value)//Gold:200 ~ 1200:증가 되며 계속호출됨
    //{
    //    AppSound.instance.SE_MENU_ITEMCOUNTGOLD.Play();
    //    ownCards.text = string.Format(" X {0}", Value);

    //}

    //void UpdateCardDisplay(int Value)//Gold:200 ~ 1200:증가 되며 계속호출됨
    //{
    //    LevelCount.text = string.Format("{0} / {1}", Value, nextCardAmount);
    //}

    //void UpdateSliderDisplay(float Value)
    //{
    //    slider.value = Value;
    //}

    void OnAmountChanged()
    {

    }
}
