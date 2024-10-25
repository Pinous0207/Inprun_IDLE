using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill_Base : MonoBehaviour
{
    public GameObject SkillParticle;
    protected Monster[] monsters { get { return Spawner.m_Monsters.ToArray(); } }
    protected Player[] players { get { return Spawner.m_Players.ToArray(); } }

    protected Character m_Player { get { return GetComponent<Character>(); } }

    private void Start()
    {
        Stage_Mng.m_DeadEvent += OnDead;
    }

    public virtual void Set_Skill()
    {

    }

    protected double SkillDamage(double value)
    {
        return m_Player.ATK * (value / 100.0f);
    }

    protected bool Distance(Vector3 startPos, Vector3 endPos, float distanceValue)
    {
        return Vector3.Distance(startPos, endPos) <= distanceValue;
    }

    private void OnDead()
    {
        StopAllCoroutines();
    }

    public virtual void ReturnSkill()
    {
        m_Player.isGetSkill = false;
        m_Player.AnimatorChange("isIDLE");
    }

    public Character HP_Check()
    {
        Character player = null;
        double hpCount = Mathf.Infinity; // 5.0f

        for(int i = 0; i< players.Length; i++)
        {
            double hp = players[i].HP;

            if(hp < hpCount)
            {
                hpCount = hp;
                player = players[i];
            }
        }

        return player;
    }
 
}
