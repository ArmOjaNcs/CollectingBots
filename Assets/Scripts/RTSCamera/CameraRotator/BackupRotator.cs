using UnityEngine;

public class BackupRotator : MonoBehaviour
{
    [SerializeField] private CameraHorizontalRotator _cameraRotator;
    
    private void OnEnable()
    {
        _cameraRotator.RotationChanged += OnRotationChanged;
    }

    private void OnDisable()
    {
        _cameraRotator.RotationChanged -= OnRotationChanged;
    }

    private void OnRotationChanged(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}