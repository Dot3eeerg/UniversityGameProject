using System.Numerics;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Render.Camera;
using UniversityGameProject.Render.Material;
using UniversityGameProject.Resources;
using UniversityGameProject.Resources.Primitives;

namespace UniversityGameProject.Game;

public abstract class Enemy : Node2D
{
    private Body _body;
    private ICamera _camera;
    private MeshInstance2D _playerPosition;
    private CollisionShape _collision = new Circle("Collision", 0.04f);

    public CollisionShape Circle => _collision;

    internal abstract EntityEnemy EnemyStats { get; set; }
    
    public Enemy(string name, MeshInstance2D playerPosition) : base(name)
    {
        _body = new Body("Enemy body");
        _body.MeshData = new RectanglePrimitiveTextured();
        _body.MeshData.ApplyScale(0.04f, 0.08f);

        _playerPosition = playerPosition;
        
        AddChild(_body, TexturePath, ShaderType.TextureShader);
        AddChild(_collision);
    }

    public override void Process(float delta)
    {
        base.Process(delta);
        ChasePlayer(delta);
    }

    public abstract string TexturePath { get; }

    private void ChasePlayer(float delta)
    {
        Vector3 kek = new Vector3(_playerPosition.GlobalTransform.Position.X, _playerPosition.GlobalTransform.Position.Y, 0);
        Vector3 direction = Vector3.Normalize(kek - _body.GlobalTransform.Position) * EnemyStats.Speed * delta;
        
        Translate(direction);
        _collision.GlobalTransform.Position = GlobalTransform.Position;
    }

    public void InflictDamage(int damage)
    {
        EnemyStats.CurrentHealth -= damage;

        if (EnemyStats.CurrentHealth <= 0)
        {
            Console.WriteLine("Enemy is dead");
        }
    }

    public void DoScale()
    {
        _body.MeshData.ApplyScale(0.04f, 0.08f);
    }
        
    private sealed class Body : MeshInstance2D
    {
        public Body(string name) : base(name) { }
    }

}

public class SlimeEnemy : Enemy
{
    public SlimeEnemy(string name, MeshInstance2D playerPosition) : base(name, playerPosition) {}

    internal override EntityEnemy EnemyStats { get; set; } = new SlimeStats();

    public override string TexturePath => "Textures/slime.png";

    private class SlimeStats : EntityEnemy
    {
        public override float Speed { get; set; } = 0.1f;
        public override int CurrentHealth { get; set; } = 10;
        public override int MaxHealth { get; set; } = 10;
        public override int Damage { get; set; } = 10;
    }
}

public class HeadEnemy : Enemy
{
    public HeadEnemy(string name, MeshInstance2D playerPosition) : base(name, playerPosition) { }

    internal override EntityEnemy EnemyStats { get; set; } = new SlimeStats();

    public override string TexturePath => "Textures/enemy.png";

    private class SlimeStats : EntityEnemy
    {
        public override float Speed { get; set; } = 0.07f;
        public override int CurrentHealth { get; set; } = 10;
        public override int MaxHealth { get; set; } = 10;
        public override int Damage { get; set; } = 10;
    }
}