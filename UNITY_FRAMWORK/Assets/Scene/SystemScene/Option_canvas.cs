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
    bool IsPause;                           //Pause
    Button CloseBtn;                        //창닫기
    Button PauseBtn;
    Button ResumBtn;

    private void Awake()
    {
        OptionSound = transform.Find("OptionPanel").transform;
        sounds = GameObject.FindGameObjectsWithTag("Sound");

      

        for (int i = 0; i < sounds.Length; ++i)
        {
         
            if (sounds[i].gameObject.name == "Slider_BGM")
            {
               
                soundsSliderBGM = sounds[i].transform.GetComponent<Slider>();
                bgmValue = sounds[i].transform.Find("Tx_Value").GetComponent<Text>();

            }
            else if (sounds[i].gameObject.name == "Slider_SE")
            {
              
                soundsSliderSE = sounds[i].transform.GetComponent<Slider>();
                seValue = sounds[i].transform.Find("Tx_Value").GetComponent<Text>();
            }
        }

        soundsSliderBGM.onValueChanged.AddListener(delegate { ValueChangeCheckBGM(); });

        soundsSliderSE.onValueChanged.AddListener(delegate { ValueChangeCheckSE(); });


        CloseBtn = transform.Find("OptionPanel/Button_Close").GetComponent<Button>();
        PauseBtn = transform.Find("OptionPanel/Btn_Pause").GetComponent<Button>();
        ResumBtn = transform.Find("OptionPanel/Btn_Resum").GetComponent<Button>();

        CloseBtn.onClick.AddListener(delegate
        {
            IsPause = false;
            Pause();
            OptionSound.gameObject.SetActive(false);
        });

        PauseBtn.onClick.AddListener(delegate
        {
            IsPause = !IsPause;
            Pause();
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
        ResumBtn.gameObject.SetActive(false);
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
        
        OptionSound.gameObject.SetActive(true);
    }

    private void Pause()
    {
        if (IsPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
      
    }

    public void PauseA()
    {
        IsPause = true;
        Pause();
    }

    //외부에서 옵션창을 닫을수 있도록한다.
    public void Option_canvasClose()
    {
        OptionSound.gameObject.SetActive(false);
    }

}
