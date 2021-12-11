using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if FIRESTORE
using DataConfig;
#endif

public class InputFieldName : MonoBehaviour
{
#if FIRESTORE
    [SerializeField] InputField inputField;
    [SerializeField] bool uppercase = true;

    private ConfigType config => DataManager.Instance.config.config;
    private LocalizationManager localization => LocalizationManager.Instance;

    private ErrorPopup _errorPopup;
    private ErrorPopup errorPopup => _errorPopup ?? (_errorPopup = UIManager.Instance.GetPopup(ResourcesPath.PREFAB_ERROR_POPUP).GetComponent<ErrorPopup>());

    private string lastInputNameText;

    void Start()
    {
        inputField.characterLimit = (int)config.maxNameCharacters;
        inputField.SetTextWithoutNotify(DataSave.Instance.username);
        lastInputNameText = inputField.text;
        inputField.onValueChanged.AddListener((c) =>
        {
            if (c.IndexOfAny(Utility.badChars) >= 0)
            {
                inputField.text = lastInputNameText;
            }
            else
            {
                if (uppercase) inputField.text = inputField.text.ToUpper();
                lastInputNameText = inputField.text;
            }
        });
    }

    public void SetName()
    {
        var name = inputField.text;
        if (name.Trim().CompareTo("") == 0)
        {
            errorPopup.Init(localization.GetText("inputName.nameInvalid"), localization.GetText("inputName.nameEmpty"));
            errorPopup.Show();
            return;
        }
        else if (name.Trim().Length < config.minNameCharacters)
        {
            errorPopup.Init(localization.GetText("inputName.nameInvalid"), localization.GetText("inputName.nameMinError"));
            errorPopup.Show();
            return;
        }
        else
        {
            foreach (var t in DataManager.Instance.config.banned_words.Values)
            {
                if (t.name.CompareTo(name.Trim()) == 0)
                {
                    errorPopup.Init(localization.GetText("inputName.inappropriateNameTittle"), localization.GetText("inputName.inappropriateNameDesc"));
                    errorPopup.Show();
                    return;
                }
            }
        }

        DataSave.Instance.username = name;
        SaveGameManager.Instance.Save();
    }
#endif
}
