using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//하위의 요소중 하나만 활성화 되게 한다.
public class Toggle_SwitchEff : MonoBehaviour {

    //이벤트 발생시 딕셔너리에 넣는다.
    //재선택 됐을시 고유한 것인지 판단하기 위한 데이터는? 클릭시 자신의 고유한 아이디를 같이 넘겨준다.
    //현재활성화된 ID 를 기록한다.
    //만약 요소가 처음이라면 오브젝트를 활성화 시킨다.
    //이벤트 발생시 2개 이상이라면 딕셔너리를 모두 비활성화 시키고 현재것을 활성화 시킨다.
    //만약 일정요소가 모두 찾다면 현재의 선택된ID 를 찾아서 활성화 시킨다.
    //버튼글이벤트 일때활성화시킨다.
   
        
    int leng;
    public int curID;
    int agterID;

    public UnityCard[] childs;

    private void Start()
    {
        leng = transform.childCount;
        childs = new UnityCard[leng];
    }

    public  void Toggle()
    {
      
       
        childs = transform.GetComponentsInChildren<UnityCard>();

        //print("Length " + leng);
        //print("childs " + childs.Length);
        //print("child name :" + childs[2].name);
        //ERROR 찾지 못하고 계속 에러발생-------------------------------------------------------
        //UnityCard가 꺼져 있어서 차지 못하는것 같음
        // 오브젝트 이름으로 찾아서 활성화 시켜 볼것

        //childs[2].gameObject.transform.Find("Panel").gameObject.SetActive(true);
        //------------------------------------------------------------------------------------
        //print("child ====name" + "[" + 0 + "]" + childs[0].transform.transform.name);
        //print("num --->>>"+a);

        for (int i = 0; i < childs.Length; i++)
        {
            if (childs[i].GetComponentInChildren<UnityCard>().ID == curID)
            {
                childs[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                childs[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

}
