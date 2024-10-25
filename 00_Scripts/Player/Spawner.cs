using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int m_Count; // 몬스터의 수
    private float m_SpawnTime; // 몇 초 마다

    public static List<Monster> m_Monsters = new List<Monster>();
    public static List<Player> m_Players = new List<Player>();

    Coroutine coroutine;

    public GameObject[] Maps;

    private void Start()
    {
        Stage_Mng.m_ReadyEvent += OnReady;
        Stage_Mng.m_PlayEvent += OnPlay;
        Stage_Mng.m_BossEvent += OnBoss;
        Stage_Mng.m_DungeonEvent += OnDungeon;
    }

    public void OnReady()
    {
        //m_Count = int.Parse(CSV_Importer.Spawn_Design[Base_Mng.Data.Stage]["Spawn_Count"].ToString());
        //m_SpawnTime = float.Parse(CSV_Importer.Spawn_Design[Base_Mng.Data.Stage]["Spawn_Timer"].ToString());

        Initalize();
        for (int i = 0; i < Maps.Length; i++) Maps[i].SetActive(false);
        m_Count = 5;
        m_SpawnTime = 2.0f;
    }

    public void OnPlay()
    {
        if (Stage_Mng.isDungeon) return;

        coroutine = StartCoroutine(SpawnCoroutine(m_Count, m_SpawnTime));
    }

    public void OnBoss()
    {
        Initalize();

        StartCoroutine(BossSetCoroutine());
    }

    public void OnDungeon(int value)
    {
        Maps[value].SetActive(true);
        Initalize();
        if(value == 0)
        {
            coroutine = StartCoroutine(SpawnCoroutine(30, -1, Stage_Mng.DungeonLevel * 5));
        }
        else if(value == 1)
        {
            StartCoroutine(BossSetCoroutine());
        }
    }

    private void Initalize()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        for (int i = 0; i < m_Monsters.Count; i++)
        {
            m_Monsters[i].isDead = true;
            Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(m_Monsters[i].gameObject);
        }
        m_Monsters.Clear();
    }

    IEnumerator BossSetCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        Monster monster = null;
        if (!Stage_Mng.isDungeon)
        {
            monster = Instantiate(Resources.Load<Monster>("Pool_OBJ/Boss"), Vector3.zero, Quaternion.Euler(0, 180, 0));
            monster.Init();
        }
        else
        {
            monster = Instantiate(Resources.Load<Monster>("Pool_OBJ/Gold"), Vector3.zero, Quaternion.Euler(0, 180, 0));
            monster.Init((Stage_Mng.DungeonLevel + 1) * 5);
        }
        Vector3 pos = monster.transform.position;

        for(int i = 0; i < m_Players.Count; i++)
        {
            if(Vector3.Distance(pos, m_Players[i].transform.position) <= 3.0f)
            {
                m_Players[i].transform.LookAt(monster.transform.position);
                m_Players[i].KnockBack();
            }
        }
        yield return new WaitForSeconds(1.5f);

        m_Monsters.Add(monster);
   
        Stage_Mng.State_Change(Stage_State.Boss_Play);
    }
    IEnumerator SpawnCoroutine(int Count, float spawnTime, int stage = 0)
    {
        Vector3 pos;
        int value = Count - m_Monsters.Count;

        for(int i = 0; i < value; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f; 
            pos.y = 0.0f;

            while(Vector3.Distance(pos, Vector3.zero) <= 3.0f) 
            {
                pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
                pos.y = 0.0f;
            }

            var goObj = Base_Mng.Pool.Pooling_OBJ("Monster").Get((value) =>
            {
                value.GetComponent<Monster>().Init(stage);
                value.transform.position = pos;
                value.transform.LookAt(Vector3.zero);

                m_Monsters.Add(value.GetComponent<Monster>());
            });
        }

        yield return new WaitForSeconds(spawnTime);

        if(spawnTime > 0.0f)
            coroutine = StartCoroutine(SpawnCoroutine(Count, spawnTime));
    }
}
