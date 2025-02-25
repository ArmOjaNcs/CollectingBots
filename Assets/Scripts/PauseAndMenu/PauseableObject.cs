using UnityEngine;

public class PauseableObject : MonoBehaviour, IPauseable
{
    private protected bool IsPaused;

    private protected virtual void Awake()
    {
        Register(this);
    }

    public void Register(IPauseable pauseable)
    {
        Pause.Register(pauseable);
    }

    public virtual void Stop()
    {
        IsPaused = true;
    }

    public virtual void Resume()
    {
        IsPaused = false;
    }
}