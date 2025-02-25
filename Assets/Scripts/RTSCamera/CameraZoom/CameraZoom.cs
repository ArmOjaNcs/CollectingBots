using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : PauseableObject
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraInput _input;
    [SerializeField] private ZoomProperties _zoomProperties;
    [SerializeField] private Toggle _orthographicToggle;

    private IZoomable _orthographic;
    private IZoomable _perspective;
    private float _currentZoom;
    private bool _isOrthographic;

    private protected override void Awake()
    {
        base.Awake();
        _orthographic = new OrthographicCameraZoomHandler(_camera, _zoomProperties);
        _perspective = new PerspectiveCameraZoomHandler(_camera, _zoomProperties);
        _orthographicToggle.onValueChanged.AddListener(ChangeCameraProjection);
        _isOrthographic = true;
        CameraUtils.SetDefaultForOrthographicCamera(_zoomProperties);
    }

    private void OnEnable()
    {
        _input.ZoomChanged += OnZoomChanged;
    }

    private void OnDisable()
    {
        _input.ZoomChanged -= OnZoomChanged;
        _orthographicToggle.onValueChanged.RemoveListener(ChangeCameraProjection);
    }

    private void LateUpdate()
    {
        Zoom();
    }

    public float GetCurrentZoom()
    {
        if(_isOrthographic)
            return _orthographic.GetCurrentZoom();
        else
            return _perspective.GetCurrentZoom() / CameraUtils.DividerForPerspectiveCameraZoom;
    }

    private void Zoom()
    {
        if(IsPaused == false)
        {
            if (_isOrthographic)
                _orthographic.Zoom(_currentZoom);
            else
                _perspective.Zoom(_currentZoom);
        }
    }

    private void ChangeCameraProjection(bool isOn)
    {
        if (isOn)
        {
            _isOrthographic = isOn;
            _camera.orthographic = isOn;
            CameraUtils.SetDefaultForOrthographicCamera(_zoomProperties);
        }
        else
        {
            _isOrthographic = isOn;
            _camera.orthographic = isOn;
            CameraUtils.SetDefaultForPerspectiveCamera(_zoomProperties);
        }
    }

    private void OnZoomChanged(float zoom)
    {
        _currentZoom = zoom;
    }
}