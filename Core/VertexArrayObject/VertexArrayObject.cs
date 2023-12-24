using Silk.NET.OpenGL;
using UniversityGameProject.Core.BufferObject;

namespace UniversityGameProject.Core.VertexArrayObject;

public class VertexArrayObject<TVertexType> : IDisposable
    where TVertexType : unmanaged
{
    private uint _handle;
    private GL _gl;

    public VertexArrayObject(GL gl, BufferObject<TVertexType> vbo)
    {
        _gl = gl;

        _handle = _gl.GenVertexArray();
        Bind();

        vbo.Bind();
    }

    public unsafe void VertexAttribPointer(uint index, int count, VertexAttribPointerType type, uint vertexSize,
        int offSet)
    {
        _gl.VertexAttribPointer(index, count, type, false, vertexSize * (uint)sizeof(TVertexType),
            (void*)(offSet * sizeof(TVertexType)));
        _gl.EnableVertexAttribArray(index);
    }

    public void Bind()
    {
        _gl.BindVertexArray(_handle);
    }

    public void Dispose()
    {
        _gl.DeleteVertexArray(_handle);
    }
}