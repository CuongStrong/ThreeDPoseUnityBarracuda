using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class BasePopup : BaseMenu
{
    [SerializeField] protected Transform boardTranform;

    public override void Show(Action callback = null)
    {
        if (!isShowing)
        {
            isShowing = true;
            this.callback = callback;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();

            DOTween.Sequence()
                .Append(canvasGroup.DOFade(0.5f, 0))
                .Append(boardTranform.DOScale(0.7f, 0))
                .Append(canvasGroup.DOFade(1, 0.3f))
                .Join(boardTranform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        }
    }

    public override void Hide(Action callback = null)
    {
        if (isShowing)
        {
            isShowing = false;
            this.callback = callback;

            DOTween.Sequence()
                .Append(canvasGroup.DOFade(0, 0.3f))
                .Join(boardTranform.DOScale(0.7f, 0.3f).SetEase(Ease.InBack))
                .AppendCallback(() =>
                {
                    gameObject.SetActive(false);
                    callback?.Invoke();
                });

        }
    }
}