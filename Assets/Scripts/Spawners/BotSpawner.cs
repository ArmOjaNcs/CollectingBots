using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private int _maxCapacity;

    private ObjectPool<Bot> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Bot>(_botPrefab, _maxCapacity, transform);
    }

    public Bot SpawnBot(BaseFacility baseFacility)
    {
        Bot spawnedBot = _pool.GetElement();
        spawnedBot.transform.position = new Vector3(baseFacility.transform.position.x + 
            baseFacility.Collider.bounds.extents.x, 0, 0);
        spawnedBot.gameObject.SetActive(true);
        spawnedBot.SetBaseFacility(baseFacility);
        return spawnedBot;
    }
}