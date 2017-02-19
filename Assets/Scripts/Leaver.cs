using UnityEngine;

public class Leaver : Triggerable
{
    public enum LightState
    {
        Open,
        Closed
    }

    public Triggerable[] TriggerOnTap;

    public float MoveTime;
    public Vector3 startRotation;
    public Vector3 endRotation;
    public Transform pivot;
    public LightState state;

    public AudioClipType OpenSound;
    public AudioClipType CloseSound;
    public AudioClipType MovingSound;
    public AudioSource audioSource;

    private bool moving;
    private float timeMoving;

    void Start()
    {
        pivot.localRotation = Quaternion.Euler(startRotation);
        if(state == LightState.Closed)
        {
            pivot.localRotation = Quaternion.Euler(endRotation);
        }
        else
        {
            pivot.localRotation = Quaternion.Euler(startRotation);
        }
    }

    void FixedUpdate()
    {
        if (!moving)
        {
            return;
        }

        timeMoving = Mathf.Min(MoveTime, timeMoving + Time.fixedDeltaTime);

        if (state == LightState.Closed)
        {
            pivot.localRotation = Quaternion.Slerp(Quaternion.Euler(endRotation), Quaternion.Euler(startRotation), timeMoving / MoveTime);
        }
        else
        {
            pivot.localRotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(endRotation), timeMoving / MoveTime);
        }

        if (timeMoving == MoveTime)
        {
            moving = false;
            timeMoving = 0;

            foreach (Triggerable t in TriggerOnTap)
            {
                t.Toggle();
            }

            if(state == LightState.Closed)
            {
                state = LightState.Open;
                AudioManager.Instance.PlaySound(OpenSound, false, audioSource);
            }
            else if (state == LightState.Open)
            {
                state = LightState.Closed;
                AudioManager.Instance.PlaySound(CloseSound, false, audioSource);
            }
        }
    }

    public override void Toggle()
    {
        if (!moving)
        {
            AudioManager.Instance.PlaySound(MovingSound, false, audioSource);
            moving = true;
        }
    }

    public override bool IsLocked()
    {
        return state == LightState.Closed;
    }
}
