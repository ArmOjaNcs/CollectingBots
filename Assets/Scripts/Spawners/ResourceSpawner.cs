using UnityEngine;
using Zenject;

public class ResourceSpawner : PauseableObject
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private int _maxCapacity;
    [SerializeField] private Ground _ground;
    [Inject] Pause _pause;

    private ObjectPool<Resource> _resourcePool;
    private Vector3 _resourceColliderSize;
    private float _timer;
    private int _currentResourcesCount;

    private void Awake()
    {
        _resourcePool = new ObjectPool<Resource>(_resourcePrefab, _maxCapacity, transform);
        _resourceColliderSize = _resourcePrefab.GetComponent<BoxCollider>().size;
        _pause.Register(this);
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
            spawnedResource.transform.position = availablePosition;
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
            Vector3 position = RandomPosition(resourceColliderSize);
            Collider[] colliders = Physics.OverlapBox(position, resourceColliderSize,
                Quaternion.identity, LayerMask.NameToLayer(GameUtils.Ground));

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

    private Vector3 RandomPosition(Vector3 resourceColliderSize)
    {
        return new Vector3(Random.Range(_ground.MinXPosition + resourceColliderSize.x / GameUtils.Half,
                    _ground.MaxXPosition - resourceColliderSize.x / GameUtils.Half),
                    0 + resourceColliderSize.y / GameUtils.Half,
                    Random.Range(_ground.MinZPosition + resourceColliderSize.z / GameUtils.Half,
                    _ground.MaxZPosition - resourceColliderSize.z / GameUtils.Half));
    }
}