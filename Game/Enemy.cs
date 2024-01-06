﻿using System.Numerics;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Render.Camera;
using UniversityGameProject.Render.Material;
using UniversityGameProject.Resources;
using UniversityGameProject.Resources.Primitives;

namespace UniversityGameProject.Game;

public abstract class Enemy : Node2D
{
    internal Body _body;
    private ICamera _camera;
    private MeshInstance2D _playerPosition;
    internal Circle _collision = new Circle("Collision", 0.04f);

    public Circle Circle => _collision;

    internal abstract EntityEnemy EnemyStats { get; set; }
    
    public Enemy(string name, MeshInstance2D playerPosition) : base(name)
    {
        _body = new Body("Enemy body");
        _body.MeshData = new RectanglePrimitiveTextured();
        DoScale();

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
            // I can't do this, Dispose is killing me and breaking memory usage
            //Dispose();
        }
    }

    public bool IsDead()
    {
        if (EnemyStats.CurrentHealth <= 0)
        {
            return true;
        }

        return false;
    }

    public void ChangePosition(Vector3 direction, float delta)
    {
        Translate(direction * EnemyStats.Speed * delta);
        _collision.GlobalTransform.Position = GlobalTransform.Position;
    }

    public virtual void DoScale()
    {
        _body.MeshData.ApplyScale(0.04f, 0.08f);
    }
        
    internal sealed class Body : MeshInstance2D
    {
        public Body(string name) : base(name) { }
    }

    public void Dispose()
    {
        _body.Dispose();
    }
}

public class SlimeEnemy : Enemy
{
    public SlimeEnemy(string name, MeshInstance2D playerPosition) : base(name, playerPosition) {}
    
    private CollisionShape _collision = new Circle("Collision", 0.04f);

    internal override EntityEnemy EnemyStats { get; set; } = new Stats();

    public override string TexturePath => "Textures/slime.png";

    private class Stats : EntityEnemy
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

    private CollisionShape _collision = new Circle("Collision", 0.04f);
    
    internal override EntityEnemy EnemyStats { get; set; } = new Stats();

    public override string TexturePath => "Textures/ghost.png";

    private class Stats : EntityEnemy
    {
        public override float Speed { get; set; } = 0.07f;
        public override int CurrentHealth { get; set; } = 10;
        public override int MaxHealth { get; set; } = 10;
        public override int Damage { get; set; } = 10;
    }
}

public class GiantEnemy : Enemy
{
    public GiantEnemy(string name, MeshInstance2D playerPosition) : base(name, playerPosition) { }

    private CollisionShape _collision = new Circle("Collision", 0.12f);
    
    internal override EntityEnemy EnemyStats { get; set; } = new Stats();

    public override string TexturePath => "Textures/giant.png";

    public override void DoScale()
    {
        _body.MeshData.ApplyScale(0.10f, 0.15f);
    }

    private class Stats : EntityEnemy
    {
        public override float Speed { get; set; } = 0.06f;
        public override int CurrentHealth { get; set; } = 200;
        public override int MaxHealth { get; set; } = 200;
        public override int Damage { get; set; } = 20;
    }
}

public class BossEnemy : Enemy
{
    public BossEnemy(string name, MeshInstance2D playerPosition) : base(name, playerPosition) { }
    
    private CollisionShape _collision = new Circle("Collision", 0.18f);

    internal override EntityEnemy EnemyStats { get; set; } = new Stats();

    public override string TexturePath => "Textures/boss.png";

    public override void DoScale()
    {
        _body.MeshData.ApplyScale(0.14f, 0.25f);
    }

    private class Stats : EntityEnemy
    {
        public override float Speed { get; set; } = 0.06f;
        public override int CurrentHealth { get; set; } = 200;
        public override int MaxHealth { get; set; } = 200;
        public override int Damage { get; set; } = 20;
    }
}