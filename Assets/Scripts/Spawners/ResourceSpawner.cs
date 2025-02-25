using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private int _maxCapacity;
    [SerializeField] private Ground _ground;

    private ObjectPool<Resource> _resourcePool;
    private Vector3 _resourceColliderSize;

    private void Awake()
    {
        _resourcePool = new ObjectPool<Resource>(_resourcePrefab, _maxCapacity, transform);
        _resourceColliderSize = _resourcePrefab.GetComponent<BoxCollider>().size;
    }

    public Resource SpawnResource()
    {
        Resource spawnedResource = _resourcePool.GetElement();

        if (TryGetSpawnPosition(_resourceColliderSize, out Vector3 availablePosition))
        {
            spawnedResource.transform.position = availablePosition;
            spawnedResource.gameObject.SetActive(true);
            return spawnedResource;
        }

        return null;
    }

    private bool TryGetSpawnPosition(Vector3 resourceColliderSize, out Vector3 availablePosition)
    {
        int protector = 100;

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