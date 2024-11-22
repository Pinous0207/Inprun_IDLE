using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Achievement_Part : MonoBehaviour
{
    [SerializeField] private GameObject ContentPart;
    [SerializeField] private Transform Content;
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI RewardTitle;
    [SerializeField] private Button CollectButton;
    [SerializeField] private GameObject Collected;
    List<GameObject> Gorvage = new List<GameObject>();
    int valueCount;
    Achievement Data;
    UI_Achievement parent;
    public void Init(Achievement data, int value, UI_Achievement parentData)
    {
        parent = parentData;
        Data = data;
        CollectButton.onClick.RemoveAllListeners();
        valueCount = value;

        if(Gorvage.Count > 0)
        {
            for (int i = 0; i < Gorvage.Count; i++) Destroy(Gorvage[i]);
            Gorvage.Clear();
        }

        if (Data_Mng.m_Data.Achievement_B[valueCount] == true)
        {
            Collected.SetActive(true);
        }

        switch (data.type)
        {
            case Achievement_Type.Hero:
                bool CanCollect = true;
                for(int i = 0; i < data.Achievement_Characters.Count; i++)
                {
                    var character = data.Achievement_Characters[i].character;
                    var go = Instantiate(ContentPart, Content);
                    go.SetActive(true);
                    Gorvage.Add(go);

                    bool LevelCompleted = Base_Mng.Data.Character_Holder[character.m_Character_Name].Level >= data.Achievement_Characters[i].Level;

                    var image = go.transform.Find("Character_Icon").GetComponent<Image>();
                    image.sprite = Utils.Get_Atlas(character.m_Character_Name);
                    image.SetNativeSize();

                    var m_text = go.transform.GetComponentInChildren<TextMeshProUGUI>();

                    m_text.text =
                        string.Format("Lv.{0}/{1}",
                        Base_Mng.Data.Character_Holder[character.m_Character_Name].Level,
                        data.Achievement_Characters[i].Level);
                    m_text.color = LevelCompleted ? Color.green : Color.red;

                    if (LevelCompleted == false) CanCollect = false;

                    go.transform.GetChild(go.transform.childCount - 1).gameObject.SetActive(!LevelCompleted);
                }

                if(CanCollect)
                {
                    CollectButton.onClick.AddListener(() => GetCollect(valueCount));
                }
                break;
        }

        Title.text = data.Title;
        var temp = (int)Mathf.Sign(data.RewardValue) == 1 ? "+" : "";
        RewardTitle.text = Local_Mng.local_Data[data.GetRewardStatus.ToString()].Get_Data() + temp +data.RewardValue +"%";
    }

    private void GetCollect(int valueCount)
    {
        Data_Mng.m_Data.Achievement_B[valueCount] = true;
        Base_Mng.Quest.LoadAchivement_Data();
        parent.Init();
    }
}
