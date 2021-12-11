using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BaseButton : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    protected CanvasGroup canvasGroup
    {
        get
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
                if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            return _canvasGroup;
        }
    }

    public virtual void SetInteractable(bool interactable)
    {
        canvasGroup.interactable = interactable;
        canvasGroup.alpha = canvasGroup.interactable ? 1 : 0.7f;
    }

    public virtual void ClickFx(bool active, float punch = 0.2f)
    {
        DOTween.Sequence()
            .Append(transform.DOPunchScale((active ? Vector3.one : Vector3.right) * punch, 0.2f).SetEase(Ease.InOutSine))
            .Append(transform.DOScale(Vector3.one, 0));
    }
}
