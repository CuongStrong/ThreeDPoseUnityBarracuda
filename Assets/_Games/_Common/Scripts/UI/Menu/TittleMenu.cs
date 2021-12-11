using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;


public partial class TittleMenu : BaseMenu
{
    [SerializeField] Transform coinRoot;
    [SerializeField] Text textCoin, textLevel;
    [SerializeField] Button settingButton, removeAdsButton;
    [SerializeField] Text tapToStartText;

    public Action tapToStartCallback;
    private Sequence sequenceTapToStart;

    void Start()
    {
        settingButton.onClick.AddListener(() =>
        {
            UIManager.Instance.GetPopup(ResourcesPath.PREFAB_SETTING_POPUP).Show();
            PlayButtonSfx();
        });

        removeAdsButton.onClick.AddListener(() =>
        {
            UIManager.Instance.GetPopup(ResourcesPath.PREFAB_REMOVE_ADS_POPUP).Show();
            PlayButtonSfx();
        });
    }

    private void OnEnable()
    {
        // OnEnablePartial();

        TapToStartButton.onPointerDown += onPointerDown;
        DataSave.OnCoinChanged += OnCoinChanged;
        DataSave.OnAdsRemoved += OnAdsRemoved;

        OnAdsRemoved(DataSave.Instance.adsRemoved);

        textLevel.text = string.Format("Lv.{0}", DataSave.Instance.level);
        textCoin.text = DataSave.Instance.coin.ToString();

        if (sequenceTapToStart != null) DOTween.Kill(sequenceTapToStart);
        sequenceTapToStart = DOTween.Sequence();
        sequenceTapToStart.Append(tapToStartText.DOFade(0, 0.5f).SetEase(Ease.InOutSine));
        sequenceTapToStart.Append(tapToStartText.DOFade(1, 0.5f).SetEase(Ease.InOutSine));
        sequenceTapToStart.SetLoops(-1);
    }

    private void OnDisable()
    {
        // OnDisablePartial();

        TapToStartButton.onPointerDown -= onPointerDown;
        DataSave.OnCoinChanged -= OnCoinChanged;
        DataSave.OnAdsRemoved -= OnAdsRemoved;
    }

    // private void OnEnablePartial() { }
    // private void OnDisablePartial() { }

    void onPointerDown()
    {
        tapToStartCallback?.Invoke();

    }

    void OnCoinChanged(int coin)
    {
        textCoin.text = coin.ToString();
        coinRoot.DOPunchScale(Vector3.one * 0.1f, 0.1f).OnComplete(() => coinRoot.localScale = Vector3.one);
    }

    void OnAdsRemoved(bool remove)
    {
        if (remove)
        {
            removeAdsButton.gameObject.SetActive(false);
        }
        else
        {
#if FIREBASE
                var shopRemoveAds = DataManager.Instance.config.shop["removeAds"];
                if (shopRemoveAds != null && shopRemoveAds.enabled)
                {
                    removeAdsButton.gameObject.SetActive(true);
                }
                else
                {
                    removeAdsButton.gameObject.SetActive(false);
                }
#endif
        }
    }
}
