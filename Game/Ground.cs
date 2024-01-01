using System.Numerics;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Render.Material;
using UniversityGameProject.Resources.Primitives;

namespace UniversityGameProject.Game;

public class Ground : Node2D
{
    private MeshInstance2D _tile = new MeshInstance2D("Ground tile");
    public float Speed = 1.0f;
    
    public Ground(string name, string path) : base(name)
    {
        _tile.MeshData = new RectanglePrimitiveTextured();
        //_tile.Transform.Scale = new Vector3(0.3f, 0.3f, 1.0f);
        
        AddChild(_tile, path, ShaderType.GroundShader);
    }
    
}