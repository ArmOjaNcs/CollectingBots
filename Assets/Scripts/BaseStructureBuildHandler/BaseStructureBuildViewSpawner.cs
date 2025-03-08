using UnityEngine;
using Zenject;

public class BaseStructureBuildViewSpawner : MonoBehaviour
{
    [SerializeField] private BaseStructureBuildView _baseStructureBuildViewPrefab;
    [SerializeField] private int _maxCapacity;
    [Inject] private Pause _pause;

    private ObjectPool<BaseStructureBuildView> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<BaseStructureBuildView>(_baseStructureBuildViewPrefab, _maxCapacity, transform);
    }

    public BaseStructureBuildView GetBaseStructureBuildView()
    {
        BaseStructureBuildView baseStructureBuildView = _pool.GetElement();

        if (baseStructureBuildView.IsInitialized == false)
        {
            _pause.Register(baseStructureBuildView);
            baseStructureBuildView.SetInitialized();
        }

        return baseStructureBuildView;
    }
}