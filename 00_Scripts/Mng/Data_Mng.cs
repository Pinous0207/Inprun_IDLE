using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

// 서버에서 관리할 DB(Data base)를 저장하는 용도

[System.Serializable]
public class Percentage
{
    public Rarity rarity;
    public float Min, Max;
}
public class Smelt_Holder
{
    public Status_Holder holder;
    public Rarity rarity;
    public float Value;
}
public class Character_Holder
{
    public Character_Scriptable Data;
    public Holder holder;
}

public class Item_Holder
{
    public Item_Scriptable Data;
    public Holder holder;
}

public class Holder
{
    public int Level;
    public int Count;
}

public class Data
{
    public double Money;
    public double ATK;
    public double HP;
    public int Dia;
    public int Level;
    public int UpgradeCount;
    public int Quest_Count;
    public double EXP;
    public int Stage;

    public float[] Buff_timers = { 0.0f, 0.0f, 0.0f };
    public float Buff_x2 = 0.0f;
    public int BuffLevel, BuffCount;

    // Shop
    public int Hero_Summon_Count;
    public int Hero_PickUp_Count;

    public string StartDate;
    public string EndDate;

    // Dungeon
    public int[] Key = { 2, 2 };
    public int[] KeyAssets = { 0, 0 };

    public int[] Dungeon_Clear_Level  = { 0, 0 };

    public bool ADS_Remove = false;

}
public class Data_Mng
{
    public static Data m_Data = new Data();

    public Dictionary<string, Holder> Character_Holder = new Dictionary<string, Holder>();
    public Dictionary<string, Holder> Item_Holder = new Dictionary<string, Holder>();
    public Dictionary<string, Character_Holder> m_Data_Character = new Dictionary<string, Character_Holder>(); // 플레이어가 가지고 있는 캐릭터를 저장
    public Dictionary<string, Item_Scriptable> m_Data_Item = new Dictionary<string , Item_Scriptable>();
    public List<Smelt_Holder> m_Data_Smelt = new List<Smelt_Holder>();

    public Item_Scriptable[] m_Set_Item = new Item_Scriptable[6];

    public void Init()
    {
        Set_Character();
        Set_Item();
    }

    public Character_Scriptable Get_Rarity_Character(Rarity rarity)
    {
        List<Character_Scriptable> holder = new List<Character_Scriptable>();
        foreach(var data in m_Data_Character)
        {
            if(data.Value.Data.m_Rarity == rarity && data.Value.Data.Main_Character == false)
            {
                holder.Add(data.Value.Data);
            }
        }
        return holder[Random.Range(0, holder.Count)];
    }

    private void Set_Character()
    {
        var datas = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");

        foreach (var data in datas)
        {
            var character = new Character_Holder();

            character.Data = data;
            Holder s_holder = new Holder();
            if (Character_Holder.ContainsKey(data.m_Character_Name))
            {
                s_holder = Character_Holder[data.m_Character_Name];
            }
            else
            {
                Character_Holder.Add(data.m_Character_Name, s_holder);
            }
            character.holder = s_holder;

            m_Data_Character.Add(data.m_Character_Name, character);
        }
    }

    private void Set_Item()
    {
        var datas = Resources.LoadAll<Item_Scriptable>("Scriptable/Item");

        foreach (var data in datas)
        {
            var item = new Item_Scriptable();

            item = data;

            m_Data_Item.Add(data.name, item);
        }
    }

    public float valueSmelt(Status_Holder status)
    {
        float value = 0.0f;
        for(int i = 0; i < m_Data_Smelt.Count; i++)
        {
            if (m_Data_Smelt[i].holder == status)
            {
                value += m_Data_Smelt[i].Value;
            }
        }
        return value;
    }
}
