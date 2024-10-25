using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Stage_Mng
{
    // 상태 패턴 ( State Pattern )   
    // 디자인 패턴
    public static Stage_State m_State;

    public static int MaxCount = 3;
    public static int Count;
    public static int DungeonCount = 30;

    public static bool isDead = false;
    public static bool isDungeon = false;
    public static int DungeonType = 0;
    public static int DungeonLevel = 0;

    // 델리게이트 체인 ( Delegate Chain )
    // 하나의 델리게이트가 여러 함수를 참조 할 수 있다.

    public static OnReadyEvent m_ReadyEvent;
    public static OnPlayEvent m_PlayEvent;
    public static OnBossEvent m_BossEvent;
    public static OnBossPlayEvent m_BossPlayEvent;
    public static OnClearEvent m_ClearEvent;
    public static OnDeadEvent m_DeadEvent;
    public static OnDungeonEvent m_DungeonEvent;
    public static OnDungeonClearEvent m_DungeonClearEvent;
    public static OnDungeonFailEvent m_DungeonFailEvent;
    public static void State_Change(Stage_State state, int value = 0)
    {
        m_State = state;
        switch(state)
        {
            case Stage_State.Ready:
                Count = 0;
                MaxCount = int.Parse(CSV_Importer.Spawn_Design[Data_Mng.m_Data.Stage]["MaxCount"].ToString());
                m_ReadyEvent?.Invoke();
                Base_Mng.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;
            case Stage_State.Play:
                Debug.Log("isPlay!");
                m_PlayEvent?.Invoke();
                break;
            case Stage_State.Boss:
                Count = 0;
                Debug.Log("isBoss!");
                m_BossEvent?.Invoke();
                break;
            case Stage_State.Boss_Play:
                Debug.Log("isBossPlay!");
                m_BossPlayEvent?.Invoke();
                break;
            case Stage_State.Clear:
                Data_Mng.m_Data.Stage++;
                Debug.Log("isClear!");
                m_ClearEvent?.Invoke();
               break;
            case Stage_State.Dead:
                Debug.Log("isDead!");
                isDead = true;
                m_DeadEvent?.Invoke();
                break;
            case Stage_State.Dungeon:
                isDungeon = true;
                DungeonType = value;
                DungeonCount = 30;
                m_DungeonEvent?.Invoke(value);
                Base_Mng.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;
            case Stage_State.DungeonClear:
                isDungeon = false;
                m_DungeonClearEvent?.Invoke(value);
                break;
            case Stage_State.DungeonFail:
                isDungeon = false;
                m_DungeonFailEvent?.Invoke();
                break;
        }
    }
}
