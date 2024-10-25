using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region STAGE
public delegate void OnReadyEvent();
public delegate void OnPlayEvent();
public delegate void OnBossEvent();
public delegate void OnBossPlayEvent();
public delegate void OnClearEvent();
public delegate void OnDeadEvent();
public delegate void OnDungeonEvent(int value);
public delegate void OnDungeonClearEvent(int value);
public delegate void OnDungeonFailEvent();
#endregion

#region RELIC
public delegate void Monster_Dead(Monster monster);
public delegate void Player_Attack(Player player, Monster monster);
public delegate void Player_Hit(Player player);
#endregion

public class Delegate_Holder
{
    public static event Monster_Dead M_Dead_Event;
    public static event Player_Attack P_Attack_Event;
    public static event Player_Hit P_Hit_Event;

    public static void ClearEvent()
    {
        M_Dead_Event = null;
        P_Attack_Event = null;
        P_Hit_Event = null;
    }

    public static void Monster_Dead(Monster monster)
    {
        M_Dead_Event?.Invoke(monster);
    }

    public static void Player_Attack(Player player,  Monster monster)
    {
        P_Attack_Event?.Invoke(player, monster);
    }

    public static void Player_Hit(Player player)
    {
        P_Hit_Event?.Invoke(player);
    }
}
