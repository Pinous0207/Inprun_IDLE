using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        if(Utils.TimerCheck() >= 10.0d)
        {
            Get_UI("#Offline");
        }

        Base_Mng.Sound.Play(Sound.BGM, "Main");

        STATUS_BUTTON.onClick.AddListener(() => Get_UI("#Stat", true, true, true, 0));

        HERO_BUTTON.onClick.AddListener(() => Get_UI("#Heros", true, true, true, 1));

        RELIC_BUTTON.onClick.AddListener(() => Get_UI("#Relic", false, true, true, 2));

        DUNGEON_BUTTON.onClick.AddListener(() => Get_UI("#Dungeon", true, true, true, 3));

        SMLET_BUTTON.onClick.AddListener(() => Get_UI("#Smelt", false, true, true, 4));

        SHOP_BUTTON.onClick.AddListener(() => Get_UI("#Shop", false, true, true, 5));

        INVENTORY_BUTTON.onClick.AddListener(() => Get_UI("#Inventory"));
        SAVINGMODE_BUTTON.onClick.AddListener(() => {
            Get_UI("#SavingMode");
            isSave = true;
        });
        ADSBUFF_BUTTON.onClick.AddListener(() => Get_UI("#ADS_Buff"));
        SETTING_BUTTON.onClick.AddListener(() => Get_UI("#Setting"));
        DAILYQUEST_BUTTON.onClick.AddListener(() => Get_UI("#DailyQuest"));
        ACHIEVEMENT_BUTTON.onClick.AddListener(() => Get_UI("#Achievement"));
    }

    public Transform COIN;
    [SerializeField] private Transform LAYER;
    [SerializeField] private Transform BACK_LAYER;
    
    [SerializeField] private Button 
        STATUS_BUTTON,
        HERO_BUTTON, 
        RELIC_BUTTON , 
        DUNGEON_BUTTON, 
        SMLET_BUTTON,
        SHOP_BUTTON,
        INVENTORY_BUTTON, 
        SAVINGMODE_BUTTON,
        ADSBUFF_BUTTON,
        SETTING_BUTTON,
        DAILYQUEST_BUTTON,
        ACHIEVEMENT_BUTTON;

    [HideInInspector] public PopUp_UI popup = null;
    [HideInInspector] public UI_Base m_UI;
    public static bool isSave = false;
    private void Update()
    {
        // KeyCode.Escape = Window, MacOS -> ESC
        // KeyCode.Escape = Android -> 뒤로가기
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Utils.UI_Holder.Count > 0)
                Utils.ClosePopupUI();
            else
            {
                Get_UI("#BackButton");
            }
        }
    }

    public UI_ToastPopUP Get_Toast()
    {
        return Instantiate(Resources.Load<UI_ToastPopUP>("UI/#PopUp"), transform);
    }
    public Transform HOLDER_LAYER(int value)
    {
        return LAYER.GetChild(value);        
    }
   
    public void Get_UI(string temp, bool Fade = false, bool Back = false, bool Close = false, int value = -1)
    {
        if (Utils.UI_Holder.Count > 0)
        {
            var peekData = Utils.UI_Holder.Peek();
            if (peekData.name == temp)
            {
                Utils.CloseAllPopupUI();
                Main_UI.instance.LayerCheck(-1);
                return;
            }
        }

        if (Close)
        {
            Utils.CloseAllPopupUI();
        }
        if(Fade)
        {
            Main_UI.instance.FadeInOut(false, true,() => GetPopupUI(temp, Back));
            Main_UI.instance.LayerCheck(value);
            return;
        }
        Main_UI.instance.LayerCheck(value);
        Debug.Log(temp);
        GetPopupUI(temp, Back);
    }

    void GetPopupUI(string temp, bool Back = false)
    {
        if (m_UI != null) m_UI = null;

        var go = Instantiate(Resources.Load<UI_Base>("UI/" + temp), Back == true ? BACK_LAYER : transform);
        m_UI = go;
        m_UI.name = temp;
        
        Utils.UI_Holder.Push(go);
    }

    public PopUp_UI PopUPItem()
    {
        if (popup != null) Destroy(popup.gameObject);

        popup = Instantiate(Resources.Load<PopUp_UI>("UI/PopUp_Item"), transform);

        return popup;
    }
}
