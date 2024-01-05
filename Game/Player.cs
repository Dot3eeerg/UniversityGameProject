using System.Numerics;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Render.Material;
using UniversityGameProject.Resources;
using UniversityGameProject.Resources.Primitives;
using Timer = UniversityGameProject.Main.Timer.Timer;

namespace UniversityGameProject.Game;

public class Player : Node2D
{
    private MeshInstance2D _body;
    private CharacterCamera _camera;
    private Circle _collision = new Circle("Collision", 0.02f);
    private HitTimer _hitTime;
    private List<Weapon> _whip;
    
    public EntityPlayer PlayerStats = new Stats();
    public Camera2D Camera => _camera;
    public MeshInstance2D BodyData => _body;
    public Circle Circle => _collision;

    public Player(string name, string path, List<Weapon> weapon) : base(name)
    {
        _body = new Body("Player body", path);
        _body.MeshData = new RectanglePrimitiveTextured();
        _body.MeshData.ApplyScale(0.02f, 0.08f);
        //_body.Transform.Scale = new Vector3(Transform.Scale.X, Transform.Scale.Y + 1.0f, Transform.Scale.Z);

        _whip = weapon;
        for (int i = 0; i < weapon.Count; i++)
            _whip[i].SetPosition((Weapon.WeaponPositionType)(i % 4 + 1));

        _camera = new CharacterCamera("Main camera");

        _hitTime = new HitTimer("Hit timer");
        
        AddChild(_body, path, ShaderType.TextureShader);
        AddChild(_camera);
        AddChild(_collision);
        AddChild(_hitTime);
    }

    public override void Process(float delta)
    {
        base.Process(delta);
        
        _hitTime.Update((long) (delta * 1000));

        if (!IsAlive())
        {
            return;
        }
        
        InputHandle(delta);
    }

    private void InputHandle(float delta)
    {
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
            
            // You don't need Translate main node, idk why but it works that way
            // Translate(direction)
            _camera.Translate(direction);
            _body.Translate(direction);
            _collision.Translate(direction);


            for (int whipID = 0; whipID < _whip.Count; whipID++)
            {
                
                if (_whip[whipID].CanRender)
                {
                    _whip[whipID].Translate(direction);
                    _whip[whipID].Rectangle.Translate(direction);
                }
            }
        }
    }

    public void InflictDamage(int damage)
    {
        if (!IsInvul())
        {
            PlayerStats.CurrentHealth -= damage;
            Console.WriteLine("Damage taken");
            
            if (PlayerStats.CurrentHealth <= 0)
            {
                Console.WriteLine("Player is dead");
            }
            
            _hitTime.Start();
        }
    }

    private bool IsInvul()
    {
        if (_hitTime.Time < PlayerStats.InvulTime && _hitTime.IsActive())
        {
            return true;
        }
        
        return false;
    }

    public bool IsAlive()
    {
        if (PlayerStats.CurrentHealth <= 0)
        {
            return false;
        }

        return true;
    }
    
    public sealed class Body : MeshInstance2D
    {
        public Body(string name, string path) : base(name) { }
    }
    
    private sealed class HitTimer : Timer 
    {
        public HitTimer(string name) : base(name) { }
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

    private class Stats : EntityPlayer
    {
        public override float Speed { get; set; } = 0.3f;
        public override int MaxHealth { get; set; } = 100;
        public override int CurrentHealth { get; set; } = 100;
        public override long InvulTime { get; set; } = 2000;
    }
}