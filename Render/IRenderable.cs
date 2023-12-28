using System.Numerics;
using UniversityGameProject.Render.Camera;
using UniversityGameProject.Render.BufferObject;
using UniversityGameProject.Render.VertexArrayObject;

namespace UniversityGameProject.Render;

public interface IRenderable
{
    public short VisualMask { get; set; }
    public Material.Material Material { get; }
    public Matrix4x4 View { get; }
    public VertexArrayObject<float, ushort> Vao { get; }
    public BufferObject<float>? Vbo { get; }
    public BufferObject<ushort>? Ebo { get; }
    public float[] Vertices { get; }
    public ushort[] Indices { get; }

    public void Initialize(Material.Material material, VertexArrayObject<float, ushort> vao, BufferObject<float> vbo,
        BufferObject<ushort> ebo);
    public void Draw(ICamera camera);
}