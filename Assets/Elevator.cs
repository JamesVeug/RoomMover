using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Triggerable
{
    public float timeToNextFloor = 2;
    public float zoffset = 0;
    public Transform Model;
    public MapNode[] Floors;
    public int StartingPosition = 0;
    public AudioSource audioSource;
    public AudioClipType ElevatorMovingSound;

    private int floor = 0;

    void Start()
    {
        floor = StartingPosition;
    }

    public override void Toggle()
    {
        int nextFloor = (int)Mathc.Wrap(floor + 1, 0, Floors.Length);

        audioSource = AudioManager.Instance.PlaySound(AudioClipType.ElevatorMoving, true, audioSource);
        StartCoroutine(GameCamera.Instance.TransitionCamera(GameCamera.Instance.currentPosition, Floors[nextFloor], timeToNextFloor, 1, true));
        StartCoroutine(Move(nextFloor));
    }

    public IEnumerator Move(int nextFloor)
    {
        Vector3 startPos = Floors[floor].transform.transform.position;
        startPos.z = 0;

        Vector3 endPos = Floors[nextFloor].transform.position;
        endPos.z = 0;

        float time = 0;
        while (time < timeToNextFloor)
        {
            Model.position = Vector3.Lerp(startPos, endPos, time / timeToNextFloor);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        if(audioSource != null)
        {
            audioSource.Stop();
        }

        Model.position = endPos;
        floor = nextFloor;

        GameCamera.Instance.currentPosition = Floors[floor];
    }
}
