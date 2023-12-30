using System.Numerics;
using UniversityGameProject.Render.Camera;

using ReMath = UniversityGameProject.Math;

namespace UniversityGameProject.Main._2d;

public class Camera2D : Node2D, ICamera
{
    protected Vector3 _cameraPosition = new Vector3(0.0f, 0.0f, 1.0f);
    protected Vector3 _cameraFront = new Vector3(0.0f, 0.0f, -1.0f);
    protected Vector3 _cameraUp = Vector3.UnitY;
    protected Vector3 _cameraDirection = Vector3.Zero;

    public Camera2D(string name) : base(name)
    {
        Transform = new ReMath.Transform();
        Transform.Position = _cameraPosition;
    }
    
    public Vector3 CameraPosition
    {
        get => _cameraPosition;
        set
        {
            _cameraPosition = value;
            Transform.Position = _cameraPosition;
        }
    }

    public short VisualMask { get; set; } = 1;
    public float MinDistance { get; set; } = 0.001f;
    public float MaxDistance { get; set; } = 100.0f;
    public Vector3 Position => Transform.Position;

    public Matrix4x4 GetProjection(Vector2 viewportSize) =>
        //Matrix4x4.CreateOrthographicOffCenter( - viewportSize.X / 2.0f, viewportSize.X / 2.0f, -viewportSize.Y / 2.0f,
        //    viewportSize.Y / 2.0f, MinDistance, MaxDistance);
        //Matrix4x4.CreateOrthographic(1.0f, 2.0f * viewportSize.Y / viewportSize.X, MinDistance, MaxDistance);
        Matrix4x4.CreateOrthographic(1.0f, 1.0f, MinDistance, MaxDistance);
        //Matrix4x4.CreateOrthographicOffCenter(-viewportSize.X / 1920.0f, viewportSize.X / 1920.0f, -viewportSize.Y / 1080.0f,  viewportSize.Y / 1080.0f, MinDistance, MaxDistance);

    public override Matrix4x4 View => Matrix4x4.CreateLookAt(GlobalTransform.Position, GlobalTransform.Position + _cameraFront, _cameraUp);
    // Matrix4x4.CreateTranslation(-GlobalTransform.Position) *
    // Matrix4x4.CreateFromQuaternion(GlobalTransform.Rotation) *
    // Matrix4x4.CreateScale(GlobalTransform.Scale);

}