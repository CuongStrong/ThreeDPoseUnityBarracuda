using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if GAME_ANALYTICS
using GameAnalyticsSDK;
#endif

public class TrackingMangager : MonoBehaviourPersistence<TrackingMangager>
#if GAME_ANALYTICS
, IGameAnalyticsATTListener
#endif
{
    public override void Init()
    {
#if FIREBASE_ANALYTICS
        Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
#endif

#if GAME_ANALYTICS
        //https://gameanalytics.com/docs/s/article/Integration-Unity-SDK#install
        string path = "Config/GameAnalytics";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null) Debug.LogError(string.Format("TrackingMangager Resources/{0} cannot found ", path));
        Instantiate(prefab, transform);

        if (Application.platform == RuntimePlatform.IPhonePlayer)
            GameAnalytics.RequestTrackingAuthorization(this);
        else
            GameAnalytics.Initialize();
#endif

#if TENJIN
        TenjinConnect();
#endif

#if FACEBOOK
        if (!Facebook.Unity.FB.IsInitialized)
        {
            Facebook.Unity.FB.Init(() => FB.ActivateApp());
        }
        else
        {
            Facebook.Unity.FB.ActivateApp();
        }
#endif
    }

    public void LogEvent(string eventId, string name, string prop = "")
    {
#if FIREBASE_ANALYTICS
        Firebase.Analytics.FirebaseAnalytics.LogEvent(eventId, name, prop);
#endif

#if GAME_ANALYTICS
        GameAnalytics.NewDesignEvent(eventId);
#endif
    }

    public void LogEvent(string eventId) => LogEvent(eventId, eventId);

    public void SetUserProperty(string propId, string value)
    {
#if FIREBASE_ANALYTICS
        Firebase.Analytics.FirebaseAnalytics.SetUserProperty(propId, value);
#endif
    }

#if TENJIN
    public void TenjinConnect()
    {
        BaseTenjin instance = Tenjin.getInstance("QENGYAKZDN1V4C9UQUBFLMGPYKIJ8VXU");

#if UNITY_IOS
      instance.RegisterAppForAdNetworkAttribution();
      instance.RequestTrackingAuthorizationWithCompletionHandler((status) => {
        Debug.Log("===> App Tracking Transparency Authorization Status: " + status);
        instance.Connect();
      });

#elif UNITY_ANDROID
        instance.Connect();
#endif
    }
#endif


#if GAME_ANALYTICS
    public void GameAnalyticsATTListenerNotDetermined()
    {
        GameAnalytics.Initialize();
    }
    public void GameAnalyticsATTListenerRestricted()
    {
        GameAnalytics.Initialize();
    }
    public void GameAnalyticsATTListenerDenied()
    {
        GameAnalytics.Initialize();
    }
    public void GameAnalyticsATTListenerAuthorized()
    {
        GameAnalytics.Initialize();
    }
#endif


    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
#if TENJIN
            TenjinConnect();
#endif
        }
    }
}