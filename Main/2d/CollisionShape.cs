using System.Numerics;

namespace UniversityGameProject.Main._2d;

public abstract class CollisionShape : Node2D
{
    public CollisionShape(string name) : base(name) { }
}

public class Circle : CollisionShape
{
    public float Radius { get; set; }

    public Circle(string name) : base(name) { }

    public Circle(string name, float radius) : base(name)
    {
        Radius = radius;
    }

    public bool CheckCollision(Circle circle)
    {
        float distance = Single.Sqrt(Single.Pow(GlobalTransform.Position.X - circle.GlobalTransform.Position.X, 2) +
                                     Single.Pow(GlobalTransform.Position.Y - circle.GlobalTransform.Position.Y, 2));

        if (distance > Radius + circle.Radius)
        {
            return false;
        }

        return true;
    }
}