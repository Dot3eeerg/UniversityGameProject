using System.Media;
using System.Windows.Media;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Resources.Primitives;
using Timer = UniversityGameProject.Main.Timer.Timer;

namespace UniversityGameProject.Game;

public class Weapon : Node2D
{
    private MeshInstance2D _body;
    private Rectangle _collision = new Rectangle("Collision", 0.03f, 0.01f);
    private Timer _attack;
    private Timer _cooldown;

    private MediaPlayer _mediaPlayer = new MediaPlayer();

    public Stats WeaponStats = new Stats();
    public Rectangle Rectangle => _collision;
    public bool IsActive = false;
    
    public Weapon(string name, string path) : base(name)
    {
        _body = new Body("Weapon body", path);
        _body.MeshData = new RectanglePrimitiveTextured();
        _body.MeshData.ApplyScale(0.03f, 0.01f);
        _body.CanRender = false;
        _attack = new Timer("Alive timer");
        _cooldown = new Timer("Cooldown");

        _cooldown.Start();
        AddChild(_body);
    }

    public override void Process(float delta)
    {
        base.Process(delta);

        if (IsActive)
        {
            HandleCooldown(delta);
        }
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
            _body.Transform.Scale = _body.Transform.Scale with { X = _body.Transform.Scale.X + delta * 12, Y = _body.Transform.Scale.Y + delta * 10 };
            _collision.Transform.Scale = _body.Transform.Scale with { X = _body.Transform.Scale.X + delta * 12, Y = _body.Transform.Scale.Y + delta * 10 };
        }

        if (_cooldown.Time > WeaponStats.TimeCooldown)
        {

            _mediaPlayer.Open(new Uri(Path.GetFullPath("Sounds/whip.wav")));
            _mediaPlayer.Play();
            _body.CanRender = true;
            _cooldown.Stop();
            _cooldown.Reset();
            _attack.Start();
            _body.Transform.Scale = _body.Transform.Scale with { X = 1.0f, Y = 1.0f };
            
            //Console.WriteLine("Start attack");
        }

        else if (_attack.Time > WeaponStats.TimeAttack)
        {
            _body.CanRender = false;
            _attack.Stop();
            _attack.Reset();
            _cooldown.Start();
            
            //Console.WriteLine("Stop attack");
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

    public void SetPosition(WeaponPositionType type)
    {
        switch (type)
        {
            case WeaponPositionType.Right:
                Translate(0.0625f, 0.0f, 0.0f);
                _collision.Translate(0.0625f, 0.0f, 0.0f);
                break;
            case WeaponPositionType.Left:
                Translate(-0.0625f, 0.0f, 0.0f);
                _collision.Translate(-0.0625f, 0.0f, 0.0f);
                break;
            case WeaponPositionType.Top:
                Translate(0.0f, 0.0625f, 0.0f);
                _collision.Translate(0.0f, 0.0625f, 0.0f);
                break;
            case WeaponPositionType.Bottom:
                Translate(0.0f, -0.0625f, 0.0f);
                _collision.Translate(0.0f, -0.0625f, 0.0f);
                break;
            default:
                break;
        }
    }

    public sealed class Body : MeshInstance2D
    {
        public Body(string name, string path) : base(name) { }
    }

    public sealed class Stats
    {
        public int Damage { get; set; } = 5;
        public long TimeAttack { get; set; } = 250;
        public long TimeCooldown { get; set; } = 1500;
    }

    public enum WeaponPositionType
    {
        Center = 0, Right, Left, Top, Bottom
    }
}