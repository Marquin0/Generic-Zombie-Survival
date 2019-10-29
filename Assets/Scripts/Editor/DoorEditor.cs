using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Door myScript = (Door)target;
        if (GUILayout.Button("Set Destination to current position"))
        {
            myScript.destination = myScript.doorObject.transform.localPosition;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
