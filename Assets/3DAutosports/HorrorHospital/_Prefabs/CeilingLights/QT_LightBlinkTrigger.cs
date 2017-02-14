using UnityEngine;
using System.Collections;

public class QT_LightBlinkTrigger : MonoBehaviour 
{
	public float MinTime=0f;
	public float MaxTime=.005f;
	public float MinIntensity=0.0f;
	public float MaxIntensity = 4.0f;
	private float m_Intensity;
	private float m_CurIntensity;
	private float m_PauseTime = 0;

	private GameObject m_HatMan = null;
	
	// Use this for initialization
	
	void Awake()
	{
		m_Intensity = this.GetComponent<Light>().intensity;
	}
    

	void Update()
	{
		float curIntensity = m_Intensity;

		if (m_HatMan != null && m_HatMan.activeInHierarchy)
		{
			// deal with all distances as distance squared (saves a lot of square root calcs)
			Vector3 vDist = transform.position - m_HatMan.transform.position;

			if (vDist.y < 3 && vDist.y > 0)	// on the same floor
			{
				float dist = Vector3.Dot(vDist, vDist);

				if (dist < 400)
				{
					m_PauseTime -= Time.deltaTime;
					if (m_PauseTime <= 0)
					{
						m_PauseTime = Random.Range(MinTime, MaxTime);
						m_CurIntensity = Random.Range(MinIntensity, MaxIntensity);
					}
					curIntensity = m_CurIntensity;
				}
			}
		}

		this.GetComponent<Light>().intensity = curIntensity;
	}
}
