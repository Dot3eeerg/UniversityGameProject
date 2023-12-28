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
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
        -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
        0.5f, 0.5f, 0.0f, 1.0f, 1.0f,
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f
    };

    public ushort[] Indices { get; } = new ushort[6]
    {
        0, 1, 2,
        0, 2, 3
    };
}
