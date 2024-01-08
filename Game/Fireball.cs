using System.Media;
using System.Numerics;
using System.Windows.Media;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Resources.Primitives;
using Timer = UniversityGameProject.Main.Timer.Timer;

namespace UniversityGameProject.Game;

public class Fireball : Node2D
{
    private MeshInstance2D _body;
    private Circle _collision = new Circle( "Fireball", 0.07f);
    private Timer _attack;
    private Timer _cooldown;
    private int _numPierced = 0;
    private MeshInstance2D _playerPosition;
    private Vector3 _direction;

    private MediaPlayer _mediaPlayer = new MediaPlayer();

    public Stats WeaponStats = new Stats();
    public Circle Circle => _collision;
    public bool DirectionPicked = false;
    
    public Fireball(string name, string path, MeshInstance2D playerPosition) : base(name)
    {

        _body = new Body(name, path);
        _body.MeshData = new RectanglePrimitiveTextured();
        _body.MeshData.ApplyScale(0.08f, 0.1f);
        _body.CanRender = false;


        _playerPosition = playerPosition;

        _attack = new Timer("Alive timer");
        _cooldown = new Timer("Cooldown");
        
        _cooldown.Start();
        AddChild(_body);
        AddChild(_collision);
    }

    public override void Process(float delta)
    {
        base.Process(delta);
        
        HandleCooldown(delta);
        
        if (DirectionPicked && IsAttacking())
        {
            CastHandle(delta);
        }
    }

    private void CastHandle(float delta)
    {
        Translate(_direction * delta);
    }

    public void GiveDirection(Vector3 position)
    {
        _direction = Vector3.Normalize(position - GlobalTransform.Position) * WeaponStats.Speed;
        DirectionPicked = true;
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

            _mediaPlayer.Open(new Uri(Path.GetFullPath("Sounds/fireball.wav")));
            _mediaPlayer.Play();
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
        SetPosition();
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
        DirectionPicked = false;
        _numPierced = 0;
    }

    public bool IsAttacking()
    {
        if (_attack.IsActive())
        {
            return true;
        }

        return false;
    }

    private void SetPosition()
    {
        SetTransform(_playerPosition.GlobalTransform.Position);
    }
    
    public sealed class Body : MeshInstance2D
    {
        public Body(string name, string path) : base(name) { }
    }
    
    public sealed class Stats
    {
        public int Damage { get; set; } = 5;
        public float Speed { get; set; } = 0.6f;
        public long TimeAttack { get; set; } = 1500;
        public long TimeCooldown { get; set; } = 1000;
        public int Pierce { get; set; } = 3;
    }
}