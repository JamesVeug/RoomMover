using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inspectable))]
public class InspectibleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Inspectable inspect = (Inspectable)target;
        if (GUILayout.Button("PreviewCamera"))
        {
            Game.Instance.InspectElement(inspect);
        }
    }
}
