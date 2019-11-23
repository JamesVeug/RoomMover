using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    public Image PainOverlay;

    public Text InspectText;
    public GameObject InspectMessageContainer;

    private void Update()
    {
        InspectMessageContainer.SetActive(GameCamera.Instance.IsInspecting);
    }

    public void ShowInspector(string text)
    {
        InspectText.text = text;
        InspectMessageContainer.SetActive(true);
    }

    public void HideInspector()
    {
        InspectMessageContainer.SetActive(false);
    }
}
