#if FIRESTORE
using Firebase.Firestore;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

[FirestoreData]
public class ProfileData : BaseData
{
    [FirestoreProperty]
    public string userId { get; set; }

    [FirestoreProperty]
    public string name { get; set; }

    [FirestoreProperty]
    public string photo { get; set; }

    [FirestoreProperty]
    public bool isCharacterCreated { get; set; }

    [FirestoreProperty]
    public bool isTOSAccepted { get; set; }

    [FirestoreProperty]
    public long lastLogInTime { get; set; }

    [FirestoreProperty]
    public int supportId { get; set; }

    [FirestoreProperty]
    public Timestamp createdAt { get; set; }

    [FirestoreProperty]
    public bool adsRemoved { get; set; }

    [FirestoreProperty]
    public int premiumBeginDay { get; set; }

    [FirestoreProperty]
    public int premiumDurationDay { get; set; }

    public ProfileData()
    {
        userId = "";
        name = "";
        photo = "";
        lastLogInTime = 0;
        isCharacterCreated = false;
        supportId = new Random().Next(1000, 9999);
        isTOSAccepted = false;
        premiumBeginDay = 0;
        premiumDurationDay = 0;
    }

    public override void SetDefaults()
    {
    }

    public override void UpgradeDefaults()
    {
    }
}

[FirestoreData]
public class ProfileModel : BaseModel<ProfileData>
{

    public ProfileModel()
    {
        Name = "profiles";
    }

    public override async Task Start(string userId, string envSuffix)
    {
        data.createdAt = Firebase.Firestore.Timestamp.GetCurrentTimestamp();

        await base.Start(userId, envSuffix);
    }

    public string GetSupportId()
    {
        var createdAt = (Firebase.Firestore.Timestamp)data.createdAt;
        var createdAtTime = createdAt.ToDateTime();
        var epoch = new DateTime(2020, 1, 1);
        TimeSpan span = createdAtTime - epoch;
        return data.supportId.ToString() + Math.Round(span.TotalMilliseconds).ToString();
    }

    public void AcceptTOS()
    {
        data.isTOSAccepted = true;
        Update("isTOSAccepted", true);
    }

    public void SetName(string name)
    {
        data.name = name;
        Update("name", name);
    }

    public void CreateCharacter()
    {
        data.isCharacterCreated = true;
        Update("isCharacterCreated", true);
    }

    public void MarkLoginTime()
    {
        data.lastLogInTime = Epoch.Current();
        Update("lastLogInTime", data.lastLogInTime);
    }

    public void RemoveAds()
    {
        data.adsRemoved = true;
        Update("adsRemoved", data.adsRemoved);
    }
}
#endif