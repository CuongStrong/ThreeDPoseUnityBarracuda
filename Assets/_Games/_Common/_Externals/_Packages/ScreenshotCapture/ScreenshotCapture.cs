
using System;
using System.IO;

using UnityEngine;
using Debug = UnityEngine.Debug;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(ScreenshotCapture), true)]
public class ScreenshotCaptureEditor : Editor
{
    private ScreenshotCapture _target;

    void OnEnable()
    {
        _target = (ScreenshotCapture)base.target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate PNG"))
        {
            _target.Take();
            EditorUtility.SetDirty(base.target);
        }
    }

    // void OnSceneGUI()
    // {
    //     Event e = Event.current;
    //     switch (e.type)
    //     {
    //         case EventType.KeyDown:
    //             {
    //                 _target.Take();
    //                 EditorUtility.SetDirty(base.target);
    //                 break;
    //             }
    //     }
    // }
}


[ExecuteInEditMode]
public class ScreenshotCapture : MonoBehaviour
{
    [Header("Target Settings")]
    [Range(-45, 45)]
    public int RotationX;
    [Range(-45, 45)]
    public int RotationY;
    [Range(-45, 45)]
    public int RotationZ;

    [Header("Camera Settings")]
    public bool isCameraOrthographic;
    [Range(-5, 5)]
    public float CameraX;
    [Range(-5, 5)]
    public float CameraY;
    [Range(-5, 5)]
    public float CameraZ = -8;

    [Header("Export Settings")]
    public string FileName = "Image";
    public int ResolutionWidth = 256;
    public int ResolutionHeight = 256;

    public static string ScreenshotFolderPath => "Assets/_Screenshots/";
    public static string GetFileName(string name, string extention, int width, int height) => $"{ScreenshotFolderPath}{name}_{width}x{height}_{System.DateTime.Now.ToString("yyyyMMddHHmmssfff")}.{extention}";

    void Update()
    {
        var objRot = transform.eulerAngles;
        var newRot = new Vector3(RotationX, RotationY, RotationZ);
        if (objRot != newRot)
            transform.localRotation = Quaternion.Euler(RotationX, RotationY, RotationZ);

        var camPos = Camera.main.transform;
        var newPos = new Vector3(CameraX, CameraY, CameraZ);
        if (camPos.position != newPos)
        {
            Camera.main.orthographic = isCameraOrthographic;
            if (isCameraOrthographic)
                Camera.main.orthographicSize = CameraZ * -1;
            camPos.localPosition = newPos;
        }

        if (Input.GetKey(KeyCode.C) && Selection.gameObjects != null)
        {
            string filename = ScreenshotFolderPath + Selection.gameObjects[0].name + ".png";
            GeneratePNG(filename);
            AssetDatabase.Refresh();
        }
    }

    public void Take()
    {
        string filename = GetFileName(FileName, "png", (int)ResolutionWidth, (int)ResolutionHeight);
        GeneratePNG(filename);
        AssetDatabase.Refresh();
    }

    public void GeneratePNG(string filename)
    {
        try
        {
            if (!Directory.Exists(ScreenshotFolderPath))
            {
                Directory.CreateDirectory(ScreenshotFolderPath);
            }

            var camera = Camera.main;
            var renderTexture = new RenderTexture((int)ResolutionWidth, (int)ResolutionHeight, 24);
            camera.targetTexture = renderTexture;
            var screenShot = new Texture2D((int)ResolutionWidth, (int)ResolutionHeight, TextureFormat.ARGB32, false);
            screenShot.alphaIsTransparency = true;
            camera.Render();
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, (int)ResolutionWidth, (int)ResolutionHeight), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null;

            DestroyImmediate(renderTexture);

            byte[] bytes = screenShot.EncodeToPNG();
            System.IO.File.WriteAllBytes(filename, bytes);

            Debug.Log(string.Format("Took screenshot to: {0}", filename));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"{ex}");
        }
    }

}


#endif