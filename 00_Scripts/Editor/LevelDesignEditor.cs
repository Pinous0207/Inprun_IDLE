using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelDesign))]
public class LevelDesignEditor : Editor
{
    LevelDesign design = null;
    public override void OnInspectorGUI()
    {
        design = (LevelDesign)target;

        EditorGUILayout.LabelField("Level Design", EditorStyles.boldLabel);

        LevelData data = design.levelData;
        StageData s_Data = design.stageData;

        DrawGraph(data, s_Data);
        EditorGUILayout.Space();

        DrawDefaultInspector();
    }

    private void DrawGraph(LevelData data, StageData s_Data)
    {
        EditorGUILayout.LabelField("레벨 데이터", EditorStyles.boldLabel);
        GetColorGUI("ATK", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_ATK, design.currentLevel, data.C_ATK)), Color.green);
        GetColorGUI("HP", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_HP, design.currentLevel, data.C_HP)), Color.red);
        GetColorGUI("EXP", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_EXP, design.currentLevel, data.C_EXP)), Color.blue);
        GetColorGUI("MAX_EXP", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_MAXEXP, design.currentLevel, data.C_MAXEXP)), Color.white);
        GetColorGUI("MONEY", StringMethod.ToCurrencyString(Utils.CalculatedValue(data.B_MONEY, design.currentLevel, data.C_MONEY)), Color.yellow);
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("스테이지 데이터", EditorStyles.boldLabel);
        GetColorGUI("ATK", StringMethod.ToCurrencyString(Utils.CalculatedValue(s_Data.B_ATK, design.currentStage, s_Data.M_ATK)), Color.green);
        GetColorGUI("HP", StringMethod.ToCurrencyString(Utils.CalculatedValue(s_Data.B_HP, design.currentStage, s_Data.M_HP)), Color.red);
        GetColorGUI("EXP", StringMethod.ToCurrencyString(Utils.CalculatedValue(s_Data.B_MONEY, design.currentStage, s_Data.M_MONEY)), Color.blue);
    }

    void GetColorGUI(string baseTemp, string dataTemp, Color color)
    {
        GUIStyle colorLabel = new GUIStyle(EditorStyles.label);
        colorLabel.normal.textColor = color;

        EditorGUILayout.LabelField(baseTemp + " : " + dataTemp, colorLabel);
    }
}
