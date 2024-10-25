using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopUp_Handler : MonoBehaviour, IPointerDownHandler
{
    private Item_Scriptable item;

    public void Init(Item_Scriptable item_Data)
    {
        item = item_Data;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Base_Canvas.instance.PopUPItem().Item_PopUp(item, eventData.position);
    }
}
