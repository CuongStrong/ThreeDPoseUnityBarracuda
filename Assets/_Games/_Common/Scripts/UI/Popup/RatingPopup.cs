using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public partial class RatingPopup : BasePopup
{
    [SerializeField] Text descText;
    [SerializeField] Button[] starButtons;

    private int rate = 0;

    private void Start()
    {
#if FIRESTORE
        descText.text = string.Format(LocalizationManager.Instance.GetText("rating.desc"), Application.productName);
#endif
    }

    private void OnEnable()
    {
        rate = 5;
        foreach (var t in starButtons)
        {
            var colors = t.colors;
            colors.normalColor = Color.white;
            t.colors = colors;
        }
    }

    public void ButtonStarOnClick(int idx)
    {
        PlayButtonSfx();

        rate = idx;
        for (int i = 0; i < starButtons.Length; i++)
        {
            var colors = starButtons[i].colors;
            colors.normalColor = i <= rate ? Color.white : Color.black;
            starButtons[i].colors = colors;
        }
    }

    public void ButtonLaterOnclick()
    {
        PlayButtonSfx();
        Hide();

        callback?.Invoke();
    }

    public void ButtonRateOnclick()
    {
        PlayButtonSfx();
        DataSave.Instance.rated = true;
        SaveGameManager.Instance.Save();
        Hide();

        if (rate > 2)
        {

#if UNITY_ANDROID
#if UNITY_EDITOR
            Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.productName);
#else
            Application.OpenURL("market://details?id=" + Application.identifier);
#endif
#endif

#if UNITY_IOS
            Application.OpenURL("https://itunes.apple.com/app/id" + Application.productName);
#endif

        }
        else
        {
            string email = "support@kagecat.com";
            string subject = UnityWebRequest.EscapeURL(Application.productName).Replace("+", "%20");
            string body = "";
            Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
        }
    }
}
