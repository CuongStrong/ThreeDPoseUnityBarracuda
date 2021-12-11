using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public partial class NoConnectionPopup : BasePopup
{
    public void ButtonOkOnClick()
    {
        PlayButtonSfx();

        if (Utility.isInternetAvailable)
        {
            Hide();
        }
        else
        {
            DOTween.Sequence()
                .Append(boardTranform.DOPunchScale(Vector3.right * 0.1f, 0.2f).SetEase(Ease.InOutSine))
                .Append(boardTranform.DOScale(Vector3.one, 0));
        }
    }
}
