using UniversityGameProject.Render;
using UniversityGameProject.Render.BufferObject;
using UniversityGameProject.Render.Camera;
using UniversityGameProject.Render.Material;
using UniversityGameProject.Render.VertexArrayObject;

namespace UniversityGameProject.Main._2d;

public abstract class VisualInstance3D : Node2D, IRenderable
{
    public VertexArrayObject<float, ushort> Vao { get; private set; }
    public BufferObject<float>? Vbo { get; private set; }
    public BufferObject<ushort>? Ebo { get; private set; }
    public virtual float[] Vertices { get; protected set; } = new float[0];
    public virtual ushort[] Indices { get; protected set; } = new ushort[0];

    public virtual short VisualMask { get; set; } = 1;
    public Material? Material { get; protected set; }

    public VisualInstance3D(string name) : base(name) { }

    public void Initialize(Material material, VertexArrayObject<float, ushort> vao, BufferObject<float> vbo, BufferObject<ushort> ebo)
    {
        Material = material;

        Vao = vao;
        Vbo = vbo;
        Ebo = ebo;
    }

    public virtual void Draw(ICamera camera) { }
}