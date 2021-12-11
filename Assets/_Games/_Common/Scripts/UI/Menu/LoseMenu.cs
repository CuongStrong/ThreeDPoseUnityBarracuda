using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public partial class LoseMenu : BaseMenu
{
    public Action videoSuccessCallback;

    public AdsButton adsButton => GetComponentInChildren<AdsButton>();

    public override void Show(Action callback = null)
    {
        base.Show(callback);
        audioManager.PlaySFX(ResourcesPath.AUDIO_LOSE);
        VibrationManager.Haptic(HapticType.Failure, true);
    }

    public void ButtonRetryOnClick()
    {
        PlayButtonSfx();
        VoodooTracking.Instance.OnGameFinished(false, 0, DataSave.Instance.level);
        Define.ReloadScene();
    }

    public void ButtonAdsOnClick() => PlayButtonSfx();

    public void VideoSuccessCallback(bool success)
    {
        if (success)
        {
            videoSuccessCallback?.Invoke();
        }
        else
        {
            ButtonRetryOnClick();
        }
    }
}
