using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//현재 오브젝트의 활성화 버튼을 끄거나 파괴한다.
public class UI_Close : MonoBehaviour {

    public  delegate void degClose();
    public static degClose eveDegClose;
  
    //자신의 부모를 비활성화 한다.
    public void Close()
    {
        transform.parent.gameObject.SetActive(false);
         GameData.Instance.isStopScroview = false;
         GameData.Instance.IsShowCard     = false;
         eveDegClose();
    }
}
