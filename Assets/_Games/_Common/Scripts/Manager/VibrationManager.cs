using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if MOREMOUNTAINS_NICEVIBRATIONS
using MoreMountains.NiceVibrations;
#endif

public enum HapticType { Selection, Success, Warning, Failure, LightImpact, MediumImpact, HeavyImpact, RigidImpact, SoftImpact, CritImpact, AirImpact, None }

public class VibrationManager : MonoBehaviourPersistence<VibrationManager>
{
    private void OnEnable()
    {
        DataSave.OnVibrateChanged += OnVibrateChanged;

        OnVibrateChanged(DataSave.Instance.vibrate);
    }

    private void OnDisable()
    {
        DataSave.OnVibrateChanged -= OnVibrateChanged;
    }

    void OnVibrateChanged(bool on)
    {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.SetHapticsActive(on);
#endif
    }

    public static void Haptic(HapticType type, bool defaultToRegularVibrate = false, bool alsoRumble = false, MonoBehaviour coroutineSupport = null, int controllerID = -1)
    {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.Haptic((MoreMountains.NiceVibrations.HapticTypes)type, defaultToRegularVibrate, alsoRumble, coroutineSupport, controllerID);
#endif
    }

    public static void TransientHaptic(float intensity, float sharpness, bool alsoRumble = false, MonoBehaviour coroutineSupport = null, int controllerID = -1)
    {
#if MOREMOUNTAINS_NICEVIBRATIONS
        MMVibrationManager.TransientHaptic(intensity, sharpness, alsoRumble, coroutineSupport, controllerID);
#endif
    }
}
