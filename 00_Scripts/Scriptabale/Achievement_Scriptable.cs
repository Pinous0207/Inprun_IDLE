using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[System.Serializable]
public class Achi_Character
{
    public Character_Scriptable character;
    public int Level;
}
public class Achi_Item
{
    public Item_Scriptable item;
    public int Level;
}

[System.Serializable]
public class Achievement
{
    public Achievement_Type type;
    public string Title;
    public Status_Holder GetRewardStatus;
    public float RewardValue;
    public List<Achi_Character> Achievement_Characters = new List<Achi_Character>();
    public List<Achi_Item> Achievement_Items = new List<Achi_Item>();
}


[CreateAssetMenu(fileName = "Achievement_Data", menuName = "Achievement Data/Aachievement")]

public class Achievement_Scriptable : ScriptableObject
{
    public List<Achievement> Achievement_Data = new List<Achievement>();
}
