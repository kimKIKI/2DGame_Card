using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login_Scene : MonoBehaviour {

    public Text name;
    [SerializeField] InputField inputField;
    private string PlayerName;

    private void Start()
    {
        //자체 이벤트에 등록
        inputField.onValueChange.AddListener(OnValueChange);
        inputField.onEndEdit.AddListener(OnSubmit);

        //inputField에 직접접근 
        inputField.onValidateInput = (string text, int charindex, char addedChar)
        =>
        {
            //대문자로 변환하는 과정
            char ret = addedChar;
            if (addedChar >= 'a' && addedChar <= 'z')
            {
                ret = (char)(addedChar + ('A' - 'a'));
            }
            return ret;
        };
    }

    public void OnValueChange(string value)
    {
       
        PlayerName = value;
    }

    public void OnSubmit(string value)
    {
       
        PlayerName = value;
        // SaveData.playerName = value;
        //string coverdName = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(PlayerName));
       
    }
    

    public void OnInputFieldTextChanged(string newText)
    {

    }

    public void OnInputFieldTextDone(string newText)
    {

    }
    void OnClick()
    {
        string coverdName = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(name.text));
    }
}
