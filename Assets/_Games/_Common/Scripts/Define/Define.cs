using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public static partial class Define
{
    public static float DEFAULT_CAMERA_FOV = 25;
    public static float DEFAULT_CAMERA_OTHORSIZE = 5;
    public static Vector3 DEFAULT_CAMERA_POS = new Vector3(-1.712f, 0.26f, -0.309f);
    public static Vector3 DEFAULT_CAMERA_ROT = new Vector3(5.112f, 83.76f, 0.117f);

    public const bool useCheat = false;
    public const string gameSceneName = "BounceAndDestroy";
    public static List<string> workingStages = new List<string>()
    {
        "001", "002","003","004"
    };

    public static void NextStage(bool next) => DataSave.Instance.level = Mathf.Max(1, (DataSave.Instance.level + (next ? 1 : -1)) % (workingStages.Count + 1));
    public static string StageNameByLevel() => string.Format("Stage{0}", workingStages[(DataSave.Instance.level - 1) % workingStages.Count]);
    public static string StageIDByLevel() => workingStages[(DataSave.Instance.level - 1) % workingStages.Count];

    public static bool CanTrackingLevel => DataSave.Instance.level < 50 || (DataSave.Instance.level % 10 == 0);

    public static void ReloadScene()
    {
        DOTween.KillAll();
        AudioManager.Instance.FadeOutBGM();
        PoolManager.Instance.DespawnAll();
        SceneManager.LoadScene(gameSceneName);
    }
}
