using System;
using UnityEngine;
using UnityEngine.AI;

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
    private bool _isDelivered;

    public event Action Ride;
    public event Action StopRide;

    public bool IsBusy { get; private set; }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = true;
        _agent.updatePosition = true;
        _agent.autoRepath = true;
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
            RecalculateDistanceToCurrentResource();
            RecalculateDistanceToBaseStructure();
        }
    }

    public void SetBaseStructure(BaseStructure currentBaseStructure)
    {
        _baseSructure = currentBaseStructure;
    }

    public void SetCurrentResourceDestination(Resource resource)
    {
        _isDelivered = false;
        _agent.isStopped = false;
        IsBusy = true;
        Ride?.Invoke();
        _currentResourceTarget = resource;
        _agent.destination = resource.transform.position;
    }

    public override void Stop()
    {
        if (gameObject.activeInHierarchy)
        {
            _velocity = _agent.velocity;
            _agent.velocity = Vector3.zero;
            _agent.isStopped = true;
            StopRide?.Invoke();
        }
    }

    public override void Resume()
    {
        if (gameObject.activeInHierarchy)
        {
            _agent.velocity = _velocity;

            if (_isDelivered == false)
            {
                _agent.isStopped = false;
                Ride?.Invoke();
            }
        }
    }

    private void RecalculateDistanceToCurrentResource()
    {
        if (_currentResourceTarget != null)
        {
            _distance = transform.position - _currentResourceTarget.transform.position;

            if (_distance.sqrMagnitude < Mathf.Pow(GameUtils.BotMinDistanceToTarget, 2))
            {
                _currentResourceTarget.transform.position = _grabPoint.transform.position;
                _currentResourceTarget.transform.SetParent(_grabPoint);
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

            if (_distance.sqrMagnitude < Mathf.Pow(GameUtils.BotMinDistanceToBaseStructure, 2))
            {

                ShipResource();
            }
        }
    }

    private void ShipResource()
    {
        if (IsBusy && _resourceOnDeliver != null)
        {
            _resourceOnDeliver.transform.parent = null;
            _baseSructure.CollectResource(_resourceOnDeliver);
            _agent.velocity = Vector3.zero;
            IsBusy = false;
            _agent.isStopped = true;
            _resourceOnDeliver = null;
            _isDelivered = true;
            StopRide?.Invoke();
        }
    }

    private void SetBaseStructureDestination()
    {
        _agent.velocity = Vector3.zero;
        Ride?.Invoke();
        _agent.destination = _baseSructure.transform.position;
    }
}