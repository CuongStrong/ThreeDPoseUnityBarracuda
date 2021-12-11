

// #define SUBSCRIPTION_MANAGER
#if IAP
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviourPersistence<IAPManager>, IStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;
    private IAppleExtensions appleExtensions;
    private IGooglePlayStoreExtensions googlePlayStoreExtensions;
    private ITransactionHistoryExtensions transactionHistoryExtensions;

    private bool isGooglePlayStoreSelected, isPurchaseInProgress;


    private System.Action finishCallback = null;
    private bool isReady => storeController != null && storeExtensionProvider != null;

    void Start()
    {
        var module = StandardPurchasingModule.Instance();
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
        var builder = ConfigurationBuilder.Instance(module);

        isGooglePlayStoreSelected = true;//Application.platform == RuntimePlatform.Android && module.appStore == AppStore.GooglePlay;

        var categories = new SortedList<int, string>();
        foreach (var kvp in DataManager.Instance.config.shop)
        {
            if (kvp.Key.CompareTo("schema") == 0 || !kvp.Value.enabled)
            {
                continue;
            }

            ProductType type = ProductType.Consumable;
            if (type.Equals("NonConsumable")) type = ProductType.NonConsumable;
            else if (type.Equals("Subscription")) type = ProductType.Subscription;

            builder.AddProduct(isGooglePlayStoreSelected ? kvp.Value.google_store_id : kvp.Value.apple_store_id, type);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;

        transactionHistoryExtensions = extensions.GetExtension<ITransactionHistoryExtensions>();
        googlePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
        appleExtensions = extensions.GetExtension<IAppleExtensions>();
        appleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

#if SUBSCRIPTION_MANAGER
        Dictionary<string, string> introductory_info_dict = appleExtensions.GetIntroductoryPriceDictionary();
#endif

        Debug.Log("Available items:");
        foreach (var item in controller.products.all)
        {
            if (item.availableToPurchase)
            {
                Debug.Log(string.Join(" - ",
                    new[]
                    {
                        item.metadata.localizedTitle,
                        item.metadata.localizedDescription,
                        item.metadata.isoCurrencyCode,
                        item.metadata.localizedPrice.ToString(),
                        item.metadata.localizedPriceString,
                        item.transactionID,
                        item.receipt
                    }));

#if SUBSCRIPTION_MANAGER
                // this is the usage of SubscriptionManager class
                if (item.receipt != null)
                {
                    if (item.definition.type == ProductType.Subscription)
                    {
                        if (CheckIfProductIsAvailableForSubscriptionManager(item.receipt))
                        {
                            string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(item.definition.storeSpecificId)) ? null : introductory_info_dict[item.definition.storeSpecificId];
                            SubscriptionManager p = new SubscriptionManager(item, intro_json);
                            SubscriptionInfo info = p.getSubscriptionInfo();
                            Debug.Log("product id is: " + info.getProductId());
                            Debug.Log("purchase date is: " + info.getPurchaseDate());
                            Debug.Log("subscription next billing date is: " + info.getExpireDate());
                            Debug.Log("is subscribed? " + info.isSubscribed().ToString());
                            Debug.Log("is expired? " + info.isExpired().ToString());
                            Debug.Log("is cancelled? " + info.isCancelled());
                            Debug.Log("product is in free trial peroid? " + info.isFreeTrial());
                            Debug.Log("product is auto renewing? " + info.isAutoRenewing());
                            Debug.Log("subscription remaining valid time until next billing date is: " + info.getRemainingTime());
                            Debug.Log("is this product in introductory price period? " + info.isIntroductoryPricePeriod());
                            Debug.Log("the product introductory localized price is: " + info.getIntroductoryPrice());
                            Debug.Log("the product introductory price period is: " + info.getIntroductoryPricePeriod());
                            Debug.Log("the number of product introductory price period cycles is: " + info.getIntroductoryPricePeriodCycles());
                        }
                        else
                        {
                            Debug.Log("This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class.");
                        }
                    }
                    else
                    {
                        Debug.Log("the product is not a subscription product");
                    }
                }
                else
                {
                    Debug.Log("the product should have a valid receipt");
                }
#endif
            }
        }

        LogProductDefinitions();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Billing failed to initialize!");
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                // Ask the user if billing is disabled in device settings.
                Debug.Log("Billing disabled!");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                // Developer configuration error; check product metadata.
                Debug.Log("No products available for purchase!");
                break;
        }
    }

    public void OnPurchaseFailed(Product item, PurchaseFailureReason reason)
    {
        Debug.Log("Purchase failed: " + item.definition.id);

        Debug.Log("Store specific error code: " + transactionHistoryExtensions.GetLastStoreSpecificPurchaseErrorCode());
        if (transactionHistoryExtensions.GetLastPurchaseFailureDescription() != null)
        {
            Debug.Log("Purchase failure description message: " +
                      transactionHistoryExtensions.GetLastPurchaseFailureDescription().message);
        }

        isPurchaseInProgress = false;
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        isPurchaseInProgress = false;
        finishCallback?.Invoke();

        return PurchaseProcessingResult.Complete;
    }

    public void BuyProductID(string productId, System.Action callBack)
    {
        if (isReady)
        {
            this.finishCallback = callBack;
            Product product = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    private void LogProductDefinitions()
    {
        var products = storeController.products.all;
        foreach (var product in products)
        {
            Debug.Log(string.Format("id: {0}\nstore-specific id: {1}\ntype: {2}\nenabled: {3}\n", product.definition.id, product.definition.storeSpecificId, product.definition.type.ToString(), product.definition.enabled ? "enabled" : "disabled"));
        }
    }



#if SUBSCRIPTION_MANAGER
    private bool CheckIfProductIsAvailableForSubscriptionManager(string receipt)
    {
        var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
        if (!receipt_wrapper.ContainsKey("Store") || !receipt_wrapper.ContainsKey("Payload"))
        {
            Debug.Log("The product receipt does not contain enough information");
            return false;
        }
        var store = (string)receipt_wrapper["Store"];
        var payload = (string)receipt_wrapper["Payload"];

        if (payload != null)
        {
            switch (store)
            {
                case GooglePlay.Name:
                    {
                        var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                        if (!payload_wrapper.ContainsKey("json"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'json' field is missing");
                            return false;
                        }
                        var original_json_payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode((string)payload_wrapper["json"]);
                        if (original_json_payload_wrapper == null || !original_json_payload_wrapper.ContainsKey("developerPayload"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'developerPayload' field is missing");
                            return false;
                        }
                        var developerPayloadJSON = (string)original_json_payload_wrapper["developerPayload"];
                        var developerPayload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(developerPayloadJSON);
                        if (developerPayload_wrapper == null || !developerPayload_wrapper.ContainsKey("is_free_trial") || !developerPayload_wrapper.ContainsKey("has_introductory_price_trial"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
                            return false;
                        }
                        return true;
                    }
                case AppleAppStore.Name:
                case AmazonApps.Name:
                case MacAppStore.Name:
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
        return false;
    }
#endif

    public void PurchaseButtonClick(string productID)
    {
        if (isPurchaseInProgress == true)
        {
            Debug.Log("Please wait, purchase in progress");
            return;
        }

        if (storeController == null)
        {
            Debug.LogError("Purchasing is not initialized");
            return;
        }

        if (storeController.products.WithID(productID) == null)
        {
            Debug.LogError("No product has id " + productID);
            return;
        }


        isPurchaseInProgress = true;
        storeController.InitiatePurchase(storeController.products.WithID(productID), "developerPayload");

    }

    public void RestoreButtonClick()
    {
        if (isGooglePlayStoreSelected)
        {
            googlePlayStoreExtensions.RestoreTransactions(OnTransactionsRestored);
        }
        else
        {
            appleExtensions.RestoreTransactions(OnTransactionsRestored);
        }
    }

    private void OnTransactionsRestored(bool success)
    {
        Debug.Log("Transactions restored." + success);
    }

    private void OnDeferred(Product item)
    {
        Debug.Log("Purchase deferred: " + item.definition.id);
    }
}
#endif