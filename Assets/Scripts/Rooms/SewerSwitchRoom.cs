/// Copyright (c) PikPok.  All rights reserved
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
/// <author>James Peter Veugelaers</author>
public class SewerSwitchRoom : Room 
{
	public Animation door;

	private bool triggered;

	public override void OnEntering()
	{
		base.OnEnter();

		if (triggered)
		{
			return;
		}

		door.Play("Foyer Door Scare");

		triggered = true;
	}
}
