using UnityEngine;

public class SaveGameManager : MonoBehaviourPersistence<SaveGameManager>
{
    private void RegisterAllSaveData()
    {
        DataSave.Instance.RegisterSaveData();
    }

    private void Awake()
    {
        RegisterAllSaveData();

        // Benchmark.Start();
        SaveGameIO.Instance.Load();
        // Benchmark.Stop("SaveGameManager Load");
    }

    public void RefeshManual()
    {
        SaveGameIO.Instance.Load();
    }

    public void Save()
    {
        // Benchmark.Start();
        SaveGameIO.Instance.Save(true, false);
        // Benchmark.Stop("SaveGameManager Save");
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Save();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Save();
        }
    }

    void OnApplicationQuit()
    {
        Save();
    }

    public static void ResetDataSave()
    {
        DataSave.NewInstance();
        SaveGameManager.Instance.Save();
    }
}