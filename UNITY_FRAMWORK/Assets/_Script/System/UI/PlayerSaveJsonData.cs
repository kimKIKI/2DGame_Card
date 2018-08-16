using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerSaveJsonData : MonoBehaviour {

    // (1 ,3), (3,7),(5,2)
    //임시로 만든 카드 종류와 카드 갯수를 담을 딕셔너리
    public Dictionary<int, int> dcPlayerhsCard = new Dictionary<int, int>();
    int key;

    private void Start()
    {
        AddDictionary();
    }

    ////인덱스를 사용하기 위해서 생성자만들어줌 
    public PlayerSaveJsonData(int key, Dictionary<int, int> data)
    {
       this.key = key;
       this.dcPlayerhsCard = data;
    }

    //임시로 카드를 만들고 배열하기 위해서 시작과 동시에 player의 카드르 입력한다.
    public void AddDictionary()
    {
        //  다음 카드를 가지고 있을때  { 0, 3, 5, 6, 7, 9, 10 };
        dcPlayerhsCard[0] = 0;
        dcPlayerhsCard[1] = 3;
        dcPlayerhsCard[2] = 5;
        dcPlayerhsCard[3] = 6;
        dcPlayerhsCard[4] = 7;
        dcPlayerhsCard[5] = 9;
        dcPlayerhsCard[6] =10;

    }

    public void printAll()
    {
        if (dcPlayerhsCard.Count >= 0)
        {
            print("데이터가 있습니다.");
        }
    }
    
    public int this[int index]
    {
        get { return dcPlayerhsCard[index]; }
        set { dcPlayerhsCard[index] = value; }
    }


}
