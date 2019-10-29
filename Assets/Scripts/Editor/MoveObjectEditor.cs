using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoveObject))]
public class MoveObjectEditor : Editor
{
    [SerializeField]
    MoveObject scriptTarget;

    public void Awake()
    {
        scriptTarget = (MoveObject)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Set Clamp to current position"))
        {
            scriptTarget.destination = scriptTarget.transform.position;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
