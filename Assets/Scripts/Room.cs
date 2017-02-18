using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Serializable]
    public class RoomTrigger
    {
        public Triggerable trigger;
        public Triggerable lockedBy;
        public Triggerable enabledBy;
        public bool TriggerOnce;
        public bool Triggered;
        
    }

	public enum Direction
	{
		Up,
		Right,
		Down,
		Left,
        Unknown
	}

    public Renderer Darkenator;
	public MonsterIndicator LeftIndicator;
	public MonsterIndicator RightIndicator;
	public MonsterIndicator UpIndicator;
	public MonsterIndicator DownIndicator;

    public RoomTrigger[] TriggerOnEntering;
    public RoomTrigger[] TriggerOnEnter;
    public RoomTrigger[] TriggerOnExiting;
    public RoomTrigger[] TriggerOnExit;

    public void SetRoomIndicator(Direction direction, float scale)
	{
		MonsterIndicator indicator = GetIndicator(direction);
		if (indicator != null) 
		{
			indicator.SetStage(scale); 
		}
	}

	public MonsterIndicator GetIndicator(Direction d)
	{
		switch (d)
		{
			case Direction.Up:
				return UpIndicator;
			case Direction.Down:
				return DownIndicator;
			case Direction.Left:
				return LeftIndicator;
			case Direction.Right:
				return RightIndicator;
		}

		return null;
	}

	public virtual void OnEntering()
    {
        Trigger(TriggerOnEntering);
    }

	public virtual void OnEnter()
    {
        Trigger(TriggerOnEnter);
    }

    public virtual void OnExit()
    {
        Trigger(TriggerOnExit);
    }

    public virtual void OnExiting()
    {
        Trigger(TriggerOnExiting);
    }

    private void Trigger(RoomTrigger[] triggers)
    {

        for (int i = 0; i < triggers.Length; i++)
        {
            var Trigger = triggers[i];
            if (Trigger.trigger == null)
            {

                Debug.Log("Null Trigger for " + gameObject.name);
                continue;
            }

            bool isLocked = Trigger.lockedBy != null && Trigger.lockedBy.IsLocked();
            if (isLocked)
            {
                continue;
            }

            bool isDisabled = Trigger.enabledBy != null && !Trigger.enabledBy.IsLocked();
            if (isDisabled)
            {
                continue;
            }

            bool alreadyTriggered = (Trigger.Triggered && Trigger.TriggerOnce);
            if (alreadyTriggered)
            {
                continue;
            }

            Trigger.trigger.Toggle();
            Trigger.Triggered = true;
        }
    }
}
