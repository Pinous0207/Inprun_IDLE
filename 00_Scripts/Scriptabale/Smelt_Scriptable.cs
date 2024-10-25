using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Smelt_Data", menuName = "Smelt Data/Smelt")]
public class Smelt_Scriptable : ScriptableObject
{
    // 1~5개의 능력치가 랜덤하게 나오는 확률
    [Header("## Appear")]
    public float[] Count_Value;

    [Space(20f)]
    [Header("## Percentage")]
    public Percentage[] ATK_percentage; // ATK
    public Percentage[] HP_percentage; // ATK
    public Percentage[] MONEY_percentage; // ATK
    public Percentage[] ITEM_percentage; // ATK
    public Percentage[] SKILL_percentage; // ATK
    public Percentage[] ATK_SPEED_percentage; // ATK
    public Percentage[] CRITICAL_P_percentage; // ATK
    public Percentage[] CRITICAL_D_percentage; // ATK
}
