using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Mng
{
    public Character_Holder[] m_Set_Character = new Character_Holder[6];

    public void GetCharacter(int value, string character_Name)
    {
        m_Set_Character[value] = Base_Mng.Data.m_Data_Character[character_Name];
    }

    public void DisableCharacter(int value)
    {
        m_Set_Character[value] = null;
    }

    public int LevelCount(Character_Holder holder)
    {
        return (holder.holder.Level + 1) * 5;
    }
}
