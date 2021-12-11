// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using DG.Tweening;
// using System;
// using System.Linq;
// using UnityEngine.SceneManagement;
// using Random = UnityEngine.Random;

// #if FIRESTORE
// using DataConfig;
// #endif

// namespace SampleGame
// {
//     public enum EGameState
//     {
//         NONE,
//         TITTLE,
//         PLAYING,
//         WIN,
//         LOSE,
//     }

//     public class GameSceneController : BaseSceneController
//     {
//         public static event Action<EGameState> OnGameStateChanged = (t) => { Debug.Log(string.Format("=>GameState {0}", t)); };
//         public static event Action<int> OnScoreChanged = delegate { };

//         [SerializeField] bool devMode;

//         public static float playingTime;
//         public static GameSceneController Instance;
//         private static bool firstLoadScene;

//         public DataSave dataSave => DataSave.Instance;
//         public UIManager uiManager => UIManager.Instance;
//         public PoolManager poolManager => PoolManager.Instance;
//         public PrefabConfig prefabConfig => PrefabConfig.Entity;
//         public bool IsPlaying => state == EGameState.PLAYING;
//         public void EndGame(bool clear) => state = clear ? EGameState.WIN : EGameState.LOSE;
//         public bool IsTutorialLevel => dataSave.level == 1;
//         // public BaseMenu tittleMenu => uiManager.GetMenu(ResourcesPath.PREFAB_TITTLE_MENU);
//         // public BaseMenu mainMenu => uiManager.GetMenu(ResourcesPath.PREFAB_MAIN_MENU);
//         // public BaseMenu debugMenu => uiManager.GetMenu(ResourcesPath.PREFAB_DEBUG_MENU);
//         // public BaseMenu winMenu => uiManager.GetMenu(ResourcesPath.PREFAB_WIN_MENU);
//         // public BaseMenu loseMenu => uiManager.GetMenu(ResourcesPath.PREFAB_LOSE_MENU);

// #if FIRESTORE
//         public AdsType adsConfig => DataManager.Instance.config.ads["unityAds"];
//         public DataConfigModel dataConfig => DataManager.Instance.config;
//         protected ConfigType config => dataConfig.config;
//         protected ProfileData profile => DataManager.Instance.profile.data;
// #endif

//         public Camera mainCamera { get; private set; }
//         public Stage currentStage { get; private set; }

//         private bool wasContinue;

//         private int _score;
//         public int score
//         {
//             get => _score;
//             set
//             {
//                 _score = value;
//                 OnScoreChanged.Invoke(_score);
//             }
//         }

// #if FIRESTORE
//         public LevelType levelConfig
//         {
//             get
//             {
//                 var levels = dataConfig.level;
//                 if (levels.TryGetValue(dataSave.level.ToString(), out LevelType levelType))
//                     return levelType;

//                 //Get config of max level
//                 int maxLevel = 0;
//                 foreach (var t in levels.Keys)
//                 {
//                     if (int.TryParse(t, out int lv))
//                     {
//                         maxLevel = Mathf.Max(maxLevel, lv);
//                     }
//                 }
//                 return levels[maxLevel.ToString()];
//             }
//         }
// #endif

//         private EGameState _state;
//         public EGameState state
//         {
//             get => _state;
//             set
//             {
//                 if (_state == value) return;
//                 _state = value;

//                 switch (_state)
//                 {
//                     case EGameState.TITTLE:
//                         {
//                             InitDevice();
//                             InitConfig();
//                         }
//                         break;
//                     case EGameState.WIN:
//                         {
//                             if (Define.CanTrackingLevel)
//                                 TrackingLevelClear();

//                             if (!dataSave.trackingFirstPlayCount)
//                             {
//                                 dataSave.trackingFirstPlayCount = true;
//                                 SaveGameManager.Instance.Save();
//                                 TrackingFirstPlayCount();
//                             }

//                             PoolManager.Instance.pools[PoolNames.CLEAR_PARTICLE].Spawn(Vector3.zero, 2);
//                             VibrationManager.Haptic(HapticType.HeavyImpact);

//                             this.Invoke(1.2f, () =>
//                             {
//                                 poolManager.DespawnAll();
//                                 winMenu.Show();
//                             });

//                             if (DataSave.Instance.level % 5 == 0 && !DataSave.Instance.rated)
//                             {
//                                 this.Invoke(1f, () => UIManager.Instance.GetPopup(ResourcesPath.PREFAB_RATING_POPUP).Show(CheckShowIntertisial));
//                             }
//                             else
//                             {
//                                 CheckShowIntertisial();
//                             }

