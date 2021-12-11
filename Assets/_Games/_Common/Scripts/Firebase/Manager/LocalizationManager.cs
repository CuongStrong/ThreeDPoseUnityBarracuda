#if FIRESTORE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using TextMapType = System.Collections.Generic.Dictionary<string, string>;
using System;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviourPersistence<LocalizationManager>
{
    public event EventHandler OnLocalizationChanged;

    private Dictionary<string, TextMapType> data;

    private string currentLanguage = "en";

    private void Start()
    {
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        OnLocalizationChanged = delegate { };
    }

    public async void PullData(string envSuffix)
    {
        try
        {
            var db = DataManager.Instance.firestoreInstance;
            var dataConfigRef = db.Collection("localization_" + envSuffix);

            Debug.Log("Localization init");

            var snapshot = await dataConfigRef.GetSnapshotAsync();

            data = new Dictionary<string, TextMapType>();
            foreach (var doc in snapshot.Documents)
            {
                data.Add(doc.Id.ToString(), doc.ConvertTo<TextMapType>());
            }

            OnLocalizationChanged?.Invoke(this, null);

            AutoDetectLanguage();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    void AutoDetectLanguage()
    {
        string prefsAutoDetectLanguage = "AutoDetectLanguage";
        if (PlayerPrefs.GetInt(prefsAutoDetectLanguage) == 0)
        {
            if (Utility.isSystemLanguageJP)
            {
                ChangeLanguage("ja");
            }
        }

        PlayerPrefs.SetInt(prefsAutoDetectLanguage, 1);
    }

    public void ChangeLanguage(string lang)
    {
        currentLanguage = lang;
        OnLocalizationChanged?.Invoke(this, null);
    }

    public string GetText(string key, string args = null)
    {
        return GetText(currentLanguage, key, args);
    }

    public string GetText(string lang, string key, string args)
    {
        if (data == null || !data.ContainsKey(lang))
        {
            return key;
        }

        var langSet = data[lang];
        string result;
        langSet.TryGetValue(key, out result);
        if (result == null)
        {
            result = key;
        }

        if (!string.IsNullOrEmpty(args))
        {
            string[] pairs = args.Split(';');
            foreach (string pair in pairs)
            {
                string[] keyvalue = pair.Split('=');
                if (keyvalue.Length >= 2)
                {
                    result = result.Replace("{{" + keyvalue[0] + "}}", keyvalue[1]);
                }
            }
        }

        if (string.IsNullOrEmpty(result))
        {
            return key;
        }

        return result;
    }
}
#endif