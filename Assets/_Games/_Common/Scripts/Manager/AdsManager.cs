using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class AdsManager : MonoBehaviourPersistence<AdsManager>
#if UNITY_ADS
, IUnityAdsListener
#endif
{
    public static event Action OnRewardVideoLoaded = delegate { };
    public static event Action<bool> OnRewardVideoCompleted = delegate { };
    public static event Action OnInterstialClosed = delegate { };

    public static bool enableAds { get; set; } = true;

    private void Awake()
    {
        DataSave.OnAdsRemoved += OnAdsRemoved;
        OnAdsRemoved(DataSave.Instance.adsRemoved);

#if UNITY_ADS
        if (enableAds)
            InitUnityAds();
#endif
    }

    public void OnDestroy()
    {
#if UNITY_ADS
        if (enableAds)
            CleanUnityAds();
#endif

        DataSave.OnAdsRemoved -= OnAdsRemoved;
    }

    void OnAdsRemoved(bool remove)
    {
        enableAds = !remove;

        ShowBanner(false);
    }

    public bool CanShowVideo()
    {
#if UNITY_ADS
        return enableAds && isUnityRewardedVideoReady;
#else
        return enableAds;
#endif
    }

    public void ShowRewardVideo()
    {
        if (!CanShowVideo()) return;

        bool needCallbackFake = true;

#if UNITY_ADS
        needCallbackFake = !ShowUnityRewardedVideo();
#endif

        if (needCallbackFake)
        {
            DOTween.Sequence()
                .AppendInterval(0.1f)
                .AppendCallback(() => FinishedPlayingRewardVideo(true));
        }
        else
        {
            PauseAudio();
        }
    }

    public bool CanShowInterstial()
    {
#if UNITY_ADS
        return enableAds && isUnityInterstialReady;
#else
        return enableAds;
#endif
    }

    public void ShowInterstial()
    {
        if (!CanShowInterstial()) return;

        bool needCallbackFake = true;

#if UNITY_ADS
        needCallbackFake = !ShowUnityInterstial();
#endif

        if (needCallbackFake)
        {
            DOTween.Sequence()
                .AppendInterval(0.1f)
                .AppendCallback(() => FinishPlayingInterstial());
        }
        else
        {
            PauseAudio();
        }
    }

    public void ShowBanner(bool show)
    {
#if UNITY_ADS
        if (enableAds)
        {
            ShowUnityBanner(show);
        }
#endif
    }

    public void ShowBannerSync()
    {
#if UNITY_ADS
        if (enableAds)
        {
            StartCoroutine(ShowUnityBannerSync());
        }
#endif
    }

    public void FinishLoadRewardVideo()
    {
        OnRewardVideoLoaded.Invoke();
    }

    public void FinishedPlayingRewardVideo(bool isSuccess)
    {
        ResumeAudio();
        OnRewardVideoCompleted.Invoke(isSuccess);
    }

    public void FinishPlayingInterstial()
    {
        ResumeAudio();
        OnInterstialClosed.Invoke();
    }

    public void PauseAudio() => AudioManager.Instance.PauseAudio();
    public void ResumeAudio() => AudioManager.Instance.ResumeAudio();

#if UNITY_ADS
    private bool testMode = true;
    private string gameId, rewardvideoSurfacingId, interstialSurfacingId, bannerSurfacingId;

    bool isUnityRewardedVideoReady => Advertisement.IsReady(rewardvideoSurfacingId);
    bool isUnityInterstialReady => Advertisement.IsReady(interstialSurfacingId);
    bool isUnityBannerReady => Advertisement.IsReady(bannerSurfacingId);

    public void InitUnityAds()
    {
        bool adsConfigAvailable = false;

#if FIRESTORE
        if (DataManager.Instance.isReady)
        {
            var unityAdsConfig = DataManager.Instance.config.ads["unityAds"];
            if (unityAdsConfig == null)
            {
                Debug.LogError("No config for unityAds");
                return;
            }
            else
            {
                enableAds &= unityAdsConfig.enabled;
                if (!enableAds) return;

                testMode = unityAdsConfig.testMode;
                rewardvideoSurfacingId = unityAdsConfig.rewardVideoId;
                interstialSurfacingId = unityAdsConfig.interstitialId;
                bannerSurfacingId = unityAdsConfig.bannerId;

#if UNITY_ANDROID
                gameId = unityAdsConfig.androidGameId;
#else
                gameId = unityAdsConfig.iosGameId;
#endif

                adsConfigAvailable = true;
            }
        }
#endif

        if (!adsConfigAvailable)
        {
            testMode = true;
            gameId = "4173309";
            rewardvideoSurfacingId = "rewardedVideo";
            interstialSurfacingId = "video";
            bannerSurfacingId = "banner";
        }

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }

    bool ShowUnityRewardedVideo()
    {
        if (isUnityRewardedVideoReady)
        {
            Advertisement.Show(rewardvideoSurfacingId);

            return true;
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }

        return false;
    }

    bool ShowUnityInterstial()
    {
        if (isUnityInterstialReady)
        {
            Advertisement.Show(interstialSurfacingId);

            return true;
        }
        else
        {
            Debug.Log("Interstial is not ready at the moment! Please try again later!");
        }

        return false;
    }

    bool ShowUnityBanner(bool show)
    {
        if (isUnityBannerReady)
        {
            if (show)
                Advertisement.Banner.Show(bannerSurfacingId);
            else
                Advertisement.Banner.Hide(false);

            return true;
        }
        else
        {
            Debug.Log("Banner is not ready at the moment! Please try again later!");
        }

        return false;
    }

    IEnumerator ShowUnityBannerSync()
    {
        while (!isUnityBannerReady)
        {
            yield return new WaitForSeconds(1f);
        }

        ShowUnityBanner(true);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        if (surfacingId == rewardvideoSurfacingId)
        {
            if (showResult == ShowResult.Finished)
            {
                FinishedPlayingRewardVideo(true);
                Debug.Log("OnUnityAdsDidFinish  rewardvideoSurfacingId Finished");
            }
            else if (showResult == ShowResult.Skipped)
            {
                FinishedPlayingRewardVideo(false);
                Debug.Log("OnUnityAdsDidFinish  rewardvideoSurfacingId Skipped");
            }
            else
            {
                Debug.LogWarning("The ad did not finish due to an error.");
                FinishedPlayingRewardVideo(false);
            }
        }
        else if (surfacingId == interstialSurfacingId)
        {
            FinishPlayingInterstial();
            Debug.Log("OnUnityAdsDidFinish  interstialSurfacingId");
        }
    }

    public void OnUnityAdsReady(string surfacingId)
    {
        if (surfacingId == rewardvideoSurfacingId)
        {
            FinishLoadRewardVideo();
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    public void CleanUnityAds()
    {
        Advertisement.RemoveListener(this);
    }
#endif

}