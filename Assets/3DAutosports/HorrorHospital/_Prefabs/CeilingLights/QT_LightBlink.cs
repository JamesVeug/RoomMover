using UnityEngine;
using System.Collections;

public class QT_LightBlink : MonoBehaviour 
{
	
	public float MinTime=0f;
	public float MaxTime=.005f;
	public float MinIntensity=0.0f;
	public float MaxIntensity=4.0f;
	public bool BlinkOn = true;
	private float pauseTime = 0;
	
	void Update()
	{
		if (!BlinkOn)
			return;

		pauseTime -= Time.deltaTime;
		if (pauseTime <= 0)
		{
			pauseTime = Random.Range(MinTime, MaxTime);
			this.GetComponent<Light>().intensity = Random.Range(MinIntensity, MaxIntensity);
		}
	}
}
