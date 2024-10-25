using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{
    [SerializeField] ItemType m_InventoryState;
    [SerializeField] RectTransform m_Bar;
    [SerializeField] private Button[] m_Top_buttons;
    [SerializeField] RectTransform TopContent;

    [SerializeField] Transform Content;
    public UI_Inventory_Part part;
    List<GameObject> Gorvage = new List<GameObject>();
    public override bool Init()
    {
        if(Gorvage.Count > 0)
        {
            for (int i = 0; i < Gorvage.Count; i++) Destroy(Gorvage[i]);
            Gorvage.Clear();
        }

        var sort_dictionary = Base_Mng.Data.m_Data_Item.OrderByDescending(x => x.Value.rarity);

        foreach (var item in sort_dictionary)
        {
            if (m_InventoryState == ItemType.ALL)
            {
                if (Base_Mng.Data.Item_Holder[item.Key].Count > 0)
                {
                    var go = Instantiate(part, Content);
                    go.Init(item.Key, Base_Mng.Data.Item_Holder[item.Key]);

                    Gorvage.Add(go.gameObject);
                }
            }
            else
            {
                if (Base_Mng.Data.Item_Holder[item.Key].Count > 0 && m_InventoryState == Base_Mng.Data.m_Data_Item[item.Key].type)
                {
                    var go = Instantiate(part, Content);
                    go.Init(item.Key,Base_Mng.Data.Item_Holder[item.Key]);

                    Gorvage.Add(go.gameObject);
                }
            }
            
        }

        for(int i = 0; i < m_Top_buttons.Length; i++)
        {
            int index = i;
            m_Top_buttons[index].onClick.RemoveAllListeners();
            m_Top_buttons[index].onClick.AddListener(() => Item_Inventory_Check((ItemType)index));
        }

        return base.Init();
    }

    public void Item_Inventory_Check(ItemType m_State)
    {
        m_InventoryState = m_State;
        StartCoroutine(barMovementCoroutine(
            m_Top_buttons[(int)m_State].GetComponent<RectTransform>().anchoredPosition,
            m_Top_buttons[(int)m_State].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x));
        Init();
    }

    IEnumerator barMovementCoroutine(Vector2 endPos, float endXPos)
    {
        float current = 0;
        float percent = 0;
        Vector2 start = m_Bar.anchoredPosition;
        Vector2 end = new Vector2(endPos.x, TopContent.anchoredPosition.y);

        float startX = m_Bar.sizeDelta.x;
        float endX = endXPos + 60.0f;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.1f;
            Vector2 LerpPos = Vector2.Lerp(start, end, percent);
            float LerpPosX = Mathf.Lerp(startX, endX, percent);

            m_Bar.anchoredPosition = LerpPos;
            // Mathf.Clamp(값, 최소값, 최대값) 
            m_Bar.sizeDelta = new Vector2(Mathf.Clamp(LerpPosX, 200.0f, Mathf.Infinity), m_Bar.sizeDelta.y);

            yield return null;
        }
    }
}
