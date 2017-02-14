using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightBox : Triggerable
{
    public AudioSource audioSource;
    public Color lightColor;
    public Renderer LightMaterial;
    public Light light;

    public bool StartsOn = true;

    private void Start()
    {
        if (!StartsOn)
            TurnOff();
        else
            TurnOn(); 
    }

    public void TurnOff()
    {
        light.gameObject.SetActive(false);
        LightMaterial.material.color = Color.black;
    }

    public void TurnOn()
    {
        light.gameObject.SetActive(true);
        LightMaterial.material.color = lightColor;
    }

    public override void Toggle()
    {
        if (light.gameObject.activeInHierarchy)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
        AudioManager.Instance.PlaySound(AudioClipType.LightOff, false, audioSource);
    }
}
