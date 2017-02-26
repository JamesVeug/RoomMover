/// Copyright (c) PikPok.  All rights reserved
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <author>James Peter Veugelaers</author>
public class MonsterIndicator : MonoBehaviour
{
	public GameObject content;

	public ParticleSystem particles;

	public float MinParticles = 100;
	public float MaxParticles = 200;

	public float MinLife = 0;
	public float MaxLife = 2;

	public Color MinColor = Color.black;
	public Color MaxColor = Color.red;

	public Transform glowTransform;
	public float MinGlowWidth = 0;
	public float MaxGlowWidth = 5;

	[Range(0,1)]
	public float stage = 0;

	private float lastStage = 0;

	public void SetStage(float scale)
	{
		stage = scale;
        Refresh();
    }

	void Start() 
    {
		stage = 0;
		UpdateIndicator();
	}

	public void Refresh()
	{
		if (lastStage != stage)
		{
			content.SetActive(true);
			UpdateIndicator();
		}

		if (stage == 0 && !particles.isEmitting)
		{
			content.SetActive(false);
		}
	}

	public void UpdateIndicator()
	{
        float particleScale = Mathf.Max(0, stage - 0.5f) * 2;

		particles.loop = particleScale > 0;
		particles.startLifetime = MinLife + (MaxLife - MinLife) * particleScale;
		particles.emissionRate = MinParticles + (MaxParticles - MinParticles) * particleScale;
        if(particles.loop && !particles.isPaused)
        {
            particles.Play();
        }
			
		float r = MinColor.r + (MaxColor.r - MinColor.r) * particleScale;
		float g = MinColor.g + (MaxColor.g - MinColor.g) * particleScale;
		float b = MinColor.b + (MaxColor.b - MinColor.b) * particleScale;
		Color col = new Color(r,g,b);
		particles.startColor = col;

		// Glow
		float xScale = MinGlowWidth + (MaxGlowWidth - MinGlowWidth) * stage;
		glowTransform.localScale = new Vector3(xScale, 1, 1);

		lastStage = stage;
	}
}
