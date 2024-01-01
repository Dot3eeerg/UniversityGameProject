using System.Numerics;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Render.Camera;
using UniversityGameProject.Render.Material;
using UniversityGameProject.Resources;
using UniversityGameProject.Resources.Primitives;

namespace UniversityGameProject.Game;

public class Enemy : Node2D
{
    private Body _body;
    private ICamera _camera;
    private MeshInstance2D _playerPosition;

    public Entity EnemyStats = new Stats();
    
    public Enemy(string name, string path, MeshInstance2D playerPosition) : base(name)
    {
        _body = new Body("Enemy body", path);
        _body.MeshData = new RectanglePrimitiveTextured();
        _body.MeshData.ApplyScale(0.04f, 0.08f);

        _playerPosition = playerPosition;
        
        AddChild(_body, path, ShaderType.TextureShader);
    }

    public override void Process(float delta)
    {
        base.Process(delta);
        
        Vector3 kek = new Vector3(_playerPosition.GlobalTransform.Position.X, _playerPosition.GlobalTransform.Position.Y, 0);
        Console.WriteLine(_body.GlobalTransform.Position);
        Vector3 direction = Vector3.Normalize(kek - _body.GlobalTransform.Position) * EnemyStats.Speed * delta;
        
        Translate(direction);
    }
        
    private sealed class Body : MeshInstance2D
    {
        public Body(string name, string path) : base(name) { }
    }
    
    private class Stats : Entity
    {
        public override float Speed { get; set; } = 0.1f;
        public override int HealthPool { get; set; } = 10;
    }
}