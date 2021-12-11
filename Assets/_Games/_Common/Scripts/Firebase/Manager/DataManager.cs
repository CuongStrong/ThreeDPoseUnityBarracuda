#if FIRESTORE
using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using DataConfig;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum EDataManagerState
{
    NONE,
    INIT,
    READY,
    ERROR
}

public class DataManager : MonoBehaviourPersistence<DataManager>
{
    public static event Action<EDataManagerState> OnDataManagerStateChanged = delegate { };
    public bool isReady => state == EDataManagerState.READY;
    public bool isError => state == EDataManagerState.ERROR;


    public Exception lastException { get; private set; }
    public FirebaseApp firebaseApp { get; private set; }
    public FirebaseFirestore firestoreInstance { get; private set; }
    public FirebaseAuth firebaseAuth { get; private set; }
    public FirebaseUser firebaseUser { get; private set; }

    public ProfileModel profile { get; private set; }

    public DataConfigModel config { get; private set; }

    private bool isRunning;

    private EDataManagerState _state;
    public EDataManagerState state
    {
        get => _state;
        set
        {
            if (_state == value) return;
            _state = value;
            OnDataManagerStateChanged.Invoke(_state);
        }
    }

    private void Awake()
    {
        isRunning = false;
        config = new DataConfigModel();
        profile = new ProfileModel();
    }

    private void Start()
    {
        Initialize();
        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    private void OnDestroy()
    {
        StopRealtimeDatabase();
        firestoreInstance = null;
        firebaseApp = null;
        firebaseAuth = null;

        SceneManager.sceneUnloaded -= SceneUnloaded;
    }

    private void SceneUnloaded(Scene arg0)
    {
        profile.UnregisterEvents();
    }

    public async void Initialize()
    {
        state = EDataManagerState.INIT;

        var dependencyStatus = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            firebaseApp = FirebaseApp.Create();
            firestoreInstance = FirebaseFirestore.GetInstance(firebaseApp);
            firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);

            PullDataConfig();
            LocalizationManager.Instance.PullData(GetEnvSuffix());
            await Login();
        }
        else
        {
            state = EDataManagerState.ERROR;
            lastException = new Exception(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        }
    }

    public async void PullDataConfig()
    {
        try
        {
            var db = firestoreInstance;

            string dataConfigBaseName = "data_config_" + GetEnvSuffix();

            var dataConfigRef = db.Collection(dataConfigBaseName);

            var snapshot = await dataConfigRef.GetSnapshotAsync();

            config = new DataConfigModel();
            foreach (var doc in snapshot.Documents)
            {
                config.AddDocument(doc);
            }

            state = EDataManagerState.READY;
        }
        catch (Exception ex)
        {
            state = EDataManagerState.ERROR;
            lastException = ex;

            Debug.LogError(ex.Message);
        }
    }

    public async Task Login()
    {
        firebaseUser = null;
        StopRealtimeDatabase();

        try
        {
            if (firebaseAuth.CurrentUser != null)
            {
                firebaseUser = firebaseAuth.CurrentUser;
                Debug.Log(String.Format("Current Anonymous User ID: {0}", firebaseUser.UserId));
            }
            else
            {
                firebaseUser = await firebaseAuth.SignInAnonymouslyAsync();
                Debug.Log(String.Format("SignInAnonymouslyAsync User ID: {0}", firebaseUser.UserId));
            }

            if (firebaseUser != null)
            {
                StartRealtimeDatabase(firebaseUser.UserId);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());

            state = EDataManagerState.ERROR;
            lastException = ex;
        }
    }

    public async void StartRealtimeDatabase(string userId)
    {
        if (isRunning) return;
        isRunning = true;

        Debug.Log("Start Realtime Databases");

        string envSuffix = GetEnvSuffix();

        await Task.WhenAll(
            profile.Start(userId, envSuffix)
        );
    }

    public void StopRealtimeDatabase()
    {
        if (!isRunning) return;
        isRunning = false;

        Debug.Log("Stop Realtime Databases");

        profile.Stop();
    }


    public string GetEnvSuffix()
    {
#if PRODUCTION
        return "pro";
#elif STAGING
        return "sta";
#else
        return "dev";
#endif
    }
}
#endif

