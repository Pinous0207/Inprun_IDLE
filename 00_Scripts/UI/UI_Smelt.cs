using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class UI_Smelt : UI_Base
{
    Smelt_Scriptable smelt_Data;
    public GameObject Smelt_Panel;
    public Transform Content;
    bool Opening = false;
    public TextMeshProUGUI CountText;
    [HideInInspector] public List<GameObject> Gorvage = new List<GameObject>();

    public override bool Init()
    {
        smelt_Data = Resources.Load<Smelt_Scriptable>("Scriptable/Smelt_Data");

        if (Base_Mng.Data.m_Data_Smelt.Count > 0)
        {
            for (int i = 0; i < Base_Mng.Data.m_Data_Smelt.Count; i++)
            {
                GetSmelt_Panel(
                      (int)Base_Mng.Data.m_Data_Smelt[i].rarity,
                      Base_Mng.Data.m_Data_Smelt[i].holder,
                      Base_Mng.Data.m_Data_Smelt[i].Value
                      );
            }
        }

        CountCheck();

        return base.Init();
    }

    public void Smelt_Change()
    {
        if (Opening) return;
        if (!Utils.Item_Count("Monster_Energy", 100))
        {
            return;
        }

        Base_Mng.Data.Item_Holder["Monster_Energy"].Count -= 100;
        CountCheck();
        Opening = true;

        if(Gorvage.Count > 0)
        {
            for (int i = 0; i < Gorvage.Count; i++) Destroy(Gorvage[i]);
            Gorvage.Clear();
        }

        StartCoroutine(OpenCoroutine((CountPercentage())));
    }

    IEnumerator OpenCoroutine(int count)
    {
        Base_Mng.Data.m_Data_Smelt.Clear();

        for (int i = 0; i < count; i++)
        {
            Status_Holder status = (Status_Holder)Random.Range(0, 8); // 0~7
            int value = CountPercentage();
            float valueCount = Random.Range(StatusHolder(status)[value].Min, StatusHolder(status)[value].Max);

            Base_Mng.Data.m_Data_Smelt.Add(new Smelt_Holder { rarity = (Rarity)value, holder = status, Value = valueCount});

            GetSmelt_Panel(value, status, valueCount);

            yield return new WaitForSeconds(0.1f);
        }

        for(int i = 0; i < Spawner.m_Players.Count; i++)
        {
            Spawner.m_Players[i].Set_ATKHP();
        }

        Opening = false;
    }

    private void CountCheck()
    {
        CountText.text = string.Format("({0}/{1})", Base_Mng.Data.Item_Holder["Monster_Energy"].Count, 100);
        CountText.color = Utils.Item_Count("Monster_Energy", 100) ? Color.green : Color.red;
    }

    private void GetSmelt_Panel(int rarityValue, Status_Holder status, float valueCount)
    {
        var go = Instantiate(Smelt_Panel, Content);
        Gorvage.Add(go);
        go.SetActive(true);
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Utils.String_Color_Rarity((Rarity)rarityValue) + StatusString(status);
        go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Utils.String_Color_Rarity((Rarity)rarityValue) + string.Format("{0:0.00}%", valueCount);
        go.GetComponent<Animator>().SetTrigger("Open");
    }

    private int CountPercentage()
    {
        int RandomCount = Random.Range(0, 100);
        float RandomValue = 0.0f;
        int count = 0;
        for (int i = 0; i < smelt_Data.Count_Value.Length; i++)
        {
            RandomValue += smelt_Data.Count_Value[i];
            if (RandomValue >= RandomCount)
            {
                count = i + 1;
                return count;
            }
        }
        return -1;
    }

    private string StatusString(Status_Holder holder)
    {
        switch(holder)
        {
            case Status_Holder.ATK: return "공격력 증가";
            case Status_Holder.HP: return "HP 증가";
            case Status_Holder.MONEY: return "골드 드랍률 증가";
            case Status_Holder.ITEM: return "아이템 드랍률 증가";
            case Status_Holder.SKILL: return "스킬 쿨타임 감소";
            case Status_Holder.ATK_SPEED: return "공격 속도 증가";
            case Status_Holder.CRITICAL_P: return "치명타 확률 증가";
            case Status_Holder.CRITICAL_D: return "치명타 데미지 증가";
        }
        return "";
    }
    
    private Percentage[] StatusHolder(Status_Holder holder)
    {
        switch (holder)
        {
            case Status_Holder.ATK: return smelt_Data.ATK_percentage;
            case Status_Holder.HP: return smelt_Data.HP_percentage;
            case Status_Holder.MONEY: return smelt_Data.MONEY_percentage;
            case Status_Holder.ITEM: return smelt_Data.ITEM_percentage;
            case Status_Holder.SKILL: return smelt_Data.SKILL_percentage;
            case Status_Holder.ATK_SPEED: return smelt_Data.ATK_SPEED_percentage;
            case Status_Holder.CRITICAL_P: return smelt_Data.CRITICAL_P_percentage;
            case Status_Holder.CRITICAL_D: return smelt_Data.CRITICAL_D_percentage;
        }
        return null;
    }

    public override void DisableOBJ()
    {
        Main_UI.instance.LayerCheck(-1);
        base.DisableOBJ();
    }
}
