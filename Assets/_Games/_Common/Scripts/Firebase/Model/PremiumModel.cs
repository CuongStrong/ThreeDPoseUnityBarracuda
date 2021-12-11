#if FIRESTORE
using System.Collections.Generic;
using Firebase.Firestore;
using System;

[FirestoreData]
public class PremiumModel : BaseData
{
    [FirestoreProperty]
    public long firstPurchaseTime { get; set; }

    [FirestoreProperty]
    public long numOfPurchases { get; set; }

    [FirestoreProperty]
    public bool needToRenew { get; set; }

    public override void SetDefaults()
    {
        firstPurchaseTime = -1;
        numOfPurchases = 0;
        needToRenew = false;
    }

    public override void UpgradeDefaults() { }
}

[FirestoreData]
public class PremiumCardModel : BaseModel<PremiumModel>
{
    public PremiumCardModel()
    {
        Name = "premium";
    }

    public bool IsValid()
    {
        long elapsedTicks = Firebase.Firestore.Timestamp.GetCurrentTimestamp().ToDateTime().Ticks - data.firstPurchaseTime;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
        var days = DataManager.Instance.config.shop.ContainsKey("premium-1") ? DataManager.Instance.config.shop["premium-1"].quantity : 0;
        long lifeTimeInSeconds = data.numOfPurchases * days * 24 * 3600;
        return elapsedSpan.TotalSeconds <= lifeTimeInSeconds;
    }

    public long GetRemainingDays()
    {
        if (data.firstPurchaseTime > 0)
        {
            long elapsedTicks = Firebase.Firestore.Timestamp.GetCurrentTimestamp().ToDateTime().Ticks - data.firstPurchaseTime;
            var days = DataManager.Instance.config.shop["premium-1"].quantity;
            long lifeTimeInDays = data.numOfPurchases * days;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
            return (long)Math.Round((double)lifeTimeInDays - elapsedSpan.TotalDays);
        }

        return 0;
    }

    public void Reset()
    {
        Update(new Dictionary<string, object>()
        {
            { "firstPurchaseTime", -1 },
            { "numOfPurchases", 0 },
            { "needToRenew", false },
        });
    }
}
#endif