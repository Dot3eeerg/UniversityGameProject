using System.Numerics;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Render.Camera;
using UniversityGameProject.Render.Material;
using UniversityGameProject.Resources.Primitives;

namespace UniversityGameProject.Game;

public class Enemy : Node2D
{
    private Body _body;
    private ICamera _camera;

    public Entity EnemyStats = new Stats();
    
    public Enemy(string name, string path, ICamera camera) : base(name)
    {
        _body = new Body("Enemy bode", path);
        _body.MeshData = new RectanglePrimitiveTextured();
        //_body.Transform.Scale = new Vector3(0.04f, 0.08f, 1.0f);
        _body.MeshData.ApplyScale(0.04f, 0.08f);

        _camera = camera;
        
        AddChild(_body, path, ShaderType.TextureShader);
    }

    public override void Process(float delta)
    {
        base.Process(delta);
        Console.WriteLine(_camera.Position);
        Console.WriteLine(_body.Transform.Position);
        
        //Vector3 direction = 
    }
        
    private sealed class Body : MeshInstance2D
    {
        public Body(string name, string path) : base(name) { }
    }
    
    private class Stats : Entity
    {
        public override float Speed { get; set; } = 0.8f;
        public override int HealthPool { get; set; } = 10;
    }
}