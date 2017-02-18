using UnityEngine;

public class Room13BreakLadder : Triggerable
{
    public float ForcePush = 20;
    public Rigidbody[] ladders;

    public override void Toggle()
    {
        foreach(Rigidbody r in ladders)
        {
            r.constraints = RigidbodyConstraints.None;
            r.AddForce(Vector2.up * ForcePush);
        }

        GameCamera.Instance.SetCurrentPosition(GameCamera.Instance.currentPosition);
    }
}
