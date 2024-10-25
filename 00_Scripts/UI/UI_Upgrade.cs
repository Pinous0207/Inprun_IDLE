using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Upgrade : UI_Base
{
    [SerializeField] Upgrade_Part Panel;
    [SerializeField] Transform Content;

    UI_Base parentBase;

    public void Initalize(UI_Base parent)
    {
        parentBase = parent;
        StartCoroutine(UpgradeCoroutine());
    }

    IEnumerator UpgradeCoroutine()
    {
        var data = Base_Mng.Data.m_Data_Character;

        foreach (var c in data)
        {
            if (CanLevelUp(c.Value))
            {
                var go = Instantiate(Panel, Content);
                go.gameObject.SetActive(true);

                int before = c.Value.holder.Level + 1;
                int value = 0;
                UpgradeLevel(c.Value, ref value);
                go.Init(c.Value, before, value + 1);
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }

        var heroes = parentBase.GetComponent<UI_Heroes>();
        for (int i = 0; i < heroes.part.Count; i++) heroes.part[i].Init();

        Base_Mng.Firebase.WriteData();
    }

    private void UpgradeLevel(Character_Holder holder, ref int value)
    {
        while (holder.holder.Count >= Base_Mng.Character.LevelCount(holder))
        {
            holder.holder.Count -= Base_Mng.Character.LevelCount(holder);
            holder.holder.Level++;
        }
        value = holder.holder.Level;
    }

    private bool CanLevelUp(Character_Holder holder)
    {
        if(holder.holder.Count >= Base_Mng.Character.LevelCount(holder))
        {
            return true;
        }
        return false;
    }
}
