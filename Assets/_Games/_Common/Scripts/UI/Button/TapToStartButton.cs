using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class TapToStartButton : Button, IPointerUpHandler, IPointerDownHandler
{
    public static event Action onPointerDown = delegate { };
    public static event Action onPointerUp = delegate { };

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        onPointerDown.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onPointerUp.Invoke();
    }
}