/// Copyright (c) PikPok.  All rights reserved
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 
/// </summary>
/// <author>James Peter Veugelaers</author>
[CustomEditor(typeof(GameCamera))]
public class GameCameraEditor : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

        GameCamera game = (GameCamera)target;

		GUILayout.Space(5);
		GUILayout.Label("Move Camera");
		if (GUILayout.Button("Up") && game.currentPosition.hasUp)
		{
			game.SetCurrentPosition(game.currentPosition.Up.node);
		}

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Left") && game.currentPosition.hasLeft)
		{
            game.SetCurrentPosition(game.currentPosition.Left.node);
		}
		if (GUILayout.Button("Right") && game.currentPosition.hasRight)
		{
            game.SetCurrentPosition(game.currentPosition.Right.node);
		}
		GUILayout.EndHorizontal();

		if (GUILayout.Button("Down") && game.currentPosition.hasDown)
		{
            game.SetCurrentPosition(game.currentPosition.Down.node);
		}
	}
}
