using System.Numerics;
using UniversityGameProject.Render.Camera;
using UniversityGameProject.Window;

namespace UniversityGameProject.Render.Viewport;

public record struct Viewport
{
    private WindowServer _windowServer;
    public ICamera Camera { get; set; }

    public Viewport(WindowServer windowServer, ICamera camera)
    {
        _windowServer = windowServer;
        Camera = camera;
    }
    
    internal Matrix4x4 GetProjection()
    {
        return Camera.GetProjection(_windowServer.WindowSize);
    }

    public float GetRatioXY()
    {
        return _windowServer.WindowSize.X / _windowServer.WindowSize.Y;
    }

    public float GetRatioYX()
    {
        return _windowServer.WindowSize.Y / _windowServer.WindowSize.X;
    }
}