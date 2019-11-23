using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : Singleton<GameCamera>
{
    public Camera camera;

    public AudioSource audioSource;
    public float SwipeTransitionTime = 0.25f;
    public float ResetTime = 2;

    public MapNode currentPosition { get { return m_currentPosition; } set { lastPosition = m_currentPosition; m_currentPosition = value; } }
    public MapNode lastPosition { get { return m_lastPosition; } set { m_lastPosition = value; } }
    public bool IsInspecting { get { return inspecting; } }

    [SerializeField]
    private MapNode m_currentPosition;

    private bool isMoving = false;
    private float movePercent = 0;
    private bool inspecting = false;
 
    private MapNode m_lastPosition;
    private IEnumerator currentCoroutine;

    void Start()
    {
        SetCurrentPosition(currentPosition);
    }

    public void SetCurrentPosition(MapNode node)
    {
        currentPosition = node;
        ResetCamera();
    }

    private IEnumerator SetCurrentPositionCoroutine(MapNode node)
    {
        currentPosition = node;
        ResetCamera();
        yield break;
    }

    public void ResetCamera()
    {
        Start(ResetCameraCoroutine());
        inspecting = false;
    }

    private void Start(IEnumerator coroutine)
    {
        if(currentCoroutine != null)
        {
            isMoving = false;
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = coroutine;
        StartCoroutine(coroutine);
    }

    private IEnumerator ResetCameraCoroutine()
    {
        Vector3 startPosition = camera.transform.position;
        Vector3 endPosition = currentPosition.transform.position;
        Quaternion startRotation = camera.transform.rotation;
        Quaternion endRotation = currentPosition.transform.rotation;
        yield return LerpPositionAndRotationCoroutine(startPosition,endPosition,startRotation,endRotation);
    }
    
    private IEnumerator LerpPositionAndRotationCoroutine(Vector3 startPosition, Vector3 endPosition, Quaternion startRotation, Quaternion endRotation)
    {
        float time = 0;
        while ((camera.transform.position-endPosition).magnitude > 0.05f )
        {
            time = Mathf.Clamp(time + Time.deltaTime, 0, ResetTime);
            float ratio = time / ResetTime;
            camera.transform.position = Vector3.Lerp(startPosition, endPosition, ratio);
            camera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, ratio);
            yield return new WaitForEndOfFrame();
        }
        camera.transform.position = endPosition;
        camera.transform.rotation = currentPosition.transform.rotation;
        camera.transform.rotation = endRotation;
    }

    public void Inspect(Transform position)
    {
        Start(InspectCoroutine(position));
    }
    
    private IEnumerator InspectCoroutine(Transform position)
    {
        Vector3 startPosition = camera.transform.position;
        Vector3 endPosition = position.position;
        Quaternion startRotation = camera.transform.rotation;
        Quaternion endRotation = position.rotation;
        yield return LerpPositionAndRotationCoroutine(startPosition,endPosition,startRotation,endRotation);
        inspecting = true;
    }
    
    public void OnSwipe(ScreenSwipeListener.SwipeInfo swipeInfo, float percent)
    {
        if (inspecting || percent == movePercent)
        {
            return;
        }

        MapNode targetNode = null;
        if (swipeInfo.direction == ScreenSwipeListener.SwipeDirection.Right)
        {
            if (currentPosition.Left != null && !currentPosition.Left.isLocked)
            {
                targetNode = currentPosition.Left.node;
            }
        }
        if (swipeInfo.direction == ScreenSwipeListener.SwipeDirection.Left)
        {
            if (currentPosition.Right != null && !currentPosition.Right.isLocked)
            {
                targetNode = currentPosition.Right.node;
            }
        }
        if (swipeInfo.direction == ScreenSwipeListener.SwipeDirection.Down)
        {
            if (currentPosition.Up != null && !currentPosition.Up.isLocked)
            {
                targetNode = currentPosition.Up.node;
            }
        }
        if (swipeInfo.direction == ScreenSwipeListener.SwipeDirection.Up)
        {
            if (currentPosition.Down != null && !currentPosition.Down.isLocked)
            {
                targetNode = currentPosition.Down.node;
            }
        }

        if(targetNode == null)
        {
            return;
        }
        

        Start(TransitionCamera(currentPosition, targetNode, SwipeTransitionTime, percent));
    }

    public IEnumerator TransitionCamera(MapNode start, MapNode end, float TransitionTime = 0, float percent = 1, bool lockCamera = false)
    {
        if (isMoving)
        {
            yield break;
        }

        if (lockCamera)
        {
            isMoving = true;
        }

        Vector3 startPosition = camera.transform.position;
        Vector3 endPosition = start.transform.position + (end.transform.position - start.transform.position) * percent;
        Vector3 distance = endPosition - startPosition;
        float timeToTravel = distance.magnitude / (end.transform.position - startPosition).magnitude * TransitionTime;

        if (percent == 1)
        {
            // Triggers
            start.room.OnExiting();
            end.room.OnEntering();

            // Camera move sound
            AudioManager.Instance.PlaySound(AudioClipType.CameraMove);
            currentPosition = end;
        }
        else
        {
            movePercent = percent;
        }

        float time = 0;
        while (time < timeToTravel && (!lockCamera || isMoving))
        {
            time += Time.fixedDeltaTime;
            float scale = time / TransitionTime;

            // Camera
            camera.transform.position = Vector3.Lerp(startPosition, endPosition, scale);
            camera.transform.rotation = Quaternion.Lerp(start.transform.rotation, end.transform.rotation, scale);

            // Darkenators
            if (start.room.Darkenator != null)
            {
                Color startColor = start.room.Darkenator.material.color;
                start.room.Darkenator.material.color = new Color(startColor.r, startColor.r, startColor.r, scale);
            }

            if (end.room.Darkenator != null)
            {
                Color endColor = end.room.Darkenator.material.color;
                end.room.Darkenator.material.color = new Color(endColor.r, endColor.r, endColor.r, 1 - scale);
            }


            yield return new WaitForFixedUpdate();
        }

        if (time >= timeToTravel)
        {
            // Triggers
            start.room.OnExit();
            end.room.OnEnter();

            // Camera
            camera.transform.position = endPosition;
            camera.transform.rotation = end.transform.rotation;
            movePercent = 0;
        }

        isMoving = false;
    }
}
