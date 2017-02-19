using UnityEngine;

public class Room13BreakLadder : Triggerable
{
    public float ForcePush = 20;
    public Rigidbody[] ladders;
    public AudioSource Source;
    public MapNode lastMapNode;

    private bool triggered;

    public override void Toggle()
    {
        foreach(Rigidbody r in ladders)
        {
            r.constraints = RigidbodyConstraints.None;
            r.AddForce(Vector2.up * ForcePush);
        }
        triggered = true;

        Source = AudioManager.Instance.PlaySound(AudioClipType.LadderBreak, false, Source);
        GameCamera.Instance.SetCurrentPosition(lastMapNode);
    }

    public override bool IsLocked()
    {
        return triggered;
    }
}
