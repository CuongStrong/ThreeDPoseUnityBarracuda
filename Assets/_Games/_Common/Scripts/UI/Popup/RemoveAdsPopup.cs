using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public partial class RemoveAdsPopup : BasePopup
{
    [SerializeField] Text priceText;

    private void Start()
    {
#if FIRESTORE
        priceText.text = string.Format("${0}", DataManager.Instance.config.shop["removeAds"].usd.ToString());
#endif
    }

    public void ButtonLaterOnclick()
    {
        PlayButtonSfx();
        Hide();
    }

    public void ButtonBuyOnclick()
    {
#if IAP
        IAPManager.Instance.BuyProductID("removeads", BoughtCallback);
#endif
    }

    void BoughtCallback()
    {
        DataSave.Instance.adsRemoved = true;
        SaveGameManager.Instance.Save();

#if FIRESTORE
        DataManager.Instance.profile.RemoveAds();
#endif

        Hide();
    }
}
