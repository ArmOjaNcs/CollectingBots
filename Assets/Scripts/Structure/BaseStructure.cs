using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseStructure : ObjectToSpawn
{
    [SerializeField] private Scanner _scanner;

    private PickedResourcesHandler _pickedResourcesHandler;
    private List<Bot> _bots = new();
    private List<Resource> _availableResources = new();
    private Resource _nearestResource;
    private float _nearestPosition = float.MinValue;
    private float _timer;
    private bool _isBuilded;

    public event Action<int> CollectedResourcesCountChanged;
    public event Action<int> AvailableResourcesCountChanged;
    public event Action<string> BotStatusChanged;

    public int CollectedResourcesCount { get; private set; }
    public int AvailableResourcesCount { get; private set; }
    public BoxCollider Collider { get; private set; }

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        _bots.Clear();
    }

    private void OnDisable()
    {
        CollectedResourcesCount = 0;
        AvailableResourcesCount = 0;
    }

    private void Update()
    {
        if (IsPaused == false && _isBuilded)
        {
            if (IsScanRecharged())
                Scan();
        }
    }

    public void SetPickedResourcesHolder(PickedResourcesHandler pickedResourcesHolder)
    {
        _pickedResourcesHandler = pickedResourcesHolder;
    }

    public void CollectResource(Resource resource)
    {
        CollectedResourcesCount++;
        resource.Release();
        CollectedResourcesCountChanged?.Invoke(CollectedResourcesCount);
        _pickedResourcesHandler.RemoveReleasedResource(resource);
    }

    public void AddBot(Bot bot)
    {
        _bots.Add(bot);
    }

    public void Build()
    {
        _isBuilded = true;
    }

    private bool IsScanRecharged()
    {
        _timer += Time.deltaTime;

        if (_timer > GameUtils.TimeToScan)
        {
            _timer = 0;
            return true;
        }

        return false;
    }

    private void Scan()
    {
        _availableResources.Clear();
        _availableResources = _pickedResourcesHandler.GetAvailableResources(_scanner.ScanArea()).ToList();
        AvailableResourcesCount = _availableResources.Count;
        AvailableResourcesCountChanged?.Invoke(AvailableResourcesCount);

        SendBot();
    }

    private void SendBot()
    {
        if (_availableResources.Count > 0)
        {
            Bot bot = GetFreeBot();
       
            if (bot != null && bot.isActiveAndEnabled)
            {
                for (int i = 0; i < _availableResources.Count; i++)
                {
                    float distance = transform.position.sqrMagnitude - _availableResources[i].transform.position.sqrMagnitude;

                    if (distance > _nearestPosition)
                    {
                        _nearestPosition = distance;
                        _nearestResource = _availableResources[i];
                    }
                }

                bot.SetCurrentResourceDestination(_nearestResource);
                _pickedResourcesHandler.AddPickedResource(_nearestResource);
                AvailableResourcesCountChanged?.Invoke(--AvailableResourcesCount);
                BotStatusChanged?.Invoke(GameUtils.BotSendedMessage);
                _nearestPosition = float.MinValue;
                _nearestResource = null;
            }
        }
    }

    private Bot GetFreeBot()
    {
        foreach (Bot bot in _bots)
        {
            if (bot.IsBusy == false)
                return bot;
        }

        BotStatusChanged?.Invoke(GameUtils.BotsBusyMessage);
        return null;
    }
}