﻿using System.Numerics;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Resources.Primitives;

namespace UniversityGameProject.Game;

public class UIElement : Node2D
{
    private MeshInstance2D _body;
    
    public UIElement(string name, string path) : base(name)
    {
        _body = new Body("UI element", path);
        _body.MeshData = new RectanglePrimitiveTextured();
        _body.MeshData.ApplyScale(0.25f, 0.1f);

        AddChild(_body);
    }
    
    public sealed class Body : MeshInstance2D
    {
        public Body(string name, string path) : base(name) { }
    }
}