using UniversityGameProject.Main._2d;
using UniversityGameProject.Resources.Primitives;
using Timer = UniversityGameProject.Main.Timer.Timer;

namespace UniversityGameProject.Game;

public class Fireball : Node2D
{
    private MeshInstance2D _body;
    private Circle _collision = new Circle( "Fireball", 0.02f);
    private Timer _attack;
    private Timer _cooldown;
    private int _numPierced = 0;
    private MeshInstance2D _playerPostion;

    public Stats WeaponStats = new Stats();
    public Circle Circle => _collision;
    
    public Fireball(string name, string path, MeshInstance2D playerPosition) : base(name)
    {
        _body = new Body(name, path);
        _body.MeshData = new RectanglePrimitiveTextured();
        _body.MeshData.ApplyScale(0.02f, 0.02f);
        _body.CanRender = false;

        _playerPostion = playerPosition;

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
            StartAttack();
        }
        
        else if (_attack.Time > WeaponStats.TimeAttack)
        {
            StartCooldown();
        }
    }

    public void PiercedEnemy()
    {
        _numPierced++;
        if (_numPierced >= WeaponStats.Pierce)
        {
            StartCooldown();
        }
    }

    private void StartAttack()
    {
        _body.CanRender = true;
        _cooldown.Stop();
        _cooldown.Reset();
        _attack.Start();
    }

    private void StartCooldown()
    {
        _body.CanRender = false;
        _attack.Stop();
        _attack.Reset();
        _cooldown.Start();
    }

    public bool IsAttacking()
    {
        if (_attack.IsActive())
        {
            return true;
        }

        return false;
    }

    public void SetPosition()
    {
        Translate(_playerPostion.GlobalTransform.Position);
        _collision.Translate(_playerPostion.GlobalTransform.Position);
    }
    
    public sealed class Body : MeshInstance2D
    {
        public Body(string name, string path) : base(name) { }
    }
    
    public sealed class Stats
    {
        public int Damage { get; set; } = 5;
        public float Speed { get; set; } = 0.6f;
        public long TimeAttack { get; set; } = 5000;
        public long TimeCooldown { get; set; } = 3000;
        public int Pierce { get; set; } = 3;
    }
}