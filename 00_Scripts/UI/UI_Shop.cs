using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Shop : UI_Base
{
    // Hero
    [SerializeField] TextMeshProUGUI H_LevelText;
    [SerializeField] TextMeshProUGUI H_CountText;
    [SerializeField] TextMeshProUGUI H_PickUpText;
    [SerializeField] Image H_CountFill;
    [SerializeField] Image H_PickUpFill;

    [SerializeField] TextMeshProUGUI[] Percentage_Texts;
    [SerializeField] TextMeshProUGUI Percentage_LevelText;

    [SerializeField] GameObject InformationPanel;
    int backBoard_Level;

    public override bool Init()
    {
        GetInit();
        return base.Init();
    }

    public void GetProduct(string name)
    {
        Base_Mng.IAP.Purchase(name);
    }

    public void GetInit()
    {
        H_LevelText.text = "영웅 소환 레벨 Lv." + (Utils.Summon_Level(Data_Mng.m_Data.Hero_Summon_Count) + 1).ToString();

        int level = Utils.Summon_Level(Data_Mng.m_Data.Hero_Summon_Count);
        if (level < 9)
        {
            int valueCount = Data_Mng.m_Data.Hero_Summon_Count;
            int MaximumValueCount = Utils.summon_level[level];
            H_CountText.text = "(" + valueCount.ToString() + "/" + MaximumValueCount.ToString() + ")";
            H_CountFill.fillAmount = (float)valueCount / (float)MaximumValueCount;
        }
        else if (level >= 9)
        {
            H_CountText.text = "Max Level";
            H_CountFill.fillAmount = 1.0f;
        }

        int valuePickUp = Data_Mng.m_Data.Hero_PickUp_Count;
        H_PickUpText.text = "(" + valuePickUp.ToString() + "/110)";
        H_PickUpFill.fillAmount = (float)valuePickUp / 110.0f;
    }

    public void GetInformation()
    {
        InformationPanel.SetActive(true);

        Percentage_Check(Utils.Summon_Level(Data_Mng.m_Data.Hero_Summon_Count));
    }

    public void ArrowButton(int value)
    {
        backBoard_Level += value;
        if (backBoard_Level < 0) backBoard_Level = 9;
        else if (backBoard_Level > 9) backBoard_Level = 0;

        Percentage_Check(backBoard_Level);
    }

    private void Percentage_Check(int value)
    {
        backBoard_Level = value;
        for (int i = 0; i < Percentage_Texts.Length; i++)
        {
            float temp = float.Parse(CSV_Importer.Summon[value][((Rarity)i).ToString()].ToString());
            Percentage_Texts[i].text = temp.ToString("F5") + "%";
        }
        Percentage_LevelText.text = "LEVEL." + (value+1).ToString();
    }

    public void DisableInformation() => InformationPanel.SetActive(false);
    public void GachaButton(int value)
    {
        Base_Canvas.instance.Get_UI("#Gacha");
        var ui = Utils.UI_Holder.Peek().GetComponent<UI_Gacha>();
        ui.GetGachaHero(value);
    }
    public void GachaButton_ADS()
    {
        Base_Mng.ADS.ShowRewardedAds(() => GachaButton(1));
    }

    public override void DisableOBJ()
    {
        Main_UI.instance.LayerCheck(-1);
        base.DisableOBJ();
    }
}
