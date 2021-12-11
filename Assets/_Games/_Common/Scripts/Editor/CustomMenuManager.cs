#if UNITY_EDITOR

using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class CustomMenuManager
{
    private static Dictionary<string, string> _filePathDict, _directoryPathDict;

    [MenuItem("Custom/Copy Asset Path")]
    public static void CopyAssetPath()
    {
        GUIUtility.systemCopyBuffer = AssetDatabase.GetAssetPath(Selection.activeObject);
    }

    [MenuItem("Custom/Reset Data Save")]
    private static void ResetDataSave()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        try
        {
            string savepath = Application.persistentDataPath + '/';

            if (File.Exists(savepath + SaveGameIO.MANDATORY_SAVE_NAME))
            {
                File.Delete(savepath + SaveGameIO.MANDATORY_SAVE_NAME);
            }
            if (File.Exists(savepath + "_" + SaveGameIO.MANDATORY_SAVE_NAME))
            {
                File.Delete(savepath + "_" + SaveGameIO.MANDATORY_SAVE_NAME);
            }

            if (File.Exists(savepath + SaveGameIO.OPTIONAL_SAVE_NAME))
            {
                File.Delete(savepath + SaveGameIO.OPTIONAL_SAVE_NAME);
            }
            if (File.Exists(savepath + "_" + SaveGameIO.OPTIONAL_SAVE_NAME))
            {
                File.Delete(savepath + "_" + SaveGameIO.OPTIONAL_SAVE_NAME);
            }

            Debug.Log("Reset Data Save SUCCESS !!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Reset Data Save FAIL!! : " + e.Message);
        }
    }

    [MenuItem("Custom/Create Prefab (by select gameObjects)")]
    private static void CreatePrefab()
    {
        GameObject[] objectArray = Selection.gameObjects;

        foreach (GameObject gameObject in objectArray)
        {
            MeshRenderer[] selectedMeshRendererChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var t in selectedMeshRendererChildren)
            {
                MeshRenderer selectedMeshRenderer = t;
                if (selectedMeshRenderer != null)
                {
                    Material selectedMat = selectedMeshRenderer.sharedMaterial;
                    if (selectedMat != null && selectedMat.name.Contains("(Instance)"))
                    {
                        Material materialInstance = new Material(selectedMat);

                        string path = "Assets/_Prefab/_MaterialInstance/";
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        path += materialInstance.name + ".mat";
                        path = AssetDatabase.GenerateUniqueAssetPath(path);
                        AssetDatabase.CreateAsset(materialInstance, path);

                        selectedMeshRenderer.sharedMaterial = materialInstance;
                    }
                }
            }

            string localPath = "Assets/_Prefab/";
            if (!Directory.Exists(localPath)) Directory.CreateDirectory(localPath);
            localPath += gameObject.name + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);

            Debug.Log("Create Prefab SUCCESS at " + localPath);
        }

        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }

    [MenuItem("Custom/Auto Generate Constants Class")]
    private static void GenerateConstantsClass()
    {
        Dictionary<string, string> tagDic = InternalEditorUtility.tags.ToDictionary(value => value);
        global::GenerateConstantsClass.Create("Tags", tagDic);

        Dictionary<string, int> layerNoDic = InternalEditorUtility.layers.ToDictionary(layer => layer, layer => LayerMask.NameToLayer(layer));
        Dictionary<string, int> layerMaskNoDic = InternalEditorUtility.layers.ToDictionary(layer => layer, layer => 1 << LayerMask.NameToLayer(layer));

        global::GenerateConstantsClass.Create("Layers", layerNoDic);
        global::GenerateConstantsClass.Create("LayersMask", layerMaskNoDic);

        Dictionary<string, string> sortingLayerDic = GetSortingLayerNames().ToDictionary(value => value);
        global::GenerateConstantsClass.Create("SortingLayers", sortingLayerDic);

        Dictionary<string, string> playerSettingsDic = new Dictionary<string, string>()
        {
            {"ProductName"     , PlayerSettings.productName},
            {"BundleVersion"   , PlayerSettings.bundleVersion},
            {"BundleIdentifier", PlayerSettings.applicationIdentifier}
        };

        global::GenerateConstantsClass.Create("PlayerSetting", playerSettingsDic);

        GenerateResourcesFilePath();
    }

    [MenuItem("Custom/Remove MissingComponents")]
    public static void RemoveMissingScripstsInPrefabsAtPath()
    {
        string PATH = "Asset";

        EditorUtility.DisplayProgressBar("Modify Prefab", "Please wait...", 0);
        string[] ids = AssetDatabase.FindAssets("t:Prefab", new string[] { PATH });
        for (int i = 0; i < ids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(ids[i]);
            GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            int delCount = 0;
            RecursivelyModifyPrefabChilds(instance, ref delCount);

            if (delCount > 0)
            {
                Debug.Log($"Removed({delCount}) on {path}", prefab);
                PrefabUtility.SaveAsPrefabAssetAndConnect(instance, path, InteractionMode.AutomatedAction);
            }

            UnityEngine.Object.DestroyImmediate(instance);
            EditorUtility.DisplayProgressBar("Modify Prefab", "Please wait...", i / (float)ids.Length);
        }
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
    }

    private static void RecursivelyModifyPrefabChilds(GameObject obj, ref int delCount)
    {
        if (obj.transform.childCount > 0)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                var _childObj = obj.transform.GetChild(i).gameObject;
                RecursivelyModifyPrefabChilds(_childObj, ref delCount);
            }
        }

        int innerDelCount = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
        delCount += innerDelCount;
    }

    private static string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        List<string[]> assetsList = new List<string[]>()
        {
            importedAssets
        };

        List<string> targetDirectoryNameList = new List<string>()
        {
            "ProjectSettings"
        };

        if (ExistsDirectoryInAssets(assetsList, targetDirectoryNameList))
        {
            GenerateConstantsClass();
        }
    }

    static bool ExistsDirectoryInAssets(List<string[]> assetsList, List<string> targetDirectoryNameList)
    {
        return assetsList
                .Any(assets => assets
                   .Select(asset => System.IO.Path.GetDirectoryName(asset))
                   .Intersect(targetDirectoryNameList)
                   .Count() > 0);
    }

    private static void GenerateResourcesFilePath()
    {
        _filePathDict = new Dictionary<string, string>();
        _directoryPathDict = new Dictionary<string, string>();

        string[] resourcesDirectoryPaths = Directory.GetDirectories("Assets", "Resources", SearchOption.AllDirectories);
        ValidateOSPath(ref resourcesDirectoryPaths);

        foreach (string resourcesDirectoryPath in resourcesDirectoryPaths)
        {
            SetFilePath(resourcesDirectoryPath, "");
            SetDirectoryPath(resourcesDirectoryPath);
        }

        global::GenerateConstantsClass.Create("ResourcesPath", _filePathDict);
        global::GenerateConstantsClass.Create("ResourcesDirPath", _directoryPathDict);
    }

    private static void SetDirectoryPath(string parentPath)
    {
        string[] childPaths = Directory.GetDirectories(parentPath, "*", SearchOption.AllDirectories);
        ValidateOSPath(ref childPaths);

        foreach (string childPath in childPaths)
        {
            string relativePath = childPath.Substring(parentPath.Length + 1, childPath.Length - parentPath.Length - 1);
            _directoryPathDict[relativePath] = relativePath;

            SetFilePath(childPath, relativePath + "/");
        }
    }

    private static void SetFilePath(string absolutePath, string relativePath)
    {
        string[] childPaths = Directory.GetFiles(absolutePath, "*", SearchOption.TopDirectoryOnly);
        ValidateOSPath(ref childPaths);

        foreach (string childPath in childPaths)
        {
            string fileType = GetFileTypeFromExtention(Path.GetExtension(childPath));
            if (string.IsNullOrEmpty(fileType))
            {
                continue;
            }

            string fileName = Path.GetFileNameWithoutExtension(childPath);
            string key = fileType + "_" + fileName;
            if (_filePathDict.ContainsKey(key))
            {
                Debug.Log(key + " duplicate!");
            }
            _filePathDict[key] = relativePath + fileName;
        }
    }

    private static string GetFileTypeFromExtention(string extension)
    {
        if (new List<string>() { ".anim" }.Contains(extension))
        {
            return "Animation";
        }
        else if (new List<string>() { ".wav", ".mp3", ".ogg", ".aif", ".aiff", ".xm", ".mod", ".it", ".s3m" }.Contains(extension))
        {
            return "Audio";
        }
        else if (new List<string>() { ".cubemap", ".hdr" }.Contains(extension))
        {
            return "Cubemap";
        }
        else if (new List<string>() { ".ttf", ".otf", ".dfont" }.Contains(extension))
        {
            return "Font";
        }
        else if (new List<string>() { ".prefab" }.Contains(extension))
        {
            return "Prefab";
        }
        else if (new List<string>() { ".mat", ".material" }.Contains(extension))
        {
            return "Material";
        }
        else if (new List<string>() { ".fbx", ".obj", ".max", ".blend" }.Contains(extension))
        {
            return "Mesh";
        }
        else if (new List<string>() { ".mov", ".mpg", ".mpeg", ".mp4", ".avi", ".asf" }.Contains(extension))
        {
            return "Movie";
        }
        else if (new List<string>() { ".physicmaterial" }.Contains(extension))
        {
            return "Physicmaterial";
        }
        else if (new List<string>() { ".shader" }.Contains(extension))
        {
            return "Shader";
        }
        else if (new List<string>() { ".txt", ".htm", " .html", ".xml", ".bytes", ".json", ".csv", ".yaml", ".fnt" }.Contains(extension))
        {
            return "Text";
        }
        else if (new List<string>() { ".exr", ".psd", ".tif", ".tiff", ".jpg", ".tga", ".png", ".gif", ".bmp", ".iff", ".pict" }.Contains(extension))
        {
            return "Texture";
        }
        else if (new List<string>() { ".asset" }.Contains(extension))
        {
            return "Asset";
        }
        else if (new List<string>() { ".unity" }.Contains(extension))
        {
            return "Scene";
        }

        return "";
    }

    private static void ValidateOSPath(ref string[] paths)
    {
#if UNITY_EDITOR_WIN
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = paths[i].Replace("\\", "/"); 
            }
#endif
    }
}
#endif //UNITY_EDITOR
