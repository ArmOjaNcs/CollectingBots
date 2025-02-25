using UnityEngine;
using UnityEngine.UI;

public abstract class CameraRotator : PauseableObject
{
    [SerializeField] private CameraInput _input;
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private protected float RotationSpeed;

    private bool _isRotate;
    private protected float HorizontalDirection;
    private protected float VerticalDirection;

    private void Start()
    {
        _speedSlider.value = RotationSpeed;
    }

    private void OnEnable()
    {
        _input.IsRotate += SetRotate;
        _input.HorizontalRotationChanged += SetHorizontalDirection;
        _input.VerticalRotationChanged += SetVerticalDirection;
        _speedSlider.onValueChanged.AddListener(SetSpeed);
    }

    private void OnDisable()
    {
        _input.IsRotate -= SetRotate;
        _input.HorizontalRotationChanged -= SetHorizontalDirection;
        _input.VerticalRotationChanged -= SetVerticalDirection;
        _speedSlider.onValueChanged.RemoveListener(SetSpeed);
    }

    private void LateUpdate()
    {
        if(IsPaused == false)
        {
            if (_isRotate)
            {
                HorizontalRotate();
                VerticalRotate();
            }
        }
    }

    private void SetHorizontalDirection(float direction)
    {
        HorizontalDirection = direction;
    }

    private void SetVerticalDirection(float direction)
    {
        VerticalDirection = direction;
    }

    private void SetRotate(bool isRotate)
    {
        _isRotate = isRotate;
    }

    private void SetSpeed(float speed)
    {
        RotationSpeed = speed;
    }

    private protected virtual void VerticalRotate() { }

    private protected virtual void HorizontalRotate() { }
}