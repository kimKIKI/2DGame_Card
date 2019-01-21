using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login_Scene : MonoBehaviour {

    //public Text name;
    //[SerializeField] InputField inputField;
    //private string PlayerName;

    //private void Start()
    //{
    //    //자체 이벤트에 등록
    //    //inputField.onValueChange.AddListener(OnValueChange);
    //    inputField.onEndEdit.AddListener(OnSubmit);

    //    //inputField에 직접접근 
    //    inputField.onValidateInput = (string text, int charindex, char addedChar)
    //    =>
    //    {
    //        //대문자로 변환하는 과정
    //        char ret = addedChar;
    //        if (addedChar >= 'a' && addedChar <= 'z')
    //        {
    //            ret = (char)(addedChar + ('A' - 'a'));
    //        }
    //        return ret;
    //    };
    //}

    //public void OnValueChange(string value)
    //{
       
    //    PlayerName = value;
    //}

    ////save 파일에 직접데이터이름...보류
    //public void OnSubmit(string value)
    //{
       
    //    PlayerName = value;
    //    // SaveData.playerName = value;
    //    //string coverdName = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(PlayerName));
    //}

    //playerPref에 저장되고 이미 있다면 데이터를 이어서 하게 한다.
    //public void GuestLogin()
    //{

    //}

    public void Login()
    {
        //세이브와 동시에 게임저장이 시작됨
        SceneManager.LoadScene("2_Main_Scene");
    }

    //public void OnInputFieldTextChanged(string newText)
    //{

    //}

    //public void OnInputFieldTextDone(string newText)
    //{

    //}
    //void OnClick()
    //{
    //    string coverdName = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(name.text));
    //}
}
