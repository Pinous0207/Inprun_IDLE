using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : UI_Base
{
    public StatusType m_StatusType;
    [SerializeField] RectTransform m_Bar;
    [SerializeField] RectTransform TopContent;
    [SerializeField] private Button[] m_Bottom_Buttons;
    [SerializeField] private GameObject[] Panel_Objs;

    [SerializeField] private TextMeshProUGUI AllDPS, ATK, HP;
    [SerializeField] private TextMeshProUGUI GoldDrop, ItemDrop, ATKSpeed, CriticalPercentage, CriticalDamage;

    // ## Mastery
    [SerializeField] private Transform Content;
    [SerializeField] private GameObject HorizontalOBJ;
    [SerializeField] private GameObject OBJ;

    public override bool Init()
    {
        Main_UI.instance.FadeInOut(true, true, null);

        AllDPS.text = StringMethod.ToCurrencyString(Base_Mng.Player.Average_ATK());
        ATK.text = StringMethod.ToCurrencyString(Base_Mng.Player.Main_ATK());
        HP.text = StringMethod.ToCurrencyString(Base_Mng.Player.Main_HP());
        GoldDrop.text = string.Format("{0:0}%", Base_Mng.Player.GoldDropPercentage());
        ItemDrop.text = string.Format("{0:0}%", Base_Mng.Player.ItemDropPercentage());
        ATKSpeed.text = string.Format("{0:0.0}", Base_Mng.Player.ATKSpeed());
        CriticalPercentage.text = string.Format("{0:0}%", Base_Mng.Player.CriticalPercentage());
        CriticalDamage.text = string.Format("{0:0}%", Base_Mng.Player.CriticalDamage());

        for (int i = 0; i < m_Bottom_Buttons.Length; i++)
        {
            int index = i;
            m_Bottom_Buttons[index].onClick.RemoveAllListeners();
            m_Bottom_Buttons[index].onClick.AddListener(() => Status_Check((StatusType)index));
        }

        return base.Init();
    }

    public void Status_Check(StatusType m_State)
    {
        for(int i = 0; i< Panel_Objs.Length; i++)
        {
            Panel_Objs[i].SetActive(false);
        }

        Panel_Objs[(int)m_State].SetActive(true);

        m_StatusType = m_State;
        StartCoroutine(barMovementCoroutine(
            m_Bottom_Buttons[(int)m_State].GetComponent<RectTransform>().anchoredPosition,
            m_Bottom_Buttons[(int)m_State].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x));
    }

    IEnumerator barMovementCoroutine(Vector2 endPos, float endXPos)
    {
        float current = 0;
        float percent = 0;
        Vector2 start = m_Bar.anchoredPosition;
        Vector2 end = new Vector2(endPos.x, TopContent.anchoredPosition.y);

        float startX = m_Bar.sizeDelta.x;
        float endX = endXPos + 60.0f;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.1f;
            Vector2 LerpPos = Vector2.Lerp(start, end, percent);
            float LerpPosX = Mathf.Lerp(startX, endX, percent);

            m_Bar.anchoredPosition = LerpPos;
            // Mathf.Clamp(값, 최소값, 최대값) 
            m_Bar.sizeDelta = new Vector2(Mathf.Clamp(LerpPosX, 200.0f, Mathf.Infinity), m_Bar.sizeDelta.y);

            yield return null;
        }
    }

    public override void DisableOBJ()
    {
        Main_UI.instance.LayerCheck(-1);
        base.DisableOBJ();
    }
}
