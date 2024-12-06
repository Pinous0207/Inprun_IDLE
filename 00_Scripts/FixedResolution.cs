using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �ػ� -> ���� ���� ���� ���� ���ӿ����� ���X
public class FixedResolution : MonoBehaviour
{
    public int targetWidth = 1440; // ���� �ػ��� �ʺ�
    public int targetHeight = 2560; // ���� �ػ��� ����
    private void Start()
    {
        ApplyFixedResoultion();
    }

    private void ApplyFixedResoultion()
    {
        float targetAspect = (float)targetWidth / targetHeight; // ��ǥ ȭ�� ����
        float windowAspect = (float)Screen.width / Screen.height; // ���� ȭ�� ����
        float scaleHeight = windowAspect / targetAspect;

        Camera mainCamera = Camera.main;

        if(scaleHeight < 1.0f) // ���� ������ ���Ʒ��� ����
        {
            Rect rect = mainCamera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            mainCamera.rect = rect;
        }
        else // ���� ������ �¿�� ����
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

    // ����Ƽ �����Ϳ��� �� ����� �ٷ� ����
    private void OnValidate()
    {
        if(Application.isPlaying)
        {
            ApplyFixedResoultion();
        }
    }
}
