using UniversityGameProject.Main._2d;
using UniversityGameProject.Resources.Primitives;
using Timer = UniversityGameProject.Main.Timer.Timer;

namespace UniversityGameProject.Game;

public class Weapon : Node2D
{
    private MeshInstance2D _body;
    private Rectangle _collision = new Rectangle("Collision", 0.1f, 0.04f);
    private Timer _attack;
    private Timer _cooldown;

    public Stats WeaponStats = new Stats();
    
    public Weapon(string name, string path1, string path2) : base(name)
    {
        _body = new Body("Weapon body", path1);
        _body.MeshData = new RectanglePrimitiveTextured();
        _body.MeshData.ApplyScale(0.08f, 0.04f);
        _body.CanRender = false;

        _attack = new Timer("Alive timer");
        _cooldown = new Timer("Cooldown");
        
        _cooldown.Start();
        AddChild(_body);
    }

    public override void Process(float delta)
    {
        base.Process(delta);

        HandleCooldown(delta);
    }

    private void HandleCooldown(float delta)
    {
        if (_cooldown.IsActive())
        {
            _cooldown.Update((long) (delta * 1000));
        }
        else
        {
            _attack.Update((long) (delta * 1000));
        }

        if (_cooldown.Time > WeaponStats.TimeCooldown)
        {
            _body.CanRender = true;
            _cooldown.Stop();
            _cooldown.Reset();
            _attack.Start();
            
            Console.WriteLine("Start attack");
        }

        else if (_attack.Time > WeaponStats.TimeAttack)
        {
            _body.CanRender = false;
            _attack.Stop();
            _attack.Reset();
            _cooldown.Start();
            
            Console.WriteLine("Stop attack");
        }
    }

    public bool IsAttacking()
    {
        if (_attack.IsActive())
        {
            return true;
        }

        return false;
    }
    
    public sealed class Body : MeshInstance2D
    {
        public Body(string name, string path) : base(name) { }
    }

    public sealed class Stats
    {
        public int Damage { get; set; } = 5;
        public long TimeAttack { get; set; } = 5000;
        public long TimeCooldown { get; set; } = 1000;
    }
}