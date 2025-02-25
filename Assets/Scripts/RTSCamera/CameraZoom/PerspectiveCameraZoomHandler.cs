using UnityEngine;

public class PerspectiveCameraZoomHandler : ZoomHandler
{
    public PerspectiveCameraZoomHandler(Camera camera, ZoomProperties zoomProperties) : base(camera, zoomProperties)
    {
        CurrentZoom = Camera.fieldOfView;
    }

    public override void Zoom(float inputDelta)
    {
        base.Zoom(inputDelta);

        float newFieldOfView = Mathf.SmoothDamp(Camera.fieldOfView, CurrentZoom, ref Velocity, ZoomProperties.Smoothness);
        Camera.fieldOfView = newFieldOfView;
    }
}