using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Utility
{
    public static readonly char[] badChars = { '[', ']', '{', '}', '!', '@', '#', '$', '%', '+', '-', '&', '*', '/', '(', ')', '=', '|', ';', '"', ',', '.', '<', '>', '?', '^', '`', '~', '_', @"\"[0], @"'"[0], '。', '、', '・', '゠', '＝' };

    public static bool isTallPhone => 1f * Screen.height / Screen.width >= 2f;
    public static bool isTablet => 1f * Screen.height / Screen.width <= 1.613f;
    public static bool isInternetAvailable => Application.internetReachability != NetworkReachability.NotReachable;


    public static void AdaptUITallPhone(GameObject go)
    {
        if (isTallPhone)
        {
            var rectTransform = go.GetComponent<RectTransform>();
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -60);
        }
    }

    public static void AdaptUIBannerAds(GameObject go, bool bannerComing)
    {
        var rectTransform = go.GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bannerComing ? 105 : 0);
    }

    public static bool isSystemLanguageJP
    {
        get
        {
#if UNITY_EDITOR
            return false;
#else
            return Application.systemLanguage == SystemLanguage.Japanese;
#endif
        }
    }


    public static T RandomInArray<T>(T[] array)
    {
        try
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return default(T);
        }
    }

    public static T RandomInList<T>(List<T> list)
    {
        try
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return default(T);
        }
    }

    private static string sSavePath = "";
    public static string GetSavePath()
    {
        if (sSavePath.Length <= 0)
        {
            sSavePath = Application.persistentDataPath + '/';
        }
        return sSavePath;
    }

    public static float RandomTolerance(float center, float delta)
    {
        float min = (1f - delta) * center;
        float max = (1f + delta) * center;
        return UnityEngine.Random.Range(min, max);
    }

    public static float NormalizeAngle(float angle)
    {
        float result = angle % 360;

        if (result < 0)
            result += 360;

        return result;
    }

    public static float GetShiftedAngle(int wayIndex, float baseAngle, float betweenAngle)
    {
        float angle = wayIndex % 2 == 0 ?
            baseAngle - (betweenAngle * (float)wayIndex / 2f) :
            baseAngle + (betweenAngle * Mathf.Ceil((float)wayIndex / 2f));
        return angle;
    }

    public static string ToTimeStr(int second)
    {
        int minute = second / 60;
        return string.Format("{0:00}:{1:00}", minute, second - minute * 60);
    }

    public static GameObject Create(string name, GameObject parent, params Type[] components)
    {
        var res = new GameObject(name, components);
        res.transform.parent = parent.transform;
        res.transform.localPosition = Vector3.zero;
        res.transform.localScale = Vector3.one;
        res.transform.localRotation = Quaternion.identity;
        return res;
    }

    public static GameObject Instantiate(GameObject prefab, Transform parent)
    {
        var res = UnityEngine.Object.Instantiate(prefab, parent);
        res.transform.localPosition = Vector3.zero;
        res.transform.localRotation = Quaternion.identity;
        res.transform.localScale = Vector3.one;
        return res;
    }

    public static void Destroy(GameObject go)
    {
        if (Application.isPlaying)
        {
            UnityEngine.Object.Destroy(go);
        }
        else
        {
            UnityEngine.Object.DestroyImmediate(go);
        }
    }

    public static void Destroy(Component comp)
    {
        if (comp)
        {
            if (Application.isPlaying)
            {
                UnityEngine.Object.Destroy(comp);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(comp);
            }
        }
    }

    public static void DestroyChildren(GameObject go)
    {
        if (go)
        {
            var childList = go.transform.Cast<Transform>().ToList();
            foreach (Transform childTransform in childList)
            {
                Destroy(childTransform.gameObject);
            }
        }
    }
}

