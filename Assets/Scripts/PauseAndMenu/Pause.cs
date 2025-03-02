using System;
using System.Collections.Generic;

public class Pause 
{
    private List<IPauseable> _pauseables = new List<IPauseable>();

    public event Action IsPaused;
    public event Action IsUnPaused;

    public void Stop()
    {
        foreach(IPauseable pauseable in _pauseables)
            pauseable.Stop();

        IsPaused?.Invoke();
    }

    public void Resume()
    {
        foreach (IPauseable pauseable in _pauseables)
            pauseable.Resume();

        IsUnPaused?.Invoke();
    }

    public void Register(IPauseable pauseable)
    {
        _pauseables.Add(pauseable);
    }
}