using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(NavMeshAgent))]
public class Bot : ObjectToSpawn
{
    [SerializeField] private Transform _grabPoint;

    private BaseStructure _baseSructure;
    private Resource _currentResourceTarget;
    private Resource _resourceOnDeliver;
    private NavMeshAgent _agent;
    private Vector3 _velocity;
    private Vector3 _distance;
    private BaseStructureBuildView _baseStructureBuildView;
    private bool _isDelivered;

    public event Action Ride;
    public event Action Pause;
    public event Action Play;
    public event Action StopRide;
    public event Action<Bot> BuildStarted;

    public bool IsBusy { get; private set; }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = true;
        _agent.updatePosition = true;
        _agent.autoRepath = true;
        _agent.updateUpAxis = true;
    }

    private void OnEnable()
    {
        IsBusy = false;
        _isDelivered = false;
    }

    private void Update()
    {
        if (IsPaused == false)
        {
            if(IsBusy)
                _agent.Move(transform.forward * Time.deltaTime);

            RecalculateDistanceToCurrentResource();
            RecalculateDistanceToBaseStructure();
            RecalculateDistanceToBuildNewBase();
        }
    }

    public void SetBaseStructure(BaseStructure currentBaseStructure)
    {
        _baseSructure = currentBaseStructure;
    }

    public void SetBusy()
    {
        _isDelivered = false;
        _agent.isStopped = false;
        IsBusy = true;
        Ride?.Invoke();
    }

    public void FollowToBuildNewBase(BaseStructureBuildView baseStructureBuildView)
    {
        _baseStructureBuildView = baseStructureBuildView;
        _agent.destination = baseStructureBuildView.transform.position;
    }

    public void SetCurrentResourceDestination(Resource resource)
    {
        SetBusy();
        _currentResourceTarget = resource;
        _agent.destination = resource.transform.position;
    }

    public override void Stop()
    {
        if (gameObject.activeInHierarchy)
        {
            base.Stop();
            _velocity = _agent.velocity;
            _agent.velocity = Vector3.zero;
            _agent.isStopped = true;
            Pause?.Invoke();
        }
    }

    public override void Resume()
    {
        if (gameObject.activeInHierarchy)
        {
            base.Resume();
            _agent.velocity = _velocity;

            if (_isDelivered == false)
            {
                _agent.isStopped = false;
                Play?.Invoke();
            }
        }
    }

    private void SetUnBusy()
    {
        _agent.velocity = Vector3.zero;
        IsBusy = false;
        _agent.isStopped = true;
        _resourceOnDeliver = null;
        _isDelivered = true;
        StopRide?.Invoke();
    }

    private void RecalculateDistanceToCurrentResource()
    {
        if (_currentResourceTarget != null)
        {
            _distance = transform.position - _currentResourceTarget.transform.position;

            if (_distance.sqrMagnitude < Mathf.Pow(GameUtils.BotMinDistanceToTarget, GameUtils.Quad))
            {
                _currentResourceTarget.transform.position = _grabPoint.transform.position;
                _currentResourceTarget.transform.SetParent(_grabPoint);
                _currentResourceTarget.DisableNavMeshObstacle();
                _resourceOnDeliver = _currentResourceTarget;
                _currentResourceTarget = null;
                SetBaseStructureDestination();
            }
        }
    }

    private void RecalculateDistanceToBaseStructure()
    {
        if (_resourceOnDeliver != null)
        {
            _distance = transform.position - _baseSructure.transform.position;

            if (_distance.sqrMagnitude < Mathf.Pow(GameUtils.BotMinDistanceToBaseStructure, GameUtils.Quad))
                ShipResource();
        }
    }

    private void ShipResource()
    {
        if (IsBusy && _resourceOnDeliver != null)
        {
            _resourceOnDeliver.transform.parent = null;
            _baseSructure.CollectResource(_resourceOnDeliver);
            SetUnBusy();
        }
    }

    private void SetBaseStructureDestination()
    {
        _agent.velocity = Vector3.zero;
        _agent.destination = _baseSructure.transform.position;
    }

    private void RecalculateDistanceToBuildNewBase()
    {
        if (_baseStructureBuildView != null)
        {
            _distance = transform.position - _baseStructureBuildView.transform.position;

            if (_distance.sqrMagnitude < Mathf.Pow(GameUtils.BotMinDistanceToBaseStructure, GameUtils.Quad) 
                && _baseStructureBuildView.IsAccepted)
            {
                _baseStructureBuildView = null;
                SetUnBusy();
                _baseStructureBuildView = null;
                BuildStarted?.Invoke(this);
            }
        }
    }
}