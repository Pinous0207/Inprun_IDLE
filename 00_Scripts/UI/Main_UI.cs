using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Main_UI : MonoBehaviour
{
    public static Main_UI instance = null;

    #region Parameter
    [Header("##Default")]
    [SerializeField] private TextMeshProUGUI m_Level_Text;
    [SerializeField] private TextMeshProUGUI m_ALLATK_Text;
    [SerializeField] private TextMeshProUGUI m_LevelUp_Money_Text;
    [SerializeField] private TextMeshProUGUI m_Money_Text;
    [SerializeField] private TextMeshProUGUI m_Stage_Count_Text;
    [SerializeField] private TextMeshProUGUI m_Stage_Text;
    [SerializeField] private TextMeshProUGUI m_Dia_Text;
    // Color = (float,float,float,float)
    //            R     G     B     A
    Color m_Stage_Color = new Color(0, 0.7295136f, 1.0f, 1.0f);

    [Space(20f)]
    [Header("##Fade")]
    [SerializeField] private Image m_Fade;
    [SerializeField] private float m_FadeDuration;

    [Space(20f)]
    [Header("##Monster_Slider")]
    [SerializeField] private GameObject m_Monster_Slider_OBJ;
    [SerializeField] private Image m_Monster_Slider;
    [SerializeField] private TextMeshProUGUI m_Monster_Value_Text;

    [Space(20f)]
    [Header("##Boss_Slider")]
    [SerializeField] private GameObject m_Boss_Slider_OBJ;
    [SerializeField] private Image m_Boss_Slider;
    [SerializeField] private TextMeshProUGUI m_Boss_Value_Text, m_Boss_Stage_Text;

    [Space(20f)]
    [Header("##Dungeon_Slider")]
    [SerializeField] private GameObject m_Dungeon_Slider_OBJ;
    [SerializeField] private Image m_Dungeon_Slider;
    [SerializeField] private TextMeshProUGUI m_Dungeon_Value_Text, m_Dungeon_Stage_Text;
    [SerializeField] private GameObject[] Dungeon_Value_OBJS;
    [SerializeField] private TextMeshProUGUI MonsterCountText;
    [SerializeField] private Image m_MonsterSlider_Fill;
    [SerializeField] private TextMeshProUGUI m_MonsterSlider_Text;
    Coroutine DungeonCoroutine = null;

    [Space(20f)]
    [Header("##Dead_Frame")]
    [SerializeField] private GameObject Dead_Frame;

    [Space(20f)]
    [Header("## Legendary_PopUP")]
    [SerializeField] private Animator m_Legendary_PopUP;
    [SerializeField] private Image m_Item_Frame;
    [SerializeField] private Image m_PopUp_Image;
    [SerializeField] private TextMeshProUGUI m_PopUp_Text;
    Coroutine Legendary_Coroutine;
    bool isPopUP = false;

    [Space(20f)]
    [Header("## Item_PopUP")]
    [SerializeField] private Transform m_ItemContent;
    private List<TextMeshProUGUI> m_Item_Texts = new List<TextMeshProUGUI>();
    private List<Coroutine> m_Item_Coroutines = new List<Coroutine>();

    [Space(20f)]
    [Header("## Hero_Frame")]
    [SerializeField] private UI_Main_Part[] m_Main_Parts;
    public Image Main_Character_Skill_Fill;
    Dictionary<Player, UI_Main_Part> m_Part = new Dictionary<Player, UI_Main_Part>();

    [Header("## ADS")]
    [SerializeField] private Image Fast_Lock;
    [SerializeField] private GameObject Fase_Fade;
    [SerializeField] private GameObject[] Buffs_Lock;
    [SerializeField] private Image x2Fill;
    [SerializeField] private TextMeshProUGUI x2Text;

    [Header("## LayerButtons")]
    [SerializeField] private Transform[] Layer_images; 
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        TextCheck();
        Monster_Slider_Count();

        Base_Mng.isFast = PlayerPrefs.GetInt("FAST") == 1 ? true : false;
        TimeCheck();
        BuffCheck();
        for (int i = 0; i < m_ItemContent.childCount; i++)
        {
            m_Item_Texts.Add(m_ItemContent.GetChild(i).GetComponent<TextMeshProUGUI>());
            m_Item_Coroutines.Add(null);
        }

        Stage_Mng.m_ReadyEvent += OnReady;
        Stage_Mng.m_BossEvent += OnBoss;
        Stage_Mng.m_ClearEvent += OnClear;
        Stage_Mng.m_DeadEvent += OnDead;
        Stage_Mng.m_DungeonEvent += OnDungeon;
        Stage_Mng.m_DungeonClearEvent += OnDungeonClear;
        Stage_Mng.m_DungeonFailEvent += OnDungeonFail;

        Stage_Mng.State_Change(Stage_State.Ready);
    }

    private void Update()
    {
        if(Data_Mng.m_Data.Buff_x2 > 0.0f)
        {
            x2Fill.fillAmount = Data_Mng.m_Data.Buff_x2 / 1800.0f;
            x2Text.text = Utils.GetTimer(Data_Mng.m_Data.Buff_x2);
        }
    }

    public void LayerCheck(int value)
    {
        if (value != -1)
        {
            StartCoroutine(imageMoveCoroutine(value));
            Layer_images[value].transform.GetChild(0).gameObject.SetActive(true);
        }

        for (int i = 0; i < Layer_images.Length; i++)
        {
            if(value != i && Layer_images[i].localScale.x >= 1.3f)
            {
                StartCoroutine(imageMoveCoroutine(i, true));
                Layer_images[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator imageMoveCoroutine(int value, bool ScaleDown = false)
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = ScaleDown ? 50.0f : 25.0f; // yPosition
        float end = ScaleDown ? 25.0f : 50.0f; // yPosition
        float startScale = ScaleDown ? 1.5f : 1.0f;
        float endScale = ScaleDown ? 1.0f : 1.5f;
        while(percent < 1.0f)
        {
            current += Time.unscaledDeltaTime;
            percent = current / 0.2f;
            float yPos = Mathf.Lerp(start, end, percent);
            float ScalePos = Mathf.Lerp(startScale, endScale, percent);
            Layer_images[value].localPosition = new Vector2(0.0f, yPos);
            Layer_images[value].localScale = new Vector3(ScalePos, ScalePos, 1.0f);
            yield return null;
        }
    }

    
    // PlayerPrefs
    
    public void BuffCheck()
    {
        for(int i = 0; i < Data_Mng.m_Data.Buff_timers.Length; i++)
        {
            if (Data_Mng.m_Data.Buff_timers[i] > 0.0f)
            {
                Buffs_Lock[i].SetActive(false);
            }
            else Buffs_Lock[i].SetActive(true);
        }
        if (Data_Mng.m_Data.Buff_x2 > 0.0f)
        {
            x2Fill.transform.parent.gameObject.SetActive(true);
        }
        else x2Fill.transform.parent.gameObject.SetActive(false);
    }

    private void TimeCheck()
    {
        Time.timeScale = Base_Mng.isFast ? 1.5f : 1.0f;
        Fast_Lock.gameObject.SetActive(!Base_Mng.isFast);
        Fase_Fade.SetActive(Base_Mng.isFast);
    }
    public void GetFast()
    {
        bool fast = !Base_Mng.isFast;
        if(fast == true)
        {
            if (Data_Mng.m_Data.Buff_x2 <= 0.0f)
            {
                Base_Mng.ADS.ShowRewardedAds(() =>
                {
                    Data_Mng.m_Data.Buff_x2 = 1800.0f;
                    BuffCheck();
                    TimeCheck();
                });
            }
        }

        Base_Mng.isFast = fast;
        PlayerPrefs.SetInt("FAST", fast == true ? 1 : 0);

        BuffCheck();
        TimeCheck();
    }

    public void GetItem(Item_Scriptable item)
    {
        bool AllActive = true;

        for(int i = 0; i< m_Item_Texts.Count; i++)
        {
            if (m_Item_Texts[i].gameObject.activeSelf == false)
            {
                m_Item_Texts[i].gameObject.SetActive(true);
                m_Item_Texts[i].text = string.Format(Local_Mng.local_Data["PopUP/GetItem"].Get_Data(), Utils.String_Color_Rarity(item.rarity) + "[" + item.Item_Name + "]</color>");

                for (int j = 0; j < i; j++)
                {
                    RectTransform rect = m_Item_Texts[j].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + 50.0f);
                }

                if (m_Item_Coroutines[i] != null)
                {
                    StopCoroutine(m_Item_Coroutines[i]);
                }
                m_Item_Coroutines[i] = StartCoroutine(Item_Text_FadeOut(m_Item_Texts[i].GetComponent<RectTransform>()));
                AllActive = false;
                break;
            }
        }

        if(AllActive)
        {
            GameObject BaseRect = null;
            float yCount = 0.0f;
            for(int i = 0; i < m_Item_Texts.Count; i++)
            {
                RectTransform rect = m_Item_Texts[i].GetComponent<RectTransform>();
                if(rect.anchoredPosition.y > yCount)
                {
                    BaseRect = rect.gameObject;
                    yCount = rect.anchoredPosition.y;
                }
            }

            for (int i = 0; i < m_Item_Texts.Count; i++)
            {
                if (BaseRect == m_Item_Texts[i].gameObject)
                {
                    m_Item_Texts[i].gameObject.SetActive(false);
                    m_Item_Texts[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

                    m_Item_Texts[i].gameObject.SetActive(true);
                    m_Item_Texts[i].text = string.Format(Local_Mng.local_Data["PopUP/GetItem"].Get_Data(), Utils.String_Color_Rarity(item.rarity) + "[" + item.Item_Name + "]</color>");

                    if (m_Item_Coroutines[i] != null)
                    {
                        StopCoroutine(m_Item_Coroutines[i]);
                    }
                    m_Item_Coroutines[i] = StartCoroutine(Item_Text_FadeOut(m_Item_Texts[i].GetComponent<RectTransform>()));
                }
                else
                {
                    RectTransform rect = m_Item_Texts[i].GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(0.0f, rect.anchoredPosition.y + 50.0f);
                }
            }
        }

        if ((int)item.rarity >= (int)Rarity.Hero)
        {
            GetLegendaryPopUP(item);
        }
    }

    IEnumerator Item_Text_FadeOut(RectTransform rect)
    {
        yield return new WaitForSeconds(2.0f);
        rect.gameObject.SetActive(false);
        rect.anchoredPosition = new Vector2(0.0f, 0.0f);
    }

    public void Set_Boss_State()
    {
        Stage_Mng.isDead = false;
        Stage_Mng.State_Change(Stage_State.Boss);
    }

    private void SliderOBJCheck(SliderType type)
    {
        m_Monster_Slider_OBJ.SetActive(false);
        m_Boss_Slider_OBJ.SetActive(false);
        m_Dungeon_Slider_OBJ.SetActive(false);

        if (Stage_Mng.isDead)
        {
            Dead_Frame.SetActive(true);
            return;
        }
        switch(type)
        {
            case SliderType.Default: 
                m_Monster_Slider_OBJ.SetActive(true);
                Monster_Slider_Count();
                break;
            case SliderType.Boss:
                m_Boss_Slider_OBJ.SetActive(true);
                break;
            case SliderType.Dungeon:
                m_Dungeon_Slider_OBJ.SetActive(true);
                
                DungeonCoroutine = StartCoroutine(Dungeon_Slider_Coroutine());
                break;
        }
        Dead_Frame.SetActive(false);
        
        // MonsterSlider
        
        float value = type == SliderType.Default ? 0.0f: 1.0f;
        Boss_Slider_Count(value, 1.0f);
    }

    private void OnReady()
    {
        FadeInOut(true);
        Monster_Slider_Count();
        PartInitalize();
    }

    private void PartInitalize()
    {
        m_Part.Clear();

        for (int i = 0; i < 6; i++) m_Main_Parts[i].Initalize();
        int indexValue = 0;
        for (int i = 0; i < Base_Mng.Character.m_Set_Character.Length; i++)
        {
            var data = Base_Mng.Character.m_Set_Character[i];
            if (data != null)
            {
                indexValue++;
                m_Main_Parts[i].InitData(data.Data, false);
                m_Main_Parts[i].transform.SetSiblingIndex(indexValue);
                m_Part.Add(Character_Spawner.players[i], m_Main_Parts[i]);
            }
        }
    }

    public void Set_Character_Data()
    {
        int indexValue = 0;
        for (int i = 0; i < Base_Mng.Character.m_Set_Character.Length; i++)
        {
            var data = Base_Mng.Character.m_Set_Character[i];
            if (data != null)
            {
                indexValue++;
                m_Main_Parts[i].InitData(data.Data, true);
                m_Main_Parts[i].transform.SetSiblingIndex(indexValue);
            }
        }
    }

    public void Character_State_Check(Player player)
    {
        m_Part[player].StateCheck(player);
    }

    private void OnBoss()
    {
        TextCheck();
        SliderOBJCheck(SliderType.Boss);
    }

    private void OnClear()
    {
        SliderOBJCheck(SliderType.Default);
        StartCoroutine(Clear_Delay());
    }

    private void OnDungeonClear(int value)
    {
        if (DungeonCoroutine != null)
        {
            StopCoroutine(DungeonCoroutine);
            DungeonCoroutine = null;
        }
        int level = Stage_Mng.DungeonLevel;
        
        if (Stage_Mng.DungeonLevel == Data_Mng.m_Data.Dungeon_Clear_Level[value])
         Data_Mng.m_Data.Dungeon_Clear_Level[value]++;

        if (Data_Mng.m_Data.Key[value] > 0) Data_Mng.m_Data.Key[value]--;
        else Data_Mng.m_Data.KeyAssets[value]--;

        switch (value)
        {
            case 0:
                Data_Mng.m_Data.Dia += (level + 1) * 50;
                break;
            case 1:
                Data_Mng.m_Data.Money += Utils.Data.stageData.MONEY((level + 1) * 5 * 10);
                break;
        }

        TextCheck();
        Base_Canvas.instance.Get_Toast().Initalize("던전 공략에 성공하였습니다!", Color.white);
        OnClear();
    }

    private void OnDungeon(int value)
    {
        for (int i = 0; i < Dungeon_Value_OBJS.Length; i++) Dungeon_Value_OBJS[i].SetActive(false);
        m_MonsterSlider_Fill.fillAmount = 1.0f;
        m_MonsterSlider_Text.text = "100%";
        MonsterCountText.text = "30";

        FadeInOut(true, true);
        PartInitalize();
        Dungeon_Value_OBJS[value].gameObject.SetActive(true);
        SliderOBJCheck(SliderType.Dungeon);
    }

    private void OnDead()
    {
        TextCheck();
        StartCoroutine(Dead_Delay());
    }

    private void OnDungeonFail()
    {
        if (DungeonCoroutine != null)
        {
            StopCoroutine(DungeonCoroutine);
            DungeonCoroutine = null;
        }
        TextCheck();
        StartCoroutine(Dead_Delay());
        Base_Canvas.instance.Get_Toast().Initalize("던전 공략에 실패하였습니다.", Color.red);
    }

    IEnumerator Dead_Delay()
    {
        yield return StartCoroutine(Clear_Delay());
        SliderOBJCheck(SliderType.Default);
        
        for(int i = 0; i < Spawner.m_Monsters.Count; i++)
        {
            if (Spawner.m_Monsters[i].isBoss == true)
            {
                Destroy(Spawner.m_Monsters[i].gameObject);
            }
            else
            {
                Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(Spawner.m_Monsters[i].gameObject);
            }
        }
        Spawner.m_Monsters.Clear();
    }

    IEnumerator Clear_Delay()
    {
        yield return new WaitForSeconds(2.0f);
        FadeInOut(false);

        yield return new WaitForSeconds(1.0f);
        Stage_Mng.State_Change(Stage_State.Ready);
    }

    public void Monster_Slider_Count()
    {
        float value = (float)Stage_Mng.Count / (float)Stage_Mng.MaxCount;

        if (value >= 1.0f)
        {
            value = 1.0f;
            
            if(Stage_Mng.m_State != Stage_State.Boss)
                Stage_Mng.State_Change(Stage_State.Boss);
        }

        m_Monster_Slider.fillAmount = value;
        m_Monster_Value_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
    }

    public void Dungeon_Monster_Slider_Count()
    {
        MonsterCountText.text = "("+ Stage_Mng.DungeonCount.ToString() +")";

        if(Stage_Mng.DungeonCount <= 0)
        {
            Stage_Mng.State_Change(Stage_State.DungeonClear, Stage_Mng.DungeonType);
        }
    }

    public void Boss_Slider_Count(double hp, double MaxHp)
    {
        float value = (float)hp / (float)MaxHp;
        Debug.Log(value + " : " + hp + " ; " + MaxHp);
        if(value <= 0.0f)
        {
            value = 0.0f;
        }
        if (!Stage_Mng.isDungeon)
        {
            m_Boss_Slider.fillAmount = value;
            m_Boss_Value_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
        }
        else
        {
            m_MonsterSlider_Fill.fillAmount = value;
            m_MonsterSlider_Text.text = string.Format("{0:0.0}", value * 100.0f) + "%";
        }
    }

    IEnumerator Dungeon_Slider_Coroutine()
    {
        float t = 60.0f;
        while(t >= 0.0f)
        {
            t -= Time.deltaTime;
            m_Dungeon_Slider.fillAmount = t / 60.0f;
            m_Dungeon_Value_Text.text = string.Format("{0:0.00}/s", t);
            yield return null;
        }
        Stage_Mng.State_Change(Stage_State.DungeonFail);
    }


    public void FadeInOut(bool FadeInOut, bool Sibling = false, Action action = null)
    {
        if(!Sibling)
        {
            m_Fade.transform.parent = this.transform;
            m_Fade.transform.SetSiblingIndex(0);
        }
        else
        {
            m_Fade.transform.parent = Base_Canvas.instance.transform;
            m_Fade.transform.SetAsLastSibling();
        }

        StartCoroutine(FadeInOut_Coroutine(FadeInOut, action));
    }

    IEnumerator FadeInOut_Coroutine(bool FadeInOut, Action action = null)
    {
        if(FadeInOut == false)
        {
            m_Fade.raycastTarget = true;
        }

        float current = 0.0f;
        float percent = 0.0f;
        float start = FadeInOut ? 1.0f : 0.0f;
        float end = FadeInOut ? 0.0f : 1.0f;

        while(percent < 1.0f)
        {
            current += Time.deltaTime;
            percent = current / m_FadeDuration;
            float LerpPos = Mathf.Lerp(start, end, percent);
            m_Fade.color = new Color(0, 0, 0, LerpPos);

            yield return null;
        }

        if (action != null) action?.Invoke();

        m_Fade.raycastTarget = false;
    }

    public void TextCheck()
    {
        m_Level_Text.text = "LV." + (Data_Mng.m_Data.Level + 1).ToString();
        m_ALLATK_Text.text = StringMethod.ToCurrencyString(Base_Mng.Player.Average_ATK());

        double LevelUpMoneyValue = Utils.Data.levelData.MONEY();

        m_LevelUp_Money_Text.text = StringMethod.ToCurrencyString(LevelUpMoneyValue);
        m_LevelUp_Money_Text.color = Utils.Coin_Check(LevelUpMoneyValue) ? Color.green : Color.red;

        m_Money_Text.text = StringMethod.ToCurrencyString(Data_Mng.m_Data.Money);

        m_Stage_Text.text = Stage_Mng.isDead ? "반복중..." : "진행중...";
        m_Stage_Text.color = Stage_Mng.isDead ? Color.yellow : m_Stage_Color;

        int stageValue = Data_Mng.m_Data.Stage + 1;
        int stageForward = (stageValue / 10) + 1;
        int stageBack = stageValue % 10;

        m_Stage_Count_Text.text = "보통 " + stageForward.ToString() + " - " + stageBack.ToString();
        m_Dia_Text.text = Data_Mng.m_Data.Dia.ToString();
    }

    private void GetLegendaryPopUP(Item_Scriptable item)
    {
        if (isPopUP)
        {
            m_Legendary_PopUP.gameObject.SetActive(false);
        }
        isPopUP = true;
        m_Legendary_PopUP.gameObject.SetActive(true);

        m_Item_Frame.sprite = Utils.Get_Atlas(item.rarity.ToString());

        m_PopUp_Image.sprite = Utils.Get_Atlas(item.name);
        m_PopUp_Image.SetNativeSize();

        m_PopUp_Text.text = string.Format(Local_Mng.local_Data["PopUP/GetItem02"].Get_Data(), Utils.String_Color_Rarity(item.rarity) + item.Item_Name);

        if (Legendary_Coroutine != null)
        {
            StopCoroutine(Legendary_Coroutine);
        }
        Legendary_Coroutine = StartCoroutine(Legendary_PopUp_Coroutine());
    }

    IEnumerator Legendary_PopUp_Coroutine()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        isPopUP = false;
        m_Legendary_PopUP.SetTrigger("Close");
    }
}
