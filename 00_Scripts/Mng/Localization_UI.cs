using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Localization_UI : MonoBehaviour
{
    public string Local_Name;
    TextMeshProUGUI T;
    public string[] Semi_Data;
    
    private void Awake()
    {
        T= GetComponent<TextMeshProUGUI>();
        Set_LocalData();
    }

    public void Set_LocalData()
    {
        if(Local_Name != "")
        {
            string temp = "";
            if(Semi_Data.Length > 0)
                temp = string.Format(Local_Mng.local_Data["UI/" + Local_Name].Get_Data(), Semi_Data);
            else temp = Local_Mng.local_Data["UI/" + Local_Name].Get_Data();
            T.text = temp;
        }
    }
}
