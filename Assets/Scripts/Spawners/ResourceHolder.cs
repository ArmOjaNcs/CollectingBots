using System.Collections.Generic;
using UnityEngine;

public class ResourceHolder : PauseableObject
{
    [SerializeField] private ResourceSpawner _resourceSpawner;

    private float _timer;
    private int _currentResourcesCount;

    private List<Resource> _resources = new();
    private List<Resource> _availableResources = new();

    private void Update()
    {
        if(IsPaused == false)
        {
            PostResource();
        }
    }

    public List<Resource> GetAvailableResources()
    {
        _availableResources.Clear();

        foreach (Resource resource in _resources)
            if (resource.IsPicked == false && resource.isActiveAndEnabled)
                _availableResources.Add(resource);
        
        return _availableResources;
    }

    public void PickResource(Resource resource)
    {
        resource.Pick();
        _currentResourcesCount--;
    }

    public void ReleaseResource(Resource resource)
    {
        if(resource != null)
            resource.gameObject.SetActive(false);
    }

    private void PostResource()
    {
        if (_currentResourcesCount < GameUtils.MaxResourcesCount)
        {
            _timer -= Time.deltaTime;

            if (_timer < 0)
            {
                AddResource();
                _timer = Random.Range(GameUtils.MinTimeToSpawn, GameUtils.MaxTimeToSpawn);
                _currentResourcesCount++;
            }
        }
    }

    private void AddResource()
    {
        Resource spawnedResource = _resourceSpawner.SpawnResource();

        if (spawnedResource != null && _resources.Contains(spawnedResource) == false)
            _resources.Add(spawnedResource);
    }
}