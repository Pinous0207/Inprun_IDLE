using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UI_Heroes_Part : MonoBehaviour
{
    [SerializeField] private Image m_Slider, m_CharacterImage, m_RarityImage;
    [SerializeField] private TextMeshProUGUI m_Level, m_Count;
    [SerializeField] private GameObject GetLock;
    [SerializeField] private Button onClickButton;
    public GameObject LockOBJ;
    [SerializeField] private GameObject Plus, Minus;

    public Character_Scriptable m_Character;
    UI_Heroes parent;

    public void LockCheck(bool Lock)
    {
        switch(Lock)
        {
            case true: LockOBJ.SetActive(true); Plus.SetActive(false); Minus.SetActive(true); break;
            case false: LockOBJ.SetActive(false); Plus.SetActive(true); Minus.SetActive(false); break;  
        }
    }
    
    public void Initalize(Character_Scriptable data, UI_Heroes parentBASE)
    {
        parent = parentBASE;
        m_Character = data;

        Init();

        m_RarityImage.sprite = Utils.Get_Atlas(data.m_Rarity.ToString());
        m_CharacterImage.sprite = Utils.Get_Atlas(data.m_Character_Name);
        m_CharacterImage.SetNativeSize();
        RectTransform rect = m_CharacterImage.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.3f, rect.sizeDelta.y / 2.3f);

        Get_Character_Check();
    }

    public void Init()
    {
        int levelCount = (Base_Mng.Data.Character_Holder[m_Character.name].Level + 1) * 5;
        m_Slider.fillAmount = (float)Base_Mng.Data.Character_Holder[m_Character.name].Count / (float)levelCount;
        m_Count.text = Base_Mng.Data.Character_Holder[m_Character.name].Count.ToString() + "/" + levelCount.ToString();
        m_Level.text = "LV." + (Base_Mng.Data.Character_Holder[m_Character.name].Level + 1).ToString();
    }

    public void Get_Character_Check()
    {
        bool Get = false;
        for(int i = 0; i< Base_Mng.Character.m_Set_Character.Length; i++)
        {
            if (Base_Mng.Character.m_Set_Character[i] != null)
            {
                if (Base_Mng.Character.m_Set_Character[i].Data == m_Character)
                {
                    Get = true;
                }
            }
        }
        GetLock.SetActive(Get);
        Plus.SetActive(!Get);
        Minus.SetActive(Get);
    }

    public void Click_My_Button()
    {
        Render_Manager.instance.HERO.GetParticle(true);
        parent.Set_Click(this);
    }

    public void Click_My_Hero()
    {
        parent.GetHeroInformation(m_Character);
    }
}
