using System;
using UnityEngine;
using Zenject;

public class BuildInput : PauseableObject
{
    [Inject] private Pause _pause;

    private Camera _camera;

    public event Action<Ray> RayChanged;
    public event Action IsRotateToLeft;
    public event Action IsRotateToRight;
    public event Action Placed;
    public event Action Canceled;

    private Ray ScreenPoitToRay => _camera.ScreenPointToRay(Input.mousePosition);
    private bool IsLeftRotation => Input.GetKey(KeyCode.Q);
    private bool IsRightRotation => Input.GetKey(KeyCode.E);
    private bool IsPlaceFounded => Input.GetMouseButtonDown(0);
    private bool IsCanceled => Input.GetMouseButtonDown(1);

    private void Awake()
    {
        _camera = Camera.main;
        _pause.Register(this);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (IsPaused == false && isActiveAndEnabled)
        {
            ReadInput();
        }
    }

    private void ReadInput()
    {
        RayChanged?.Invoke(ScreenPoitToRay);

        if (IsLeftRotation)
            IsRotateToLeft?.Invoke();
       
        if (IsRightRotation)
            IsRotateToRight?.Invoke();

        if (IsPlaceFounded)
            Placed?.Invoke();

        if (IsCanceled)
            Canceled?.Invoke();
    }
}