//                             DataSave.Instance.level++;
//                             SaveGameManager.Instance.Save();
//                         }
//                         break;
//                     case EGameState.LOSE:
//                         {
//                             this.Invoke(1, () =>
//                             {
//                                 uiManager.GetMenu(ResourcesPath.PREFAB_LOSE_MENU).Show();

//                                 if (wasContinue)
//                                 {
// #if UNITY_ADS
//                                     (uiManager.GetMenu(ResourcesPath.PREFAB_LOSE_MENU) as LoseMenu).adsButton.SetInteractable(false);
// #endif
//                                 }
//                             });
//                         }
//                         break;
//                 }

//                 OnGameStateChanged.Invoke(_state);
//             }
//         }

//         private void Awake()
//         {
//             if (!BootstrapSceneController.isReady)
//             {
//                 SceneManager.LoadScene("Bootstrap");
//                 return;
//             }

//             Instance = this;
//             state = EGameState.TITTLE;
//         }

//         private void OnEnable()
//         {
//             FullScreenTouchHandler.OnTouchDown += OnTouchDown;
//         }

//         private void OnDisable()
//         {
//             FullScreenTouchHandler.OnTouchDown -= OnTouchDown;
//         }

//         void OnTouchDown(Vector2 position)
//         {

//         }

//         void InitDevice()
//         {
//             Application.targetFrameRate = 60;
//             mainCamera = Camera.main;

//             if (Utility.isTallPhone)
//             {

//             }
//             else if (Utility.isTablet)
//             {

//             }
//         }

//         void InitConfig()
//         {
//             if (!dataSave.firstPlay)
//             {
//                 dataSave.adsRemoved = profile.adsRemoved;
//                 dataSave.firstPlay = true;
//                 SaveGameManager.Instance.Save();
//             }

//             if (!firstLoadScene)
//             {
//                 firstLoadScene = true;

//                 Define.DEFAULT_CAMERA_FOV = (float)config.defaultCameraFov;
//                 Define.DEFAULT_CAMERA_POS = new Vector3((float)config.defaultCameraPosX, (float)config.defaultCameraPosY, (float)config.defaultCameraPosZ);
//                 Define.DEFAULT_CAMERA_ROT = new Vector3((float)config.defaultCameraRotX, (float)config.defaultCameraRotY, (float)config.defaultCameraRotZ);
//             }

//             mainCamera.fieldOfView = Define.DEFAULT_CAMERA_FOV;
//             mainCamera.transform.localPosition = Define.DEFAULT_CAMERA_POS;
//             mainCamera.transform.localEulerAngles = Define.DEFAULT_CAMERA_ROT;

//             tittleMenu.Show();
//             mainMenu.Hide();
//             debugMenu.Hide();
//             winMenu.Hide();
//             loseMenu.Hide();

// #if !PRODUCTION
//             if (Define.useCheat) uiManager.GetMenu(ResourcesPath.PREFAB_DEBUG_MENU).Show();
// #endif
//         }

//         public void GoPlay()
//         {
//             state = EGameState.PLAYING;
//             TrackingPlayCount();

//             if (IsTutorialLevel)
//                 ShowTutorial();
//             else
//             {

//             }
//         }

//         public void Continue()
//         {
//             state = EGameState.PLAYING;
//             wasContinue = true;
//             loseMenu.Hide();
//             TrackingPlayCount();
//         }

//         void ShowTutorial()
//         {

//         }

//         private void Update()
//         {
//             if (IsPlaying)
//             {

//             }

//             playingTime += Time.deltaTime;
//         }

//         void CheckShowIntertisial()
//         {
// #if FIRESTORE
//             if (!DataSave.Instance.adsRemoved && playingTime > adsConfig.interstitialSeqTime)
//             {
//                 playingTime = 0;
//                 this.Invoke(1f, () => AdsManager.Instance.ShowInterstial());
//             }
// #endif
//         }

//         void LoadStage()
//         {
//             if (devMode)
//             {
//                 currentStage = FindObjectOfType<Stage>();
//             }
//             else
//             {
//                 var prefapStage = Resources.Load<Stage>(string.Format("Stages/{0}", Define.StageNameByLevel()));
//                 currentStage = Instantiate(prefapStage, prefapStage.transform.position, prefapStage.transform.rotation, this.transform);
//             }
//         }

//         void TrackingPlayCount() => TrackingMangager.Instance.LogEvent("GamePlayCount");
//         void TrackingFirstPlayCount() => TrackingMangager.Instance.LogEvent("FirstPlayCount");
//         void TrackingLevelClear() => TrackingMangager.Instance.LogEvent(string.Format("Lv_{0}_Clear", dataSave.level));

//     }
// }