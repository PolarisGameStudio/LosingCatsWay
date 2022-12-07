using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RenameEditor : EditorWindow 
{
    string newName;
    int index;

    [MenuItem("Window/RenameEditor")]
    public static void ShowWindow()
    {
        GetWindow<RenameEditor>("RenameEditor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Rename selected objects.", EditorStyles.boldLabel);
        newName = EditorGUILayout.TextField("New Name", newName);
        index = EditorGUILayout.IntField("Index", index);

        if (GUILayout.Button("Rename"))
        {
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                Selection.gameObjects[i].name = newName;
            }
        }

        if (GUILayout.Button("ReIndex"))
        {
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                GameObject tmp = Selection.gameObjects[i];
                string m_name = newName;
                tmp.name = $"{m_name} ({index})";
                index++;
            }
        }
    }
}
