using System;
using System.Collections;
using UnityEngine;

// 모든 매니저 스크립트의 집합체
public class Base_Mng : MonoBehaviour
{
    public static Base_Mng instance = null;

    #region Parameter

    private static Pool_Mng s_Pool = new Pool_Mng();
    private static Player_Mng s_Player = new Player_Mng();
    private static Data_Mng s_Data = new Data_Mng();
    private static Item_Mng s_Item = new Item_Mng();
    private static Inventory_Mng s_Inventory = new Inventory_Mng();
    private static Character_Mng s_Character = new Character_Mng();
    private static ADS_Mng s_ADS = new ADS_Mng();
    private static Firebase_Mng s_Firebase = new Firebase_Mng();
    private static IAP_Mng s_IAP = new IAP_Mng();
    private static Sound_Mng s_Sound = new Sound_Mng();
    private static Local_Mng s_Local = new Local_Mng();
    private static Quest_Mng s_Quest = new Quest_Mng();
    private static Time_Mng s_Time = new Time_Mng();
    public static Pool_Mng Pool { get { return s_Pool; } }
    public static Player_Mng Player { get { return s_Player; } }
    public static Data_Mng Data { get { return s_Data; } }
    public static Item_Mng Item { get { return s_Item; } }
    public static Inventory_Mng Inventory { get { return s_Inventory; } }
    public static Character_Mng Character { get { return s_Character; } }
    public static ADS_Mng ADS { get { return s_ADS; } }
    public static Firebase_Mng Firebase { get { return s_Firebase; } }
    public static IAP_Mng IAP { get { return s_IAP; } }
    public static Sound_Mng Sound { get { return s_Sound; } }
    public static Local_Mng Local { get { return s_Local; } }
    public static Quest_Mng Quest { get { return s_Quest; } }
    public static Time_Mng m_Time { get { return s_Time; } }
    #endregion

    public static bool isFast = false;
    public static bool GetGameStart = false;
    float Save_Timer = 0.0f;

    private void Awake()
    {
        Initalize();
    }

    private void Update()
    {
        if (GetGameStart == false) return;

        Save_Timer += Time.unscaledDeltaTime;
        if (Save_Timer >= 10.0f)
        {
            Save_Timer = 0.0f;
            Firebase.WriteData();
        }

        for (int i = 0; i < Data_Mng.m_Data.Buff_timers.Length; i++)
        {
            if (Data_Mng.m_Data.Buff_timers[i] >= 0.0f)
            {
                Data_Mng.m_Data.Buff_timers[i] -= Time.unscaledDeltaTime;
            }
        }
        if (Data_Mng.m_Data.Buff_x2 > 0.0f) Data_Mng.m_Data.Buff_x2 -= Time.unscaledDeltaTime;
    }

    private void Initalize()
    {
        if(instance == null)
        {
            instance = this;

            Pool.Initalize(transform);
            ADS.Init();
            Firebase.Init();
            IAP.InitUnityIAP();
            Sound.Init();
            Local.Init();
            m_Time.Init();

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void GetReward() => Debug.Log("보상형 광고를 시청하고, 아이템을 획득하였습니다!");

    public GameObject Instantiate_Path(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }

    public void Return_Pool(float timer, GameObject obj, string path)
    {
        StartCoroutine(Return_Pool_Coroutine(timer, obj, path));
    }

    public void Coroutine_Action(float timer, Action action)
    {
        StartCoroutine(Action_Coroutine(action, timer));
    }

    IEnumerator Return_Pool_Coroutine(float time, GameObject obj, string path)
    {
        yield return new WaitForSeconds(time);
        Pool.m_pool_Dictionary[path].Return(obj);
    }

    IEnumerator Action_Coroutine(Action action, float timer)
    {
        yield return new WaitForSeconds(timer);
        action?.Invoke();
    }

    private void OnDestroy()
    {
        if (GetGameStart)
        {
            Debug.Log("Check");
            Firebase.WriteData();
        }
    }
}
