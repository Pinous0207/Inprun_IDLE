using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class LoadingScene : MonoBehaviour
{
    public static LoadingScene instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    GameObject sliderParent;
    public Slider slider;
    public TextMeshProUGUI versionText;
    public TextMeshProUGUI PercentageText;
    public GameObject TapToStartOBJ;
    private AsyncOperation asyncOperation;

    private void Start()
    {
        versionText.text = "App Version." + Application.version;
        sliderParent = slider.transform.parent.gameObject;
    }

    private void Update()
    {
        if(asyncOperation != null)
        if(asyncOperation.progress >= 0.9f && Input.GetMouseButtonDown(0))
        {
            asyncOperation.allowSceneActivation = true;
            Base_Mng.GetGameStart = true;
        }
    }

    IEnumerator LoadDataCoroutine()
    {
        // SceneManager.LoadScene - �ﰢ������ ���� �̵�
        // SceneManager.LoadSceneAsync - �̸� �ε��� ���Ŀ� �ε尡 ������� �Ϸ�Ǿ��� �� ���� �̵�

        asyncOperation = SceneManager.LoadSceneAsync("Main");
        asyncOperation.allowSceneActivation = false; // �� �ڵ� ��ȯ ����

        while(asyncOperation.progress < 0.9f)
        {
            LoadingUpdate(asyncOperation.progress);
            yield return null;
        }
        LoadingUpdate(1.0f);
        TapToStartOBJ.SetActive(true);
    }

    public void LoadingMain()
    {
        StartCoroutine(LoadDataCoroutine());
    }

    private void LoadingUpdate(float progress)
    {
        slider.value = progress;
        PercentageText.text = string.Format("�����͸� �������� �ֽ��ϴ�..<color=#FFFF00>{0}%", progress * 100.0f);
    }
}
