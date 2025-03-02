using UnityEngine;
using Zenject;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private int _maxCapacity;
    [Inject] private Pause _pause;

    private ObjectPool<Bot> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Bot>(_botPrefab, _maxCapacity, transform);
    }

    public Bot SpawnBot(BaseStructure baseStructure)
    {
        Bot spawnedBot = _pool.GetElement();

        if(spawnedBot.IsInitialized == false)
        {
           _pause.Register(spawnedBot);
            spawnedBot.SetInitialized();
        }

        spawnedBot.transform.position = new Vector3(baseStructure.transform.position.x + baseStructure.Collider.bounds.extents.x, 0, 0);
        spawnedBot.gameObject.SetActive(true);
        spawnedBot.SetBaseStructure(baseStructure);
        return spawnedBot;
    }
}