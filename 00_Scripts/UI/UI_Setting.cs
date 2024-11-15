using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Setting : UI_Base
{
    public Slider BGM, VFX;
    public TextMeshProUGUI UniqueID;
    public Image CheckBox;
    public GameObject ChangeLanguagePanel;
    public TextMeshProUGUI NoneChangeText, GetChangeText;
    string saveLang;
    public void GetChangeLanguage(string lang)
    {
        saveLang = lang;
        ChangeLanguagePanel.SetActive(true);
        NoneChangeText.text = Local_Mng.local_Data["UI/ChangeLanguage"].Get_Data();
        GetChangeText.text = Local_Mng.local_Data["UI/ChangeLanguage"].Get_Data(lang);
    }

    public void Yes()
    {
        PlayerPrefs.SetString("LOCAL", saveLang);
        Application.Quit();
    }

    public override bool Init()
    {
        UniqueID.text = "UNIQUE ID : " + Base_Mng.Firebase.currentUser.UserId;

        BGM.value = Base_Mng.Sound.BGMValue;
        VFX.value = Base_Mng.Sound.VFXValue;
        
        CameraShakeCheck();

        return base.Init();
    }

    private bool CameraShakeCheck()
    {
        bool checkBox = PlayerPrefs.GetInt("CAM") == 0 ? true : false;
        CheckBox.gameObject.SetActive(checkBox);
        return checkBox;
    }

    public void CameraShakeButton()
    {
        PlayerPrefs.SetInt("CAM", CameraShakeCheck() == true ? 1 : 0);
        CameraShakeCheck();
    }

  

    public void ApplicationURL(string url)
    {
        Application.OpenURL(url);
    }

    public void GetUniqueClipboard()
    {
        GUIUtility.systemCopyBuffer = Base_Mng.Firebase.currentUser.UserId;
    }

    private void Update()
    {
        Base_Mng.Sound.BGMValue = BGM.value;
        Base_Mng.Sound._audioSource[0].volume = BGM.value;
        Base_Mng.Sound.VFXValue = VFX.value;
        Base_Mng.Sound._audioSource[1].volume = VFX.value;
    }

    public override void DisableOBJ()
    {
        PlayerPrefs.SetFloat("BGM", BGM.value);
        PlayerPrefs.SetFloat("VFX", VFX.value);
        base.DisableOBJ();
    }
}
