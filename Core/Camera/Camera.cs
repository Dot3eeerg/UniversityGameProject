using System.Numerics;
using UniversityGameProject.Math.Transform;

namespace UniversityGameProject.Core.Camera;

public class Camera
{
    public Transform Transform;
    public Vector2 ViewportSize { get; set; }

    private Vector3 _cameraPosition = new Vector3(0.0f, 0.0f, 1.0f);
    private Vector3 _cameraFront = new Vector3(0.0f, 0.0f, -1.0f);
    private Vector3 _cameraUp = Vector3.UnitY;
    private Vector3 _cameraDirection = Vector3.Zero;

    public Camera()
    {
        Transform = new Transform();
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
}