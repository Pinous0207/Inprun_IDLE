using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public delegate void OnSavingMode();
public class UI_SavingMode : UI_Base
{
    [SerializeField] private TextMeshProUGUI BatteryText, TimerText, FightText, StageText;
    [SerializeField] private Image BatteryFill, LandImage;
    [SerializeField] private Transform Content;
    [SerializeField] private UI_Inventory_Part item_Part;

    public Dictionary<string, Item_Holder> m_SaveItem = new Dictionary<string, Item_Holder>();
    public Dictionary<string, UI_Inventory_Part> m_Parts = new Dictionary<string, UI_Inventory_Part>();

    public static OnSavingMode m_OnSaving;

    Vector2 StartPos, EndPos;
    Camera camera;
    public override bool Init()
    {
        camera = Camera.main;
        camera.enabled = false;

        m_OnSaving?.Invoke();

        return base.Init();
    }

    public override void DisableOBJ()
    {
        camera.enabled = true;
        Base_Canvas.isSave = false;
        base.DisableOBJ();
    }

    private void Update()
    {
        // SystemInfo.batteryLevel <- 0.0f ~ 1.0f 0.6
        BatteryText.text = (SystemInfo.batteryLevel * 100.0f).ToString() + "%";
        BatteryFill.fillAmount = SystemInfo.batteryLevel;

        // DateTime 
        TimerText.text = System.DateTime.Now.ToString("tt hh:mm");
        int stageValue = Data_Mng.m_Data.Stage + 1;
        int stageForward = (stageValue / 10) + 1;
        int stageBack = stageValue % 10;

        StageText.text = "보통 " + stageForward.ToString() + " - " + stageBack.ToString();
        FightText.text = Stage_Mng.isDead ? "반복중..." : "진행중...";


        if(Input.GetMouseButtonDown(0)) // 마우스가 한 번 눌렸을 때 
        {
            StartPos = Input.mousePosition;
        }
        if(Input.GetMouseButton(0)) // 마우스를 누르는 동안
        {
            EndPos = Input.mousePosition;
            
            float distance = Vector2.Distance(EndPos, StartPos);
            LandImage.color = new Color(1, 1, 1, Mathf.Clamp(distance / (Screen.width / 2), 0.3f, 1.0f));

            if (distance >= Screen.width / 2)
            {
                DisableOBJ();
            }
        }
        if(Input.GetMouseButtonUp(0)) // 마우스를 눌렀다가 뗀 순간
        {
            StartPos = Vector2.zero;
            EndPos = Vector2.zero;
            LandImage.color = new Color(1, 1, 1, 0.3f);
        }
    }

    public void GetItem(Item_Scriptable item)
    {
        if(m_SaveItem.ContainsKey(item.name))
        {
            m_SaveItem[item.name].holder.Count++;
            m_Parts[item.name].Init(item.name, Base_Mng.Data.Item_Holder[item.name]);
            return;
        }
        Item_Holder items = new Item_Holder { Data = item, holder = new Holder()};
        items.holder.Count = 1;
        m_SaveItem.Add(item.name, items);
        var go = Instantiate(item_Part, Content);
        m_Parts.Add(item.name, go);
        go.Init(items.Data.name, Base_Mng.Data.Item_Holder[item.name]);
    }
}
