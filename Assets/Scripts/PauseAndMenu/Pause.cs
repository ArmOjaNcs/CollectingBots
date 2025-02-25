using System;
using System.Collections.Generic;

public static class Pause 
{
    private static List<IPauseable> _pauseables = new List<IPauseable>();

    public static event Action IsPaused;
    public static event Action IsUnPaused;

    public static void Stop()
    {
        foreach(IPauseable pauseable in _pauseables)
            pauseable.Stop();

        IsPaused?.Invoke();
    }

    public static void Resume()
    {
        foreach (IPauseable pauseable in _pauseables)
            pauseable.Resume();

        IsUnPaused?.Invoke();
    }

    public static void Register(IPauseable pauseable)
    {
        _pauseables.Add(pauseable);
    }
}