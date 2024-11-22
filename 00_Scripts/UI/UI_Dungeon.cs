using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Dungeon : UI_Base
{
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private TextMeshProUGUI[] KeyTexts;
    [SerializeField] private TextMeshProUGUI[] KeyHolders;
    [SerializeField] private TextMeshProUGUI[] ClearAssets;
    [SerializeField] private TextMeshProUGUI[] Levels;

    [SerializeField] Button[] Key01ArrowButton, Key02ArrowButton;

    int[] Level = new int[2];

    public override bool Init()
    {
        Main_UI.instance.FadeInOut(true, true, null);
        Render_Manager.instance.DUNGEON.Init();

        for(int i = 0; i < KeyTexts.Length; i++)
        {
            KeyTexts[i].text = "(" + Data_Mng.m_Data.Key[i].ToString() + "/2)";
            KeyHolders[i].color = (Data_Mng.m_Data.Key[i] + Data_Mng.m_Data.KeyAssets[i]) <= 0 ? Color.red : Color.green;
            Levels[i].text = (Data_Mng.m_Data.Dungeon_Clear_Level[i] + 1).ToString();
            Level[i] = Data_Mng.m_Data.Dungeon_Clear_Level[i];
        }
        int levelCount = (Data_Mng.m_Data.Dungeon_Clear_Level[1] + 1) * 5;
        var value = Utils.CalculatedValue(Utils.Data.stageData.B_MONEY, levelCount, Utils.Data.stageData.M_MONEY);

        ClearAssets[0].text = ((Data_Mng.m_Data.Dungeon_Clear_Level[0] + 1) * 50).ToString();
        ClearAssets[1].text = StringMethod.ToCurrencyString(value);

        Key01ArrowButton[0].onClick.AddListener(() => ArrowButton(0, -1));
        Key01ArrowButton[1].onClick.AddListener(() => ArrowButton(0, 1));
        Key02ArrowButton[0].onClick.AddListener(() => ArrowButton(1, -1));
        Key02ArrowButton[1].onClick.AddListener(() => ArrowButton(1, 1));

        return base.Init();
    }

    private void Update()
    {
        TimerText.text = Utils.NextDayTimer();
    }

    public void GetDungeon(int value)
    {
        if (Data_Mng.m_Data.Key[value] + Data_Mng.m_Data.KeyAssets[value] <= 0)
        {
            Base_Canvas.instance.Get_Toast().Initalize("던전 입장에 필요한 재화가 부족합니다.", Color.red);
            return;
        }
        Stage_Mng.DungeonLevel = Level[value];
        Stage_Mng.State_Change(Stage_State.Dungeon, value);
        Data_Mng.m_Data.Dungeon++;
        Utils.CloseAllPopupUI();
    }

    public void ArrowButton(int KeyValue, int value)
    {
        Level[KeyValue] += value;
        if (Level[KeyValue] <= 0)
        {
            Level[KeyValue] = 0;
        }
        else if (Level[KeyValue] >= Data_Mng.m_Data.Dungeon_Clear_Level[KeyValue])
        {
            Base_Canvas.instance.Get_Toast().Initalize("해당 난이도가 해금되지 않았습니다.", Color.white);
            Level[KeyValue] = Data_Mng.m_Data.Dungeon_Clear_Level[KeyValue];
        }
        Levels[KeyValue].text = (Level[KeyValue]+1).ToString();
    }

    public override void DisableOBJ()
    {
        Main_UI.instance.LayerCheck(-1);

        for (int i = 0; i < Render_Manager.instance.DUNGEON.characters.Length; i++)
            Render_Manager.instance.DUNGEON.characters[i].DisableCoroutine();
        base.DisableOBJ();
    }
}
