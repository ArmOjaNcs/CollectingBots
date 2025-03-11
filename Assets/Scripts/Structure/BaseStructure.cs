using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseStructure : ObjectToSpawn
{
    [SerializeField] private Scanner _scanner;

    private PickedResourcesHandler _pickedResourcesHandler;
    private List<Bot> _bots = new();
    private SpawnPointFinder _spawnPointFinder;
    private BoxCollider _collider;
    private float _nearestPosition = float.MaxValue;
    private float _timer;
    private bool _isBuilded;
    private bool _isNewBaseStructureNeeded;

    public IReadOnlyList<Vector3> Positions = new List<Vector3>();

    public event Action<int> CollectedResourcesCountChanged;
    public event Action<int> AvailableResourcesCountChanged;
    public event Action<BaseStructure> NeedNewBot;
    public event Action NewBaseAcceptedToBuild;
    public event Action<BaseStructure> BuildNewBase;

    public Bot BotSendedToBuild { get; private set; }
    public int CollectedResourcesCount { get; private set; }
    public int AvailableResourcesCount { get; private set; }
    public BaseStructureBuildView BuildView { get; private set; }
    public bool IsCanBuild { get; private set; }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        _isNewBaseStructureNeeded = false;
        _bots.Clear();
        _isBuilded = false;
        IsCanBuild = true;
        BotSendedToBuild = null;
        _scanner.gameObject.transform.position = transform.position;
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
            if (IsCanBuildNewBase())
                BuildNewBaseStructure();

            if (BotSendedToBuild != null)
                BotSendedToBuild.FollowToBuildNewBase(BuildView);

            if (IsScanRecharged())
                Scan();
        }
    }

    public void SetPickedResourcesHolder(PickedResourcesHandler pickedResourcesHolder)
    {
        _pickedResourcesHandler = pickedResourcesHolder;
    }

    public void SetSpawnPointFinder(SpawnPointFinder spawnPointFinder)
    {
        _spawnPointFinder = spawnPointFinder;
    }

    public void CollectResource(Resource resource)
    {
        CollectedResourcesCount++;
        resource.Release();
        _pickedResourcesHandler.RemoveReleasedResource(resource);
        InvokeNewBotNeeded();
        CollectedResourcesCountChanged?.Invoke(CollectedResourcesCount);
    }

    public void BuildNewBaseStructure()
    {
        Bot bot = GetFreeBot();

        if (bot != null)
        {
            BotSendedToBuild = bot;
            BotSendedToBuild.SetBusy();
            _bots.Remove(bot);
            BotSendedToBuild.BuildStarted += OnBuildStarted;
        }
    }

    public void AddBot(Bot bot)
    {
        _bots.Add(bot);
    }

    public void SetBuildView(BaseStructureBuildView baseStructureBuildView)
    {
        BuildView = baseStructureBuildView;
    }

    public void Build()
    {
        _isBuilded = true;
        FillSpawnPositions();
    }

    public void SetNewBaseNeeded()
    {
        _isNewBaseStructureNeeded = true;
    }

    public void InvokeBuildBase()
    {
        IsCanBuild = false;
        NewBaseAcceptedToBuild?.Invoke();
    }

    public void ClearBotSendedToBuild()
    {
        BotSendedToBuild = null;
    }

    public void SpendResourcesToBot()
    {
        CollectedResourcesCount -= GameUtils.BotCost;
    }

    private void OnBuildStarted(Bot bot)
    {
        bot.BuildStarted -= OnBuildStarted;
        _isNewBaseStructureNeeded = false;
        BuildView.gameObject.SetActive(false);
        CollectedResourcesCount -= GameUtils.BaseCost;
        BuildNewBase?.Invoke(this);
    }

    private void InvokeNewBotNeeded()
    {
        if (IsEnoughResources())
            NeedNewBot?.Invoke(this);
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
        IEnumerable<Resource> availableResources = _pickedResourcesHandler.GetAvailableResources(_scanner.ScanArea());
        AvailableResourcesCount = availableResources.Count<Resource>();
        AvailableResourcesCountChanged?.Invoke(AvailableResourcesCount);
        SendBot(availableResources);
    }

    private void SendBot(IEnumerable<Resource> availableResources)
    {
        if (availableResources.Count<Resource>() > 0)
        {
            Bot bot = GetFreeBot();
            Resource nearestResource = null;

            if (bot != null && bot.isActiveAndEnabled)
            {
                for (int i = 0; i < availableResources.Count<Resource>(); i++)
                {
                    float distance = Vector3.Distance(availableResources.ElementAt(i).transform.position, transform.position);
                   
                    if (distance < _nearestPosition)
                    {
                        _nearestPosition = distance;
                        nearestResource = availableResources.ElementAt(i);
                    }
                }

                bot.SetCurrentResourceDestination(nearestResource);
                _pickedResourcesHandler.AddPickedResource(nearestResource);
                AvailableResourcesCountChanged?.Invoke(--AvailableResourcesCount);
                _nearestPosition = float.MaxValue;
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

        return null;
    }

    private bool IsCanBuildNewBase()
    {
        return BotSendedToBuild == null && _isNewBaseStructureNeeded && 
            CollectedResourcesCount >= GameUtils.BaseCost && _bots.Count > GameUtils.MinBotsCountToBuild;
    }

    private bool IsEnoughResources()
    {
        return _isNewBaseStructureNeeded == false && CollectedResourcesCount >= GameUtils.BotCost && 
            _bots.Count < GameUtils.MaxBotsCountOnBase || _isNewBaseStructureNeeded && 
            CollectedResourcesCount - GameUtils.BotCost >= GameUtils.BaseCost && _bots.Count < GameUtils.MaxBotsCountOnBase;
    }

    private void FillSpawnPositions()
    {
        Positions = _spawnPointFinder.FindPlaceWithRing(transform.position, _collider.size.x,
            GameUtils.BotDiameter, GameUtils.RingRadius);
    }
}