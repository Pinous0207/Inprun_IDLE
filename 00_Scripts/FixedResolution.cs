using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 고정 해상도 -> 지금 현재 저희가 만든 게임에서는 사용X
public class FixedResolution : MonoBehaviour
{
    public int targetWidth = 1440; // 고정 해상도의 너비
    public int targetHeight = 2560; // 고정 해상도의 높이
    private void Start()
    {
        ApplyFixedResoultion();
    }

    private void ApplyFixedResoultion()
    {
        float targetAspect = (float)targetWidth / targetHeight; // 목표 화면 비율
        float windowAspect = (float)Screen.width / Screen.height; // 현재 화면 비율
        float scaleHeight = windowAspect / targetAspect;

        Camera mainCamera = Camera.main;

        if(scaleHeight < 1.0f) // 검은 여백이 위아래로 생김
        {
            Rect rect = mainCamera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            mainCamera.rect = rect;
        }
        else // 검은 여백이 좌우로 생김
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = mainCamera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            mainCamera.rect = rect;
        }
        Screen.SetResolution(targetWidth, targetHeight, true);
    }

    // 유니티 에디터에서 값 변경시 바로 적용
    private void OnValidate()
    {
        if(Application.isPlaying)
        {
            ApplyFixedResoultion();
        }
    }
}
