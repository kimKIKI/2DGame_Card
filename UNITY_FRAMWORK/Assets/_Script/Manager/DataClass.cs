using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Data_TYPE
{
    Cards,
    Market,
};


public class DataClass {
    //데이터를 리스트화 딕셔너리에 담기 위한 클래스 
        public int ID;
        public string Name;
        public int CardKinds;
        
}

//플레이어가 가지고 있는 카드의 수량을 파악하고 카드의 모양을 세팅하는데 필요한 정보를 담는다.
public class Card
{
    public int     ID;
    public int     level;    //현재 카드별 가지고 있는 레벨
    public int     hasCard;
    public int     Attack;   //man 피해량
    public int     Up_Hp;    //증가할 타워량
    public int     AtK_zone; //성벽Level 피해량
    public string  atk_type; //이펙트의 타입을 결정해주기 위해서설정
    public string  Name;     //카드이름
    public string  Shap;     //카드 모양
    public string  IconName; //카드이미지
    public string  Spable;   //현재 카드가 가지고 있는 특수한 능력
    

}

public class MarketInfo
{
    public int ID;
    public int EA;
    public int Gold;
    public int Jew;
}

//Nor상자의 케이스 세팅
public class NorInfo
{
    public   int      PriceGold;
    public   string   caseName;
}

//보석상자 
public class CaseInfo
{
    public int gold;
    public int jew;
    public int generNum;
    public int rareNum;
    public int heroNum;
    public int legendaryNum;
}

public class MarketRoyle
{
    public int Jew;
    public int CaseIndex;
    public string CaseName;

}

public class MarketItemBoxInfo
{
    public string Type;
    public int General;
    public int Rare;
    public int Hero;
    public int Legendary;
    public int PGold;
}

public class ItemBoxInfo
{
    //특별 상품별 항목 
    public string boxName;
    public int Gold;
    public int Jew;
    public int generalNum;
    public int RareNum;
    public int HeroNum;
    public int HeroNum2;
    public int HeroNum3;
    public int LengendaryNum;
    public int productNum;
    //num의 개별 숫자가 필요한가 ?
}


public class PlayerInfo
{
    string id;              //고유ID
    int    JoinData;        //가입일
    string idName;          //게임닉네임
    string name;            //플레이어이름

    [HideInInspector]
   public int telephone;    //핸드폰
    [HideInInspector]
    public int coin;        //보유코인
    [HideInInspector]
    public int jew;         //보유 보석
    [HideInInspector]
    public int cardHas = 0; //전체 종류중에 찾은 카드의 수
    [HideInInspector]
    public int selectNum = 4;//tab에 선택된 카드의 수
    [HideInInspector]
    public int exp;
    [HideInInspector]
    public int expCount;

    public PlayerInfo(){}
    public PlayerInfo(string ID,string NAME,string NickName,int join,int hasCards,int Gold,int Jew,int expL,int expCount)
    {
        id            =ID;
        name          = NAME;
        idName        = NickName;
        JoinData      = join;
        cardHas       = hasCards;
        coin          = Gold;
        jew           = Jew;
        exp           = expL;
        this.expCount = expCount;
    }

    List<int> cards;                          //가지고 있는 카드의 id의 배열
    Dictionary<int, int> cardNums;            //레벨별 카드수량 저장
    Dictionary<int, List<Card>> hasKinds;     //카드별 수량 계속 추가또는 삭제 가능해야됨
    Dictionary<string,List<int>> dicCardNum;  //카드네임, 갯수  
    //1tab{4,5,6,8,9} tab2 z{5,6,8,1,10}
    Dictionary<int, int[]> selectCards = new Dictionary<int, int[]>();

}



//Json에서의 데이더 딕셔너리 리스트에 저장하기 위한 데이터임
////메인및 다른곳에서 사용할수있게 단순 데이터 형태로 
public class UnityInfo
{
    //{"ID":0,"Name":"Ice Gollum ","ATK_Type":"Top","Kinds":"Unit","Coin":300,"Jew":10,"Elixir":2,"HP":400,"Speed":"Slow","Attack":4,"Atk_Zone":100,"BuildTime":1,"Sp_atk":4,"Up_Hp":10,"Up_atk":10,"Life":0,"EA":1},
    int id;
    string name;
    string IconName;//메인아이콘
    string atk_Type; //유닛타입 지상,공중
    string spAble;  //유닛의 특수한 능력
    string kinds;    //유닛의 레벨  일반유닛,희귀유닛,히어로, 레전드
    int coin;        //상점에서 살때의 고유가격
    int jew;         //보석으로 살때의 가격
    int hp;          //고유 체력
    int speed;       //이동시 속도
    int attack;      //공격시 피해량
    int atk_Zone;    //공격 광역법위
    int build;       //생성속도
    int life;        //건물일때 유지 시간
    int up_atk_Zone; //업글시 파괴영역 증가량
    int up_atk;      //업글시 공격력 증가량
    int up_Hp;       //추가될 HP량
    int elixir;      //엘릭서
    int spawnEA = 1; //생성될 겟수 default = 1;


    public UnityInfo() { }
    public UnityInfo(string _iconName)
    {
        IconName = _iconName;
        //temp.GetComponent<UnityCard>().mainICon.sprite = SpriteManager.GetSpriteByName("Sprite", "Sample_UI_1");
    }

  

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Atk_Type
    {
        get
        {
            return atk_Type;
        }
        set
        {
            atk_Type = value;
        }
    }

    public string Kinds
    {
        get
        {
            return kinds;
        }

        set
        {
            kinds = value;
        }
    }

    public int Coin
    {
        get
        {
            return coin;
        }

        set
        {
            coin = value;
        }
    }

    public int Jew
    {
        get
        {
            return jew;
        }

        set
        {
            jew = value;
        }
    }

    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }

    public int Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    public int Attack
    {
        get
        {
            return attack;
        }

        set
        {
            attack = value;
        }
    }

    public int Atk_Zone
    {
        get
        {
            return atk_Zone;
        }

        set
        {
            atk_Zone = value;
        }
    }

    public int Build
    {
        get
        {
            return build;
        }

        set
        {
            build = value;
        }
    }

    public int Life
    {
        get
        {
            return life;
        }

        set
        {
            life = value;
        }
    }

    public int Up_atk_Zone
    {
        get
        {
            return up_atk_Zone;
        }

        set
        {
            up_atk_Zone = value;
        }
    }

    public int Up_atk
    {
        get
        {
            return up_atk;
        }

        set
        {
            up_atk = value;
        }
    }

    public int SpawnEA
    {
        get
        {
            return spawnEA;
        }

        set
        {
            spawnEA = value;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public int Elixir
    {
        get;  set;
    }

    public int Up_Hp
    {
        get; set;
    }

    public string SpAble
    {
        get
        {
            return spAble;
        }

        set
        {
            spAble = value;
        }
    }
}


//json  saveData 클래스 json파일을 저장하고 편집하기 위해서 먼저 데이터 타입에 대입되는 클래스 타입을 만든다.
    [Serializable]
    class DataJson
    {
       public string name;
       public List<string> likes;
       public int level;
    }

