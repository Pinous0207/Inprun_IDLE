using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class Item_OBJ : MonoBehaviour
{
    [SerializeField] private Transform ItemTextRect;
    [SerializeField] private TextMeshProUGUI m_Text;
    [SerializeField] private GameObject[] Raritys;
    [SerializeField] private ParticleSystem m_Loot;
    [SerializeField] private float firingAngle = 45.0f;
    [SerializeField] private float gravity = 9.8f;


    Item_Scriptable m_Item;
    Rarity rarity;
    bool isCheck = false;

    void RarityCheck()
    {
        isCheck = true;

        transform.rotation = Quaternion.identity; // (0,0,0)

        Raritys[(int)rarity].SetActive(true);

        ItemTextRect.gameObject.SetActive(true);
        ItemTextRect.parent = Base_Canvas.instance.HOLDER_LAYER(2);

        m_Text.text = Utils.String_Color_Rarity(rarity) + m_Item.Item_Name + "</color>";

        StartCoroutine(LootItem());
    }

    IEnumerator LootItem()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));

        for (int i = 0; i < Raritys.Length; i++) Raritys[i].SetActive(false);

        ItemTextRect.transform.SetParent(this.transform);
        ItemTextRect.gameObject.SetActive(false);

        m_Loot.Play();

        Main_UI.instance.GetItem(m_Item);
        Base_Mng.Inventory.GetItem(m_Item);

        if(Base_Canvas.isSave)
        {
            Base_Canvas.instance.m_UI.GetComponent<UI_SavingMode>().GetItem(m_Item);
        }

        yield return new WaitForSeconds(0.5f);

        Base_Mng.Pool.m_pool_Dictionary["Item_OBJ"].Return(this.gameObject);
    }

    private void Update()
    {
        if (isCheck == false) return;
        if (Base_Canvas.isSave) return;
        ItemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnSave()
    {
        Main_UI.instance.GetItem(m_Item);
        Base_Mng.Inventory.GetItem(m_Item);

        ItemTextRect.gameObject.SetActive(false);

        if (Base_Canvas.isSave)
        {
            Base_Canvas.instance.m_UI.GetComponent<UI_SavingMode>().GetItem(m_Item);
        }

        Base_Mng.Pool.m_pool_Dictionary["Item_OBJ"].Return(this.gameObject);
    }

    public void Init(Vector3 pos, Item_Scriptable data)
    {
        m_Item = data;
        rarity = m_Item.rarity;

        UI_SavingMode.m_OnSaving += OnSave;

        isCheck = false;
        transform.position = pos;
        Vector3 Target_Pos = new Vector3(pos.x + (Random.insideUnitSphere.x * 2.0f), 0.5f, pos.z + (Random.insideUnitSphere.z * 2.0f));
        StartCoroutine(SimulateProjectile(Target_Pos));
    }

    public void OnDisable()
    {
        UI_SavingMode.m_OnSaving -= OnSave;
    }

    IEnumerator SimulateProjectile(Vector3 pos)
    {
        // Mathf.Abs(절대값) , Mathf.Min,Max(최솟값, 최댓값) , Mathf.Sign( 음수 일 경우에는 -1, 양수 일 경우에는 +1 )
        
        // Mathf.Sin(사인)
        // Mathf.Cos(코스)
        // Mathf.Deg2Rad (각도(degree) -> 호도(radian)) : Degree to Radian
        // Mathf.Sqrt(제곱근)

        float target_Distance = Vector3.Distance(transform.position, pos);

        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
        float flightDuration = target_Distance / Vx;

        transform.rotation = Quaternion.LookRotation(pos - transform.position);

        float time = 0.0f;

        while (time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * time)) * Time.deltaTime, Vx * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        RarityCheck();
    }
}
