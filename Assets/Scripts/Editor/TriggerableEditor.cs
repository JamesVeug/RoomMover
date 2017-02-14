using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Elevator))]
public class TriggerableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Trigger"))
        {
            Triggerable t = (Triggerable)target;
            t.Toggle();
        }
    }
}
