using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseFacility : MonoBehaviour
{
    [SerializeField] private FacilityMenu _facilityMenu;

    private BaseFacilityAnimator _animator;
    private ResourceHolder _resourceHolder;
    private List<Bot> _bots = new();
    private List<Resource> _scanedResources = new();
    private Resource _nearestResource;
    private float _nearestPosition = float.MinValue;
    private int _collectedResourcesCount;
    private int _availableResourcesCount;

    public event Action<BaseFacility> FacilityBuilded;

    public BoxCollider Collider {  get; private set; }

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        _animator = GetComponent<BaseFacilityAnimator>();
        _facilityMenu.SetCollectedResourcesCount(_collectedResourcesCount);
        _facilityMenu.SetAvailableResourcesCount(_availableResourcesCount);
        _facilityMenu.SetBotStatus("");
        _facilityMenu.Scan.onClick.AddListener(Scan);
    }

    private void OnEnable()
    {
        _animator.FacilityBuilded += OnBaseFacilityBuilded;
        _bots.Clear();
    }

    private void OnDisable()
    {
        _animator.FacilityBuilded -= OnBaseFacilityBuilded;
    }

    public void CollectResource(Resource resource)
    {
        _collectedResourcesCount++;
        _resourceHolder.ReleaseResource(resource);
        _facilityMenu.SetCollectedResourcesCount(_collectedResourcesCount);
    }

    public void AddBot(Bot bot)
    {
        _bots.Add(bot);
    }

    public void SetResourceHolder(ResourceHolder resourceHolder)
    {
        _resourceHolder = resourceHolder;
    }

    public void Build()
    {
        _animator.BuildFacility();
    }

    private void Scan()
    {
        _scanedResources.Clear();
        _scanedResources = _resourceHolder.GetAvailableResources();
        _availableResourcesCount = _scanedResources.Count;
        _facilityMenu.SetAvailableResourcesCount(_availableResourcesCount);

        SendBot();
    }

    private void SendBot()
    {
        if (_scanedResources.Count > 0)
        {
            Bot bot = GetFreeBot();

            if (bot != null && bot.isActiveAndEnabled)
            {
                for (int i = 0; i < _scanedResources.Count; i++)
                {
                    float distance = transform.position.sqrMagnitude - _scanedResources[i].transform.position.sqrMagnitude;

                    if(distance > _nearestPosition)
                    {
                        _nearestPosition = distance;
                        _nearestResource = _scanedResources[i];
                    }
                }

                bot.SetCurrentResourceDestination(_nearestResource);
                _resourceHolder.PickResource(_nearestResource);
                _facilityMenu.SetAvailableResourcesCount(--_availableResourcesCount);
                _facilityMenu.SetBotStatus(GameUtils.BotSendedMessage);
                _nearestPosition = float.MinValue;
                _nearestResource = null;
            }
        }
    }

    private Bot GetFreeBot()
    {
        foreach(Bot bot in _bots)
        {
            if(bot.IsBusy == false)
                return bot;
        }

        _facilityMenu.SetBotStatus(GameUtils.BotsBusyMessage);
        return null;
    }

    private void OnBaseFacilityBuilded()
    {
        FacilityBuilded?.Invoke(this);
    }
}