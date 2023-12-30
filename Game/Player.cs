using System.Numerics;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Resources.Primitives;

namespace UniversityGameProject.Game;

public class Player : Node2D
{
    private Body _body;
    private CharacterCamera _camera;
    //public CharacterStats Stats = new CharacterStats();
    public PlayerStats _stats = new PlayerStats();

    public Camera2D Camera => _camera;

    public Player(string name, string path) : base(name)
    {
        _body = new Body("Player body", path);
        _body.MeshData = new RectanglePrimitiveTextured();
        _body.Transform.Scale = new Vector3(0.1f, 0.1f, 1.0f);

        _camera = new CharacterCamera("Main camera", _body);
        
        AddChild(_body, path);
        AddChild(_camera);
    }

    private sealed class Body : MeshInstance2D
    {
        public Body(string name, string path) : base(name) { }
    }

    private sealed class CharacterCamera : Camera2D
    {
        private Camera2D _camera;
        private MeshInstance2D _body;
        private float Speed { get; set; } = 1.0f;

        public CharacterCamera(string name, MeshInstance2D body) : base(name)
        {
            _body = body;
            _camera = new Camera2D("Camera");
            AddChild(_camera);
        }

        public override void Process(float delta)
        {
            base.Process(delta);
            
            Vector3 direction = Vector3.Zero;

            if (InputServer!.IsActionPressed("movement_forward"))
            {
                direction.Y -= 1.0f;
            }
            
            if (InputServer!.IsActionPressed("movement_backward"))
            {
                direction.Y += 1.0f;
            }
            
            if (InputServer!.IsActionPressed("movement_left"))
            {
                direction.X += 1.0f;
            }
            
            if (InputServer!.IsActionPressed("movement_right"))
            {
                direction.X -= 1.0f;
            }

            if (direction != Vector3.Zero)
            {
                direction = Vector3.Normalize(direction);
                direction = direction * delta * CharacterStats;
                Translate(direction);
                _body.Translate(direction);
            }
        }

        public override void Ready()
        {
            base.Ready();
        }
    }

    public class PlayerStats : Entity
    {
        public override float Speed => 0.5f;
        public override int HealthPool => 100;
    }
}