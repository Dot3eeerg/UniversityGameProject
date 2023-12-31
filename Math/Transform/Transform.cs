﻿using System.Numerics;

namespace UniversityGameProject.Math;

public class Transform
{
    public Vector3 Position { get; set; } = new Vector3(0, 0, 0);

    public Vector3 Scale { get; set; } = new Vector3(1.0f, 1.0f, 1.0f);

    public Quaternion Rotation { get; set; } = Quaternion.Identity;

    public Matrix4x4 ViewMatrix =>
        Matrix4x4.Identity *
        Matrix4x4.CreateScale(Scale) *
        Matrix4x4.CreateFromQuaternion(Rotation) *
        Matrix4x4.CreateTranslation(Position);

    public void Revert()
    {
        Position = Position with { X = 0, Y = 0, Z = 0 };
        Scale = Scale with { X = 1.0f, Y = 1.0f, Z = 1.0f };
        Rotation = Quaternion.Identity;
    }
} 