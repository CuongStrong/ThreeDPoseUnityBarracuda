using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Required when using Event data.
using System;

public class ButtonAssistance : Button, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] Text LabelText;

    //OnPointerDown is also required to receive OnPointerUp callbacks
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        LabelText.color = Color.gray;
    }

    //Do this when the mouse click on this selectable UI object is released.
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        LabelText.color = Color.white;
    }
}