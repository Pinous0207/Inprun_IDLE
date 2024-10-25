using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Main_Part : MonoBehaviour
{
    [SerializeField] private GameObject Lock, Plus;
    [SerializeField] private Image Icon, FillImage;
    [SerializeField] private TextMeshProUGUI HP, MP;
    [SerializeField] private GameObject GetReadyCharacter;

    Character_Scriptable m_Data = null;

    private void Start()
    {
        Initalize();
    }

    public void Initalize()
    {
        if(m_Data == null)
        {
            HP.gameObject.SetActive(false);
            FillImage.transform.parent.gameObject.SetActive(false);
            Plus.gameObject.SetActive(true);
            Icon.gameObject.SetActive(false);
        }
        else
        {
            InitData(m_Data, true);
        }
    }

    public void Get_Hero_Set_PopUp()
    {
        Base_Canvas.instance.Get_UI("#Heros");
    }

    public void InitData(Character_Scriptable data, bool Ready)
    {
        m_Data = data;
        
        Lock.SetActive(false);
        Plus.SetActive(false);
        Icon.gameObject.SetActive(true);

        HP.gameObject.SetActive(!Ready);
        FillImage.transform.parent.gameObject.SetActive(!Ready);

        GetReadyCharacter.SetActive(Ready);
        
        Icon.sprite = Utils.Get_Atlas(data.m_Character_Name);
        Icon.SetNativeSize();
        RectTransform rect = Icon.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x / 2, rect.sizeDelta.y / 2);
    }

    public void StateCheck(Player player)
    {
        FillImage.fillAmount = (float)player.MP / (float)m_Data.MaxMP;
        HP.text = StringMethod.ToCurrencyString(player.HP);
        MP.text = player.MP.ToString() + "/" + m_Data.MaxMP;
    }
}
