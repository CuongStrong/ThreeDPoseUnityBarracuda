using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Required when using Event data.
using System;

public class ButtonMoveFullScreen : Button, IPointerUpHandler, IPointerDownHandler
{
    public static event Action onPointerDown = delegate { };
    public static event Action onPointerUp = delegate { };

    //OnPointerDown is also required to receive OnPointerUp callbacks
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        onPointerDown.Invoke();
    }

    //Do this when the mouse click on this selectable UI object is released.
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        onPointerUp.Invoke();
    }
}