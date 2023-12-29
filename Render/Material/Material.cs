using System.Numerics;
using UniversityGameProject.Render.Camera;

namespace UniversityGameProject.Render.Material;

public abstract class Material
{
    protected ShaderContext _context;

    public Material(ShaderContext context)
    {
        _context = context;
    }
    
    internal virtual void Use(Viewport.Viewport viewport, Matrix4x4 view) { }
    
    internal virtual void Use(Viewport.Viewport viewport, Matrix4x4 view, Texture.Texture texture) { }
    
    public virtual void Attach(ICamera camera) { }
    
    public virtual void Attach(Texture.Texture texture) { }
}