using System;
using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[Serializable]
public partial class DataSave : SaveBaseSingleton<DataSave>
{
    public static Action<string> OnUsernameChanged = delegate { };
    public static Action<int> OnLevelChanged = delegate { };
    public static Action<int> OnBestScoreChanged = delegate { };
    public static Action<int> OnCoinChanged = delegate { };
    public static Action<int> OnGemChanged = delegate { };
    public static Action<float> OnBgmVolumeChanged = delegate { };
    public static Action<float> OnSfxVolumeChanged = delegate { };
    public static Action<bool> OnVibrateChanged = delegate { };
    public static Action<bool> OnPushNotificationChanged = delegate { };
    public static Action<bool> OnAdsRemoved = delegate { };
    public static Action<int> OnVipLevelChanged = delegate { };

    public bool isVipPurchased => vipLevel > 0;

    public bool isSubcripting => daySubcription > 0;

    protected override void OnLoaded()
    {
        base.OnLoaded();
    }

    [SerializeField] private string _username = "PLAYER";
    public string username
    {
        get => _username;
        set
        {
            if (_username.Equals(value)) return;
            _username = value;
            OnUsernameChanged.Invoke(_username);
        }
    }

    [SerializeField] private int _level = 1;
    public int level
    {
        get => _level;
        set
        {
            if (_level == value) return;
            _level = value;
            OnLevelChanged.Invoke(value);
        }
    }

    [SerializeField] private int _bestScore = 0;
    public int bestScore
    {
        get => _bestScore;
        set
        {
            if (_bestScore == value) return;
            _bestScore = value;
            OnBestScoreChanged.Invoke(value);
        }
    }

    [SerializeField] private int _coin = 0;
    public int coin
    {
        get => _coin;
        set
        {
            if (_coin == value) return;
            _coin = value;
            OnCoinChanged.Invoke(value);
        }
    }

    [SerializeField] private int _gem = 0;
    public int gem
    {
        get => _gem;
        set
        {
            if (_gem < 0 || _gem == value) return;
            _gem = value;
            OnGemChanged.Invoke(value);
        }
    }

    [SerializeField] private float _bgmVolume = 0.7f;
    public float bgmVolume
    {
        get => _bgmVolume;
        set
        {
            _bgmVolume = value;
            OnBgmVolumeChanged.Invoke(value);
        }
    }

    public bool muteBGM => bgmVolume == 0;

    [SerializeField] private float _sfxVolume = 0.7f;
    public float sfxVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            OnSfxVolumeChanged.Invoke(value);
        }
    }

    public bool muteSFX => sfxVolume == 0;

    [SerializeField] private bool _vibrate = true;
    public bool vibrate
    {
        get => _vibrate;
        set
        {
            if (_vibrate == value) return;
            _vibrate = value;
            OnVibrateChanged.Invoke(value);
        }
    }

    [SerializeField] private bool _pushNotification = true;
    public bool pushNotification
    {
        get => _pushNotification;
        set
        {
            if (_pushNotification == value) return;
            _pushNotification = value;
            OnPushNotificationChanged.Invoke(value);
        }
    }

    [SerializeField] private bool _adsRemoved;
    public bool adsRemoved
    {
        get => _adsRemoved;
        set
        {
            if (_adsRemoved == value) return;
            _adsRemoved = value;
            OnAdsRemoved.Invoke(_adsRemoved);
        }
    }

    [SerializeField] private int _vipLevel = 0;
    public int vipLevel
    {
        get => _vipLevel;
        set
        {
            if (_vipLevel == value) return;
            _vipLevel = value;
            OnVipLevelChanged.Invoke(value);
        }
    }


    [SerializeField] private bool _firstPlay = false;
    public bool firstPlay { get => _firstPlay; set => _firstPlay = value; }

    [SerializeField] private int _playerFlagID;
    public int playerFlagID { get => _playerFlagID; set => _playerFlagID = value; }

    [SerializeField] private bool _trackingFirstPlayCount = false;
    public bool trackingFirstPlayCount { get => _trackingFirstPlayCount; set => _trackingFirstPlayCount = value; }

    [SerializeField] private bool _rated = false;
    public bool rated { get => _rated; set => _rated = value; }

    [SerializeField] private bool _tosAgreed = false;
    public bool tosAgreed { get => _tosAgreed; set => _tosAgreed = value; }

    [SerializeField] private int _versionUpdate = 1;
    public int versionUpdate { get => _versionUpdate; set => _versionUpdate = value; }

    [SerializeField] private int _daySubcription = 0;
    public int daySubcription { get => _daySubcription; set => _daySubcription = value; }

    [SerializeField] private int _dayClaimDailyReward = 0;
    public int dayClaimDailyReward { get => _dayClaimDailyReward; set => _dayClaimDailyReward = value; }

    [SerializeField] private int _dayDailyTask = 0;
    public int dayDailyTask { get => _dayDailyTask; set => _dayDailyTask = value; }

    [SerializeField] private int _lastDayOfYear = 0;
    public int lastDayOfYear { get => _lastDayOfYear; set => _lastDayOfYear = value; }

    [SerializeField] private string _dateFirstOpenGame = DateTime.UtcNow.ToString();
    public string dateFirstOpenGame { get => _dateFirstOpenGame; set => _dateFirstOpenGame = value; }

    [SerializeField] private string _dateOfflineGold = DateTime.UtcNow.ToString();
    public string dateOfflineGold { get => _dateOfflineGold; set => _dateOfflineGold = value; }

    [SerializeField] private string _dateDailyReward = DateTime.UtcNow.ToString();
    public string dateDailyReward { get => _dateDailyReward; set => _dateDailyReward = value; }

    [SerializeField] private string _dateDiamondMemembership = DateTime.UtcNow.ToString();
    public string dateDiamondMemembership { get => _dateDiamondMemembership; set => _dateDiamondMemembership = value; }

    [SerializeField] private string _dateSubcripting = DateTime.UtcNow.ToString();
    public string dateSubcripting { get => _dateSubcripting; set => _dateSubcripting = value; }

    [SerializeField] private string _dateTimeMissionStart;
    public string dateTimeMissionStart { get => _dateTimeMissionStart; set => _dateTimeMissionStart = value; }
}