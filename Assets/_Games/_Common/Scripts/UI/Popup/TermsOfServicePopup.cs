using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public partial class TermsOfServicePopup : BasePopup
{
    public void ButtonTermDetailOnclick()
    {
        PlayButtonSfx();
        UIManager.Instance.GetPopup(ResourcesPath.PREFAB_TERM_DETAIL_POPUP).Show();
    }

    public void ButtonPrivacyDetailOnclick()
    {
        PlayButtonSfx();
        UIManager.Instance.GetPopup(ResourcesPath.PREFAB_PRIVACY_DETAIL_POPUP).Show();
    }

    public void ButtonOkOnclick()
    {
        PlayButtonSfx();
        DataSave.Instance.tosAgreed = true;
        SaveGameManager.Instance.Save();

        Hide(delegate { SceneManager.LoadScene(Define.gameSceneName); });
    }
}
