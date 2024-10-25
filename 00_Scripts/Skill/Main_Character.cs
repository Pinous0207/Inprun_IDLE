using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Character : Skill_Base
{
    float delay = 5.0f;

    Coroutine coroutine = null;
    private void Start()
    {
        Stage_Mng.m_ReadyEvent += OnReady;
    }

    public override void Set_Skill()
    {
        m_Player.isGetSkill = true;
        m_Player.AnimatorChange("isSKILL");

        var character = HP_Check();
        m_Player.transform.LookAt(character.transform.position);
        character.Heal(SkillDamage(110));
        SkillParticle.gameObject.SetActive(true);
        SkillParticle.transform.position = character.transform.position;

        Invoke("ReturnSkill", 1.0f);

        base.Set_Skill();
    }

    public override void ReturnSkill()
    {
        OnReady();
        SkillParticle.gameObject.SetActive(false);
        base.ReturnSkill();
    }

    public void OnReady()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(SkillCoroutine(delay));
    }
    IEnumerator SkillCoroutine(float value)
    {
        float timer = value;
        while(timer > 0.0f)
        {
            timer -= Time.deltaTime;
            Main_UI.instance.Main_Character_Skill_Fill.fillAmount = timer / value;
            yield return null;
        }
        Set_Skill();
    }
}
