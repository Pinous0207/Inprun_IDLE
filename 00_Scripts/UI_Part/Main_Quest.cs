using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main_Quest : MonoBehaviour
{
    List<Dictionary<string, object>> Data = new List<Dictionary<string, object>>();
    Dictionary<string, object> quest;

    public static int monster_index;
    [SerializeField] private TextMeshProUGUI TitleText, ExplaneText, CountText, RewardText; 
    Quest_Type m_State;
    public static bool GetEnemy = false;
    bool reward = false;
    [SerializeField] private GameObject HandObj;

    private void Start()
    {
        Data = CSV_Importer.Quest;
        NextQuest();
    }

    private void Update()
    {
        GetQuest();
    }

    public void NextQuest()
    {
        monster_index = 0;

        quest = Data[Data_Mng.m_Data.Quest_Count];
        m_State = (Quest_Type)Enum.Parse(typeof(Quest_Type), quest["Key"].ToString());
        if (m_State == Quest_Type.Monster) GetEnemy = true;

        TitleText.text = "퀘스트." + (Data_Mng.m_Data.Quest_Count + 1).ToString();
        ExplaneText.text = Localization_Counting(m_State);
        RewardText.text = quest["Reward"].ToString();
    }

    void GetQuest()
    {
        Color color = Counting(m_State) >= Convert.ToInt32(quest["Value"]) ? Color.green : Color.red;

        CountText.text = "(" + Counting(m_State).ToString() + "/" + Convert.ToInt32(quest["Value"]) + ")";
        CountText.color = color;

        reward = Counting(m_State) >= Convert.ToInt32(quest["Value"]) ? true : false;
        
        if(HandObj.activeSelf != reward)
            HandObj.SetActive(reward);
    }

    public void GetQuestButton()
    {
        if (reward == false) return;

        Base_Mng.Pool.Pooling_OBJ("COIN_PARENT").Get((value) =>
        {
            value.GetComponent<COIN_PARENT>().Init(Camera.main.ScreenToWorldPoint(transform.position), Coin_Type.Dia, Convert.ToInt32(quest["Reward"]));
        });

        Data_Mng.m_Data.Quest_Count++;
        NextQuest();
    }

    private int Counting(Quest_Type type)
    {
        switch (type)
        {
            case Quest_Type.Monster: return monster_index;
            case Quest_Type.Gold:  return Data_Mng.m_Data.Dungeon_Clear_Level[1];
            case Quest_Type.Dia: return Data_Mng.m_Data.Dungeon_Clear_Level[0];
            case Quest_Type.Upgrade: return Data_Mng.m_Data.UpgradeCount;
            case Quest_Type.Hero: return Data_Mng.m_Data.Hero_Summon_Count;
            case Quest_Type.Stage: return Data_Mng.m_Data.Stage;
        }
        return 0;
    }

    private string Localization_Counting(Quest_Type type)
    {
        switch (type)
        {
            case Quest_Type.Monster: return "몬스터 처치";
            case Quest_Type.Gold: return "골드 던전 클리어";
            case Quest_Type.Dia: return "보물 던전 클리어";
            case Quest_Type.Upgrade: return "경험치 획득";
            case Quest_Type.Hero: return "영웅 소환";
            case Quest_Type.Stage: return "스테이지 클리어";
        }
        return "";
    }
}
