using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="LevelDesignData", menuName = "Level Design/Level Design Data")]
public class LevelDesign : ScriptableObject
{
    public int currentLevel;
    public int currentStage;

    [Space(20f)]
    public LevelData levelData;
    [Space(20f)]
    public StageData stageData;
}

[System.Serializable]
public class LevelData
{
    [Range(0.0f, 10.0f)]
    public float C_ATK, C_HP, C_EXP, C_MAXEXP, C_MONEY;

    [Space(20f)]
    [Header("## Base Value")]
    public int B_ATK;
    public int B_HP;
    public int B_EXP;
    public int B_MAXEXP;
    public int B_MONEY;

    public double ATK() => Utils.CalculatedValue(B_ATK, Data_Mng.m_Data.Level, C_ATK);
    public double HP() => Utils.CalculatedValue(B_HP, Data_Mng.m_Data.Level, C_HP);
    public double EXP() => Utils.CalculatedValue(B_EXP, Data_Mng.m_Data.Level, C_EXP);
    public double MAXEXP() => Utils.CalculatedValue(B_MAXEXP, Data_Mng.m_Data.Level, C_MAXEXP);
    public double MONEY() => Utils.CalculatedValue(B_MONEY, Data_Mng.m_Data.Level, C_MONEY);

}

[System.Serializable]
public class StageData
{
    [Range(0.0f, 10.0f)]
    public float M_ATK, M_HP, M_MONEY;

    [Space(20f)]
    [Header("## Base Value")]
    public int B_ATK;
    public int B_HP;
    public int B_MONEY;

    public double ATK(int value = 0) => Utils.CalculatedValue(B_ATK, value == 0 ? Data_Mng.m_Data.Stage : value, M_ATK);
    public double HP(int value = 0) => Utils.CalculatedValue(B_HP, value == 0 ? Data_Mng.m_Data.Stage : value, M_HP);
    public double MONEY(int value = 0) => Utils.CalculatedValue(B_MONEY, value == 0 ? Data_Mng.m_Data.Stage : value, M_MONEY);

}
