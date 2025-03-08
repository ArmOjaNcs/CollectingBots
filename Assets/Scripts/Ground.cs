using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class Ground : MonoBehaviour
{
    [SerializeField] private MeshCollider _meshCollider;

    public float MinXPosition { get; private set; }
    public float MaxXPosition { get; private set; }
    public float MinZPosition { get; private set; }
    public float MaxZPosition { get; private set; }
    public MeshCollider MeshCollider => _meshCollider;

    private void Awake()
    {
        _meshCollider = GetComponent<MeshCollider>();
        SetPositions();
    }

    private void SetPositions()
    {
        MinXPosition = _meshCollider.bounds.center.x - _meshCollider.bounds.extents.x;
        MaxXPosition = _meshCollider.bounds.center.x + _meshCollider.bounds.extents.x;
        MinZPosition = _meshCollider.bounds.center.z - _meshCollider.bounds.extents.z;
        MaxZPosition = _meshCollider.bounds.center.z + _meshCollider.bounds.extents.z;
    }
}