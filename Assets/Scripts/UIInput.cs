using UnityEngine;
using Zenject;

public class UIInput : PauseableObject
{
    [SerializeField] private LayerMask _clickableMask;
    [SerializeField] private BaseStructureMenu _baseStructureMenu;
    [SerializeField] private BaseStructureBuildHandler _buildHandler;
    [Inject] private Pause _pause;

    private BaseStructure _currentStructure;
    private Camera _camera;

    private bool IsClicked => Input.GetMouseButtonDown(1);

    private void Awake()
    {
        _pause.Register(this);
        _camera = Camera.main;
    }

    private void Update()
    {
        if (IsPaused == false)
            CastRay();
    }

    private void CastRay()
    {
        if (IsClicked)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _clickableMask.value))
            {
                if (hit.collider != null && hit.collider.TryGetComponent(out BaseStructure baseStructure))
                {
                    _currentStructure = baseStructure;
                    _baseStructureMenu.SetBaseStructure(_currentStructure);
                    _baseStructureMenu.BuildInitiated += _buildHandler.OnNewBaseBuildInitiated;
                    _baseStructureMenu.ShowMenu();
                }
            }
            else
            {
                _baseStructureMenu.HideMenu();
                _baseStructureMenu.BuildInitiated -= _buildHandler.OnNewBaseBuildInitiated;
            }
        }
    }
}