using UnityEngine;
using Zenject;

public class BaseStructureSpawner : PauseableObject
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
        _pause.Register(this);
    }

    private void Start()
    {
        SpawnBaseStructure(Vector3.zero);
    }

    public void SpawnBaseStructure(Vector3 position)
    {
        if (IsPaused)
            return;

        BaseStructure baseStructure = _pool.GetElement();
        
        if(baseStructure.IsInitialized == false)
        {
            _pause.Register(baseStructure);
            baseStructure.SetPickedResourcesHolder(_pickedResourcesHandler);
            baseStructure.SetInitialized();
        }

        BaseStructureAnimator baseStructureAnimator = _baseStructureAnimatorSpawner.GetAnimator();
        baseStructure.transform.position = position;
        baseStructureAnimator.AnimatorFinished += OnAnimatorFinished;
        baseStructureAnimator.BuildStructure(baseStructure);
    }

    private void OnAnimatorFinished(BaseStructureAnimator baseStructureAnimator)
    {
        baseStructureAnimator.CurrentBaseStructure.gameObject.SetActive(true);

        for (int i = 0; i < GameUtils.MaxBotsCount; i++)
        {
            Bot bot = _botSpawner.SpawnBot(baseStructureAnimator.CurrentBaseStructure);
            baseStructureAnimator.CurrentBaseStructure.AddBot(bot);
        }

        baseStructureAnimator.AnimatorFinished -= OnAnimatorFinished;
        baseStructureAnimator.CurrentBaseStructure.Build();
        _baseStructureAnimatorSpawner.ReleaseAnimator(baseStructureAnimator);
    }
}