using UnityEngine;
using System.Collections;
using System.IO;
using RotaryHeart.Lib.SerializableDictionary;

public interface ISaveData
{
    object GetData();
    void SetData(string data);
    void RegisterSaveData();
}

public class MySingleton<T> where T : class, new()
{
    private static readonly T _instance = new T();

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }
}

public class SaveGameIO : MySingleton<SaveGameIO>
{
    [System.Serializable]
    public class SaveGameDictionary : SerializableDictionaryBase<string, ISaveData> { }
    [System.Serializable]
    public class LoadGameDictionary : SerializableDictionaryBase<string, string> { }

    public const string MANDATORY_SAVE_NAME = "mwovjtpamcjaytifnhyqlbprths";
    public const string OPTIONAL_SAVE_NAME = "nalgowuthvnapqyewngoapwvz";

    public delegate object ObjectDataCallback();
    public delegate void StringDataCallback(string data);

    [UnityEngine.SerializeField()]
    private SaveGameDictionary mMandatory = new SaveGameDictionary();
    [UnityEngine.SerializeField()]
    private SaveGameDictionary mOptional = new SaveGameDictionary();

    public void RegisterMandatoryData(string name, ISaveData data)
    {
        mMandatory[name] = data;
    }

    public void RegisterOptionalData(string name, ISaveData data)
    {
        mOptional[name] = data;
    }

    public void Save(bool mandatory = true, bool optional = true)
    {
#if USE_GPGS_SAVEGAME
        if(GooglePlayServiceManager.instance.OnLoadFromCloud)
        {
            return;
        }
#endif
        if (mandatory)
        {
            try
            {
                LoadGameDictionary temp = new LoadGameDictionary();
                bool checkValid = false;
                foreach (string key in mMandatory.Keys)
                {
                    temp[key] = JsonUtility.ToJson(mMandatory[key].GetData());
                    checkValid = true;
                }

                if (checkValid)
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(temp));
                    SaveToFile(MANDATORY_SAVE_NAME, data);
                }
            }
            catch (System.Exception)
            {
            }
        }

        if (optional)
        {
            try
            {
                LoadGameDictionary temp = new LoadGameDictionary();
                bool checkValid = false;
                foreach (string key in mOptional.Keys)
                {
                    temp[key] = JsonUtility.ToJson(mOptional[key].GetData());
                    checkValid = true;
                }

                if (checkValid)
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(temp));
                    SaveToFile(OPTIONAL_SAVE_NAME, data);
                }
            }
            catch (System.Exception)
            {
            }
        }
    }

    public void Load(bool mandatory = true, bool optional = true)
    {
        LoadGameDictionary loadDictionary = null;
        if (mandatory)
        {
            try
            {
                byte[] data = null;
                if (!LoadFromFile(MANDATORY_SAVE_NAME, ref data, true))
                {
                    LoadFromFile("_" + MANDATORY_SAVE_NAME, ref data, true);
                }
                loadDictionary = JsonUtility.FromJson<LoadGameDictionary>(data == null ? "{}" : System.Text.Encoding.UTF8.GetString(data, 0, data.Length));
            }
            catch (System.Exception)
            {
                loadDictionary = null;
            }
            foreach (string key in mMandatory.Keys)
            {
                mMandatory[key].SetData(loadDictionary != null && loadDictionary.ContainsKey(key) && loadDictionary[key] != null ? loadDictionary[key] : "");
            }
        }

        if (optional)
        {
            try
            {
                byte[] data = null;
                if (!LoadFromFile(OPTIONAL_SAVE_NAME, ref data, false))
                {
                    LoadFromFile("_" + OPTIONAL_SAVE_NAME, ref data, false);
                }
                loadDictionary = JsonUtility.FromJson<LoadGameDictionary>(data == null ? "{}" : System.Text.Encoding.UTF8.GetString(data, 0, data.Length));
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                loadDictionary = null;
            }
            foreach (string key in mOptional.Keys)
            {
                mOptional[key].SetData(loadDictionary != null && loadDictionary.ContainsKey(key) && loadDictionary[key] != null ? loadDictionary[key] : "");
            }
        }
    }

    public bool SaveToFile(string fileName, byte[] data, bool hasBackup = true)
    {
        try
        {
            string savepath = Application.persistentDataPath + '/';
            if (hasBackup)
            {
                if (File.Exists(savepath + "_" + fileName))
                {
                    File.Delete(savepath + "_" + fileName);
                }
                if (File.Exists(savepath + fileName))
                {
                    File.Move(savepath + fileName, savepath + "_" + fileName);
                }
            }

            //simple encrypt using UDID/decrypt
            SimpleEncrypt(ref data);

            File.WriteAllBytes(savepath + fileName, data);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
        return true;
    }

    public bool LoadMandatoryFile(ref byte[] data)
    {
        return LoadFromFile(MANDATORY_SAVE_NAME, ref data);
    }

    public bool SaveToMandatoryFile(byte[] data)
    {
        return SaveToFile(MANDATORY_SAVE_NAME, data);
    }


    public bool LoadFromFile(string fileName, ref byte[] data, bool hasBackup = false)
    {
        try
        {
            string savepath = Application.persistentDataPath + '/';
            if (File.Exists(savepath + fileName))
            {
                data = File.ReadAllBytes(savepath + fileName);
            }
            else if (File.Exists(savepath + "_" + fileName))
            {
                data = File.ReadAllBytes(savepath + "_" + fileName);
            }
            else
            {
                return false;
            }

            SimpleEncrypt(ref data);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
        return true;
    }

    void SimpleEncrypt(ref byte[] data)
    {
        byte[] key = System.Text.Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier);
        for (uint i = 0; i < data.Length; i++)
            data[i] ^= key[i % key.Length];
    }

    public string StringToEncryptBase64(string data)
    {
        byte[] encryptData = System.Text.Encoding.UTF8.GetBytes(data);
        SimpleEncrypt(ref encryptData);
        return System.Convert.ToBase64String(encryptData);
    }

    public string EncryptBase64ToString(string data)
    {
        try
        {
            byte[] decryptData = System.Convert.FromBase64String(data);
            SimpleEncrypt(ref decryptData);
            return System.Text.Encoding.UTF8.GetString(decryptData, 0, decryptData.Length);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return "";
        }

    }

    public void DeleteAll()
    {
        DeleteSave(MANDATORY_SAVE_NAME);
        DeleteSave(OPTIONAL_SAVE_NAME);
        mMandatory.Clear();
        mOptional.Clear();
    }

    public bool DeleteSave(string fileName)
    {
        try
        {
            string savepath = Application.persistentDataPath + '/';
            if (File.Exists(savepath + fileName))
            {
                File.Delete(savepath + fileName);
            }
            if (File.Exists(savepath + "_" + fileName))
            {
                File.Delete(savepath + "_" + fileName);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
        return true;
    }
}
