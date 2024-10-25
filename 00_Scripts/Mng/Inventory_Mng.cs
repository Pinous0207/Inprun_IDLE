using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Mng
{
    public void GetItem(Item_Scriptable item, int value = 1)
    {
        if(Base_Mng.Data.Item_Holder.ContainsKey(item.name))
        {
            Base_Mng.Data.Item_Holder[item.name].Count += value;
            return;
        }
    }
}
