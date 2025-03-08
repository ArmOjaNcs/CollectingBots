using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private int _maxCapacity;
    [Inject] private Pause _pause;
    private int _groundMask = 1 << GameUtils.Ground;
    private int _layerWithoutGround;

    private ObjectPool<Bot> _pool;
    private Queue<Bot> _translatableBots = new();

    public int TranslatableBotsCount => _translatableBots.Count;

    private void Awake()
    {
        _layerWithoutGround = ~_groundMask;
        _pool = new ObjectPool<Bot>(_botPrefab, _maxCapacity, transform);
    }

    public void AddStartBots(BaseStructure baseStructure)
    {
        for (int i = 0; i < GameUtils.MaxBotsCount; i++)
        {
            if (TrySpawnBotNearBaseStructure(baseStructure, out Bot bot))
                baseStructure.AddBot(bot);
        }
    }

    public void AddBotToBaseStructure(BaseStructure baseStructure)
    {
        if (TrySpawnBotNearBaseStructure(baseStructure, out Bot bot))
        {
            baseStructure.AddBot(bot);
            baseStructure.SpendResourcesToBot();
        }   
    }

    public void AddBotToTranslateQueue(Bot bot)
    {
        _translatableBots.Enqueue(bot);
    }

    public Bot DequeueTranslatableBot()
    {
        return _translatableBots.Dequeue();
    }

    private bool TrySpawnBotNearBaseStructure(BaseStructure baseStructure, out Bot bot)
    {
        if (TryGetSpawnPosition(baseStructure, out Vector3 availablePosition))
        {
            Bot spawnedBot = SpawnBot();
            spawnedBot.transform.position = availablePosition;
            spawnedBot.transform.Rotate(Vector3.up * Random.Range(0, GameUtils.FullAngleInDegrees));
            spawnedBot.gameObject.SetActive(true);
            spawnedBot.SetBaseStructure(baseStructure);
            bot = spawnedBot;
            return true;
        }

        bot = null;
        return false;
    }

    private Bot SpawnBot()
    {
        Bot spawnedBot = _pool.GetElement();

        if (spawnedBot.IsInitialized == false)
        {
            _pause.Register(spawnedBot);
            spawnedBot.SetInitialized();
        }

        return spawnedBot;
    }

    private bool TryGetSpawnPosition(BaseStructure baseStructure, out Vector3 availablePosition)
    {
        for (int i = 0; i < baseStructure.Positions.Count; i++)
        {
            Vector3 position = baseStructure.Positions[i];

            Collider[] colliders = Physics.OverlapSphere(position, GameUtils.BotDiameter, _layerWithoutGround);

            if (colliders.Length == 0)
            {
                availablePosition = position;
                return true;
            }
        }

        availablePosition = Vector3.zero;
        return false;
    }
}