using UnityEngine;

public class CameraVerticalRotator : CameraRotator
{
    float _xRotation;

    private void Start()
    {
        _xRotation = Camera.main.transform.localRotation.x;
    }

    private protected override void VerticalRotate()
    {
        _xRotation -= VerticalDirection * RotationSpeed * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, CameraUtils.MinXRotation, CameraUtils.MaxXRotation);
        Quaternion newRotation = Quaternion.Euler(_xRotation, 0, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, newRotation, CameraUtils.Smoothness);
    }
}