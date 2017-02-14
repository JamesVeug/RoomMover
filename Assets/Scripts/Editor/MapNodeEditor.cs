/// Copyright (c) PikPok.  All rights reserved
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 
/// </summary>
/// <author>James Peter Veugelaers</author>
[CustomEditor(typeof(MapNode))]
public class MapNodeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Move Camera here"))
		{
			MapNode node = (MapNode)target;
			GameCamera.Instance.currentPosition = node;
			GameCamera.Instance.transform.position = node.transform.position;
		}
	}
}
