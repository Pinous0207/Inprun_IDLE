using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;
public class UI_Heroes : UI_Base
{
    public Transform Content;
    public GameObject Part;
    public List<UI_Heroes_Part> part = new List<UI_Heroes_Part>();

    Dictionary<string, Character_Scriptable> m_Dictionarys = new Dictionary<string, Character_Scriptable>();
    Character_Scriptable m_Character;

    [Header("## Hero Information")]
    [SerializeField] private GameObject Information;
    [SerializeField] private TextMeshProUGUI NameText, RarityText, DescriptionText;
    [SerializeField] private TextMeshProUGUI DPS, ATK, HP;
    [SerializeField] private TextMeshProUGUI Level, Count;
    [SerializeField] private TextMeshProUGUI Default_Skill;
    [SerializeField] private TextMeshProUGUI SkillName, SkillDescription;
    [SerializeField] private Image CountFill;
    [SerializeField] private Image CharacterIcon, SkillIcon, RarityIcon;

    [SerializeField] private Button Upgrade;

    public void UpgradeButton(Character_Holder holder)
    {
        int value = (holder.holder.Level + 1) * 5;
        if(holder.holder.Count >= value)
        {
            holder.holder.Count -= value;
            holder.holder.Level++;
        }
        GetHeroInformation(holder.Data);

        for(int i = 0; i < part.Count; i++) part[i].Init();
    }

    public void AllUpgrade()
    {
        if(!GetAllUpgrade())
        {
            Base_Canvas.instance.Get_Toast().Initalize("강화할 영웅이 존재하지 않습니다.", Color.white);
            return;
        }
        Base_Canvas.instance.Get_UI("#Upgrade");
        Utils.UI_Holder.Peek().GetComponent<UI_Upgrade>().Initalize(this);
    }

    private bool GetAllUpgrade()
    {
        bool Can = false;
        foreach(var data in Base_Mng.Data.m_Data_Character)
        {
            if(data.Value.holder.Count >= Base_Mng.Character.LevelCount(data.Value))
            {
                Can = true;
            }
        }
        return Can;
    }

    public void GetShop() => Base_Canvas.instance.Get_UI("#Shop", false, true, true);

    public void GetHeroInformation(Character_Scriptable data)
    {
        int value = (Base_Mng.Data.m_Data_Character[data.name].holder.Level + 1) * 5;

        Information.SetActive(true);
        NameText.text = data.m_Character_Name;
        RarityText.text = Utils.String_Color_Rarity(data.m_Rarity) + data.m_Rarity.ToString();
        ATK.text = StringMethod.ToCurrencyString(Base_Mng.Player.Get_ATK(data.m_Rarity, Base_Mng.Data.m_Data_Character[data.name]));
        HP.text = StringMethod.ToCurrencyString(Base_Mng.Player.Get_HP(data.m_Rarity, Base_Mng.Data.m_Data_Character[data.name]));
        Level.text = "Lv." + (Base_Mng.Data.Character_Holder[data.name].Level+1).ToString();
        Count.text = "(" + Base_Mng.Data.Character_Holder[data.name].Count.ToString() + "/" + value.ToString() + ")";
        CountFill.fillAmount = (float)Base_Mng.Data.Character_Holder[data.name].Count / (float)value;
        CharacterIcon.sprite = Utils.Get_Atlas(data.name);
        CharacterIcon.SetNativeSize();

        Upgrade.onClick.RemoveAllListeners();
        Upgrade.onClick.AddListener(() => UpgradeButton(Base_Mng.Data.m_Data_Character[data.name]));
    }

    public void InitButtons()
    {
        for(int i = 0; i< Render_Manager.instance.HERO.Circles.Length; i++)
        {
            int index = i;

            var go = new GameObject("Button").AddComponent<Button>(); // 새로운 오브젝트(Button이라는 이름) 생성, AddCopmonent(Button 컴포넌트를 추가)
            
            go.onClick.AddListener(() => Set_Character_Button(index));

            go.transform.SetParent(this.transform); // 해당 오브젝트를 UI_Heroes 팝업 하단에 자식 오브젝트로 설정
            go.gameObject.AddComponent<Image>(); // Image 컴포넌트를 추가
            go.gameObject.AddComponent<RectTransform>(); // RectTranfsorm컴포넌트를 추가

            RectTransform rect = go.GetComponent<RectTransform>();

            rect.offsetMin = Vector2.zero; // (0,0)
            rect.offsetMax = Vector2.zero;

            rect.sizeDelta = new Vector2(150.0f, 150.0f);
            go.GetComponent<Image>().color = new Color(0, 0, 0, 0.01f);

            go.transform.position = Render_Manager.instance.ReturnScreenPoint(Render_Manager.instance.HERO.Circles[i]);
        }
    }

    public void Set_Character_Button(int value)
    {
        Base_Mng.Character.GetCharacter(value, m_Character.m_Character_Name);

        Initalize();
    }

    public void Initalize()
    {
        Render_Manager.instance.HERO.GetParticle(false);
        Set_Click(null);
        Render_Manager.instance.HERO.InitHero();

        for (int i = 0; i < part.Count; i++) part[i].Get_Character_Check();

        Main_UI.instance.Set_Character_Data();
    }

    // 어떤 캐릭터가 눌렸을 때 나머지 캐릭터의 음영처리를 위함
    public void Set_Click(UI_Heroes_Part s_Part)
    {
        if (s_Part == null)
        {
            for (int i = 0; i < part.Count; i++)
            {
                part[i].LockOBJ.SetActive(false);
                part[i].GetComponent<Outline>().enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < Base_Mng.Character.m_Set_Character.Length; i++)
            {
                var data = Base_Mng.Character.m_Set_Character[i];
                if (data != null)
                {
                    if (data.Data == s_Part.m_Character)
                    {
                        Base_Mng.Character.DisableCharacter(i);
                        Initalize();
                        return;
                    }
                }
            }

            m_Character = s_Part.m_Character;
            for (int i = 0; i < part.Count; i++)
            {
                part[i].LockOBJ.SetActive(true);
                part[i].GetComponent<Outline>().enabled = false;
            }
            s_Part.LockOBJ.SetActive(false);
            s_Part.GetComponent<Outline>().enabled = true;
        }
    }

    public override bool Init()
    {
        InitButtons();

        Render_Manager.instance.HERO.InitHero();

        Main_UI.instance.FadeInOut(true, true, null);

        var Datas = Base_Mng.Data.m_Data_Character;

        foreach(var data in Datas)
        {
            m_Dictionarys.Add(data.Value.Data.m_Character_Name, data.Value.Data);
        }

        var sort_dictionary = m_Dictionarys.OrderByDescending(x => x.Value.m_Rarity);

        int value = 0;

        foreach (var data in sort_dictionary)
        {
            var go = Instantiate(Part, Content).GetComponent<UI_Heroes_Part>();
            value++;
            part.Add(go);
            int index = value;
            go.Initalize(data.Value, this);
        }

        return base.Init();
    }
  
    public void Disable_Fade()
    {
        Main_UI.instance.FadeInOut(false, true, () =>
        {
            Main_UI.instance.FadeInOut(true, false, null);
            Main_UI.instance.LayerCheck(-1);
            Stage_Mng.State_Change(Stage_State.Ready);
            base.DisableOBJ();
        });
    }

}
