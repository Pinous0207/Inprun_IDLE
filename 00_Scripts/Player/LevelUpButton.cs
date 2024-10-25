using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class LevelUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image m_EXP_Slider;
    [SerializeField] private TextMeshProUGUI EXP_Text, ATK_Text, GoldText, HP_Text, Get_EXP_Text;
    bool isPush = false;
    float timer = 0.0f;
    Coroutine coroutine;

    void Start()
    {
        InitEXP();
    }

    void Update()
    {
        if(isPush)
        {
            timer += Time.deltaTime;
            if(timer >= 0.01f)
            {
                timer = 0.0f;
                EXP_UP();
            }
        }
    }

    public void EXP_UP()
    {
        Base_Mng.Player.EXP_UP();
        InitEXP();
        transform.DORewind();
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EXP_UP();
        coroutine = StartCoroutine(Push_Coroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPush = false;
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        timer = 0.0f;
    }

    private void InitEXP()
    {
        m_EXP_Slider.fillAmount = Base_Mng.Player.EXP_Percentage();
        EXP_Text.text = string.Format("{0:0.00}", Base_Mng.Player.EXP_Percentage() * 100.0f) + "%";
        ATK_Text.text = "+" + StringMethod.ToCurrencyString(Utils.Data.levelData.ATK());
        HP_Text.text = "+" + StringMethod.ToCurrencyString(Utils.Data.levelData.HP());
        Get_EXP_Text.text = "<color=#00FF00>EXP</color> +" + string.Format("{0:0.00}", Base_Mng.Player.Next_EXP()) + "%"; 
    }

    IEnumerator Push_Coroutine()
    {
        yield return new WaitForSeconds(1.0f);
        isPush = true;
    }
}
