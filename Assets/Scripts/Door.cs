using UnityEngine;

public class Door : Triggerable
{
    public enum DoorState
    {
        Open,
        Closed
    }

    public float MoveTime = 2;
    public Transform DoorPivot;
    public Transform OpenPosition;
    public Transform ClosedPosition;
    public AudioSource Source;

    public DoorState state;
    
    private bool moving;
    private float amountMoved;

    void Start()
    {
        if(state == DoorState.Closed)
        {
            isLocked = true;
            DoorPivot.transform.position = ClosedPosition.position;
        }
        else
        {
            isLocked = false;
            DoorPivot.transform.position = OpenPosition.position;
        }
    }

    void Update()
    {
        if (!moving)
        {
            return;
        }
        
        amountMoved = Mathf.Min(amountMoved + Time.deltaTime, MoveTime);

        if (state == DoorState.Closed)
        {
            DoorPivot.transform.position = Vector3.Lerp(ClosedPosition.position, OpenPosition.position, amountMoved/MoveTime);
            if (amountMoved / MoveTime >= 0.5f) isLocked = false;
        }
        else if (state == DoorState.Open)
        {
            DoorPivot.transform.position = Vector3.Lerp(OpenPosition.position, ClosedPosition.position, amountMoved / MoveTime);
            if (amountMoved / MoveTime >= 0.5f) isLocked = true;
        }

        // Stop moving
        if(amountMoved == MoveTime)
        {
            state = (DoorState)Mathc.Wrap((int)state+1,0,2);
            amountMoved = 0;
            moving = false;


            Source.Stop();
            Source = AudioManager.Instance.PlaySound(AudioClipType.SlidingDoorStop, false, Source);
        }
    }

    public override void Toggle()
    {
        if (!moving)
        {
            moving = true;
            Source = AudioManager.Instance.PlaySound(AudioClipType.SlidingDoorMove, true, Source);
        }
    }
}
