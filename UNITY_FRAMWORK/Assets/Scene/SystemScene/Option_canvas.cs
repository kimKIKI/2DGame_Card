using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option_canvas : MonoBehaviour {

    Transform OptionSound;
    GameObject[] sounds;                    //옵션에서 전체 사운드소리조절
    Slider soundsSliderBGM;
    Slider soundsSliderSE;
  
    Text bgmValue;                          //sound bgm value; 배경음
    Text seValue;                           //sound se value;   효과음
    float bgmT;                             //sound BGM 전역
    float seT;

    Button CloseBtn;                        //창닫기

    private void Awake()
    {
        OptionSound = transform.Find("OptionSound").transform;
        sounds = GameObject.FindGameObjectsWithTag("Sound");
      
        for (int i = 0; i < sounds.Length; ++i)
        {
         
            if (sounds[i].gameObject.name == "Slider_BGM")
            {
                Debug.Log("333333333333333333333333333333");
                soundsSliderBGM = sounds[i].transform.GetComponent<Slider>();
                bgmValue = sounds[i].transform.Find("Tx_Value").GetComponent<Text>();

            }
            else if (sounds[i].gameObject.name == "Slider_SE")
            {
                Debug.Log("333333333333333333333333333333");
                soundsSliderSE = sounds[i].transform.GetComponent<Slider>();
                seValue = sounds[i].transform.Find("Tx_Value").GetComponent<Text>();
            }
        }

        soundsSliderBGM.onValueChanged.AddListener(delegate { ValueChangeCheckBGM(); });

        soundsSliderSE.onValueChanged.AddListener(delegate { ValueChangeCheckSE(); });


        CloseBtn = transform.Find("OptionSound/Button_Close").GetComponent<Button>();

        CloseBtn.onClick.AddListener(delegate
        {
            OptionSound.gameObject.SetActive(false);
        });

    }

    private void Start()
    {
        soundsSliderBGM.value = SaveData.SoundBGMVolume;
        soundsSliderSE.value = SaveData.SoundSEVolume;
        OptionSound.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        soundsSliderBGM.value = SaveData.SoundBGMVolume;
        soundsSliderSE.value  = SaveData.SoundSEVolume;
    }

    private void OnDisable()
    {
        SaveData.SoundBGMVolume = bgmT;
        SaveData.SoundSEVolume = seT;
    }

    
    public void ValueChangeCheckBGM()
    {

        float bgm = soundsSliderBGM.value;
        AppSound.instance.fm.SetVolume("BGM", bgm);
        bgmValue.text = string.Format("{0}", bgm);
        bgmT = bgm;
    }

    public void ValueChangeCheckSE()
    {

        float se = soundsSliderSE.value;
        AppSound.instance.fm.SetVolume("SE", se);
        seValue.text = string.Format("{0}", se);
        seT = se;
    }

    public void OptionButton()
    {
        Debug.Log("BBBBBBBBBBBBBBBBBBBB");
        OptionSound.gameObject.SetActive(true);
    }

}
