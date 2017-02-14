using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : Singleton<GameUI>
{
    public Image PainOverlay;

    public Text InspectText;
    public GameObject InspectMessageContainer;

    public void ShowInspector(string text)
    {
        InspectText.text = text;
        InspectMessageContainer.SetActive(true);
    }

    public void HideInspector()
    {
        InspectMessageContainer.SetActive(false);
    }

    internal void ShowInspector(object messageText)
    {
        throw new NotImplementedException();
    }
}
