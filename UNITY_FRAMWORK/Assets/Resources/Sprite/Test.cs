using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {


    [SerializeField] private Image iconImage;	// 아이콘을 표시할 이미지
    string sprName = "UI";
    string path = "Sprite";
    private void Start()
    {
        SpriteMa.Load(path);
        UpImage();
    }

    public void UpImage()
    {
        iconImage.sprite = SpriteMa.GetSpriteByName(path, sprName);
        
        
    }

			                


}
