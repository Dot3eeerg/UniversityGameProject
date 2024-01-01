namespace UniversityGameProject.Resources.Primitives;

public class RectanglePrimitive : IPrimitive
{
    public float[] Vertices { get; private set; } = new float[12]
    {
        -0.5f, -0.5f, 0.0f,
        -0.5f, 0.5f, 0.0f,
        0.5f, 0.5f, 0.0f,
        0.5f, -0.5f, 0.0f
    };

    public ushort[] Indices { get; } = new ushort[6]
    {
        0, 1, 2,
        0, 2, 3
    };
}

public class RectanglePrimitiveTextured : IPrimitiveTextured
{
    public float[] Vertices { get; private set; } = new float[20]
    {
        -0.5f, -0.5f, 0.0f, 0.0f, 1.0f,
        -0.5f, 0.5f, 0.0f, 0.0f, 0.0f,
        0.5f, 0.5f, 0.0f, 1.0f, 0.0f,
        0.5f, -0.5f, 0.0f, 1.0f, 1.0f
    };

    public ushort[] Indices { get; } = new ushort[6]
    {
        0, 1, 2,
        0, 2, 3
    };

    public void ApplyScale(float scaleX, float scaleY)
    {
        Vertices[0] *= scaleX;
        Vertices[1] *= scaleY;
        
        Vertices[5] *= scaleX;
        Vertices[6] *= scaleY;
        
        Vertices[10] *= scaleX;
        Vertices[11] *= scaleY;

        Vertices[15] *= scaleX;
        Vertices[16] *= scaleY;
    }
}
