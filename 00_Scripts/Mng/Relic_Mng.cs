using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Relic_Mng : MonoBehaviour
{
    public static Relic_Mng instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Initalize()
    {
        if (Base_Mng.Item.SetItemCheck("Dice")) Delegate_Holder.M_Dead_Event += Dice;
        if (Base_Mng.Item.SetItemCheck("Axe")) Delegate_Holder.P_Attack_Event += Axe;
        if (Base_Mng.Item.SetItemCheck("Mana")) Delegate_Holder.P_Hit_Event += Mana;
    }

    public void Axe(Player player, Monster monster)
    {
        Vector3 RealPos = monster.transform.position;
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Axe"));
        go.transform.position = RealPos;

        for(int i = 0; i < Spawner.m_Monsters.Count; i++)
        {
            if (Vector3.Distance(Spawner.m_Monsters[i].transform.position, RealPos) <= 3.0f)
            {
                Spawner.m_Monsters[i].GetDamage(player.ATK * 0.7f);
            }
        }
    }

    public void Mana(Player player)
    {
        if (!RandomCount(50))
        {
            return;
        }
        player.Get_MP(2);
    }

    public void Dice(Monster monster)
    {
        if(!RandomCount(50))
        {
            return;
        }
        Vector3 RealPos = monster.transform.position;
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Dice"));
        go.transform.position = RealPos;
    }

    private bool RandomCount(float RandomValue)
    {
        float randomCount = Random.Range(0.0f, 100.0f);
        if(randomCount <= RandomValue)
        {
            return true;
        }
        return false;
    }
}
