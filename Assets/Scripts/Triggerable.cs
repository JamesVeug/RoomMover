using UnityEngine;

public abstract class Triggerable : MonoBehaviour
{
    protected bool isLocked = false;

    public abstract void Toggle();

    public virtual bool IsLocked()
    {
        return isLocked;
    }
}
