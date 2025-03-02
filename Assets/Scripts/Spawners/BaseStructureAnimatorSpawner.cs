using UnityEngine;
using Zenject;

public class BaseStructureAnimatorSpawner : MonoBehaviour
{
    [SerializeField] private BaseStructureAnimator _baseStructureAnimatorPrefab;
    [SerializeField] private int _maxCapacity;
    [Inject] private Pause _pause;

    private ObjectPool<BaseStructureAnimator> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<BaseStructureAnimator>(_baseStructureAnimatorPrefab, _maxCapacity, transform);
    }

    public BaseStructureAnimator GetAnimator()
    {
        BaseStructureAnimator baseStructureAnimator = _pool.GetElement();

        if (baseStructureAnimator.IsInitialized == false)
        {
            _pause.Register(baseStructureAnimator);
            baseStructureAnimator.SetInitialized();
        }

        baseStructureAnimator.gameObject.SetActive(true);
        return baseStructureAnimator;
    }

    public void ReleaseAnimator(BaseStructureAnimator baseStructureAnimator)
    {
        baseStructureAnimator.gameObject.SetActive(false);
    }
}