using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtensionTransform
{
    public static void SetPosX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    public static void SetPosY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
    public static void SetPosZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
    public static void AddPosX(this Transform transform, float x)
    {
        transform.position = transform.position + new Vector3(x, 0, 0);
    }
    public static void AddPosY(this Transform transform, float y)
    {
        transform.position = transform.position + new Vector3(0, y, 0);
    }
    public static void AddPosZ(this Transform transform, float z)
    {
        transform.position = transform.position + new Vector3(0, 0, z);
    }

    public static void SetLocalPosX(this Transform transform, float x)
    {
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }
    public static void SetLocalPosY(this Transform transform, float y)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }
    public static void SetLocalPosZ(this Transform transform, float z)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
    }
    public static void AddLocalPosX(this Transform transform, float x)
    {
        transform.localPosition = transform.localPosition + new Vector3(x, 0, 0);
    }
    public static void AddLocalPosY(this Transform transform, float y)
    {
        transform.localPosition = transform.localPosition + new Vector3(0, y, 0);
    }
    public static void AddLocalPosZ(this Transform transform, float z)
    {
        transform.localPosition = transform.localPosition + new Vector3(0, 0, z);
    }

    public static void SetScaleX(this Transform transform, float x)
    {
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
    public static void SetScaleY(this Transform transform, float y)
    {
        transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
    }
    public static void SetScaleZ(this Transform transform, float z)
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
    }
    public static void AddScaleX(this Transform transform, float x)
    {
        transform.localScale = transform.localScale + new Vector3(x, 0, 0);
    }
    public static void AddScaleY(this Transform transform, float y)
    {
        transform.localScale = transform.localScale + new Vector3(0, y, 0);
    }
    public static void AddScaleZ(this Transform transform, float z)
    {
        transform.localScale = transform.localScale + new Vector3(0, 0, z);
    }

    public static void SetAngleX(this Transform transform, float x)
    {
        transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    public static void SetAngleY(this Transform transform, float y)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z);
    }
    public static void SetAngleZ(this Transform transform, float z)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, z);
    }
    public static void AddAngleX(this Transform transform, float x)
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(x, 0, 0);
    }
    public static void AddAngleY(this Transform transform, float y)
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(0, y, 0);
    }
    public static void AddAngleZ(this Transform transform, float z)
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, z);
    }

    public static void SetLocalAngleX(this Transform transform, float x)
    {
        transform.localEulerAngles = new Vector3(x, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
    public static void SetLocalAngleY(this Transform transform, float y)
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
    }
    public static void SetLocalAngleZ(this Transform transform, float z)
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, z);
    }
    public static void AddLocalAngleX(this Transform transform, float x)
    {
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(x, 0, 0);
    }
    public static void AddLocalAngleY(this Transform transform, float y)
    {
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, y, 0);
    }
    public static void AddLocalAngleZ(this Transform transform, float z)
    {
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 0, z);
    }

    public static Vector2 position2D(this Transform transform)
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
    public static Vector2 localPosition2D(this Transform transform)
    {
        return new Vector2(transform.localPosition.x, transform.localPosition.y);
    }

    public static void LookAt2D(this Transform self, Vector3 target, Vector2 forward)
    {
        self.rotation = GetForwardRotation2D(self, target, forward);
    }

    public static Quaternion GetForwardRotation2D(this Transform self, Vector3 target, Vector2 forward)
    {
        var forwardDiff = GetForwardDiffPoint(forward);
        Vector3 direction = target - self.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle - forwardDiff, Vector3.forward);
    }

    private static float GetForwardDiffPoint(Vector2 forward)
    {
        if (Equals(forward, Vector2.up)) return 90;
        if (Equals(forward, Vector2.right)) return 0;
        return 0;
    }

    public static List<Transform> children(this Transform transform)
    {
        List<Transform> childList = new List<Transform>();

        foreach (Transform child in transform)
        {
            childList.Add(child);
        }

        return childList;
    }

    public static Transform FindChild(this Transform self, string name) //Benchmark 2x
    {
        int count = self.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = self.GetChild(i);
            if (child.name.Equals(name)) return child;
            Transform subChild = FindChild(child, name);
            if (subChild != null) return subChild;
        }
        return null;
    }

    public static T FindChild<T>(this Transform transform, string name) where T : Component //Benchmark 10x
    {
        return transform.GetComponentsInChildren<T>().FirstOrDefault(t => t.name.Contains(name));
    }

    public static Transform FindDeepChild(this Transform self, string name)
    {
        foreach (Transform child in self)
        {
            if (child.name.Contains(name))
                return child;
            var result = child.FindDeepChild(name);
            if (result != null)
                return result;
        }
        return null;
    }

    public static Transform[] FindDeepChilds(this Transform self, string name)
    {
        List<Transform> result = new List<Transform>();
        foreach (Transform child in self)
        {
            if (child.name.Contains(name))
            {
                result.Add(child);
            }
            else
            {
                var childResult = child.FindDeepChilds(name);
                result.AddRange(childResult);
            }
        }
        return result.ToArray(); ;
    }

    public static Transform[] FindSameDeepChilds(this Transform self, string name)
    {
        Transform result = FindDeepChild(self, name);
        List<Transform> list = new List<Transform>();
        if (result != null)
        {
            Transform parent = result.parent;
            Transform obj;
            for (int i = 0; i < parent.childCount; i++)
            {
                obj = parent.GetChild(i);
                if (obj.name.Contains(name))
                {
                    list.Add(obj);
                }
            }
        }

        return list.ToArray();
    }

    public static void ActionRecursively(this Transform transform, Action<Transform> action)
    {
        action.Invoke(transform);

        foreach (Transform child in transform)
            ActionRecursively(child, action);
    }

    public static void SetActive(this Transform transform, bool active)
    {
        transform.gameObject.SetActive(active);
    }

    public static bool activeInHierarchy(this Transform transform)
    {
        return transform.gameObject.activeInHierarchy;
    }

    public static void SetChildsLayer(this Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            SetChildsLayer(child, layer);
        }
    }
}