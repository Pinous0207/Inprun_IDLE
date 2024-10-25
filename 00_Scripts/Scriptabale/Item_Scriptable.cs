using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Data", menuName = "Item Data/Item")]
public class Item_Scriptable : ScriptableObject
{
    public string Item_Name;
    public string Item_DES;
    public ItemType type;
    public Rarity rarity;
    public float Item_Chance;
    public int MinLevel;
}
