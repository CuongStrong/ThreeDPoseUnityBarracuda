#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InjectEditorFlagToScripts : EditorWindow
{
    [MenuItem("Assets/Inject UNITY_EDITOR To Scripts")]
    public static void Inject()
    {
        Debug.ClearDeveloperConsole();
        Debug.Log("Waiting inject UNITY_EDITOR... ");

        var assetFiles = GetFiles(GetSelectedPathOrFallback()).Where(s => s.Contains(".meta") == false);

        bool success = false;
        foreach (string f in assetFiles)
        {
            success = InsertText(f, "#if UNITY_EDITOR", "#endif //UNITY_EDITOR");
            Debug.Log(string.Format("Inject {0} {1}", f, success ? "SUCCESS" : "FAIL"));
        }
    }

    //https://stackoverflow.com/questions/12333892/how-to-write-to-beginning-of-file-with-stream-writer
    public static bool InsertText(string path, string beginText, string endText)
    {
        try
        {
            if (File.Exists(path))
            {
                string oldText = File.ReadAllText(path);
                using (var sw = new StreamWriter(path, false))
                {
                    sw.WriteLine(beginText);
                    sw.WriteLine(oldText);
                    sw.WriteLine(endText);
                }
            }
            else File.WriteAllText(path, beginText);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string GetSelectedPathOrFallback()
    {
        string path = "Assets";

        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
    static IEnumerable<string> GetFiles(string path)
    {
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(path);
        while (queue.Count > 0)
        {
            path = queue.Dequeue();
            try
            {
                foreach (string subDir in Directory.GetDirectories(path))
                {
                    queue.Enqueue(subDir);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            string[] files = null;
            try
            {
                files = Directory.GetFiles(path, "*.cs");
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            if (files != null)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    yield return files[i];
                }
            }
        }
    }
}

#endif //UNITY_EDITOR
