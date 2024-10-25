using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Offline : UI_Base
{
    double moneyValue;
    [SerializeField] private TextMeshProUGUI moneyCount;
    [SerializeField] private TextMeshProUGUI TimerText;

    [SerializeField] private Transform Content;
    [SerializeField] private UI_Inventory_Part Item;
    Dictionary<string, Item_Holder> items = new Dictionary<string, Item_Holder>();

    public override bool Init()
    {
        moneyValue = (Utils.Data.stageData.MONEY() * Utils.TimerCheck()) / 3;
        moneyCount.text = StringMethod.ToCurrencyString(moneyValue);

        TimeSpan span = TimeSpan.FromSeconds(Utils.TimerCheck());
        TimerText.text = span.Hours + ":" + span.Minutes;

        Item_Collect();
        
        foreach(var item in items)
        {
            var go = Instantiate(Item, Content);
            go.Init(item.Key, item.Value.holder);
        }

        return base.Init();
    }

    private void Item_Collect()
    {
        int value = (int)Utils.TimerCheck() / 3;

        for (int i = 0; i < value; i++) // i
        {
            var GetItem = Base_Mng.Item.GetDropSet();

            for (int j = 0; j < GetItem.Count; j++) // j
            {
                if (items.ContainsKey(GetItem[j].name))
                {
                    Debug.Log(GetItem[j]);
                    items[GetItem[j].name].holder.Count++;
                }
                else
                {
                    var ItemData = new Item_Holder();
                    ItemData.Data = GetItem[j];
                    ItemData.holder = new Holder();
                    ItemData.holder.Count = 1;
                    items.Add(GetItem[j].name, ItemData);
                }
            }
        }
    }

    public void CollectButton()
    {
        Data_Mng.m_Data.Money += moneyValue;
        foreach(var item in items)
        {
            Base_Mng.Inventory.GetItem(item.Value.Data, item.Value.holder.Count);
        }
        Main_UI.instance.TextCheck();
        DisableOBJ();
    }

    public void ADSCollectButton()
    {
        Base_Mng.ADS.ShowRewardedAds(() =>
        {
            Data_Mng.m_Data.Money += (moneyValue * 2);
            foreach (var item in items)
            {
                Base_Mng.Inventory.GetItem(item.Value.Data, item.Value.holder.Count * 2);
            }
            Main_UI.instance.TextCheck();
            DisableOBJ();
        });
    }

    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }
}
