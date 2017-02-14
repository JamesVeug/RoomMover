using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TapButton : Triggerable
{
    public AudioClipType PressSound = AudioClipType.ButtonPress;

    public Triggerable TriggerOnPush = null;

    public void OnPush()
    {
        if(TriggerOnPush != null)
        {
            TriggerOnPush.Toggle();
            AudioManager.Instance.PlaySound(PressSound);
        }
    }

    public override void Toggle()
    {
        OnPush();
    }
}
