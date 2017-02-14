using UnityEngine;
using System.Collections;

/// <summary>
/// Extension for MonoBehavior to ensure there is always only 1 of this script
/// </summary>
/// <typeparam name="T">Type of Singleton this contains</typeparam>
/// <author>James Veugelaers</author>
public class Singleton<T> : MonoBehaviour where T : Component
{
    public const int Version = 1;

    [SerializeField]
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if( m_instance == null)
            {
                T[] s = GameObject.FindObjectsOfType<T>();
                if(s.Length > 1)
                {
                    Debug.LogWarning("SINGLETON WARNING: " + s.Length + " instances found of type " + s[0].GetType().ToString() + "! Using " + s[0].gameObject.name);
                    for(int i = 1; i < s.Length; i++)
                    {
                        Destroy(s[i]);
                    }
                    m_instance = s[0];
                }
                else if (s.Length == 1)
                {
                    m_instance = s[0];
                }
                else
                {
                    GameObject o = new GameObject();
                    o.name = typeof(T).ToString();
                    m_instance = o.AddComponent<T>();
                }
            }

            return m_instance;
        }
    }
}