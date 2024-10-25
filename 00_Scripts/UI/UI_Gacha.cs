using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Gacha : UI_Base
{
    Render_Gacha gachaTexture;
    public Transform[] Horizontals;
    public int Limit = 4;
    public Image Hero_Panel;

    [SerializeField] TextMeshProUGUI GachaTitle;
    [SerializeField] TextMeshProUGUI GachaPrice;
    [SerializeField] Button GachaButton;
    [SerializeField] GameObject[] LockButtons;
    int valueCount;
    List<GameObject> Gorvage = new List<GameObject>();
    private void Awake()
    {
        gachaTexture = Render_Manager.instance.GACHA;
    }
   
    public void Initalize()
    {
        for (int i = 0; i < LockButtons.Length; i++)
        {
            LockButtons[i].SetActive(true);
        }

        gachaTexture.Init();
        for(int i = 0; i < Gorvage.Count; i++)
        {
            Destroy(Gorvage[i]);
        }
        Gorvage.Clear();
    }

    public override void DisableOBJ()
    {
        gachaTexture.Init();
        base.DisableOBJ();
    }

    public void GetGachaHero(int value)
    {
        valueCount = value;

        GachaButton.onClick.RemoveAllListeners();
        
        switch (value)
        {
            case 11:
                GachaTitle.text = "11회 소환";
                GachaPrice.text = "3000";
                GachaButton.onClick.AddListener(() => OnClickReGacha(value));
                    break;
            case 1:
                GachaTitle.text = "1회 소환";
                GachaPrice.text = "300";
                GachaButton.onClick.AddListener(() => OnClickReGacha(value));
                break;
        }
        StartCoroutine(Gacha_Coroutine(value));
    }

    public void OnClickReGacha(int value)
    {
        Initalize();
        GetGachaHero(value);
    }

    IEnumerator Gacha_Coroutine(int value)
    {
        int horizontalChecker = 0;
        // 캐릭터를 미리 뽑는 작업
        List<Character_Scriptable> gacha_list = new List<Character_Scriptable>();
        for(int i = 0; i < value; i++)
        {
            Data_Mng.m_Data.Hero_Summon_Count++;
            Data_Mng.m_Data.Hero_PickUp_Count++;
            Rarity rarity = Rarity.Common;
            if (Data_Mng.m_Data.Hero_PickUp_Count >= 110)
            {
                Data_Mng.m_Data.Hero_PickUp_Count = 0;
                rarity = Rarity.Legendary;
            }
            float R_percentage = 0.0f;
            float percentage = Random.Range(0.0f, 100.0f);

            if (rarity != Rarity.Legendary)
            {
                for (int j = 0; j < 5; j++)
                {
                    R_percentage += Utils.Gacha_Percentage()[j];
                    if (percentage <= R_percentage)
                    {
                        rarity = (Rarity)j;
                        break;
                    }
                }
            }
            // 뽑기 완료된 캐릭터 데이터 결정
            Character_Scriptable data = Base_Mng.Data.Get_Rarity_Character(rarity);
            gacha_list.Add(data);
        }

        // 캐릭터 생성 연출
        for (int i = 0; i < value; i++)
        {
            var data = gacha_list[i];
            var go = Instantiate(Hero_Panel, Horizontals[horizontalChecker]);
            Gorvage.Add(go.gameObject);
            Image characterImage = go.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            go.gameObject.SetActive(true);

            Base_Mng.Data.Character_Holder[data.name].Count++;
            go.sprite = Utils.Get_Atlas(data.m_Rarity.ToString());
            characterImage.sprite = Utils.Get_Atlas(data.name);
            characterImage.SetNativeSize();

            if ((i+1) % Limit == 0)
            {
                horizontalChecker++;
            }
            switch(value)
            {
                case 11: gachaTexture.Get_Hero(11,i, data); break;
                case 1: gachaTexture.Get_Hero(1,0,data); break;
            }

            go.GetComponentInChildren<Animator>().SetTrigger((int)data.m_Rarity < 3 ? "Default" : "Rare");
            yield return new WaitForSecondsRealtime((int)data.m_Rarity < 3 ? 0.1f : 0.5f);
        }

        for(int i = 0; i< LockButtons.Length; i++)
        {
            LockButtons[i].SetActive(false);
        }

        Base_Mng.Firebase.WriteData(); // 저장
    }
}
