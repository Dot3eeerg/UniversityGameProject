using UniversityGameProject.Core.Window;

namespace UniversityGameProject.Core.Viewport;

public class Viewport
{
    private WindowServer _windowServer;
    private Camera.Camera _camera;

    public Viewport(WindowServer windowServer, Camera.Camera camera)
    {
        _windowServer = windowServer;
        _camera = camera;
    }

    public float ViewportRatioXY
        => _camera.ViewportSize.X / _camera.ViewportSize.Y;

    public float ViewportRatioYX
        => _camera.ViewportSize.Y / _camera.ViewportSize.X;
}