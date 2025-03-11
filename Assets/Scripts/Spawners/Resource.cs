using System;
using UnityEngine;
using UnityEngine.AI;

public class Resource : MonoBehaviour
{
    private NavMeshObstacle _meshObstacle;

    public event Action<Resource> Released;

    private void Awake()
    {
        _meshObstacle = GetComponent<NavMeshObstacle>();
    }

    private void OnEnable()
    {
        transform.parent = null;
        _meshObstacle.enabled = true;
    }

    public void DisableNavMeshObstacle()
    {
        _meshObstacle.enabled = false;
    }

    public void Release()
    {
        Released?.Invoke(this);
    }
}