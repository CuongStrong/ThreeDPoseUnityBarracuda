using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ExtensionMonoBehavior
{
    private static Stack _stackTimeScale = new Stack();

    public static void PushTimeScale(this MonoBehaviour mono, float scale)
    {
        _stackTimeScale.Push(Time.timeScale);
        Time.timeScale = scale;
    }

    public static void PopTimeScale(this MonoBehaviour mono)
    {
        if (_stackTimeScale.Count > 0)
            Time.timeScale = (float)_stackTimeScale.Pop();
        else
            Debug.LogWarning("No timeScale to pop");
    }

    public static IEnumerator DelayAction<T1, T2, T3, T4>(this MonoBehaviour mono, float waitTime, Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
        yield return WaitSmoothlyForSeconds(mono, waitTime);
        action(t1, t2, t3, t4);
    }

    public static IEnumerator DelayAction<T1, T2, T3>(this MonoBehaviour mono, float waitTime, Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
    {
        yield return WaitSmoothlyForSeconds(mono, waitTime);
        action(t1, t2, t3);
    }

    public static IEnumerator DelayAction<T1, T2>(this MonoBehaviour mono, float waitTime, Action<T1, T2> action, T1 t1, T2 t2)
    {
        yield return WaitSmoothlyForSeconds(mono, waitTime);
        action(t1, t2);
    }

    public static IEnumerator DelayAction<T>(this MonoBehaviour mono, float waitTime, Action<T> action, T t)
    {
        yield return WaitSmoothlyForSeconds(mono, waitTime);
        action(t);
    }

    public static IEnumerator DelayAction(this MonoBehaviour mono, float waitTime, Action action)
    {
        yield return WaitSmoothlyForSeconds(mono, waitTime);
        action();
    }

    public static void Invoke<T1, T2, T3, T4>(this MonoBehaviour mono, float waitTime, Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
    {
        mono.StartCoroutine(DelayAction<T1, T2, T3, T4>(mono, waitTime, action, t1, t2, t3, t4));
    }

    public static void Invoke<T1, T2, T3>(this MonoBehaviour mono, float waitTime, Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
    {
        mono.StartCoroutine(DelayAction<T1, T2, T3>(mono, waitTime, action, t1, t2, t3));
    }

    public static void Invoke<T1, T2>(this MonoBehaviour mono, float waitTime, Action<T1, T2> action, T1 t1, T2 t2)
    {
        mono.StartCoroutine(DelayAction<T1, T2>(mono, waitTime, action, t1, t2));
    }

    public static void Invoke<T>(this MonoBehaviour mono, float waitTime, Action<T> action, T t)
    {
        mono.StartCoroutine(DelayAction<T>(mono, waitTime, action, t));
    }

    public static void Invoke(this MonoBehaviour mono, float waitTime, Action action)
    {
        mono.StartCoroutine(DelayAction(mono, waitTime, action));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerator WaitSmoothlyForSeconds(this MonoBehaviour mono, float seconds)
    {
        int count = Mathf.CeilToInt(seconds / Time.fixedDeltaTime);

        for (int i = 0; i < count; i++)
        {
            yield return new WaitForFixedUpdate();

        }
    }

    public static IEnumerator CallRepeatelly(this MonoBehaviour mono, float waitDuration, Action action)
    {
        while (true)
        {
            action.Invoke();
            yield return new WaitForSeconds(waitDuration);
        }
    }

    public static IEnumerator WaitEveryXFrames(this MonoBehaviour mono, int waitFrameCount, Action action)
    {
        while (true)
        {
            action.Invoke();

            for (int i = 0; i < waitFrameCount; i++)
                yield return new WaitForEndOfFrame();
        }
    }
}