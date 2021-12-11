using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapSceneController : BaseSceneController
{
    public static bool isReady;

    private void Awake()
    {
        SaveGameManager.Instance.Init();
        PoolManager.Instance.Init();
        VibrationManager.Instance.Init();

#if FIRESTORE
        DataManager.Instance.Init();
#else
        Ready();
#endif
    }

    private void OnEnable()
    {
#if FIRESTORE
        DataManager.OnDataManagerStateChanged += OnDataManagerStateChanged;
#endif
    }

    private void OnDisable()
    {
#if FIRESTORE
        DataManager.OnDataManagerStateChanged -= OnDataManagerStateChanged;
#endif
    }

#if FIRESTORE
    void OnDataManagerStateChanged(EDataManagerState state)
    {
        switch (state)
        {
            case EDataManagerState.READY:
                {
                    Ready();
                }
                break;
        }
    }
#endif

    void Ready()
    {
        isReady = true;

        TrackingMangager.Instance.Init();
        PushNotifyManager.Instance.Init();
        AdsManager.Instance.Init();
        AdsManager.Instance.ShowBannerSync();

#if IAP
        IAPManager.Instance.Init();
#endif

        if (DataSave.Instance.tosAgreed)
        {
            SceneManager.LoadScene(Define.gameSceneName);
        }
        else
        {
            UIManager.Instance.GetPopup(ResourcesPath.PREFAB_TERMS_OF_SERVICE_POPUP).Show();
        }
    }

}