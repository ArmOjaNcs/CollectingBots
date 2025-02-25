using UnityEngine;

public class OrthographicCameraZoomHandler : ZoomHandler
{
    public OrthographicCameraZoomHandler(Camera camera, ZoomProperties zoomProperties) : base(camera, zoomProperties)
    {
        CurrentZoom = Camera.orthographicSize;
    }

    public override void Zoom(float inputDelta)
    {
        base.Zoom(inputDelta);

        float newOrthoSize = Mathf.SmoothDamp(Camera.orthographicSize, CurrentZoom, ref Velocity, ZoomProperties.Smoothness);
        Camera.orthographicSize = newOrthoSize;
    }
}