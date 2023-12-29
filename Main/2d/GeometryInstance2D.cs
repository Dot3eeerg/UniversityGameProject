using UniversityGameProject.Resources;

namespace UniversityGameProject.Main._2d;

public abstract class GeometryInstance2D : VisualInstance2D
{
    protected IMeshData? _meshData;
    public virtual IMeshData MeshData
    {
        get => _meshData!;
        set
        {
            Indices = value.Indices;
            Vertices = value.Vertices;

            _meshData = value;
        }
    }

    public GeometryInstance2D(string name) : base(name) { }
}