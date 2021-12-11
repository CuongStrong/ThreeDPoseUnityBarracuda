using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;

public partial class WinMenu : BaseMenu
{
    // [Header("UI")]
    // [SerializeField] Transform coinTarget;
    // [SerializeField] CanvasGroup btnAdsCanvasGroup, btnNextCanvasGroup;
    // [SerializeField] Text tittleText, coinTotalText, coinNormalText, coinDoubleText;

    // [Header("Coin")]
    // [SerializeField] [Range(0.1f, 0.9f)] float minAnimDuration = 0.2f;
    // [SerializeField] [Range(0.2f, 2f)] float maxAnimDuration = 0.6f;
    // [SerializeField] float spread = 200;

    // private int maxCoins;

    // private int _bonusCoin;
    // public int bonusCoin
    // {
    //     get => _bonusCoin;
    //     set
    //     {
    //         _bonusCoin = value;
    //         coinNormalText.text = _bonusCoin.ToString();
    //         coinDoubleText.text = (_bonusCoin * 2).ToString();
    //     }
    // }

    // private void OnEnable()
    // {
    //     OnEnablePartial();
    //     DataSave.OnCoinChanged += OnCoinChanged;
    //     OnCoinChanged(DataSave.Instance.coin);

    //     maxCoins = PoolManager.Instance.pools[PoolNames.COIN_FLYING].count;

    //     int lastLevel = DataSave.Instance.level - 1;
    //     tittleText.text = string.Format("LEVEL {0} CLEAR!", lastLevel);
    // }

    // private void OnDisable()
    // {
    //     OnDisablePartial();
    //     btnNextCanvasGroup.DOFade(1, 0);
    //     btnNextCanvasGroup.interactable = true;

    //     DataSave.OnCoinChanged -= OnCoinChanged;
    // }

    // // private void OnEnablePartial() { }
    // // private void OnDisablePartial() { }


    // public override void Show(Action callback = null)
    // {
    //     base.Show(callback);
    //     VibrationManager.Haptic(HapticType.Success, true);
    // }

    // void OnCoinChanged(int coin)
    // {
    //     if (coinTotalText != null) coinTotalText.text = coin.ToString();
    // }

    // public void ButtonNextOnClick()
    // {
    //     VideoSuccessCallback(false);
    //     PlayButtonSfx();
    // }

    // public void ButtonAdsOnClick() => PlayButtonSfx();
    // public void VideoSuccessCallback(bool success)
    // {
    //     btnAdsCanvasGroup.DOFade(0, 0.1f);
    //     btnAdsCanvasGroup.interactable = false;
    //     btnNextCanvasGroup.DOFade(0, 0.1f);
    //     btnNextCanvasGroup.interactable = false;

    //     if (success)
    //     {
    //         ShowCoinAnimation(coinDoubleText.transform.position, bonusCoin * 2);
    //     }
    //     else
    //     {
    //         ShowCoinAnimation(coinNormalText.transform.position, bonusCoin);
    //     }

    //     this.Invoke(maxAnimDuration + 1f, () =>
    //     {
    //         SaveGameManager.Instance.Save();
    //         Define.ReloadScene();
    //     });
    // }

    // void ShowCoinAnimation(Vector3 sourcePos, int amount)
    // {
    //     int step = Mathf.Max(1, amount / maxCoins);
    //     int split = Mathf.Min(amount / step, maxCoins);
    //     int odd = amount - split * step;
    //     Vector3 targetPosition = coinTarget.transform.position;

    //     float pitch = 1;

    //     for (int i = 0; i < split; i++)
    //     {
    //         var coinFlying = PoolManager.Instance.pools[PoolNames.COIN_FLYING].Spawn(transform);

    //         var sq = DOTween.Sequence();
    //         sq.Append(coinFlying.transform.DOMove(sourcePos, 0));
    //         sq.Append(coinFlying.transform.DOMove(sourcePos + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread)), 0.2f).SetEase(Ease.OutBack));
    //         sq.AppendInterval(Random.Range(minAnimDuration, maxAnimDuration));
    //         sq.AppendCallback(() =>
    //         {
    //             coinFlying.transform.DOMove(targetPosition, 0.2f).SetEase(Ease.Linear)
    //             .OnComplete(() =>
    //             {
    //                 PoolManager.Instance.Despawn(coinFlying);

    //                 coinTarget.transform.DOPunchScale(Vector3.one * 0.1f, 0.05f)
    //                 .OnComplete(() => coinTarget.transform.localScale = Vector3.one);

    //                 DataSave.Instance.coin += step + (odd-- > 0 ? 1 : 0);
    //                 VibrationManager.Haptic(HapticType.LightImpact);

    //                 pitch += 0.2f;
    //                 AudioManager.Instance.PlaySFX(ResourcesPath.AUDIO_COIN_COLLECT, pitch);
    //             });
    //         });
    //     }
    // }
}
