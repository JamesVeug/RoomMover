using UnityEngine;

public class Inspectable : Triggerable
{
    public string InspectText = "Unknown";
    public Transform InspectCamerasTransform;

    public override void Toggle()
    {
        Game.Instance.InspectElement(this);
    }
}
