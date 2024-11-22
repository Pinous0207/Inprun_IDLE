using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Achievement : UI_Base
{
    [SerializeField] private Achievement_Part part;
    [SerializeField] private Transform Content;
    [SerializeField] private TextMeshProUGUI StatusText;
    

    List<Transform> InitPanels = new List<Transform>();
    List<GameObject> Gorvage = new List<GameObject>();
    public override bool Init()
    {
        if (Gorvage.Count > 0)
        {
            for (int i = 0; i < Gorvage.Count; i++) Destroy(Gorvage[i]);
            Gorvage.Clear();
            InitPanels.Clear();
        }

        for(int i = 0; i < Base_Mng.Quest.Achievement_Lists.Count; i++)
        {
            var go = Instantiate(part, Content);
            go.gameObject.SetActive(true);
            go.Init(Base_Mng.Quest.Achievement_Lists[i], i, this);
            InitPanels.Add(go.transform);
            Gorvage.Add(go.gameObject);
        }

        for(int i = 0; i < InitPanels.Count; i++)
        {
            if (Data_Mng.m_Data.Achievement_B[i] == true)
            {
                InitPanels[i].SetAsLastSibling();
            }
        }

        CheckStatus();

        return base.Init();
    }

    private void CheckStatus()
    {
        StatusText.text =
            string.Format("{0}/{1}\n{2}/{3}\n{4}/{5}\n{6}/{7}",
            LocalTemp(Status_Holder.ATK, Base_Mng.Quest.Achivewment_status_Data.ATK),
            LocalTemp(Status_Holder.HP, Base_Mng.Quest.Achivewment_status_Data.HP),
            LocalTemp(Status_Holder.MONEY, Base_Mng.Quest.Achivewment_status_Data.MONEY),
            LocalTemp(Status_Holder.ITEM, Base_Mng.Quest.Achivewment_status_Data.ITEM),
            LocalTemp(Status_Holder.ATK_SPEED, Base_Mng.Quest.Achivewment_status_Data.ATK_SPEED),
            LocalTemp(Status_Holder.SKILL, Base_Mng.Quest.Achivewment_status_Data.SKILL),
            LocalTemp(Status_Holder.CRITICAL_P, Base_Mng.Quest.Achivewment_status_Data.CRITICAL_P),
            LocalTemp(Status_Holder.CRITICAL_D, Base_Mng.Quest.Achivewment_status_Data.CRITICAL_D));
    }

    private string PlusAndMinus(float value)
    {
        var temp = (int)Mathf.Sign(value) == 1 ? "+" : "";
        return temp;
    }

    private string LocalTemp(Status_Holder holder, float value)
    {
        string temp = Local_Mng.local_Data[holder.ToString()].Get_Data() +
            PlusAndMinus(value) + 
            string.Format("{0:0.0}", value.ToString());
        return temp;
    }
}
