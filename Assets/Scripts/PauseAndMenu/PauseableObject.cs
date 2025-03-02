using UnityEngine;

public abstract class PauseableObject : MonoBehaviour, IPauseable
{
    private protected bool IsPaused;

    public virtual void Stop()
    {
        IsPaused = true;
    }

    public virtual void Resume()
    {
        IsPaused = false;
    }
}