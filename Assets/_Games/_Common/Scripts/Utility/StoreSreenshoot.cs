using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public static class StoreSreenshoot
{
#if UNITY_EDITOR
    public static string Key_Store_Screenshot = "Path_Screenshot";

    [MenuItem("Custom/Capture/Capture Screen")]
    public static void CaptureScreen()
    {
        var path = PlayerPrefs.GetString(Key_Store_Screenshot, "");
        if (path != "")
        {
            path = EditorUtility.OpenFolderPanel("Save at", "", "");
            PlayerPrefs.SetString(Key_Store_Screenshot, path);
        }

        var FileName = string.Format("Screenshot_{0}.png", System.DateTime.Now.ToString());
        FileName = FileName.Replace(":", "_");
        FileName = FileName.Replace("/", "_");
        FileName.Trim();
        string filePath = System.IO.Path.Combine(path, FileName);
        Debug.Log(filePath);
        ScreenCapture.CaptureScreenshot(filePath);
    }

    [MenuItem("Custom/Capture/Clean Capture Path ")]
    public static void ClearCaptureScreenPath()
    {
        PlayerPrefs.SetString(Key_Store_Screenshot, "");
    }
#endif
}
