using UnityEngine;



public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
           

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType(typeof(T)) as T;

            if (_instance == null)// 해당 클래스를 찾았는데도 널이면 에러
            {
                Debug.Log("There's no active" + typeof(T) + "in this scene");
            }
        }
        DontDestroyOnLoad(_instance.gameObject);
    }
}