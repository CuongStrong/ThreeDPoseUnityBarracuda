using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(SaveMesh), true)]
public class SaveMeshEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveMesh myScript = (SaveMesh)target;
        if (GUILayout.Button("Save to Assets"))
        {
            myScript.Save();
        }

        if (GUILayout.Button("Get Center"))
        {
            myScript.GetCenter();
        }
    }
}
#endif

public class SaveMesh : MonoBehaviour
{
    public SkinnedMeshRenderer skinMeshRender;
    public MeshFilter meshFilter;

#if UNITY_EDITOR
    public void Save()
    {
        if (skinMeshRender != null)
        {
            var mesh = new Mesh();
            skinMeshRender.BakeMesh(mesh);
            AssetDatabase.CreateAsset(mesh, string.Format("Assets/{0}_skinMesh" + ".asset", skinMeshRender.name));
        }

        if (meshFilter != null)
        {
            AssetDatabase.CreateAsset(meshFilter.mesh, string.Format("Assets/{0}_meshFilter" + ".asset", meshFilter.name));
        }

        // string path = string.Format("Assets/LazyCat/GoodJob/Model/RaceMesh/{0} {1}.asset", transform.parent.name, meshFilter.gameObject.name);
        // AssetDatabase.CreateAsset(meshFilter.sharedMesh, path);
        // var asset = AssetDatabase.LoadMainAssetAtPath(path);
        // meshFilter.mesh = EditorUtility.InstanceIDToObject(asset.GetInstanceID()) as Mesh;
        // https://answers.unity.com/questions/935107/create-an-asset-and-keep-reference.html
    }

    public void GetCenter()
    {
        if (skinMeshRender != null)
        {
            var mesh = new Mesh();
            skinMeshRender.BakeMesh(mesh);
            Debug.Log(mesh.bounds.center.ToString("F10"));
        }


        if (meshFilter != null)
        {
            Debug.Log(meshFilter.mesh.bounds.center.ToString("F10"));
        }
    }
#endif

}
