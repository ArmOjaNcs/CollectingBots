using UnityEngine;
using UnityEngine.UI;

public class CameraMover : CameraPauseableComponent
{
    [SerializeField] private CameraInput _input;
    [SerializeField] private CameraHorizontalRotator _horizontalRotator;
    [SerializeField] private float _speed;
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private Ground _ground;

    private Vector3 _inputDelta;

    private void Start()
    {
        _speedSlider.value = _speed;
    }

    private void OnEnable()
    {
        _input.DirectionChanged += OnDirectionChanged;
        _horizontalRotator.RotationChanged += OnRotationChanged;
        _speedSlider.onValueChanged.AddListener(SetSpeed);
    }

    private void OnDisable()
    {
        _input.DirectionChanged -= OnDirectionChanged;
        _horizontalRotator.RotationChanged -= OnRotationChanged;
        _speedSlider.onValueChanged.RemoveListener(SetSpeed);
    }

    private void Update()
    {
        if (IsPaused == false)
            Move(CorrectDirection(_inputDelta));
    }

    private void OnDirectionChanged(Vector3 direction)
    {
        _inputDelta = direction;
    }

    private void OnRotationChanged(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    private void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private Vector3 CorrectDirection(Vector3 inputDelta)
    {
        if (transform.position.x <= _ground.MinXPosition)
        {
            transform.position = new Vector3(_ground.MinXPosition + CameraUtils.BorderViolation, 
                transform.position.y, transform.position.z);
            return Vector3.zero;
        }

        if (transform.position.z <= _ground.MinZPosition)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 
                _ground.MinZPosition + CameraUtils.BorderViolation);
            return Vector3.zero;
        }

        if (transform.position.x >= _ground.MaxXPosition)
        {
            transform.position = new Vector3(_ground.MaxXPosition - CameraUtils.BorderViolation, 
                transform.position.y, transform.position.z);
            return Vector3.zero;
        }

        if (transform.position.z >= _ground.MaxZPosition)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,
                _ground.MaxZPosition - CameraUtils.BorderViolation);
            return Vector3.zero;
        }

        return inputDelta;
    }

    private void Move(Vector3 inputDelta)
    {
        Vector3 direction = new Vector3(inputDelta.x, 0, inputDelta.y);
        transform.Translate(direction * _speed * Time.deltaTime);
    }
}