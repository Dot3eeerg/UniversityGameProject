using System.Numerics;
using Silk.NET.OpenGL;
using UniversityGameProject.Render.BufferObject;
using UniversityGameProject.Render.VertexArrayObject;
using UniversityGameProject.Render.Material;

namespace UniversityGameProject.Render;

public class RenderServer
{
    private GL _gl;
    private ShaderContext _shaderContext;

    public RenderServer(GL gl)
    {
        _gl = gl;
        _shaderContext = new ShaderContext(_gl);
    }

    public void Load(IRenderable renderable, string path, Material.ShaderType type)
    {
        BufferObject<float> vbo = new BufferObject<float>(_gl, renderable.Vertices, BufferTargetARB.ArrayBuffer,
            BufferUsageARB.DynamicDraw);
        BufferObject<ushort> ebo = new BufferObject<ushort>(_gl, renderable.Indices, BufferTargetARB.ElementArrayBuffer,
            BufferUsageARB.DynamicDraw);

        VertexArrayObject<float, ushort> vao = new VertexArrayObject<float, ushort>(_gl, vbo, ebo);
        
        vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
        vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);

        Material.Material material = new StandardMaterial(_shaderContext, type);
        Texture.Texture texture = new Texture.Texture(_gl, path);
        
        renderable.Initialize(material, texture, vao, vbo, ebo);
    }

    public void Render(Viewport.Viewport viewport, IRenderable renderable)
    {
        if ((viewport.Camera.VisualMask & renderable.VisualMask) > 0)
        {
            renderable.Vao!.Bind();
            
            renderable.Texture.Bind();
            renderable.Material.Use(viewport, renderable.View);
            //renderable.Material.Attach(viewport.Camera);

            unsafe
            {
                renderable.Ebo!.Update(renderable.Indices);
                renderable.Vbo!.Update(renderable.Vertices);

                _gl.DrawElements(PrimitiveType.Triangles, (uint)renderable.Indices.Length,
                    DrawElementsType.UnsignedShort, null);
            }
        }
    }
    
    public void Render(Viewport.Viewport viewport, IRenderable renderable, float offset)
    {
        if ((viewport.Camera.VisualMask & renderable.VisualMask) > 0)
        {
            renderable.Vao!.Bind();
            
            renderable.Texture.Bind();
            renderable.Material.Use(viewport, renderable.View, offset);
            //renderable.Material.Attach(viewport.Camera);

            unsafe
            {
                renderable.Ebo!.Update(renderable.Indices);
                renderable.Vbo!.Update(renderable.Vertices);

                _gl.DrawElements(PrimitiveType.Triangles, (uint)renderable.Indices.Length,
                    DrawElementsType.UnsignedShort, null);
            }
        }
    }

    public void ApplyEnvironment(Viewport.Viewport viewport)
    {
        _gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        _gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        
        _gl.Enable(EnableCap.DepthTest);
        _gl.Enable(EnableCap.Blend);
        _gl.Enable(EnableCap.LineSmooth);
        _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    public void ChangeContextSize(Vector2 size)
    {
        _gl.Viewport(0, 0, (uint)size.X, (uint)size.Y);
    }
}