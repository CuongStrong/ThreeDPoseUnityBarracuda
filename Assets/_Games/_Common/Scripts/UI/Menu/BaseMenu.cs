using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseMenu : MonoBehaviour
{
    [SerializeField] protected Transform panelRoot;

    protected DataSave dataSave => DataSave.Instance;
    protected UIManager uiManager => UIManager.Instance;
    protected PoolManager poolManager => PoolManager.Instance;
    protected PrefabConfig prefabConfig => PrefabConfig.Entity;
    protected AudioManager audioManager => AudioManager.Instance;

    protected bool isShowing;
    protected Action callback;

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

    protected virtual void Awake()
    {
        if (Utility.isTablet) GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        if (panelRoot != null) Utility.AdaptUITallPhone(panelRoot.gameObject);
    }

    public virtual void Show(Action callback = null) => gameObject.SetActive(true);
    public virtual void Hide(Action callback = null) => gameObject.SetActive(false);
    protected void PlayButtonSfx() => audioManager.PlaySFX(ResourcesPath.AUDIO_CLICK);
}