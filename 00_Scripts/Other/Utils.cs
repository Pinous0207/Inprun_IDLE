using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Utils
{
    public static SpriteAtlas m_Atlas = Resources.Load<SpriteAtlas>("Atlas");
    public static int[] summon_level = { 11, 22, 33, 44, 55, 66, 77, 88, 99};
    // Queue = FIFO (First in First out), Stack = LIFO (Last in First Out)
    public static Stack<UI_Base> UI_Holder = new Stack<UI_Base>();
    public static LevelDesign Data = Resources.Load<LevelDesign>("Scriptable/LevelDesignData");

    public static void CloseAllPopupUI()
    {
        while (UI_Holder.Count > 0) ClosePopupUI();
    }

    public static void ClosePopupUI()
    {
        if (UI_Holder.Count == 0) return;
        // Push(Stack에 값을 삽입), Peek(Stack에 마지막으로 들어온 값을 반환), Pop(Stack에 마지막으로 들어온 값을 반환하면서 Stack에서 제외)

        UI_Base popup = UI_Holder.Peek();
        popup.DisableOBJ();
    }
    public static Sprite Get_Atlas(string temp)
    {
        return m_Atlas.GetSprite(temp);
    }

    public static string String_Color_Rarity(Rarity rare)
    {
        switch(rare)
        {
            case Rarity.Common: return "<color=#FFFFFF>";
            case Rarity.UnCommon: return "<color=#00FF00>";
            case Rarity.Rare: return "<color=#00D8FF>";
            case Rarity.Hero: return "<color=#B750C3>";
            case Rarity.Legendary: return "<color=#FF9000>";
        }
        return "<color=#FFFFFF>";
    }

    public static string GetTimer(float time)
    {
        TimeSpan timespan = TimeSpan.FromSeconds(time);
        string timer = string.Format("{0:00}:{1:00}", timespan.Minutes, timespan.Seconds);

        return timer;
    }

    // 지수 증가 공식 // Mathf.Pow(Single, Single) - 거듭 제곱
    // 값이 일정한 비율로 지속적으로 증가한다 - 지수 증가
    public static double CalculatedValue(float baseValue, int Level, float value)
    {
        return baseValue * Mathf.Pow((Level + 1), value);
    }

    public static bool Coin_Check(double value)
    {
        if (Data_Mng.m_Data.Money >= value) return true;
        else return false;
    }

    public static float[] Gacha_Percentage()
    {
        int value = Summon_Level(Data_Mng.m_Data.Hero_Summon_Count);
        float[] valueCount = new float[5];
        for(int i = 0; i < valueCount.Length; i++)
        {
            string RarityName = ((Rarity)i).ToString();
            valueCount[i] =
                float.Parse(CSV_Importer.Summon[value][RarityName].ToString());
        }
        return valueCount;
    }

    public static int Summon_Level(int count)
    {
        if(count >= summon_level[8])
        {
            return 9;
        }

        for(int i = 0; i < summon_level.Length; i++)
        {
            if (count < summon_level[i])
            {
                return i;
            }
        }
        return -1;
    }

    public static double TimerCheck()
    {
        if(Data_Mng.m_Data.StartDate == "" || Data_Mng.m_Data.EndDate == "")
        {
            return 0.0d;
        }

        DateTime startDate = DateTime.Parse(Data_Mng.m_Data.StartDate);
        DateTime EndDate = DateTime.Parse(Data_Mng.m_Data.EndDate);

        TimeSpan timer = startDate - EndDate;
        double timeCount = timer.TotalSeconds;

        return timeCount;
    }

    public static string NextDayTimer()
    {
        DateTime nowDate = DateTime.Now;
        DateTime NextDate = nowDate.AddDays(1); // AddDays = 지정된 날짜로부터 일수를 1일 올려주는
        NextDate = new DateTime(NextDate.Year, NextDate.Month, NextDate.Day, 0, 0, 0);
        TimeSpan timer = NextDate - nowDate;
        return timer.Hours + " : " + timer.Minutes + " : " + timer.Seconds;
    }

    public static bool Item_Count(string itemName, int value)
    {
        if (Base_Mng.Data.Item_Holder[itemName].Count >= value) return true;
        else return false;
    }
}
