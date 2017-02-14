using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour {

    public Text text;
    public float RefreshDelay = 1000;

    float deltaTime = 0.0f;
    float fps = 0;
    float msec = 0;

    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        while (true)
        {
            yield return new WaitForSeconds(RefreshDelay / 1000);
            text.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        }
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;
    }
}
