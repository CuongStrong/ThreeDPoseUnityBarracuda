using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SaveBaseSingleton<T> : ISaveData where T : class, new()
{
    protected string _saveNameID;
    public string SaveNameID
    {
        set { _saveNameID = value; }
        get
        {
            if (string.IsNullOrEmpty(_saveNameID)) _saveNameID = Instance.ToString();
            return _saveNameID;
        }
    }

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null) _instance = new T();
            return _instance;
        }
    }

    public static void NewInstance()
    {
        _instance = new T();
    }

    public object GetData()
    {
        return Instance;
    }

    public void SetData(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            _instance = JsonUtility.FromJson<T>(name);
        }

        OnLoaded();
    }

    public void RegisterSaveData()
    {
        SaveGameIO.Instance.RegisterMandatoryData(SaveNameID, this as ISaveData);
    }

    protected virtual void OnLoaded() { }
}


public abstract class SaveBase<T> : ISaveData where T : class, new()
{
    protected string _saveNameID;
    public string SaveNameID
    {
        set { _saveNameID = value; }
        get
        {
            if (string.IsNullOrEmpty(_saveNameID)) _saveNameID = Data.ToString();
            return _saveNameID;
        }
    }

    private T _data;
    public T Data
    {
        set
        {
            _data = value;
        }
        get
        {
            if (_data == null) _data = new T();
            return _data;
        }
    }

    public object GetData()
    {
        return Data;
    }

    public void SetData(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            _data = new T();
        }
        else
        {
            _data = JsonUtility.FromJson<T>(name);
        }
    }

    public void RegisterSaveData()
    {
        SaveGameIO.Instance.RegisterMandatoryData(SaveNameID, this as ISaveData);
    }
}