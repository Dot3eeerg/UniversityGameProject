using System.Numerics;

namespace UniversityGameProject.Render.Camera;

public interface ICamera
{
    public short VisualMask { get; set; }
    public float MinDistance { get; set; }
    public float MaxDistance { get; set; }
    //public float Fov { get; set; }
    public Matrix4x4 View { get; }
    public Vector3 Position { get; }
    public Matrix4x4 GetProjection(Vector2 viewportSize);
}