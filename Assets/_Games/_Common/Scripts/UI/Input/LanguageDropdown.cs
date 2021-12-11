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

public class LanguageDropdown : MonoBehaviour
{
    [SerializeField] Dropdown dropdown;

    private PrefabConfig prefabConfig => PrefabConfig.Entity;

    private void Awake()
    {
        dropdown.options.Clear();

        foreach (var t in prefabConfig.flags)
        {
            dropdown.options.Add(new Dropdown.OptionData(t));
        }
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        dropdown.value = DataSave.Instance.playerFlagID;
    }

    public void OnLanguageDropdownChanged()
    {
        DataSave.Instance.playerFlagID = dropdown.value;
        SaveGameManager.Instance.Save();
    }
}
