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

            state = (LightState)Mathc.Wrap((int)state + 1, 0, 2);
        }
    }

    public override void Toggle()
    {
        if (!moving)
        {
            moving = true;
        }
    }
}
