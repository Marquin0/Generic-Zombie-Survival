using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectInteractions))]
public class ObjectInteractionsEditor : Editor
{
    [SerializeField]
    ObjectInteractions scriptTarget;

    public void Awake()
    {
        scriptTarget = (ObjectInteractions)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Set Clamp to current position"))
        {
            scriptTarget.Clamp = scriptTarget.transform.position;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
