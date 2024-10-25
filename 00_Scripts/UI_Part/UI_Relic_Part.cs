using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UI_Relic_Part : MonoBehaviour
{
    [SerializeField] private Image m_Slider, m_ItemImage, m_RarityImage;
    [SerializeField] private TextMeshProUGUI m_Level, m_Count;
    [SerializeField] private GameObject GetLock;
    [SerializeField] private Button onClickButton;
    public GameObject LockOBJ;
    [SerializeField] private GameObject Plus, Minus;

    public Item_Scriptable m_Item;
    UI_Relic parent;

    public void LockCheck(bool Lock)
    {
        switch (Lock)
        {
            case true: LockOBJ.SetActive(true); Plus.SetActive(false); Minus.SetActive(true); break;
            case false: LockOBJ.SetActive(false); Plus.SetActive(true); Minus.SetActive(false); break;
        }
    }

    public void Initalize(Item_Scriptable data, UI_Relic parentBASE)
    {
        parent = parentBASE;
        m_Item = data;

        Init();

        m_RarityImage.sprite = Utils.Get_Atlas(data.rarity.ToString());
        m_ItemImage.sprite = Utils.Get_Atlas(data.name);
        m_ItemImage.SetNativeSize();
        RectTransform rect = m_ItemImage.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.3f, rect.sizeDelta.y / 2.3f);

        Get_Item_Check();
    }

    public void Init()
    {
        int levelCount = (Base_Mng.Data.Item_Holder[m_Item.name].Level + 1) * 5;
        m_Slider.fillAmount = (float)Base_Mng.Data.Item_Holder[m_Item.name].Count / (float)levelCount;
        m_Count.text = Base_Mng.Data.Item_Holder[m_Item.name].Count.ToString() + "/" + levelCount.ToString();
        m_Level.text = "LV." + (Base_Mng.Data.Item_Holder[m_Item.name].Level + 1).ToString();
    }

    public void Get_Item_Check()
    {
        bool Get = false;
        for (int i = 0; i < Base_Mng.Character.m_Set_Character.Length; i++)
        {
            if (Base_Mng.Data.m_Set_Item[i] != null)
            {
                if (Base_Mng.Data.m_Set_Item[i] == m_Item)
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
        parent.Set_Click(this);
    }

    //public void Click_My_Hero()
    //{
    //    parent.GetHeroInformation(m_Character);
    //}

}
