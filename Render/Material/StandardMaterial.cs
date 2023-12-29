using System.Numerics;
using UniversityGameProject.Render.Camera;
using UniversityGameProject.Render.Shader;

namespace UniversityGameProject.Render.Material;

public class StandardMaterial : Material
{
    private ShaderProgram _shaderProgram;
    private uint _shaderDescriptor;

    public StandardMaterial(ShaderContext context) : base(context)
    {
        _shaderProgram = ShaderLibrary.TextureShader(context);
        _shaderDescriptor = _shaderProgram.GetDescriptor();
    }

    internal override void Use(Viewport.Viewport viewport, Matrix4x4 view)
    {
        _context.Use(_shaderDescriptor);
        
        //_context.SetUniform(_shaderDescriptor, "model", view);
        //_context.SetUniform(_shaderDescriptor, "view", viewport.Camera.View);
        //_context.SetUniform(_shaderDescriptor, "projection", viewport.GetProjection());
    }
    
    internal override void Use(Viewport.Viewport viewport, Matrix4x4 view, Texture.Texture texture)
    {
        _context.Use(_shaderDescriptor);
        
        _context.SetUniform(_shaderDescriptor, "uTexture", 0);
        
        _context.SetUniform(_shaderDescriptor, "model", view);
        _context.SetUniform(_shaderDescriptor, "view", viewport.Camera.View);
        _context.SetUniform(_shaderDescriptor, "projection", viewport.GetProjection());
    }

    public override void Attach(ICamera camera)
    {
        _context.SetUniform(_shaderDescriptor, 
            "camera.position", 
            camera.Position.X,
            camera.Position.Y,
            camera.Position.Z
            );
    }

    public override void Attach(Texture.Texture texture)
    {
        //_context.SetUniform();
    }
}