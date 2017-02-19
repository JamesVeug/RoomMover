using System;
using UnityEngine;

public class EnableOnTrigger : Triggerable
{
    public GameObject content;

    public override void Toggle()
    {
        content.SetActive(true);
    }
}
