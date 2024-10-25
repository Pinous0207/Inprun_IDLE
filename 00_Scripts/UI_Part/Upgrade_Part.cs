using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade_Part : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Level, UpgradeLevel;
    [SerializeField] private Image CharacterIcon;
    [SerializeField] private Animator anim;
    public void Init(Character_Holder holder, int BeforeLevel, int level)
    {
        Level.text = BeforeLevel.ToString();
        UpgradeLevel.text = level.ToString();
        CharacterIcon.sprite = Utils.Get_Atlas(holder.Data.name);
        CharacterIcon.SetNativeSize();
        anim.SetTrigger("Default");
    }
}
