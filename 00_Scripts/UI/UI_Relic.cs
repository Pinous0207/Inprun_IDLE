using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Relic : UI_Base
{
    public Transform Content;
    public GameObject Part;
    public List<UI_Relic_Part> part = new List<UI_Relic_Part>();

    Dictionary<string, Item_Scriptable> m_Dictionarys = new Dictionary<string, Item_Scriptable>();
    Item_Scriptable m_Items;

    public GameObject[] RelicPanelObjects;
    public Color[] colors;

    public override bool Init()
    {
        var Datas = Base_Mng.Data.m_Data_Item;
        GetItemCheck();
        foreach (var data in Datas)
        {
            if(data.Value.type == ItemType.Equipment)
                m_Dictionarys.Add(data.Value.name, data.Value);
        }

        var sort_dictionary = m_Dictionarys.OrderByDescending(x => x.Value.rarity);

        int value = 0;

        foreach (var data in sort_dictionary)
        {
            var go = Instantiate(Part, Content).GetComponent<UI_Relic_Part>();
            value++;
            part.Add(go);
            int index = value;
            go.Initalize(data.Value, this);
        }

        return base.Init();
    }
    public void Set_Item_Button(int value)
    {
        Base_Mng.Item.GetItem(value, m_Items.name);

        Initalize();
    }
    public void Initalize()
    {
        Set_Click(null);

        for (int i = 0; i < part.Count; i++) part[i].Get_Item_Check();

        Delegate_Holder.ClearEvent();
        Relic_Mng.instance.Initalize();

        GetItemCheck();

        //Main_UI.instance.Set_Character_Data();
    }

    public void GetItemCheck()
    {
        for(int i = 0; i< RelicPanelObjects.Length; i++)
        {
            if (Base_Mng.Data.m_Set_Item[i] != null)
            {
                RelicPanelObjects[i].GetComponent<Image>().color = colors[(int)Base_Mng.Data.m_Set_Item[i].rarity];
                RelicPanelObjects[i].transform.GetChild(2).gameObject.SetActive(true);
                RelicPanelObjects[i].transform.GetChild(2).GetComponent<Image>().sprite = Utils.Get_Atlas(Base_Mng.Data.m_Set_Item[i].name);
                RelicPanelObjects[i].transform.GetChild(2).GetComponent<Image>().SetNativeSize();
                RelicPanelObjects[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = Base_Mng.Data.m_Set_Item[i].Item_Name;
                RelicPanelObjects[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "LV." + (Base_Mng.Data.Item_Holder[Base_Mng.Data.m_Set_Item[i].name].Level + 1).ToString();
            }
            else
            {
                RelicPanelObjects[i].GetComponent<Image>().color = Color.white;
                RelicPanelObjects[i].transform.GetChild(2).gameObject.SetActive(false);
                RelicPanelObjects[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
                RelicPanelObjects[i].transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }
    public void Set_Click(UI_Relic_Part s_Part)
    {
        if (s_Part == null)
        {
            for (int i = 0; i < part.Count; i++)
            {
                part[i].LockOBJ.SetActive(false);
                part[i].GetComponent<Outline>().enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < Base_Mng.Data.m_Set_Item.Length; i++)
            {
                var data = Base_Mng.Data.m_Set_Item[i];
                if (data != null)
                {
                    if (data == s_Part.m_Item)
                    {
                        Base_Mng.Item.DisableItem(i);
                        Initalize();
                        return;
                    }
                }
            }

            m_Items = s_Part.m_Item;
            for (int i = 0; i < part.Count; i++)
            {
                part[i].LockOBJ.SetActive(true);
                part[i].GetComponent<Outline>().enabled = false;
            }
            s_Part.LockOBJ.SetActive(false);
            s_Part.GetComponent<Outline>().enabled = true;
        }
    }

    public override void DisableOBJ()
    {
        Main_UI.instance.LayerCheck(-1);
        base.DisableOBJ();
    }
}
