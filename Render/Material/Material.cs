using System.Numerics;
using UniversityGameProject.Render.Camera;

namespace UniversityGameProject.Render.Material;

public abstract class Material
{
    protected ShaderContext _context;
    protected ShaderType _type;

    public Material(ShaderContext context, ShaderType type)
    {
        _context = context;
        _type = type;
    }
    
    internal virtual void Use(Viewport.Viewport viewport, Matrix4x4 view) { }
    
    internal virtual void Use(Viewport.Viewport viewport, Matrix4x4 view, float offset) { }
    
    internal virtual void Use(Viewport.Viewport viewport, Matrix4x4 view, Texture.Texture texture) { }
    
    public virtual void Attach(ICamera camera) { }
    
    public virtual void Attach(Texture.Texture texture) { }
}