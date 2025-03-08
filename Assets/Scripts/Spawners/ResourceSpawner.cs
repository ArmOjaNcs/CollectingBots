using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class ResourceSpawner : PauseableObject
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private int _maxCapacity;
    [SerializeField] private Ground _ground;
    [Inject] private Pause _pause;

    private List<Vector3> _positions = new();
    private ObjectPool<Resource> _resourcePool;
    private Vector3 _resourceColliderSize;
    private float _timer;
    private int _currentResourcesCount;
    private int _groundMask = 1 << GameUtils.Ground;
    private int _layerWithoutGround;

    private void Awake()
    {
        _layerWithoutGround = ~_groundMask;
        _resourcePool = new ObjectPool<Resource>(_resourcePrefab, _maxCapacity, transform);
        _resourceColliderSize = _resourcePrefab.GetComponent<BoxCollider>().size;
        _pause.Register(this);
        SpawnPointFinder spawnPointFinder = new SpawnPointFinder();
        float resourceDiameter = _resourceColliderSize.x * GameUtils.MultiplierForResourceDiameter;
        _positions = spawnPointFinder.FindPlaceWithQuad(
            _ground.MinXPosition + resourceDiameter / GameUtils.Half,
            _ground.MinZPosition + resourceDiameter / GameUtils.Half,
            _ground.MaxXPosition - resourceDiameter / GameUtils.Half, 
            _ground.MaxZPosition - resourceDiameter / GameUtils.Half, resourceDiameter);
    }

    private void Update()
    {
        if (IsPaused == false)
        {
            PostResource();
        }
    }

    private Resource GetResource()
    {
        Resource spawnedResource = _resourcePool.GetElement();

        if (TryGetSpawnPosition(_resourceColliderSize, out Vector3 availablePosition))
        {
            availablePosition.y = _resourceColliderSize.x / GameUtils.Half;
            spawnedResource.transform.position = availablePosition;
            spawnedResource.transform.Rotate(Vector3.up * Random.Range(0,GameUtils.FullAngleInDegrees));
            spawnedResource.gameObject.SetActive(true);
            spawnedResource.Released += ReleaseResource;
            return spawnedResource;
        }

        return null;
    }

    private void ReleaseResource(Resource resource)
    {
        resource.Released -= ReleaseResource;
        _currentResourcesCount--;
        resource.gameObject.SetActive(false);
    }

    private void PostResource()
    {
        if (_currentResourcesCount < GameUtils.MaxResourcesCount)
        {
            _timer -= Time.deltaTime;

            if (_timer < 0)
            {
                GetResource();
                _timer = Random.Range(GameUtils.MinTimeToSpawn, GameUtils.MaxTimeToSpawn);
                _currentResourcesCount++;
            }
        }
    }

    private bool TryGetSpawnPosition(Vector3 resourceColliderSize, out Vector3 availablePosition)
    {
        int protector = GameUtils.MaxAttemptsCount;

        while (protector > 0)
        {
            int randomIndex = Random.Range(0, _positions.Count);
            Vector3 position = _positions[randomIndex];
            Collider[] colliders = Physics.OverlapBox(position, resourceColliderSize,
                Quaternion.identity, _layerWithoutGround);

            if (colliders.Length == 0)
            {
                availablePosition = position;
                return true;
            }

            protector--;
        }

        availablePosition = Vector3.zero;
        return false;
    }
}