public static class CameraUtils
{
    public static readonly float MinXRotation = 25f;
    public static readonly float MaxXRotation = 55f;
    public static readonly float Smoothness = 0.2f;
    public static readonly float PositiveMouse = 0.5f;
    public static readonly float NegativeMouse = -0.5f;
    public static readonly float BorderViolation = 0.1f;
    public static readonly float DividerForPerspectiveCameraZoom = 5f;
    public static readonly float PanBorderThickness = 50;
    public static readonly string Vertical = nameof(Vertical);
    public static readonly string Horizontal = nameof(Horizontal);
    public static readonly string MouseX = "Mouse X";
    public static readonly string MouseY = "Mouse Y";
    public static readonly string MouseScrollWheel = "Mouse ScrollWheel";

    public static void SetDefaultForPerspectiveCamera(ZoomProperties properties)
    {
        properties.ZoomSpeed = 30f;
        properties.ZoomMin = 10f;
        properties.ZoomMax = 50f;
        properties.Smoothness = Smoothness;
    }

    public static void SetDefaultForOrthographicCamera(ZoomProperties properties)
    {
        properties.ZoomSpeed = 10f;
        properties.ZoomMin = 2f;
        properties.ZoomMax = 10f;
        properties.Smoothness = Smoothness;
    }
}