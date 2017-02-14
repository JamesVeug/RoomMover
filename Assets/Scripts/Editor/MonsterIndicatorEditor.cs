/// Copyright (c) PikPok.  All rights reserved
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 
/// </summary>
/// <author>James Peter Veugelaers</author>
[CustomEditor(typeof(MonsterIndicator))]
public class MonsterIndicatorEditor : Editor
{
	private float lastStage;


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		MonsterIndicator indicator = (MonsterIndicator)target;

		if (lastStage != indicator.stage)
		{
			indicator.Refresh();
			lastStage = indicator.stage;
		}
	}
}
