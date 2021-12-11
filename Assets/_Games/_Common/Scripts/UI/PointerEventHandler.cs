using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerEventHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool debug;
    public UnityEvent
    onPointerClick,
    onPointerDown,
    onPointerUp,
    onPointerEnter,
    onPointerExit;

    public void OnPointerClick(PointerEventData eventData)
    {
        onPointerClick.Invoke();
        if (debug)
            Debug.Log("Clicked " + name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp.Invoke();
        if (debug)
            Debug.Log("Released " + name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown.Invoke();
        if (debug)
            Debug.Log("Pressed " + name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter.Invoke();
        if (debug)
            Debug.Log("Entered " + name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit.Invoke();
        if (debug)
            Debug.Log("Exited " + name);
    }
}