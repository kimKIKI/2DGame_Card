using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public GameObject target;       // 따라다닐 타겟 오브젝트

    private UILabel uiLabel;        // 네임 라벨
    private Camera targetCamera;    // 타겟 기준의 카메라
    private Camera uiCamera;        // UI 카메라

    private int nFontSize = 140;          // 폰트 크기
    private Vector3 uiPos;          // UI 위치

    private void Awake()
    {
        uiLabel = this.GetComponent<UILabel>();
        //targetCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        targetCamera = Camera.main;
        uiCamera = this.GetComponentInParent<Camera>();
    }

    private void Update()
    {
        // 대상의 위치(월드)를 화면상의 위치로 계산한다.
        Vector3 screenPos = targetCamera.WorldToScreenPoint(target.transform.position);

        // 타겟이 카메라의 정면에 있을 때만
        if (screenPos.z >= 0.0f)
        {
            // UI의 위치를 캐릭터의 스크린 상의 위치를 사용해서 계산한다.
            uiPos = uiCamera.ScreenToWorldPoint(screenPos);

            // 거리에 따른 폰트 사이즈 변경
            uiLabel.fontSize = nFontSize - (int)(uiPos.z * 10.0f);

            uiPos.z = 0.0f; // UI의 z는 0으로 고정
            this.transform.position = uiPos;
        }
    }
}
