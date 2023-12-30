using System.Numerics;
using UniversityGameProject.Main._2d;

namespace UniversityGameProject.Game;

public class Player : Node2D
{
    private CharacterCamera _camera = new CharacterCamera("Main camera");

    public Camera2D Camera => _camera;

    public Player(string name) : base(name)
    {
        AddChild(_camera);
    }

    public sealed class CharacterCamera : Camera2D
    {
        private Camera2D _camera;
        public float Speed { get; private set; } = 20.0f;

        public CharacterCamera(string name) : base(name)
        {
            _camera = new Camera2D("Camera");
            AddChild(_camera);
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
                Translate(direction * delta * Speed);
            }
        }

        public override void Ready()
        {
            base.Ready();
        }
        
        
    }
    
}