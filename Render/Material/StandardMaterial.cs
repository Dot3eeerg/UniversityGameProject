using System.Numerics;
using UniversityGameProject.Render.Camera;
using UniversityGameProject.Render.Shader;

namespace UniversityGameProject.Render.Material;

public enum ShaderType
{
    TextureShader,
    GroundShader,
    HealthBarShader,
}

public class StandardMaterial : Material, IDisposable
{
    private ShaderProgram _shaderProgram;
    private uint _shaderDescriptor;
    private ShaderType _type;
    private Matrix4x4 _groundView = 
        Matrix4x4.CreateLookAt(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY);

    public StandardMaterial(ShaderContext context, ShaderType type) : base(context, type)
    {
        _type = type;
        
        switch (_type)
        {
            case ShaderType.TextureShader:
                _shaderProgram = ShaderLibrary.TextureShader(context);
                break;
            
            case ShaderType.GroundShader:
                _shaderProgram = ShaderLibrary.GroundShader(context);
                break;
            
            case ShaderType.HealthBarShader:
                _shaderProgram = ShaderLibrary.HealthBarShader(context);
                break;
        }
        
        _shaderDescriptor = _shaderProgram.GetDescriptor();
    }

    internal override void Use(Viewport.Viewport viewport, Matrix4x4 view)
    {
        _context.Use(_shaderDescriptor);
        
        switch (_type)
        {  
            case ShaderType.TextureShader:
                _context.SetUniform(_shaderDescriptor, "model", view);
                _context.SetUniform(_shaderDescriptor, "view", viewport.Camera.View);
                _context.SetUniform(_shaderDescriptor, "projection", viewport.GetProjection());
                break;
            
            case ShaderType.GroundShader:
                _context.SetUniform(_shaderDescriptor, "offset", viewport.Camera.Position.X,
                    -viewport.Camera.Position.Y);
            
                _context.SetUniform(_shaderDescriptor, "model", view);
                _context.SetUniform(_shaderDescriptor, "view", _groundView);
                _context.SetUniform(_shaderDescriptor, "projection", viewport.GetProjection());
                break;
            
            case ShaderType.HealthBarShader:
                _context.SetUniform(_shaderDescriptor, "uFilled", 0.8f);
                
                _context.SetUniform(_shaderDescriptor, "model", view);
                _context.SetUniform(_shaderDescriptor, "view", viewport.Camera.View);
                _context.SetUniform(_shaderDescriptor, "projection", viewport.GetProjection());
                break;
        }
        
    }
    
    internal override void Use(Viewport.Viewport viewport, Matrix4x4 view, float offset)
    {
        _context.Use(_shaderDescriptor);
        
        switch (_type)
        {  
            case ShaderType.HealthBarShader:
                _context.SetUniform(_shaderDescriptor, "uFilled", offset);
                
                _context.SetUniform(_shaderDescriptor, "model", view);
                _context.SetUniform(_shaderDescriptor, "view", viewport.Camera.View);
                _context.SetUniform(_shaderDescriptor, "projection", viewport.GetProjection());
                break;
        }
        
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

    public void Dispose()
    {
        _shaderProgram.Dispose();
    }
}