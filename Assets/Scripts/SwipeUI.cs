using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    public Transform SwipeContents;
    public Image SwipeImage;

    void OnEnable()
    {
        ScreenSwipeListener.Instance.OnSwipe += ShowSwipeEffects;
    }

    void OnDisable()
    {
        ScreenSwipeListener.Instance.OnSwipe -= ShowSwipeEffects;
    }

    private void ShowSwipeEffects(ScreenSwipeListener.SwipeInfo info)
    {
        Vector2 path = info.endPosition - info.startPosition;
        Vector2 middle = info.startPosition + path;

        SwipeContents.transform.position = middle;

        SwipeContents.transform.localScale = new Vector3( Mathf.Abs(path.x), SwipeContents.transform.localScale.y, SwipeContents.transform.localScale.z);

        SwipeContents.transform.rotation = Quaternion.identity;
        SwipeContents.transform.Rotate(path.normalized);

        //SwipeImage.color = new Color(1, 1, 1, 1);
    }

	void Update ()
    {
		
	}
}
