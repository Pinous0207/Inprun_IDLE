using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_DailyQuest : UI_Base
{
    public Quest_Part QuestPanel;
    public Transform Content;
    List<GameObject> GorvageObject = new List<GameObject>();
    List<Transform> InitPanels = new List<Transform>();
    public override bool Init()
    {
        if(GorvageObject.Count > 0)
        {
            for (int i = 0; i < GorvageObject.Count; i++) Destroy(GorvageObject[i]);
            GorvageObject.Clear();
            InitPanels.Clear();
        }

        foreach(var quest in Base_Mng.Quest.activeQuests)
        {
            var go = Instantiate(QuestPanel, Content);
            go.gameObject.SetActive(true);

            go.Init(quest, this);
            GorvageObject.Add(go.gameObject);
            InitPanels.Add(go.transform);
        }

        for(int i = 0; i < InitPanels.Count; i++)
        {
            if (Data_Mng.m_Data.DailyQuests[i] == true)
            {
                InitPanels[i].SetAsLastSibling();
            }
        }
        return base.Init();
    }
}
