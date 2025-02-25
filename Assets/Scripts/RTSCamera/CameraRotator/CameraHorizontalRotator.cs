using System;
using UnityEngine;

public class CameraHorizontalRotator : CameraRotator
{
    private float _yRotation;

    public event Action<Quaternion> RotationChanged;
    
    private protected override void HorizontalRotate()
    {
        _yRotation += HorizontalDirection * RotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _yRotation, 0), CameraUtils.Smoothness);
        RotationChanged?.Invoke(transform.rotation);
    }
}