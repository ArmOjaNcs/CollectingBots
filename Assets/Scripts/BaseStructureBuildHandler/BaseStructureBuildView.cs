using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Renderer))]
public class BaseStructureBuildView : ObjectToSpawn
{
    [SerializeField] private Color _validBuildColor;
    [SerializeField] private Color _invalidBuildColor;
    [SerializeField] private Color _acceptedBuildColor;

    private Renderer _renderer;
    private BoxCollider _boxCollider;
    private Material[] _materials;
    private int _groundMask = 1 << GameUtils.Ground;
    private int _layerWithoutGround;

    public bool IsAccepted { get; private set; }
    public bool IsCanBuild { get; private set; }

    private void Awake()
    {
        _layerWithoutGround = ~_groundMask;
        _renderer = GetComponent<Renderer>();
        _materials = _renderer.materials;
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        SetDefaultParameters();
    }

    private void Update()
    {
        if (IsPaused == false && isActiveAndEnabled && IsAccepted == false)
            CastOverlapBox();
    }

    public void AcceptPosition()
    {
        IsAccepted = true;
        ChangeColor(_acceptedBuildColor);
        _boxCollider.enabled = true;
    }

    public void SetDefaultParameters()
    {
        ChangeColor(_invalidBuildColor);
        IsCanBuild = false;
        _boxCollider.enabled = false;
        IsAccepted = false;
    }

    private void CastOverlapBox()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, _boxCollider.size,
            transform.rotation, _layerWithoutGround);

        if (colliders.Length == 0)
        {
            if (IsCanBuild == false)
                ChangeColor(_validBuildColor);

            IsCanBuild = true;
        }
        else
        {
            if (IsCanBuild)
                ChangeColor(_invalidBuildColor);

            IsCanBuild = false;
        }
    }

    private void ChangeColor(Color color)
    {
        foreach (Material material in _materials)
            material.color = color;
    }
}