using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Hunter : Skill_Base
{
    public override void Set_Skill()
    {
        base.Set_Skill();
        StartCoroutine(Set_Skill_Coroutine());
    }

    IEnumerator Set_Skill_Coroutine()
    {
        m_Player.ATK_Speed = 2.0f;
        SkillParticle.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        m_Player.ATK_Speed = 1.0f;
        SkillParticle.SetActive(false);
        ReturnSkill();
    }
}
