using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Mng
{
    public void EXP_UP()
    {
        // EX)
        // Base_Mng.Data.EXP -> Data_Mng.m_Data
        Data_Mng.m_Data.EXP += Utils.Data.levelData.EXP();
        Data_Mng.m_Data.ATK += Utils.Data.levelData.ATK();
        Data_Mng.m_Data.HP += Utils.Data.levelData.HP();
        Data_Mng.m_Data.UpgradeCount++;

        if(Data_Mng.m_Data.EXP >= Utils.Data.levelData.MAXEXP())
        {
            Data_Mng.m_Data.Level++;
            Data_Mng.m_Data.EXP = 0;
            Main_UI.instance.TextCheck();
        }

        for (int i = 0; i < Spawner.m_Players.Count; i++) Spawner.m_Players[i].Set_ATKHP();
    }

    public float EXP_Percentage()
    {
        float exp = (float)Utils.Data.levelData.MAXEXP();
        double myEXP = Data_Mng.m_Data.EXP;

        return (float)myEXP / exp;
    }

    public float Next_EXP()
    {
        float exp = (float)Utils.Data.levelData.MAXEXP();
        float myExp = (float)Utils.Data.levelData.EXP();

        return (myExp / exp) * 100.0f;
    }
    public double Get_ATK(Rarity rarity, Character_Holder holder)
    {
        var Damage = Data_Mng.m_Data.ATK * ((int)rarity + 1); // 500
        float Level = (holder.holder.Level + 1) * 10 / 100.0f; // 0.3
        var RealDamage = Damage + (Damage * Level);
        
        RealDamage += RealDamage * (Base_Mng.Data.valueSmelt(Status_Holder.ATK) / 100.0f);
        return RealDamage;
    }

    public double Get_HP(Rarity rarity, Character_Holder holder)
    {
        var hp = Data_Mng.m_Data.HP * ((int)rarity + 1);
        float Level = (holder.holder.Level + 1) * 10 / 100.0f; // 0.3
        var RealHP = hp + (hp * Level);

        RealHP += RealHP * (Base_Mng.Data.valueSmelt(Status_Holder.HP) / 100.0f);
        return RealHP;
    }

    public double Main_ATK()
    {
        double atk = Get_ATK(Rarity.Common, Base_Mng.Data.m_Data_Character["Cleric"]); // 0
        int value = 1;
        for(int i = 0; i< Base_Mng.Character.m_Set_Character.Length; i++)
        {
            if (Base_Mng.Character.m_Set_Character[i] != null)
            {
                var data = Base_Mng.Character.m_Set_Character[i].Data;

                atk += Get_ATK(data.m_Rarity, Base_Mng.Data.m_Data_Character[data.name]); // 6
                value++;
            }
        }
        return atk / value;
    }

    public double Main_HP()
    {
        double hp = Get_HP(Rarity.Common, Base_Mng.Data.m_Data_Character["Cleric"]); // 0
        int value = 1;
        for (int i = 0; i < Base_Mng.Character.m_Set_Character.Length; i++)
        {
            if (Base_Mng.Character.m_Set_Character[i] != null)
            {
                var data = Base_Mng.Character.m_Set_Character[i].Data;

                hp += Get_HP(data.m_Rarity, Base_Mng.Data.m_Data_Character[data.name]); // 6
                value++;
            }
        }
        return hp / value;
    }

    public float GoldDropPercentage()
    {
        return 0.0f + Base_Mng.Data.valueSmelt(Status_Holder.MONEY);
    }

    public float ItemDropPercentage()
    {
        return 0.0f + Base_Mng.Data.valueSmelt(Status_Holder.ITEM);
    }

    public float ATKSpeed()
    {
        return 1.0f + Base_Mng.Data.valueSmelt(Status_Holder.ATK_SPEED);
    }

    public float CriticalPercentage()
    {
        return 20.0f + Base_Mng.Data.valueSmelt(Status_Holder.CRITICAL_P);
    }

    public float CriticalDamage()
    {
        return 140.0f + Base_Mng.Data.valueSmelt(Status_Holder.CRITICAL_D);
    }

    public double Average_ATK()
    {
        return Main_ATK() + Main_HP();
    }
}
