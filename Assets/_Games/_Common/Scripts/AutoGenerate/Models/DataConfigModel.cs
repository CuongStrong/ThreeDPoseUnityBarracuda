/// <summary>
/// AutoGenerate Class, do not modify
/// </summary>
#if FIRESTORE
using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;

namespace DataConfig
{
  [Serializable]
  public class DataConfigModel
  {
    public Dictionary<string, AdsType> ads = new Dictionary<string, AdsType>();
    public Dictionary<string, Banned_wordsType> banned_words = new Dictionary<string, Banned_wordsType>();
    public Dictionary<string, BonusType> bonus = new Dictionary<string, BonusType>();
    public Dictionary<string, BotsType> bots = new Dictionary<string, BotsType>();
    public ConfigType config = new ConfigType();
    public Dictionary<string, LevelType> level = new Dictionary<string, LevelType>();
    public Dictionary<string, Power_upsType> power_ups = new Dictionary<string, Power_upsType>();
    public Dictionary<string, Push_notificationType> push_notification = new Dictionary<string, Push_notificationType>();
    public Dictionary<string, ShopType> shop = new Dictionary<string, ShopType>();
    public Dictionary<string, TrackingType> tracking = new Dictionary<string, TrackingType>();

    public void AddDocument(DocumentSnapshot doc)
    {
      if (doc.Id.CompareTo("ads") == 0)
      {
        this.ads = doc.ConvertTo<Dictionary<string, DataConfig.AdsType>>();
      }
      if (doc.Id.CompareTo("banned_words") == 0)
      {
        this.banned_words = doc.ConvertTo<Dictionary<string, DataConfig.Banned_wordsType>>();
      }
      if (doc.Id.CompareTo("bonus") == 0)
      {
        this.bonus = doc.ConvertTo<Dictionary<string, DataConfig.BonusType>>();
      }
      if (doc.Id.CompareTo("bots") == 0)
      {
        this.bots = doc.ConvertTo<Dictionary<string, DataConfig.BotsType>>();
      }
      if (doc.Id.CompareTo("config") == 0)
      {
        this.config = doc.ConvertTo<ConfigType>();
      }
      if (doc.Id.CompareTo("level") == 0)
      {
        this.level = doc.ConvertTo<Dictionary<string, DataConfig.LevelType>>();
      }
      if (doc.Id.CompareTo("power_ups") == 0)
      {
        this.power_ups = doc.ConvertTo<Dictionary<string, DataConfig.Power_upsType>>();
      }
      if (doc.Id.CompareTo("push_notification") == 0)
      {
        this.push_notification = doc.ConvertTo<Dictionary<string, DataConfig.Push_notificationType>>();
      }
      if (doc.Id.CompareTo("shop") == 0)
      {
        this.shop = doc.ConvertTo<Dictionary<string, DataConfig.ShopType>>();
      }
      if (doc.Id.CompareTo("tracking") == 0)
      {
        this.tracking = doc.ConvertTo<Dictionary<string, DataConfig.TrackingType>>();
      }
    }
  }

  [FirestoreData]
  public class AdsType
  {
    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int rowIdx { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int interstitialSeqTime { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string rewardVideoId { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreBoolConverter))]
    public bool enabled { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string interstitialId { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string iosGameId { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string id { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string bannerId { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreBoolConverter))]
    public bool testMode { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string androidGameId { get; set; }

  }

  [FirestoreData]
  public class Banned_wordsType
  {
    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int rowIdx { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string name { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int id { get; set; }

  }

  [FirestoreData]
  public class BonusType
  {
    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int roadLengthBonus { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float playerSpeed { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float offsetItemY { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float offsetItemX { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int bonus { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float descendSpeed { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int minItem { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float spawnRate { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float ascendSpeed { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int factor5Weight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int diamondWeight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int factor3Weight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int factor4Weight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int rowIdx { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int factor2Weight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int maxItem { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float distanceItemY { get; set; }

  }

  [FirestoreData]
  public class BotsType
  {
    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int rowIdx { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string name { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string id { get; set; }

  }

  [FirestoreData]
  public class ConfigType
  {
    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string backgroundColor { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long minNameCharacters { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreBoolConverter))]
    public bool enableAds { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long defaultCameraFov { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long defaultCameraOthorSize { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long defaultCameraRotX { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long levelTriggerRating { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long defaultCameraPosZ { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreDoubleConverter))]
    public double defaultCameraPosY { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long defaultCameraRotY { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long defaultCameraBonusFov { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreDoubleConverter))]
    public double defaultCameraPosX { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string earthColor { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long defaultCameraRotZ { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long amountEnemies { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string fireColor { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string lineRendererColor { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreDoubleConverter))]
    public double lineRendererMindistance { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreDoubleConverter))]
    public double lineRendererThreshold { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreDoubleConverter))]
    public double lineRendererWidth { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long maxNameCharacters { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string neutralColor { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string plaformBorderColor { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string plaformColor { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreLongConverter))]
    public long playerSize { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string soundBgmName { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string waterColor { get; set; }

  }

  [FirestoreData]
  public class LevelType
  {
    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float playerSpeed { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float offsetItemY { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float offsetItemX { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int magnetWeight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float spawnRate { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int skullWeight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float ascendSpeed { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int factor5Weight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int diamondWeight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int factor3Weight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int maxItem { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float distanceItemY { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int factor4Weight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int rowIdx { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int factor2Weight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int roadLength { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int level { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int stoneWeight { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int minItem { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreFloatConverter))]
    public float descendSpeed { get; set; }

  }

  [FirestoreData]
  public class Power_upsType
  {
    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int qty { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int cooldown { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int index { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int init { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string id { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int rowIdx { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int cost { get; set; }

  }

  [FirestoreData]
  public class Push_notificationType
  {
    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string title { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string id { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int minute { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string Note { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string body { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int rowIdx { get; set; }

  }

  [FirestoreData]
  public class ShopType
  {
    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string id { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string icon { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string type { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreBoolConverter))]
    public bool enabled { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string usd { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int quantity { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string effect { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string apple_store_id { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string category { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int rowIdx { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string google_store_id { get; set; }

  }

  [FirestoreData]
  public class TrackingType
  {
    [FirestoreProperty(ConverterType=typeof(FirestoreIntConverter))]
    public int rowIdx { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string properties { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string name { get; set; }

    [FirestoreProperty(ConverterType=typeof(FirestoreStringConverter))]
    public string id { get; set; }

  }
}
#endif
