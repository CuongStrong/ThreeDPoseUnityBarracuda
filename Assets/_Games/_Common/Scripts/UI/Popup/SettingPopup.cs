using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public partial class SettingPopup : BasePopup
{
    [SerializeField] Text tittleText, versionText;
    [SerializeField] Slider sfxSlider, bgmSlider;
    [SerializeField] ToggleButton vibrateButton, pushNotifyButton;
    [SerializeField] GameObject iconSfxOn, iconSfxOff, iconBgmOn, iconBgmOff, iconVibrateOn, iconVibrateOff, iconPushnotifyOn, iconPushnotifyOff;

    private void Start()
    {
        versionText.text = string.Format("Game Version {0}", Application.version);

        //SFX
        Action RefeshSfx = delegate
        {
            iconSfxOff.SetActive(DataSave.Instance.muteSFX);
            iconSfxOn.SetActive(!iconSfxOff.activeInHierarchy);
            sfxSlider.value = DataSave.Instance.sfxVolume;
        };

        RefeshSfx();
        sfxSlider.onValueChanged.AddListener((t) =>
        {
            DataSave.Instance.sfxVolume = t;
            SaveGameManager.Instance.Save();
            RefeshSfx();
        });

        //BGM
        Action RefeshBgm = delegate
        {
            iconBgmOff.SetActive(DataSave.Instance.muteBGM);
            iconBgmOn.SetActive(!iconBgmOff.activeInHierarchy);
            bgmSlider.value = DataSave.Instance.bgmVolume;
            // AudioManager.Instance.PlayBGM(ResourcesPath.AUDIO_MUSIC175_BPM);
        };

        RefeshBgm();
        bgmSlider.onValueChanged.AddListener((t) =>
        {
            DataSave.Instance.bgmVolume = t;
            SaveGameManager.Instance.Save();
            RefeshBgm();
        });

        //Vibration
        Action RefeshVibrate = delegate
        {
            iconVibrateOff.SetActive(!DataSave.Instance.vibrate);
            iconVibrateOn.SetActive(!iconVibrateOff.activeInHierarchy);
            vibrateButton.SetStatus(DataSave.Instance.vibrate);
        };

        RefeshVibrate();
        vibrateButton.onClick.AddListener(() =>
        {
            PlayButtonSfx();
            DataSave.Instance.vibrate = !DataSave.Instance.vibrate;
            SaveGameManager.Instance.Save();
            RefeshVibrate();
        });

        //Push notify
        Action RefeshPushnotify = delegate
        {
            iconPushnotifyOff.SetActive(!DataSave.Instance.pushNotification);
            iconPushnotifyOn.SetActive(!iconPushnotifyOff.activeInHierarchy);
            pushNotifyButton.SetStatus(DataSave.Instance.pushNotification);
        };

        RefeshPushnotify();
        pushNotifyButton.onClick.AddListener(() =>
        {
            PlayButtonSfx();
            DataSave.Instance.pushNotification = !DataSave.Instance.pushNotification;
            SaveGameManager.Instance.Save();
            RefeshPushnotify();
        });
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void ButtonCloseOnClick()
    {
        audioManager.PlaySFX(ResourcesPath.AUDIO_CLOSE);
        Hide();
    }
}
