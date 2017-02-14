using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerActivateMonsterRoom : Room
{
    public Triggerable triggerOnEnter;
    private bool triggered;

    public override void OnEnter()
    {
        base.OnEnter();

        if (triggered)
        {
            return;
        }

        triggerOnEnter.Toggle();

        triggered = true;
    }
}
