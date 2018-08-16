using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utils_Proto
    : MonoBehaviour {

    //stati 함수로 집접접근해서 사용할수 있게 한다.

    static private Bounds _camBounds;

    public static Bounds camBounds
    {
        get
        {
            if (_camBounds.size == Vector3.zero)
            {

            }
            return (_camBounds);
        }
    }



    public static void SetCamBounds(Camera cam = null)
    {
        //카메라가 없을때메인을 바운드 한다.
        if (cam == null)
        {
           cam =  Camera.main;
        }
        // 바운딩박스 의 좌측상단 0.0.0
        Vector3 topLeft = new Vector3(0, 0, 0);
        //바운딩 박스의 우측하단 은 카메라의 크기로 설정
        Vector3 bottomRight = new Vector3(Screen.width, Screen.height, 0);
        //스크린 좌표를 월드 좌표로 변환
        Vector3 boundTLN = cam.ScreenToViewportPoint(topLeft);
        Vector3 boundBRF = cam.ScreenToViewportPoint(bottomRight);
        //z값으로 카메라와의 거리를 나타나게 한다.
        boundTLN.z += cam.nearClipPlane;
        boundBRF.z += cam.farClipPlane;
        //카메라 바운딩의 센터를 정한다.
        Vector3 center = (boundTLN + boundBRF) / 2f;
        _camBounds = new Bounds(center, Vector3.zero);
        //입력한 좌표가 포함되도록 카메라 바운딩 영역설정
        _camBounds.Encapsulate(boundTLN);
        _camBounds.Encapsulate(boundBRF);
    }

    // 선형 보간
    public static void LinearInterpolation(out float x, out float y,
         float fromX,  float fromY,  float toX,  float toY, float t)
    {
        x = fromX * (1.0f - t) + toX * t;
        y = fromY * (1.0f - t) + toY * t;
    }

    // 베지어 곡선
    public static void BezierInterpolation2(out float x, out float y,
         float fromX,  float fromY,  float viaX,  float viaY,  float toX,  float toY,  float t)
    {
        float x1, y1, x2, y2, x3, y3;

        float viaX2 = viaX;
        float viaY2 = fromY + fromY - viaY;

        LinearInterpolation(out x1,out  y1, fromX, fromY, viaX, viaY, t);
        LinearInterpolation(out x2,out  y2, viaX, viaY, viaX2, viaY2, t);
        LinearInterpolation(out x3, out y3, viaX2, viaY2, toX, toY, t);

        BezierInterpolation(out x, out y, x1, y1, x2, y2, x3, y3, t);
    }

    // 베지어 곡선
    public static void BezierInterpolation(out float x, out float y,
         float fromX,  float fromY,  float viaX,  float viaY, float toX,  float toY, float t)
    {
        float x1, y1, x2, y2;

        //x1 = fromX * (1.0f - t) + viaX * t;
        //y1 = fromY * (1.0f - t) + viaY * t;
        LinearInterpolation(out x1, out y1, fromX, fromY, viaX, viaY, t);

        //x2 = viaX * (1.0f - t) + toX * t;
        //y2 = viaY * (1.0f - t) + toY * t;
        LinearInterpolation(out x2,out  y2, viaX, viaY, toX, toY, t);

        //x = x1 * (1.0f - t) + x2 * t;
        //y = y1 * (1.0f - t) + y2 * t;
        LinearInterpolation(out x,out  y, x1, y1, x2, y2, t);
    }



    /// <summary>
    /// /////추가할 메소드 
    /// 
    /// </summary>

    //// 두 점 사이의 각도 구하는 함수 (1: 시작점, 2: 도착점)
    //inline float GetAngle(float x1, float y1, float x2, float y2)
    //{
    //    float fLengthDiagonal = GetLength(x1, y1, x2, y2);

    //    if (fLengthDiagonal < FLT_EPSILON) return 0;

    //    float fAngle = acosf((x2 - x1) / fLengthDiagonal) / PI * 180.0f;

    //    // 각도 예외 처리
    //    if (y1 < y2)
    //    {
    //        fAngle = 360.0f - fAngle;

    //        if (fAngle >= 360.0f)
    //            fAngle -= 360.0f;
    //    }

    //    return fAngle;
    //}
    //#define FLT_EPSILON      1.192092896e-07F        // smallest such that 1.0+FLT_EPSILON != 1.0




    //// 선형 보간
    //inline void LinearInterpolation(OUT float& x, OUT float& y,
    //    IN float fromX, IN float fromY, IN float toX, IN float toY, IN float t)
    //{
    //    x = fromX * (1.0f - t) + toX * t;
    //    y = fromY * (1.0f - t) + toY * t;
    //}

    //// 베지어 곡선
    //inline void BezierInterpolation(OUT float& x, OUT float& y,
    //    IN float fromX, IN float fromY, IN float viaX, IN float viaY, IN float toX, IN float toY, IN float t)
    //{
    //    float x1, y1, x2, y2;

    //    //x1 = fromX * (1.0f - t) + viaX * t;
    //    //y1 = fromY * (1.0f - t) + viaY * t;
    //    LinearInterpolation(x1, y1, fromX, fromY, viaX, viaY, t);

    //    //x2 = viaX * (1.0f - t) + toX * t;
    //    //y2 = viaY * (1.0f - t) + toY * t;
    //    LinearInterpolation(x2, y2, viaX, viaY, toX, toY, t);

    //    //x = x1 * (1.0f - t) + x2 * t;
    //    //y = y1 * (1.0f - t) + y2 * t;
    //    LinearInterpolation(x, y, x1, y1, x2, y2, t);
    //}

    //// 베지어 곡선
    //inline void BezierInterpolation2(OUT float& x, OUT float& y,
    //    IN float fromX, IN float fromY, IN float viaX, IN float viaY, IN float toX, IN float toY, IN float t)
    //{
    //    float x1, y1, x2, y2, x3, y3;

    //    float viaX2 = viaX;
    //    float viaY2 = fromY + fromY - viaY;

    //    LinearInterpolation(x1, y1, fromX, fromY, viaX, viaY, t);
    //    LinearInterpolation(x2, y2, viaX, viaY, viaX2, viaY2, t);
    //    LinearInterpolation(x3, y3, viaX2, viaY2, toX, toY, t);

    //    BezierInterpolation(x, y, x1, y1, x2, y2, x3, y3, t);
    //}


}
