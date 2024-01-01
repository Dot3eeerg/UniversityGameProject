using System.Numerics;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Render.Material;
using UniversityGameProject.Resources;
using UniversityGameProject.Resources.Primitives;

namespace UniversityGameProject.Game;

public class Player : Node2D
{
    private MeshInstance2D _body;
    private CharacterCamera _camera;
    private Circle _collision = new Circle("Collision", 0.1f);
    
    public Entity PlayerStats = new Stats();
    public Camera2D Camera => _camera;
    public MeshInstance2D BodyData => _body;
    public Circle Circle => _collision;

    public Player(string name, string path) : base(name)
    {
        _body = new Body("Player body", path);
        _body.MeshData = new RectanglePrimitiveTextured();
        _body.MeshData.ApplyScale(0.02f, 0.08f);

        _camera = new CharacterCamera("Main camera");
        
        AddChild(_body, path, ShaderType.TextureShader);
        AddChild(_camera);
        AddChild(_collision);
    }

    public override void Process(float delta)
    {
        base.Process(delta);
        
        Vector3 direction = Vector3.Zero;

        if (InputServer!.IsActionPressed("movement_forward"))
        {
            direction.Y += 1.0f;
        }
        
        if (InputServer!.IsActionPressed("movement_backward"))
        {
            direction.Y -= 1.0f;
        }
        
        if (InputServer!.IsActionPressed("movement_left"))
        {
            direction.X -= 1.0f;
        }
        
        if (InputServer!.IsActionPressed("movement_right"))
        {
            direction.X += 1.0f;
        }

        if (direction != Vector3.Zero)
        {
            direction = Vector3.Normalize(direction);
            direction = direction * delta * PlayerStats.Speed;
            
            _camera.Translate(direction);
            _body.Translate(direction);
            _collision.Translate(direction);
            Translate(direction);
        }
    }

    public sealed class Body : MeshInstance2D
    {
        public Body(string name, string path) : base(name) { }
    }

    private sealed class CharacterCamera : Camera2D
    {
        private Camera2D _camera;

        public CharacterCamera(string name) : base(name)
        {
            _camera = new Camera2D("Camera");
            AddChild(_camera);
        }
    }

    private class Stats : Entity
    {
        public override float Speed { get; set; } = 0.3f;
        public override int HealthPool { get; set; } = 100;
    }
}