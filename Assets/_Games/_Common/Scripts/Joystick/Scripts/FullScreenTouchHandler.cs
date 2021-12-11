using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class FullScreenTouchHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public static event Action<Vector2> OnnTouchBeginDrag = delegate { };
    public static event Action<Vector2, Vector2> OnTouchDrag = delegate { };
    public static event Action<Vector2> OnTouchnEndDrag = delegate { };
    public static event Action<Vector2> OnTouchDown = delegate { };
    public static event Action<Vector2> OnTouchUp = delegate { };

    private float pixelPerUnit;

    protected virtual void Awake()
    {
        var rectTransform = GetComponent<RectTransform>();
        pixelPerUnit = Screen.height / rectTransform.rect.height;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        OnnTouchBeginDrag.Invoke(ConvertToUnitVector(eventData.position));
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        OnTouchDrag.Invoke(ConvertToUnitVector(eventData.position), ConvertToUnitVector(eventData.delta));
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        OnTouchnEndDrag.Invoke(ConvertToUnitVector(eventData.position));
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnTouchDown.Invoke(ConvertToUnitVector(eventData.position));
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        OnTouchUp.Invoke(ConvertToUnitVector(eventData.position));
    }

    protected Vector2 ConvertToUnitVector(Vector2 pixelVector)
    {
        return pixelVector / pixelPerUnit;
    }
}