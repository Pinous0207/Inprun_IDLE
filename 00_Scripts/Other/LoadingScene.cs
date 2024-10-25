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
        // SceneManager.LoadScene - 즉각적으로 씬을 이동
        // SceneManager.LoadSceneAsync - 미리 로드한 이후에 로드가 어느정도 완료되었을 때 씬을 이동

        asyncOperation = SceneManager.LoadSceneAsync("Main");
        asyncOperation.allowSceneActivation = false; // 씬 자동 전환 방지

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
        PercentageText.text = string.Format("데이터를 가져오고 있습니다..<color=#FFFF00>{0}%", progress * 100.0f);
    }
}
