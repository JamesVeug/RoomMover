using System;
using UnityEngine;

public class DisableOnTrigger : Triggerable
{
    public GameObject content;

    public override void Toggle()
    {
        content.SetActive(false);
    }
}
