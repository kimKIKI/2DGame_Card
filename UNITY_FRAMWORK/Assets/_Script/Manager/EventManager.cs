
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum EVENT_TYPE
{
    GAME_INIT,
    GAME_END,
    AMMO_CHANGE,
    HEALTH_CHANGE,
    SLOTNUMBER,        //slot 즉 카드가 숫자가 얼마나 남아 있는지 판단하기위해서 리스너 등록
    GOLD_CHANGE,       //어디서든 Top의Gold가 에 접근해서 자동으로 애니가 작동되게 한다. 
    JEW_CHANGE,       
    LEVEL_CHANGE,
    DEAD
};

public class EventManager : MonoBehaviour
{

    #region C#프러퍼티

    //인스턴스에 어디서나 접근할수 있게 static 프로퍼티 사용
    public static EventManager Instance
    {
        get { return instance; }
        set { }
    }
    //내부 호출 isntacne
    private static EventManager instance = null;
    #endregion
    public delegate void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null);

    //큰항목의 EVENT_TYPE를 리스트로 생성
    private Dictionary<EVENT_TYPE, List<OnEvent>> Listeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();

    //하나만 존재하면 파괴되지 않게 한다.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    //리스트를 추가  리스터 배열에 지정된 리스너 오브젝트를 추가 하는 과정
    public void AddListener(EVENT_TYPE Event_Type, OnEvent Listener)
    {
        //이벤트를 수신할 리스트
        List<OnEvent> ListenList = null;

        //리스트의 키가 존재하는지 검사 존재하면 항목을 추가 한다.
        if (Listeners.TryGetValue(Event_Type, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }
        //존재하지 않는다면 새 항목에 추가 한다.
        ListenList = new List<OnEvent>();
        ListenList.Add(Listener);
        //내부의 리스너 리스트에 추가 
        Listeners.Add(Event_Type, ListenList);
    }

    //이벤트를 리스너에 전달하기 위한 과정
    public void PostNotification(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        //모든 리스너에게 이벤트에 대해 알린다
        //수신할 이벤트 리스너
        List<OnEvent> ListenList = null;
        //이벤트 항목이 없으면 알릴리스너가 없으므로 끝낸다.
        if (!Listeners.TryGetValue(Event_Type, out ListenList))
        {
            return;
        }
        for (int i = 0; i < ListenList.Count; i++)
        {
            //오브젝트가 null이 아니면 인터페이스를 통해서 메세지 보낸다.
            if (!ListenList[i].Equals(null))
            {
                ListenList[i](Event_Type, Sender, Param);
            }
        }
    }


    //이벤트 종류와 리스너 항목을 딕셔너리에서 제거한다.
    public void RemoveEvent(EVENT_TYPE Event_Type)
    {
        Listeners.Remove(Event_Type);

    }

    //딕셔너리에서 쓸모없는 항목을 제거한다/
    public void RemoveRedundancies()
    {
        Dictionary<EVENT_TYPE, List<OnEvent>> TmpListeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();
        //모든 딕셔너리 항목을 순회한다.
        foreach (KeyValuePair<EVENT_TYPE, List<OnEvent>> Item in Listeners)
        {
            //리스트의 모든 오브젝트를 순회하면서 null 오브젝트를 제거한다.
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if (Item.Value[i].Equals(null))
                {
                    Item.Value.RemoveAt(i);
                }
            }

            //알림받기위한 항목만 남으면 항목을 임시딕셔너리에 넣는다.
            if (Item.Value.Count > 0)
            {
                TmpListeners.Add(Item.Key, Item.Value);
            }
        }
        //새로 최적화된 딕서너리로 교체한다.
        Listeners = TmpListeners;
    }

    //씬이 변경될때 호출한다.
    void OnLevelWasLoaded()
    {
        RemoveRedundancies();
    }
}
