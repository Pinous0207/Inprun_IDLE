using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Inventory_Part : MonoBehaviour
{
    [SerializeField] private Image RarityImage, IconImage;
    [SerializeField] private TextMeshProUGUI CountText;

    public void Init(string name, Holder holder)
    {
        Item_Scriptable scriptable = Base_Mng.Data.m_Data_Item[name];
        RarityImage.sprite = Utils.Get_Atlas(scriptable.rarity.ToString());
        IconImage.sprite = Utils.Get_Atlas(scriptable.name);
        CountText.text = holder.Count.ToString();

        GetComponent<PopUp_Handler>().Init(scriptable);
    }
}
