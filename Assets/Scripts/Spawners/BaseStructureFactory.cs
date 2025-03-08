using UnityEngine;
using Zenject;

public class BaseStructureFactory : MonoBehaviour
{
    [SerializeField] private BotSpawner _botSpawner;
    [SerializeField] private BaseStructureAnimatorSpawner _baseStructureAnimatorSpawner;
    [SerializeField] private BaseStructure _baseStructurePrefab;
    [SerializeField] private int _maxCapacity;
    [Inject] private Pause _pause;
    [Inject] private PickedResourcesHandler _pickedResourcesHandler;

    private ObjectPool<BaseStructure> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<BaseStructure>(_baseStructurePrefab, _maxCapacity, transform);
    }

    private void Start()
    {
        StartGame();
    }

    private void StartBuildBaseStructure(BaseStructureBuildView baseStructureBuildView)
    {
        BaseStructureAnimator baseStructureAnimator = _baseStructureAnimatorSpawner.GetAnimator();
        baseStructureAnimator.transform.position = baseStructureBuildView.transform.position;
        baseStructureAnimator.transform.rotation = baseStructureBuildView.transform.rotation;
        baseStructureAnimator.AnimatorFinished += OnAnimatorFinished;
        baseStructureAnimator.BuildStructure();
    }

    private void OnAnimatorFinished(BaseStructureAnimator baseStructureAnimator)
    {
        SpawnBaseStructure(baseStructureAnimator.transform.position, baseStructureAnimator.transform.rotation);
        baseStructureAnimator.AnimatorFinished -= OnAnimatorFinished;
        _baseStructureAnimatorSpawner.ReleaseAnimator(baseStructureAnimator);
    }

    private BaseStructure SpawnBaseStructure(Vector3 position, Quaternion rotation)
    {
        BaseStructure baseStructure = _pool.GetElement();
        baseStructure.transform.position = position;
        baseStructure.transform.rotation = rotation;

        if (baseStructure.IsInitialized == false)
        {
            _pause.Register(baseStructure);
            baseStructure.SetPickedResourcesHolder(_pickedResourcesHandler);
            SpawnPointFinder spawnPointFinder = new SpawnPointFinder();
            baseStructure.SetSpawnPointFinder(spawnPointFinder);
            baseStructure.SetInitialized();
        }

        baseStructure.gameObject.SetActive(true);

        if (_botSpawner.TranslatableBotsCount > 0)
        {
            Bot bot = _botSpawner.DequeueTranslatableBot();
            bot.SetBaseStructure(baseStructure);
            baseStructure.AddBot(bot);
        }

        baseStructure.NeedNewBot += _botSpawner.AddBotToBaseStructure;
        baseStructure.BuildNewBase += OnBuildNewBase;
        baseStructure.Build();

        return baseStructure;
    }

    private void OnBuildNewBase(BaseStructure baseStructure)
    {
        baseStructure.BuildNewBase -= OnBuildNewBase;
        StartBuildBaseStructure(baseStructure.BuildView);
        _botSpawner.AddBotToTranslateQueue(baseStructure.BotSendedToBuild);
        baseStructure.ClearBotSendedToBuild();
    }

    private void StartGame()
    {
        BaseStructure baseStructure = SpawnBaseStructure(Vector3.zero, Quaternion.identity);
        _botSpawner.AddStartBots(baseStructure);
    }
}