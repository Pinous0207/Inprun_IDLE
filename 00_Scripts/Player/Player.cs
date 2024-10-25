using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Character
{
    public Character_Scriptable CH_Data;
    public ParticleSystem Provocation_Effect;
    public GameObject TrailObject;
    public string CH_Name;
    public int MP;
    Vector3 startPos;
    Quaternion rot;
    public bool MainCharacter = false;

    protected override void Start()
    {
        base.Start();

        Data_Set(Resources.Load<Character_Scriptable>("Scriptable/Character/" + CH_Name));

        Spawner.m_Players.Add(this);

        Stage_Mng.m_ReadyEvent += OnReady;
        Stage_Mng.m_BossEvent += OnBoss;
        Stage_Mng.m_ClearEvent += OnClear;
        Stage_Mng.m_DeadEvent += OnDead;
        Stage_Mng.m_DungeonEvent += OnDungeon;
        Stage_Mng.m_DungeonClearEvent += OnDungeonClear;

        startPos = transform.position;
        rot = transform.rotation;
    }
  
    private void Data_Set(Character_Scriptable data)
    {
        CH_Data = data;
        Bullet_Name = CH_Data.m_Character_Name;
        Attack_Range = data.m_Attack_Range;
        ATK_Speed = data.m_Attack_Speed;
        Set_ATKHP();
    }

    public void Set_ATKHP()
    {
        ATK = Base_Mng.Player.Get_ATK(CH_Data.m_Rarity, Base_Mng.Data.m_Data_Character[CH_Data.name]);
        HP = Base_Mng.Player.Get_HP(CH_Data.m_Rarity, Base_Mng.Data.m_Data_Character[CH_Data.name]);
    }

    private void OnReady()
    {
        Initalize();
        Spawner.m_Players.Add(this);
    }

    private void OnBoss()
    {
        AnimatorChange("isIDLE");
        Provocation_Effect.Play();
    }

    private void OnClear()
    {
        AnimatorChange("isCLEAR");
    }

    private void OnDead()
    {
        Spawner.m_Players.Add(this);
    }

    private void OnDungeon(int value)
    {
        Initalize();
        Spawner.m_Players.Add(this);
    }

    private void OnDungeonClear(int value)
    {
        AnimatorChange("isCLEAR");
    }

    private void Initalize()
    {
        AnimatorChange("isIDLE");
        isDead = false;
        Set_ATKHP();
        transform.position = startPos;
        transform.rotation = rot;
    }

    public void Get_MP(int mp)
    {
        if (isGetSkill) return;
        if(MainCharacter) return;

        MP += mp;

        if(MP >= CH_Data.MaxMP)
        {
            MP = 0;
            if (GetComponent<Skill_Base>() != null)
            {
                GetComponent<Skill_Base>().Set_Skill();
            }
            isGetSkill = true;
        }
        Main_UI.instance.Character_State_Check(this);
    }

    private void Update()
    {
        if (isDead) return;
        if (Stage_Mng.m_State == Stage_State.Play || Stage_Mng.m_State == Stage_State.Boss_Play)
        {
            FindClosetTarget(Spawner.m_Monsters.ToArray());

            if (m_Target == null)
            {
                float targetPos = Vector3.Distance(transform.position, startPos);
                if (targetPos > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
                    transform.LookAt(startPos);
                    AnimatorChange("isMOVE");
                }
                else
                {
                    transform.rotation = rot;
                    AnimatorChange("isIDLE");
                }
            }
            else
            {
                if (m_Target.GetComponent<Character>().isDead)
                {
                    FindClosetTarget(Spawner.m_Monsters.ToArray());
                }

                float targetDistance = Vector3.Distance(transform.position, m_Target.position);
                if (targetDistance <= target_Range && targetDistance > Attack_Range && isATTACK == false) // 현재 타겟이 추적 범위안에는 있지만 공격 범위 안에는 없을 때
                {
                    AnimatorChange("isMOVE");
                    transform.LookAt(m_Target.position);
                    transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime);
                }
                else if (targetDistance <= Attack_Range && isATTACK == false)
                {
                    isATTACK = true;
                    AnimatorChange("isATTACK");
                    Get_MP(5);
                    Invoke("InitAttack", 1.0f / ATK_Speed); // 함수를 몇 초 뒤에 실행시켜라
                }
            }
        }
    }

    public void KnockBack()
    {
        StartCoroutine(Knockback_Coroutine(3.0f, 0.3f));
    }

    IEnumerator Knockback_Coroutine(float power, float duration)
    {
        float t = duration;
        Vector3 force = this.transform.forward * -power;
        force.y = 0f;

        while(t > 0f)
        {
            t -= Time.deltaTime;
            if(Vector3.Distance(Vector3.zero, transform.position) < 3.0f)
            {
                transform.position += force * Time.deltaTime;
            }
            yield return null;
        }
    }

 

    public override void GetDamage(double dmg)
    {
        base.GetDamage(dmg);

        if (Stage_Mng.isDead) return;

        Delegate_Holder.Player_Hit(this);

        Get_MP(3);

        var goObj = Base_Mng.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<HIT_TEXT>().Init(transform.position, dmg, Color.red, true);
        });

        HP -= dmg;

        if(HP <= 0)
        {
            isDead = true;
            DeadEvent();
        }
    }

    private void DeadEvent()
    {
        Spawner.m_Players.Remove(this);
        if(Spawner.m_Players.Count <= 0 && Stage_Mng.isDead == false)
        {
            Stage_Mng.State_Change(Stage_State.Dead);
        }
        AnimatorChange("isDEAD");
        m_Target = null;
    }

    protected override void Attack()
    {
        base.Attack();
        TrailObject.SetActive(true);
        Delegate_Holder.Player_Attack(this, m_Target.GetComponent<Monster>());
        Invoke("TrailDisable", 1.0f);
    }

    private void TrailDisable() => TrailObject.SetActive(false);
}
