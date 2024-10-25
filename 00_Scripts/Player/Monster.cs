using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    public float m_Speed;

    bool isSpawn = false;

    public float R_Attack_Range;
    public bool isBoss = false;
    public bool isDungeon = false;
    Vector3 scalePos;
    double MaxHP;
    private void Awake()
    {
        scalePos = transform.localScale;
    }
    protected override void Start()
    {
        base.Start();
        Stage_Mng.m_DeadEvent += OnDead;
    }

    public void Init(int value = 0)
    {
        isDead = false;
        isSpawn = false;
        ATK = isBoss ? Utils.Data.stageData.ATK(value) * 10.0f : Utils.Data.stageData.ATK(value);
        HP = isBoss ? Utils.Data.stageData.HP(value) * 10.0f : Utils.Data.stageData.HP(value);
        ATK_Speed = 1.0f;
        MaxHP = HP;
        transform.localScale = scalePos;
        Attack_Range = R_Attack_Range;
        target_Range = Mathf.Infinity;

        if(isBoss && !isDungeon)
        {
            StartCoroutine(SkillCoroutine());
        }

        StartCoroutine(Spawn_Start());
    }

    IEnumerator SkillCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<Skill_Base>().Set_Skill();

        StartCoroutine(SkillCoroutine());
    }

    void OnDead()
    {
        StopAllCoroutines();
        if (isDungeon) return;

        AnimatorChange("isIDLE");
    }

    private void Update()
    {
        if (isSpawn == false) return;
        if (isDungeon) return;
        if (Stage_Mng.m_State == Stage_State.Play || Stage_Mng.m_State == Stage_State.Boss_Play)
        {
            if (m_Target == null)
                FindClosetTarget(Spawner.m_Players.ToArray());

            if (m_Target != null)
            {
                if (m_Target.GetComponent<Character>().isDead)
                {
                    FindClosetTarget(Spawner.m_Players.ToArray());
                }

                float targetDistance = Vector3.Distance(transform.position, m_Target.position);

                if (targetDistance > Attack_Range && isATTACK == false) // 현재 타겟이 추적 범위안에는 있지만 공격 범위 안에는 없을 때
                {
                    AnimatorChange("isMOVE");
                    transform.LookAt(m_Target.position);
                    transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime);
                }
                else if (targetDistance <= Attack_Range && isATTACK == false)
                {
                    isATTACK = true;
                    AnimatorChange("isATTACK");
                    Invoke("InitAttack", 1.0f); // 함수를 몇 초 뒤에 실행시켜라
                }
            }
        }
    }

    IEnumerator Spawn_Start()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x;
        while(percent < 1)
        {
            current += Time.deltaTime; 
            percent = current / 0.2f;
            //              선형보간. (시작 값, 끝 값, 시간) == 시작에서 끝까지 특정 시간속도로 이동해라
            float LerpPos = Mathf.Lerp(start, end, percent); 
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }

    public override void GetDamage(double dmg)
    {
        if (isDead) return;

        bool critical = Critical(ref dmg);

        Base_Mng.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<HIT_TEXT>().Init(transform.position, dmg, Color.white, false, critical);
        });

        HP -= dmg;

        if(isBoss)
        {
            Main_UI.instance.Boss_Slider_Count(HP, MaxHP);
        }

        if(HP <= 0)
        {
            isDead = true;
            Delegate_Holder.Monster_Dead(this);
            Dead_Event();
        }
    }

    private void Dead_Event()
    {
        if(Main_Quest.GetEnemy)
        {
            Main_Quest.monster_index++;
        }

        if (Stage_Mng.isDungeon)
        {
            Stage_Mng.DungeonCount--;
            Main_UI.instance.Dungeon_Monster_Slider_Count();
            if(isBoss)
            {
                Stage_Mng.State_Change(Stage_State.DungeonClear, Stage_Mng.DungeonType);
            }
        }
        else 
        {
            if (!isBoss)
            {
                if (!Stage_Mng.isDead)
                {
                    Stage_Mng.Count++;
                    Main_UI.instance.Monster_Slider_Count();
                }
            }
            else
            {
                Stage_Mng.State_Change(Stage_State.Clear);
            }
        }

        Spawner.m_Monsters.Remove(this);

        Base_Mng.Pool.Pooling_OBJ("Smoke").Get((value) =>
        {
            value.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            Base_Mng.instance.Return_Pool(value.GetComponent<ParticleSystem>().duration, value, "Smoke");
        });

        Base_Mng.Pool.Pooling_OBJ("COIN_PARENT").Get((value) =>
        {
            value.GetComponent<COIN_PARENT>().Init(transform.position);
        });

        var Items = Base_Mng.Item.GetDropSet();

        for (int i = 0; i < Items.Count; i++)
        {
            Base_Mng.Pool.Pooling_OBJ("Item_OBJ").Get((value) =>
            {
                value.GetComponent<Item_OBJ>().Init(transform.position, Items[i]);
            });
        }

        if (!isBoss)
            Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        else Destroy(this.gameObject);
    }

    // ref, out
    private bool Critical(ref double dmg)
    {
        float RandomValue = Random.Range(0.0f, 100.0f);

        if (RandomValue <= Base_Mng.Player.CriticalPercentage())
        {
            dmg *= Base_Mng.Player.CriticalDamage() / 100;
            return true;
        }
        return false;
    }
}
