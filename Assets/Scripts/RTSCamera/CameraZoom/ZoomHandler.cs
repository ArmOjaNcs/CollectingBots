using UnityEngine;

public abstract class ZoomHandler : IZoomable
{
    private protected readonly Camera Camera;
    private protected readonly ZoomProperties ZoomProperties;

    private protected float CurrentZoom;
    private protected float Velocity;

    public ZoomHandler(Camera camera, ZoomProperties zoomProperties)
    {
        Camera = camera;
        ZoomProperties = zoomProperties;
    }

    public virtual void Zoom(float inputDelta)
    {
        float inputDeltaWithSpeed = inputDelta * ZoomProperties.ZoomSpeed;
        CurrentZoom = Mathf.Clamp(CurrentZoom - inputDeltaWithSpeed, ZoomProperties.ZoomMin, ZoomProperties.ZoomMax);
    }

    public float GetCurrentZoom()
    {
        return CurrentZoom;
    }
}