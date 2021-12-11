using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

#if FIRESTORE
using DataConfig;
#endif

[System.Serializable]
public class VideoSuccessCallback : UnityEvent<bool> { }

public class AdsButton : BaseButton
{
#if FIRESTORE
    [SerializeField] bool impressFx = true;
    [SerializeField] protected VideoSuccessCallback videoSuccessCallback;

#if FIRESTORE
    protected ConfigType config => DataManager.Instance.config.config;
#endif

    protected Button button;
    protected Sequence buttonSequence;

    protected bool canShowVideo => AdsManager.Instance.CanShowVideo();

    protected void Awake()
    {
        if (!config.enableAds)
            gameObject.SetActive(false);

        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() =>
        {
            ShowRewardVideo();
        });
    }

    protected virtual void OnEnable()
    {
        if (!config.enableAds) return;

        AdsManager.OnRewardVideoLoaded += OnVideoLoaded;
        AdsManager.OnRewardVideoCompleted += OnRewardVideoCompleted;

        EnableSelf(canShowVideo);
    }

    protected virtual void OnDisable()
    {
        if (!config.enableAds) return;

        AdsManager.OnRewardVideoLoaded -= OnVideoLoaded;
        AdsManager.OnRewardVideoCompleted -= OnRewardVideoCompleted;
    }

    protected virtual void OnVideoLoaded()
    {
        EnableSelf(canShowVideo);
    }

    public virtual void EnableSelf(bool isEnable)
    {
        if (isEnable)
        {
            if (impressFx && buttonSequence == null)
            {
                buttonSequence = DOTween.Sequence();
                buttonSequence.Append(transform.DOScale(Vector3.one * 1.05f, 0.6f).SetEase(Ease.OutSine));
                buttonSequence.Append(transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.InSine));
                buttonSequence.SetLoops(-1);
            }

            SetInteractable(true);
        }
        else
        {
            if (impressFx && buttonSequence != null)
            {
                transform.localScale = Vector3.one;
                buttonSequence.Kill();
            }

            SetInteractable(false);
        }
    }

    protected virtual void ShowRewardVideo()
    {
        // AdsManager.OnRewardVideoCompleted += OnRewardVideoCompleted;

        AdsManager.Instance.ShowRewardVideo();
        EnableSelf(false);
    }

    protected virtual void OnRewardVideoCompleted(bool isSuccess)
    {
        // AdsManager.OnRewardVideoCompleted -= OnRewardVideoCompleted;

        EnableSelf(canShowVideo);
        videoSuccessCallback?.Invoke(isSuccess);
    }
#endif
}
