using System.Numerics;

namespace UniversityGameProject.Math.Transform;

public record struct Transform
{
    public Vector3 Position { get; set; } = Vector3.Zero;

    public Vector3 Scale { get; set; } = Vector3.One;

    public Quaternion Rotation { get; set; } = Quaternion.Identity;
    
    public Transform () { }

    public Matrix4x4 ViewMatrix =>
        Matrix4x4.Identity * Matrix4x4.CreateScale(Scale) *
        Matrix4x4.CreateFromQuaternion(Rotation) * Matrix4x4.CreateTranslation(Position);
}