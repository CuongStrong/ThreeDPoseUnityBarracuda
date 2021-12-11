using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public partial class AudioManager : MonoBehaviourPersistence<AudioManager>
{
    protected AudioSource bgmAudioSource;
    protected List<AudioSource> sfxAudioSources = new List<AudioSource>();

    protected int amountAudioSource = 10;

    private void Awake()
    {
        GenerateAudioSources();

        DataSave.OnBgmVolumeChanged += OnBgmVolumeChanged;
        DataSave.OnSfxVolumeChanged += OnSfxVolumeChanged;

        OnBgmVolumeChanged(DataSave.Instance.bgmVolume);
        OnSfxVolumeChanged(DataSave.Instance.sfxVolume);
    }

    private void OnDestroy()
    {
        DataSave.OnBgmVolumeChanged -= OnBgmVolumeChanged;
        DataSave.OnSfxVolumeChanged -= OnSfxVolumeChanged;
    }

    void GenerateAudioSources()
    {
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true;

        for (int i = 0; i < amountAudioSource; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            sfxAudioSources.Add(audioSource);
        }
    }

    void OnBgmVolumeChanged(float vol)
    {
        bgmAudioSource.volume = vol;
    }

    void OnSfxVolumeChanged(float vol)
    {
        foreach (var t in sfxAudioSources)
        {
            t.volume = vol;
        }
    }

    AudioClip audioClip(string name) => Resources.Load<AudioClip>(name);

    public void PlayBGM(AudioClip clip)
    {
        if (DataSave.Instance.muteBGM) return;
        if (bgmAudioSource.clip == clip) return;

        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    public void PlayBGM(string name)
    {
        PlayBGM(audioClip(name));
    }

    public void PlaySFX(AudioClip clip, float pitch = 1f)
    {
        if (DataSave.Instance.muteSFX) return;
        foreach (var t in sfxAudioSources)
        {
            if (!t.isPlaying)
            {
                t.pitch = pitch;
                t.PlayOneShot(clip);
                break;
            }
        }
    }

    public void PlaySFX(string name, float pitch = 1f)
    {
        PlaySFX(audioClip(name), pitch);
    }

    public void PlayRandomSFX(AudioClip[] clips)
    {
        if (DataSave.Instance.muteSFX) return;
        var randomIdx = Random.Range(0, clips.Length);
        PlaySFX(clips[randomIdx]);
    }

    public void PlayRandomSFX(string[] names)
    {
        if (DataSave.Instance.muteSFX) return;
        var randomName = Random.Range(0, names.Length);
        PlaySFX(names[randomName]);
    }

    public void FadeOutBGM()
    {
        if (!DataSave.Instance.muteBGM)
        {
            DOTween.Kill(bgmAudioSource);
            bgmAudioSource.DOFade(0, 0.5f).OnComplete(() => bgmAudioSource.Pause()).SetTarget(bgmAudioSource);
        }
    }

    public void FadeInBGM()
    {
        if (!DataSave.Instance.muteBGM)
        {
            DOTween.Kill(bgmAudioSource);
            bgmAudioSource.UnPause();
            bgmAudioSource.DOFade(DataSave.Instance.bgmVolume, 0.5f).SetTarget(bgmAudioSource);
        }
    }

    public void PauseAudio()
    {
        FadeOutBGM();

        if (!DataSave.Instance.muteSFX)
        {
            foreach (var t in sfxAudioSources)
            {
                t.Pause();
            }
        }
    }

    public void ResumeAudio()
    {
        FadeInBGM();

        if (!DataSave.Instance.muteSFX)
        {
            foreach (var t in sfxAudioSources)
            {
                t.UnPause();
            }
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) ResumeAudio();
        else PauseAudio();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus) ResumeAudio();
        else PauseAudio();
    }
}
