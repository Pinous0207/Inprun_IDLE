using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator animator;


    public double HP;
    public double ATK;
    public float ATK_Speed;
    public bool isDead = false;

    protected float Attack_Range = 3.0f; // 공격하는 공격 범위
    protected float target_Range = 5.0f; // 추격하는 범위
    protected bool isATTACK = false;

    public bool isGetSkill = false;
    public bool SkillNoneAttack = false;

    protected Transform m_Target;

    [SerializeField] private Transform m_BulletTransform;

    public string Bullet_Name;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected void InitAttack() => isATTACK = false;

    public void AnimatorChange(string temp)
    {
        if (SkillNoneAttack)
        {
            if (isGetSkill) return;
        }
        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        if (temp == "isATTACK" || temp == "isCLEAR" || temp == "isDEAD" || temp == "isSKILL")
        {
            if(temp == "isATTACK")
            {
                animator.speed = ATK_Speed;
            }
            animator.SetTrigger(temp);
            return;
        }

        animator.speed = 1.0f;
        animator.SetBool(temp, true);
    }

    protected virtual void Bullet()
    {
        if (m_Target == null) return;

        Base_Mng.Pool.Pooling_OBJ("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_BulletTransform.position;
            value.GetComponent<Bullet>().Init(m_Target, ATK, Bullet_Name);
        });
    }

    protected virtual void Attack()
    {
        if (m_Target == null) return;

        Base_Mng.Pool.Pooling_OBJ("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_Target.position;
            value.GetComponent<Bullet>().Attack_Init(m_Target, ATK);
        });
    }

    public virtual void GetDamage(double dmg)
    {

    }

    public virtual void Heal(double heal)
    {
        HP += heal;

        var goObj = Base_Mng.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<HIT_TEXT>().Init(transform.position, heal, Color.green, true);
        });
    }

    protected void FindClosetTarget<T>(T[] targets) where T : Component
    {
        var monsters = targets;
        Transform closetTarget = null;
        float maxDistance = target_Range; // 5.0f

        foreach (var monster in monsters)
        {
            float targetDistance = Vector3.Distance(transform.position, monster.transform.position);

            if(targetDistance < maxDistance)
            {
                closetTarget = monster.transform;
                maxDistance = targetDistance;
            }
        }
        m_Target = closetTarget;
        if (m_Target != null) transform.LookAt(m_Target.position);
    }
}
