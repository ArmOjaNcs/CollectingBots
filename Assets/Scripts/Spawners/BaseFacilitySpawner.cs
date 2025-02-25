using UnityEngine;

public class BaseFacilitySpawner : MonoBehaviour
{
    [SerializeField] private BotSpawner _botSpawner;
    [SerializeField] private ResourceHolder _resourceHolder;
    [SerializeField] private BaseFacility _baseFacilityPrefab;
    [SerializeField] private int _maxCapacity;

    private ObjectPool<BaseFacility> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<BaseFacility>(_baseFacilityPrefab, _maxCapacity, transform);
    }

    private void Start()
    {
        SpawnBaseFacility(Vector3.zero);
    }

    public void SpawnBaseFacility(Vector3 position)
    {
        BaseFacility baseFacility = _pool.GetElement();
        baseFacility.gameObject.SetActive(true);
        baseFacility.transform.position = position;
        baseFacility.SetResourceHolder(_resourceHolder);
        baseFacility.FacilityBuilded += OnFacilityBuilded;
        baseFacility.Build();
    }

    private void OnFacilityBuilded(BaseFacility baseFacility)
    {
        for (int i = 0; i < GameUtils.MaxBotsCount; i++)
        {
            Bot bot = _botSpawner.SpawnBot(baseFacility);
            baseFacility.AddBot(bot);
        }
    }
}