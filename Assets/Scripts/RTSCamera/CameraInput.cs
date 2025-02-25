using System;
using UnityEngine;

public class CameraInput : PauseableObject
{
    [SerializeField] private CameraZoom _cameraZoomInput;

    public event Action<Vector3> DirectionChanged;
    public event Action<bool> IsRotate;
    public event Action<float> HorizontalRotationChanged;
    public event Action<float> VerticalRotationChanged;
    public event Action<float> ZoomChanged;

    private bool _isRotate => Input.GetMouseButton(2);
    private float _scrollWheel => Input.GetAxis(CameraUtils.MouseScrollWheel);
    private float _horizontalDirection => Input.GetAxis(CameraUtils.Horizontal);
    private float _verticalDirection => Input.GetAxis(CameraUtils.Vertical);
    private float _horizontalRotation => Input.GetAxis(CameraUtils.MouseX);
    private float _verticalRotation => Input.GetAxis(CameraUtils.MouseY);
    private float _mousePositionX => Input.mousePosition.x;
    private float _mousePositionY => Input.mousePosition.y;

    private void Update()
    {
        if (IsPaused == false)
        {
            ReadKeyboardAxis();
            ReadMousePosition();
            ReadZoomAxis();

            if (_isRotate)
            {
                IsRotate?.Invoke(true);
                ReadHorizontalRotation();
                ReadVerticalRotation();
            }
            else
            {
                IsRotate?.Invoke(false);
            }
        }
    }

    private void ReadKeyboardAxis()
    {
        Vector3 direction = new Vector3(_horizontalDirection, _verticalDirection, 0) * _cameraZoomInput.GetCurrentZoom();
        DirectionChanged?.Invoke(direction);
    }

    private void ReadHorizontalRotation()
    {
        float horizontalRotation = _horizontalRotation * _cameraZoomInput.GetCurrentZoom();
        HorizontalRotationChanged?.Invoke(horizontalRotation);
    }

    private void ReadVerticalRotation()
    {
        float verticalRotation = _verticalRotation * _cameraZoomInput.GetCurrentZoom();
        VerticalRotationChanged?.Invoke(verticalRotation);
    }

    private void ReadZoomAxis()
    {
        ZoomChanged?.Invoke(_scrollWheel);
    }

    private void ReadMousePosition()
    {
        if (_mousePositionY >= Screen.height - CameraUtils.PanBorderThickness)
            MoveByMouseVertical(CameraUtils.PositiveMouse);

        if (_mousePositionY <= CameraUtils.PanBorderThickness)
            MoveByMouseVertical(CameraUtils.NegativeMouse);

        if (_mousePositionX >= Screen.width - CameraUtils.PanBorderThickness)
            MoveByMouseHorizontal(CameraUtils.PositiveMouse);

        if(_mousePositionX <= CameraUtils.PanBorderThickness)
            MoveByMouseHorizontal(CameraUtils.NegativeMouse);

        if (_mousePositionY >= Screen.height - CameraUtils.PanBorderThickness && _mousePositionX >= Screen.width - CameraUtils.PanBorderThickness)
            MoveByMouseDiagonal(CameraUtils.PositiveMouse, CameraUtils.PositiveMouse);

        if (_mousePositionY >= Screen.height - CameraUtils.PanBorderThickness && _mousePositionX <= CameraUtils.PanBorderThickness)
            MoveByMouseDiagonal(CameraUtils.NegativeMouse, CameraUtils.PositiveMouse);

        if (_mousePositionY <= CameraUtils.PanBorderThickness && _mousePositionX >= Screen.width - CameraUtils.PanBorderThickness)
            MoveByMouseDiagonal(CameraUtils.PositiveMouse, CameraUtils.NegativeMouse);

        if (_mousePositionY <= CameraUtils.PanBorderThickness && _mousePositionX <= CameraUtils.PanBorderThickness)
            MoveByMouseDiagonal(CameraUtils.NegativeMouse, CameraUtils.NegativeMouse);
    }

    private void MoveByMouseVertical(float mouse)
    {
        Vector3 direction = new Vector3(0, mouse, 0) * _cameraZoomInput.GetCurrentZoom();
        DirectionChanged?.Invoke(direction);
    }

    private void MoveByMouseHorizontal(float mouse)
    {
        Vector3 direction = new Vector3(mouse, 0, 0) * _cameraZoomInput.GetCurrentZoom();
        DirectionChanged?.Invoke(direction);
    }

    private void MoveByMouseDiagonal(float mouseX, float mouseY)
    {
        Vector3 direction = new Vector3(mouseX, mouseY, 0) * _cameraZoomInput.GetCurrentZoom();
        DirectionChanged?.Invoke(direction);
    }
}