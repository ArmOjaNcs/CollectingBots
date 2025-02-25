using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float _smoothing = 10f;

    private void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 nextPosition = Vector3.Lerp(transform.position, _targetTransform.position, Time.deltaTime * _smoothing);
        transform.position = nextPosition;
    }
}