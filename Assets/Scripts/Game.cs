using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Game : Singleton<Game>
{


    private Inspectable m_currentInspect;


    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }

    public void InspectElement(Inspectable inspect)
    {
        if(m_currentInspect == inspect)
        {
            m_currentInspect = null;
            GameCamera.Instance.ResetCamera();

            GameUI.Instance.HideInspector();
            return;
        }
        else if(inspect != null)
        {
            m_currentInspect = inspect;
            GameCamera.Instance.Inspect(inspect.InspectCamerasTransform);
            GameUI.Instance.ShowInspector(inspect.InspectText);
        }

    }

    
}