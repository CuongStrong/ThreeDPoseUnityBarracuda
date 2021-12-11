﻿using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "[@" + typeof(T).Name + "]";
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
}