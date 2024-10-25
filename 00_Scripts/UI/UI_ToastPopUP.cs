using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ToastPopUP : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ToastPopUpText;
    public void Initalize(string temp, Color color)
    {
        ToastPopUpText.color = color;
        ToastPopUpText.text = temp;

        Destroy(this.gameObject, 2.0f);
    }
}
