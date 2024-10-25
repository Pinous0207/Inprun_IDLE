using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Mng
{
    public bool SetItemCheck(string name)
    {
        for (int i = 0; i < Base_Mng.Data.m_Set_Item.Length; i++)
        {
            if (Base_Mng.Data.m_Set_Item[i] != null)
            {
                if (Base_Mng.Data.m_Set_Item[i].name == name)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void GetItem(int value, string item_name)
    {
        Base_Mng.Data.m_Set_Item[value] = Base_Mng.Data.m_Data_Item[item_name];
    }

    public void DisableItem(int value)
    {
        Base_Mng.Data.m_Set_Item[value] = null;
    }

    public int LevelCount(Item_Holder holder)
    {
        return (holder.holder.Level + 1) * 5;
    }
    public List<Item_Scriptable> GetDropSet()
    {
        List<Item_Scriptable> objs = new List<Item_Scriptable>();

        foreach(var data in Base_Mng.Data.m_Data_Item)
        {
            if (data.Value.MinLevel <= Data_Mng.m_Data.Stage)
            {
                float valueCount = Random.Range(0.0f, 100.0f);
                if (valueCount <= data.Value.Item_Chance)
                {
                    objs.Add(data.Value);
                }
            }
        }

        return objs;
    }
}
