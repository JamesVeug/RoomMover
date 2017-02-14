using System;
using UnityEngine;

public class TapListener : Singleton<TapListener>
{
    public float cameraSwipeAmount = 50;
    public float cameraSwipePreviewAmount = 30;
    public float slack = 0.5f;

    private Vector2 lastSwipePosition = Vector2.zero;
    private Tapable tapped;

    private void OnEnable()
    {
        ScreenSwipeListener.Instance.OnSwipe += OnSwiped;
    }

    private void OnDisable()
    {
        ScreenSwipeListener.Instance.OnSwipe -= OnSwiped;
    }

    public void OnSwiped(ScreenSwipeListener.SwipeInfo info)
    {
        if(info.swipeType == ScreenSwipeListener.SwipeType.Swipe)
        {
            if(tapped == null)
            {
                SwipeCamera(info);
            }
            else
            {
                SwipeThingy(info);
            }
        }
        else if (info.swipeType == ScreenSwipeListener.SwipeType.Release)
        {
            if (tapped == null)
            {
                SwipeCamera(info);
            }
            else
            {
                TapThingy(info);
            }
        }
        else if (info.swipeType == ScreenSwipeListener.SwipeType.Press)
        {
            tapped = CheckForTappingOnItem(info.startPosition);
            if(tapped != null)
            {
                TapThingy(info);
            }
            else
            {
                SwipeCamera(info);
            }
        }
    }

    private void SwipeThingy(ScreenSwipeListener.SwipeInfo info)
    {
        //throw new NotImplementedException();
    }

    private void TapThingy(ScreenSwipeListener.SwipeInfo info)
    {
        tapped.TriggerOnTapped.Toggle();
    }

    private void SwipeCamera(ScreenSwipeListener.SwipeInfo info)
    {
        float magnitude = (info.endPosition - info.startPosition).magnitude;

        switch (info.swipeType)
        {
            case ScreenSwipeListener.SwipeType.Press:
                lastSwipePosition = info.endPosition;
                return;
            case ScreenSwipeListener.SwipeType.Release:
                lastSwipePosition = Vector2.zero;
                if (magnitude > cameraSwipeAmount)
                {
                    GameCamera.Instance.OnSwipe(info, Mathf.Min(magnitude, cameraSwipeAmount) / cameraSwipeAmount);
                }
                else
                {
                    GameCamera.Instance.ResetCamera();
                }
                return;
        }

        // Swiping
        if ((lastSwipePosition - info.endPosition).magnitude < slack)
        {
            return;
        }
        float moveScale = Mathf.Min(magnitude, cameraSwipePreviewAmount) / cameraSwipeAmount;
        GameCamera.Instance.OnSwipe(info, moveScale);
        lastSwipePosition = info.endPosition;
    }

    public Tapable CheckForTappingOnItem(Vector2 tapPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(tapPosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 999);
        
        for (int i = 0; i < hits.Length; i++)
        {
            var tap = hits[i].collider.GetComponent<Tapable>();
            if (tap != null)
            {
                if (tap.TriggerOnTapped != null)
                {
                    return tap;
                }
            }
        }

        return null;
    }
}
