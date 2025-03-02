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

    private bool IsStartRotate => Input.GetMouseButton(2);
    private float ScrollWheel => Input.GetAxis(CameraUtils.MouseScrollWheel);
    private float HorizontalDirection => Input.GetAxis(CameraUtils.Horizontal);
    private float VerticalDirection => Input.GetAxis(CameraUtils.Vertical);
    private float HorizontalRotation => Input.GetAxis(CameraUtils.MouseX);
    private float VerticalRotation => Input.GetAxis(CameraUtils.MouseY);
    private float MousePositionX => Input.mousePosition.x;
    private float MousePositionY => Input.mousePosition.y;

    private void Update()
    {
        if (IsPaused == false)
        {
            ReadKeyboardAxis();
            ReadMousePosition();
            ReadZoomAxis();

            if (IsStartRotate)
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
        Vector3 direction = new Vector3(HorizontalDirection, VerticalDirection, 0) * _cameraZoomInput.GetCurrentZoom();
        DirectionChanged?.Invoke(direction);
    }

    private void ReadHorizontalRotation()
    {
        float horizontalRotation = HorizontalRotation * _cameraZoomInput.GetCurrentZoom();
        HorizontalRotationChanged?.Invoke(horizontalRotation);
    }

    private void ReadVerticalRotation()
    {
        float verticalRotation = VerticalRotation * _cameraZoomInput.GetCurrentZoom();
        VerticalRotationChanged?.Invoke(verticalRotation);
    }

    private void ReadZoomAxis()
    {
        ZoomChanged?.Invoke(ScrollWheel);
    }

    private void ReadMousePosition()
    {
        if (MousePositionY >= Screen.height - CameraUtils.PanBorderThickness)
            MoveByMouseVertical(CameraUtils.PositiveMouse);

        if (MousePositionY <= CameraUtils.PanBorderThickness)
            MoveByMouseVertical(CameraUtils.NegativeMouse);

        if (MousePositionX >= Screen.width - CameraUtils.PanBorderThickness)
            MoveByMouseHorizontal(CameraUtils.PositiveMouse);

        if(MousePositionX <= CameraUtils.PanBorderThickness)
            MoveByMouseHorizontal(CameraUtils.NegativeMouse);

        if (MousePositionY >= Screen.height - CameraUtils.PanBorderThickness && MousePositionX >= Screen.width - CameraUtils.PanBorderThickness)
            MoveByMouseDiagonal(CameraUtils.PositiveMouse, CameraUtils.PositiveMouse);

        if (MousePositionY >= Screen.height - CameraUtils.PanBorderThickness && MousePositionX <= CameraUtils.PanBorderThickness)
            MoveByMouseDiagonal(CameraUtils.NegativeMouse, CameraUtils.PositiveMouse);

        if (MousePositionY <= CameraUtils.PanBorderThickness && MousePositionX >= Screen.width - CameraUtils.PanBorderThickness)
            MoveByMouseDiagonal(CameraUtils.PositiveMouse, CameraUtils.NegativeMouse);

        if (MousePositionY <= CameraUtils.PanBorderThickness && MousePositionX <= CameraUtils.PanBorderThickness)
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