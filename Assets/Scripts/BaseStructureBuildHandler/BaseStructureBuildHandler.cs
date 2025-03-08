using UnityEngine;

public class BaseStructureBuildHandler : MonoBehaviour
{
    [SerializeField] private BaseStructureBuildViewSpawner _baseStructureBuildViewSpawner;
    [SerializeField] private BuildInput _buildInput;
    [SerializeField] private Ground _ground;

    private BaseStructureBuildView _currentBaseStructureBuildView;
    private BaseStructure _currentBaseStructure;

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    public void OnNewBaseBuildInitiated(BaseStructure baseStructure)
    {
        if (baseStructure.BuildView == null)
            baseStructure.SetBuildView(_baseStructureBuildViewSpawner.GetBaseStructureBuildView());

        _currentBaseStructureBuildView = baseStructure.BuildView;
        _currentBaseStructureBuildView.SetDefaultParameters();
        _currentBaseStructureBuildView.gameObject.SetActive(true);
        _currentBaseStructure = baseStructure;
        _currentBaseStructure.SetNewBaseNeeded();
        _currentBaseStructure.BuildNewBase += OnBuildNewBase; 
        _buildInput.gameObject.SetActive(true);
    }

    private void Subscribe()
    {
        _buildInput.RayChanged += OnRayChanged;
        _buildInput.IsRotateToLeft += OnRotateToLeft;
        _buildInput.IsRotateToRight += OnRotateToRight;
        _buildInput.Placed += OnPlaced;
        _buildInput.Canceled += OnCanceled;
    }

    private void UnSubscribe()
    {
        _buildInput.RayChanged -= OnRayChanged;
        _buildInput.IsRotateToLeft -= OnRotateToLeft;
        _buildInput.IsRotateToRight -= OnRotateToRight;
        _buildInput.Placed -= OnPlaced;
        _buildInput.Canceled -= OnCanceled;
    }

    private void OnRayChanged(Ray ray)
    {
        RaycastHit hit;

        if (_ground.MeshCollider.Raycast(ray, out hit, float.MaxValue))
            _currentBaseStructureBuildView.transform.position = hit.point;
    }

    private void OnRotateToLeft(bool isRotate)
    {
        if (isRotate)
            _currentBaseStructureBuildView.transform.Rotate(Vector3.up * Time.deltaTime * GameUtils.BuildViewRotationSpeed);
    }

    private void OnRotateToRight(bool isRotate)
    {
        if (isRotate)
            _currentBaseStructureBuildView.transform.Rotate(Vector3.up * Time.deltaTime * - GameUtils.BuildViewRotationSpeed);
    }

    private void OnPlaced()
    {
        if(_currentBaseStructureBuildView.IsCanBuild)
        {
            _currentBaseStructureBuildView.AcceptPosition();
            _buildInput.gameObject.SetActive(false);
            _currentBaseStructureBuildView = null;
            return;
        }

        OnCanceled();
    }

    private void OnCanceled()
    {
        _currentBaseStructureBuildView = null;
        _currentBaseStructure.BuildView.gameObject.SetActive(false);
        _currentBaseStructure = null;
        _buildInput.gameObject.SetActive(false);
    }

    private void OnBuildNewBase(BaseStructure baseStructure)
    {
        baseStructure.BuildNewBase -= OnBuildNewBase;
        baseStructure.InvokeBuildBase();
    }
}