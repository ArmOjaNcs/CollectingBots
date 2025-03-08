using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> Released;

    private void OnEnable()
    {
        transform.parent = null;
    }

    public void Release()
    {
        Released?.Invoke(this);
    }
}