using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ADS_Buff : UI_Base
{
    public enum ADS_Buff_State { ATK, GOLD, CRITICAL }

    [SerializeField] private TextMeshProUGUI m_LevelText, m_CountText;
    [SerializeField] private Button[] m_Buttons;
    [SerializeField] private Image[] m_Buttons_Fill;
    [SerializeField] private Image m_Level_Fill;
    [SerializeField] private GameObject[] m_Buttons_Lock, m_Lock, m_ButtonFrame;
    [SerializeField] private TextMeshProUGUI[] m_Timer_Texts;

    public override bool Init()
    {
        for (int i = 0; i < Data_Mng.m_Data.Buff_timers.Length; i++)
        {
            int index = i;
            m_Buttons[index].onClick.AddListener(() => GetBuff((ADS_Buff_State)index));

            if (Data_Mng.m_Data.Buff_timers[i] > 0.0f)
            {
                SetBuff(i, true);
            }
        }

        return base.Init();
    }

    private void Update()
    {
        for(int i = 0; i< Data_Mng.m_Data.Buff_timers.Length; i++)
        {
            if (Data_Mng.m_Data.Buff_timers[i] >= 0.0f)
            {
                m_Buttons_Fill[i].fillAmount = 1 - (Data_Mng.m_Data.Buff_timers[i] / 1800.0f);
               
                m_Timer_Texts[i].text = Utils.GetTimer(Data_Mng.m_Data.Buff_timers[i]);
            }
        }
    }

    public void GetBuff(ADS_Buff_State m_State)
    {
        Base_Mng.ADS.ShowRewardedAds(() =>
        {
            int stateValue = (int)m_State;

            Data_Mng.m_Data.BuffCount++;

            Data_Mng.m_Data.Buff_timers[stateValue] = 1800.0f;
            Main_UI.instance.BuffCheck();
            SetBuff(stateValue, true);
        });
    }

    void SetBuff(int value, bool GetBool)
    {
        m_Buttons_Lock[value].SetActive(GetBool);
        m_Lock[value].SetActive(!GetBool);
        m_ButtonFrame[value].SetActive(GetBool);
    }
}
