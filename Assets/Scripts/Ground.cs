using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class Ground : MonoBehaviour
{
    [SerializeField] private MeshCollider _ground;

    public float MinXPosition { get; private set; }
    public float MaxXPosition { get; private set; }
    public float MinZPosition { get; private set; }
    public float MaxZPosition { get; private set; }

    private void Awake()
    {
        _ground = GetComponent<MeshCollider>();
        SetPositions();
    }

    private void SetPositions()
    {
        MinXPosition = _ground.bounds.center.x - _ground.bounds.extents.x;
        MaxXPosition = _ground.bounds.center.x + _ground.bounds.extents.x;
        MinZPosition = _ground.bounds.center.z - _ground.bounds.extents.z;
        MaxZPosition = _ground.bounds.center.z + _ground.bounds.extents.z;
    }
}