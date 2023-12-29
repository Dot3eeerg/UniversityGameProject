﻿using System.Numerics;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Resources.Primitives;

namespace UniversityGameProject.Game;

public class Ground : Node2D
{
    private MeshInstance2D _tile = new MeshInstance2D("Ground tile");
    
    public Ground(string name, string path) : base(name)
    {
        _tile.MeshData = new RectanglePrimitiveTextured();
        //_tile.Transform.Scale = new Vector3(0.0f, 0.0f, 0.0f);
        
        AddChild(_tile, path);
    }
}