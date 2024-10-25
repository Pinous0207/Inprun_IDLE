using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render_Gacha : MonoBehaviour
{
    [SerializeField] GameObject LootEffect;
    [SerializeField] GameObject[] RarityEffect;

    [SerializeField] Transform[] ElevenCount;
    [SerializeField] Transform OneCount;

    List<GameObject> Gorvage = new List<GameObject>();
    public void Init()
    {
        for(int i = 0; i< Gorvage.Count; i++)
        {
            Destroy(Gorvage[i]);
        }
        Gorvage.Clear();
    }
    public void Get_Hero(int GachaValue,int value, Character_Scriptable data)
    {
        Transform parent = Character_Transform(GachaValue, value);
        var loot = Instantiate(LootEffect, parent.position, Quaternion.identity, parent);
        var go = Instantiate(Resources.Load<GameObject>("Character/" + data.name), parent);
        go.GetComponent<Player>().enabled = false;
        StartCoroutine(ScaleCoroutine(go.transform));
        go.transform.rotation = Quaternion.Euler(0, 180, 0);
        go.transform.localPosition = Vector3.zero;

        var effect = Instantiate(RarityEffect[(int)data.m_Rarity], parent.position, Quaternion.identity, parent);
        
        Gorvage.Add(loot);
        Gorvage.Add(go);
        Gorvage.Add(effect);
    }

    private Transform Character_Transform(int GachaValue, int value)
    {
        switch(GachaValue)
        {
            case 11: return ElevenCount[value];
            case 1: return OneCount;
        }
        return null;
    }

    IEnumerator ScaleCoroutine(Transform obj)
    {
        float current = 0;
        float percent = 0;
        float start = 0.0f;
        float end = 5.0f;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;
            float LerpScale = Mathf.Lerp(start, end, percent);
            obj.localScale = new Vector3(LerpScale, LerpScale, LerpScale);
            yield return null;
        }
    }
}
