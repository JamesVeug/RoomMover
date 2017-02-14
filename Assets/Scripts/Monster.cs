/// Copyright (c) PikPok.  All rights reserved
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 
/// </summary>
/// <author>James Peter Veugelaers</author>
public class Monster : MonoBehaviour 
{
	public enum MonsterState
	{
		Idle,
		Walking
	}

	public float speed = 3;
	public float idleTime = 2;
	public MapNode currentRoom;
    public bool chooseDirectionInEditor = false;
    public Room.Direction chooseDirection = Room.Direction.Unknown;

    private MonsterIndicator lastMovingIndicator; // Indicator to show monster moving to room

    private MapNode destination;
	private MonsterState m_state;
	private float m_time = 0;

	void Start() 
    {
		m_state = MonsterState.Idle;
	}
	
	void Update() 
    {

		switch (m_state)
		{
			case MonsterState.Idle:
				UpdateIdle();
				break;
			case MonsterState.Walking:
				UpdateWalking();
				break;
		}
	}

	private void UpdateIdle()
	{
		if (m_time > idleTime)
		{
			m_state = MonsterState.Walking;
			m_time = 0;
			return;
		}

		m_time += Time.deltaTime;
	}

	private void UpdateWalking()
	{
        if(destination == null)
        {
            if(chooseDirectionInEditor)
            {
                if(chooseDirection != Room.Direction.Unknown)
                {
                    destination = GetNode(chooseDirection);
                    chooseDirection = Room.Direction.Unknown;
                }
                return;
            }
            else
            {
                AssignRandomDirection();
            }
        }

		if (m_time >= speed)
		{
			currentRoom = destination;
			m_state = MonsterState.Idle;
			transform.position = destination.transform.position;

            if(lastMovingIndicator != null)
            {
                lastMovingIndicator.SetStage(0);
            }
            destination = null;
            m_time = 0;
			return;
		}

		m_time = Mathf.Min(m_time + Time.deltaTime, speed);

		float enteringScale = m_time / speed;
		float leavingScale = 1 - enteringScale;

		transform.position = Vector3.Lerp(currentRoom.transform.position, destination.transform.position, enteringScale);


		// MAKE THIS BETTER OMG EW
		ShowIndicator(currentRoom.Left.node, Room.Direction.Right, leavingScale/2);
		ShowIndicator(currentRoom.Right.node, Room.Direction.Left, leavingScale/2);
		ShowIndicator(currentRoom.Up.node, Room.Direction.Down, leavingScale/2);
		ShowIndicator(currentRoom.Down.node, Room.Direction.Up, leavingScale/2);

		ShowIndicator(destination.Left.node, Room.Direction.Right, enteringScale/2);
		ShowIndicator(destination.Right.node, Room.Direction.Left, enteringScale/2);
		ShowIndicator(destination.Up.node, Room.Direction.Down, enteringScale/2);
		ShowIndicator(destination.Down.node, Room.Direction.Up, enteringScale/2);
	}

    private void AssignRandomDirection()
    {
        destination = null;
        while (destination == null)
        {
            int random = UnityEngine.Random.Range(0, 4);
            destination = GetNode((Room.Direction)random);
        }
    }

    private MapNode GetNode(Room.Direction direction)
    {
        switch (direction)
        {
            case Room.Direction.Left:
                if (currentRoom.hasLeft)
                    return currentRoom.Left.node;
                break;
            case Room.Direction.Right:
                if (currentRoom.hasRight)
                    return currentRoom.Right.node;
                break;
            case Room.Direction.Up:
                if (currentRoom.hasUp)
                    return currentRoom.Up.node;
                break;
            case Room.Direction.Down:
                if (currentRoom.hasDown)
                    return currentRoom.Down.node;
                break;
        }

        return null;
    }

	private void ShowIndicator(MapNode mapNode, Room.Direction direction, float enteringScale)
	{
		if (mapNode == null)
		{
			return;
		}


		var indicator = mapNode.room.GetIndicator(direction);
        if (mapNode == destination)
        {
            enteringScale = (1 - enteringScale);
            lastMovingIndicator = indicator;
        }

        if (indicator != null)
		{
			indicator.SetStage(enteringScale);
		}
    }

    public void SetTargetDirection(Room.Direction startMovingDirection)
    {
        destination = GetNode(startMovingDirection);
    }
}
