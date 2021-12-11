using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PoolManager))]
public class PoolManagerEditor : Editor
{
    private ReorderableList poolListEditor;
    const float LINE_MARGIN = 5f;

    private void OnEnable()
    {
        poolListEditor = new ReorderableList(serializedObject, serializedObject.FindProperty("poolListEditor"), true, true, true, true);
        poolListEditor.elementHeight = 70f;

        poolListEditor.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Pools");
        };

        poolListEditor.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = poolListEditor.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            float posX = rect.x;
            float rowWidth = rect.width / 2.5f;

            // name reference
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rowWidth, EditorGUIUtility.singleLineHeight), "Name");
            posX = rect.x + rowWidth;
            rowWidth = rect.width / 1.69f;
            EditorGUI.PropertyField(new Rect(posX, rect.y, rowWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("name"), GUIContent.none);

            // size reference
            rowWidth = rect.width / 2.5f;
            rect.y += EditorGUIUtility.singleLineHeight + LINE_MARGIN;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rowWidth, EditorGUIUtility.singleLineHeight), "Size");
            posX = rect.x + rowWidth;
            rowWidth = rect.width / 1.69f;
            EditorGUI.PropertyField(new Rect(posX, rect.y, rowWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("size"), GUIContent.none);

            // prefab reference
            rowWidth = rect.width / 2.5f;
            rect.y += EditorGUIUtility.singleLineHeight + LINE_MARGIN;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rowWidth, EditorGUIUtility.singleLineHeight), "Prefab");
            posX = rect.x + rowWidth;
            rowWidth = rect.width / 1.69f;
            EditorGUI.PropertyField(new Rect(posX, rect.y, rowWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("prefab"), GUIContent.none);
        };

        poolListEditor.onSelectCallback = (ReorderableList l) =>
        {
            // called when an entry is selected
            var gObject = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("prefab").objectReferenceValue as GameObject;
            if (gObject)
                EditorGUIUtility.PingObject(gObject);
        };

        poolListEditor.onCanRemoveCallback = (ReorderableList l) =>
        {
            return l.count > 0;
        };

        poolListEditor.onRemoveCallback = (ReorderableList l) =>
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(l);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Pools Count", poolListEditor.count.ToString());
        poolListEditor.DoLayoutList();

        PoolManager myScript = (PoolManager)target;
        if (GUILayout.Button("Generate Const Pool Names"))
        {
            Dictionary<string, string> poolNames = myScript.GetPoolNames();

            if (poolNames != null)
                global::GenerateConstantsClass.Create("PoolNames", poolNames);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
