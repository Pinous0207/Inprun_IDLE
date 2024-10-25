using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class COIN_PARENT : MonoBehaviour
{
    Vector3 target;
    Camera cam;
    RectTransform[] childs = new RectTransform[5];

    [Range(0.0f, 500.0f)]
    [SerializeField] private float m_DistanceRange, speed;
    private void Awake()
    {
        cam = Camera.main;
        for (int i = 0; i < childs.Length; i++) childs[i] = transform.GetChild(i).GetComponent<RectTransform>();

    }
    private void OnSave()
    {
        Data_Mng.m_Data.Money += Utils.Data.stageData.MONEY();
        if (Distance_Boolean_World(0.5f))
        {
            Base_Mng.Pool.m_pool_Dictionary["COIN_PARENT"].Return(this.gameObject);
        }
    }

    private void OnDisable()
    {
        UI_SavingMode.m_OnSaving -= OnSave;
    }
    public void Init(Vector3 pos, Coin_Type type = Coin_Type.Gold, int value = 0)
    {
        UI_SavingMode.m_OnSaving += OnSave;

        if (Base_Canvas.isSave) return;

        target = pos;
        
        transform.position = cam.WorldToScreenPoint(pos);
        for (int i = 0; i < 5; i++)
        {
            childs[i].GetComponent<Image>().sprite = Utils.Get_Atlas(type.ToString());
            childs[i].anchoredPosition = Vector2.zero;
        }
        transform.parent = Base_Canvas.instance.HOLDER_LAYER(0);
        switch(type)
        {
            case Coin_Type.Gold:
                Data_Mng.m_Data.Money += Utils.Data.stageData.MONEY();
                break;
            case Coin_Type.Dia:
                Data_Mng.m_Data.Dia += value;
                break;
        }

        StartCoroutine(Coin_Effect());
    }

    IEnumerator Coin_Effect()
    {
        Vector2[] RandomPos = new Vector2[childs.Length];
        for(int i = 0; i < childs.Length; i++)
        {
            RandomPos[i] = new Vector2(target.x, target.y) + Random.insideUnitCircle * Random.Range(-m_DistanceRange, m_DistanceRange);
        }

        while(true)
        {
            for(int i = 0; i < childs.Length; i++)
            {
                RectTransform rect = childs[i];

                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, RandomPos[i], Time.unscaledDeltaTime * speed);
            }

            if (Distance_Boolean(RandomPos, 0.5f)) break;

            yield return null; // yield return new WaitForSeconds(float)
            // yield return null - 한 번의 프레임을 대기하라
        }

        yield return new WaitForSeconds(0.3f);

        while(true)
        {
            for(int i = 0; i < childs.Length; i++)
            {
                RectTransform rect = childs[i];
                rect.position = Vector2.MoveTowards(rect.position, Base_Canvas.instance.COIN.position, Time.unscaledDeltaTime * (speed * 20.0f));
            }

            if(Distance_Boolean_World(0.5f))
            {
                Base_Mng.Pool.m_pool_Dictionary["COIN_PARENT"].Return(this.gameObject);
                break;
            }
            yield return null;
        }

        Main_UI.instance.TextCheck();
    }

    private bool Distance_Boolean(Vector2[] end, float range)
    {
        for(int i = 0; i < childs.Length; i++)
        {
            float distance = Vector2.Distance(childs[i].anchoredPosition, end[i]);
            if(distance > range)
            {
                return false;
            }
        }
        return true;
    }

    private bool Distance_Boolean_World(float Range)
    {
        for(int i = 0; i < childs.Length; i++)
        {
            float distance = Vector2.Distance(childs[i].position, Base_Canvas.instance.COIN.position);
            if(distance > Range)
            {
                return false;
            }
        }
        return true;
    }
}
