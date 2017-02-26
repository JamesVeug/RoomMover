using System;
using UnityEngine;

public class ScreenSwipeListener : Singleton<ScreenSwipeListener>
{
    public enum SwipeType
    {
        Press,
        Swipe,
        Release
    }

    public enum SwipeDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    public class SwipeInfo
    {
        public Vector2 startPosition;
        public Vector2 endPosition;
        public SwipeDirection direction;
        public SwipeType swipeType;

        public SwipeInfo(Vector2 pressPosition, Vector2 releasePosition, SwipeDirection down, SwipeType type)
        {
            startPosition = pressPosition;
            endPosition = releasePosition;
            direction = down;
            swipeType = type;
        }
    }

    public Action<SwipeInfo> OnSwiping = delegate { };
    public Action<SwipeInfo> OnSwipe = delegate { };

    private bool canSwipe = true;
    private bool isMouseDown = false;

    private Vector2 pressPosition = Vectorc.Vector2Zero;
    private Vector2 lastReleasePosition = Vectorc.Vector2Zero;

    void Update()
    {

        if (!TouchIsDown())
        {
            if(canSwipe && pressPosition != Vectorc.Vector2Zero)
            {
                //TapListener.Instance.CheckForTappingOnItem(lastReleasePosition);
                Swiped(lastReleasePosition, SwipeType.Release);
                pressPosition = Vectorc.Vector2Zero;
                canSwipe = true;
            }
            return;
        }

        if (!canSwipe)
        {
            return;
        }

        var currentPosition = GetPosition();
        if (pressPosition == Vectorc.Vector2Zero)
        {
            pressPosition = currentPosition;
            Swiped(pressPosition, SwipeType.Press);
        }
        else
        {
            lastReleasePosition = currentPosition;
            Swiped(lastReleasePosition, SwipeType.Swipe);
        }
    }

    private void Swiped(Vector2 currenPosition, SwipeType type)
    {
        Vector2 direction = currenPosition - pressPosition;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                OnSwipe(new SwipeInfo(pressPosition, currenPosition, SwipeDirection.Right, type));
            else
                OnSwipe(new SwipeInfo(pressPosition, currenPosition, SwipeDirection.Left, type));
        }
        else
        {
            if (direction.y > 0)
                OnSwipe(new SwipeInfo(pressPosition, currenPosition, SwipeDirection.Up, type));
            else
                OnSwipe(new SwipeInfo(pressPosition, currenPosition, SwipeDirection.Down, type));
        }
    }

    private bool TouchIsDown()
    {
#if UNITY_EDITOR
        if (isMouseDown)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMouseDown = true;
            }
        }
        return isMouseDown;
#else
        return Input.touchCount > 0;
#endif
    }

    private Vector2 GetPosition()
    {
#if UNITY_EDITOR
        return Input.mousePosition;
#else
        return Input.GetTouch(0).position;
#endif

    }
}