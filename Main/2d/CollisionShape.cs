using System.Numerics;
using System.Windows.Media.Media3D;
using Silk.NET.Maths;
using Silk.NET.Vulkan;

namespace UniversityGameProject.Main._2d;

public abstract class CollisionShape : Node2D
{
    public CollisionShape(string name) : base(name) { }

    protected static bool PointInCircle(Vector2 point, Circle circle)
    {
        Vector2 pointToOrigin =
            new Vector2(circle.GlobalTransform.Position.X, circle.GlobalTransform.Position.Y) - point;
        float lengthSquared = LengthSquared(pointToOrigin);
        return lengthSquared <= circle.Radius * circle.Radius;
    }

    public bool CheckCollision(CollisionShape collisionShape)
    {
        if (collisionShape is Circle) { return CheckCollision((Circle) collisionShape); }

        if (collisionShape is Rectangle) { return CheckCollision((Rectangle)collisionShape); }

        throw new NotImplementedException();
    }

    abstract public bool CheckCollision(Circle circle);
    abstract public bool CheckCollision(Rectangle rectangle);

    protected static float LengthSquared(Vector2 v)
    {
        return (v.X * v.X) + (v.Y * v.Y);
    }
}

public class Circle : CollisionShape
{
    public float Radius { get; set; }

    public Circle(string name) : base(name) { }

    public Circle(string name, float radius) : base(name)
    {
        Radius = radius;
    }

    public override bool CheckCollision(Circle circle)
    {
        float distance = LengthSquared(new Vector2(-GlobalTransform.Position.X + circle.GlobalTransform.Position.X,
            -GlobalTransform.Position.Y + circle.GlobalTransform.Position.Y));

        if (distance < (Radius + circle.Radius) * (Radius + circle.Radius))
        {
            return true;
        }

        return false;
    }

    public override bool CheckCollision(Rectangle rect)
    {
        //Vector2 bottomLeft =
        //    new Vector2(GlobalTransform.Position.X - Width / 2, GlobalTransform.Position.Y - Height / 2);

        Vector2 topLeft = new Vector2(rect.GlobalTransform.Position.X - rect.Width / 2,
            rect.GlobalTransform.Position.Y + rect.Height / 2);

        Vector2 bottomRight = new Vector2(rect.GlobalTransform.Position.X + rect.Width / 2,
            rect.GlobalTransform.Position.Y - rect.Height / 2);

        //Vector2 topRight = new Vector2(GlobalTransform.Position.X + Width / 2, GlobalTransform.Position.Y + Height / 2);

        Vector2 projectedPos = new Vector2(0.0f);
        projectedPos.Y = Single.Clamp(GlobalTransform.Position.Y, bottomRight.Y, topLeft.Y);
        projectedPos.X = Single.Clamp(GlobalTransform.Position.X, topLeft.X, bottomRight.X);

        return PointInCircle(projectedPos, this);
    }
}

public class Rectangle: CollisionShape
{
    public float Width { get; set; }
    public float Height { get; set; }

    public Rectangle(string name) : base(name) { }

    public Rectangle(string name, float width, float height) : base(name)
    {
        Width = width;
        Height = height;
    }

    public override bool CheckCollision(Circle circle)
    {
        //Vector2 bottomLeft =
        //    new Vector2(GlobalTransform.Position.X - Width / 2, GlobalTransform.Position.Y - Height / 2);
        Vector2 topLeft = new Vector2(GlobalTransform.Position.X - Width / 2, GlobalTransform.Position.Y + Height / 2);

        Vector2 bottomRight =
            new Vector2(GlobalTransform.Position.X + Width / 2, GlobalTransform.Position.Y - Height / 2);
        //Vector2 topRight = new Vector2(GlobalTransform.Position.X + Width / 2, GlobalTransform.Position.Y + Height / 2);

        Vector2 projectedPos = new Vector2(0.0f);
        projectedPos.Y = Single.Clamp(circle.GlobalTransform.Position.Y, bottomRight.Y, topLeft.Y);
        projectedPos.X = Single.Clamp(circle.GlobalTransform.Position.X, topLeft.X, bottomRight.X);

        return PointInCircle(projectedPos, circle);
    }

    public override bool CheckCollision(Rectangle rectangle)
    {
        Vector2 topLeftOwn = new Vector2(GlobalTransform.Position.X - Width / 2, GlobalTransform.Position.Y + Height / 2);

        Vector2 bottomRightOwn =
            new Vector2(GlobalTransform.Position.X + Width / 2, GlobalTransform.Position.Y - Height / 2);

        Vector2 topLeftRect = new Vector2(rectangle.GlobalTransform.Position.X - rectangle.Width / 2, rectangle.GlobalTransform.Position.Y + rectangle.Height / 2);

        Vector2 bottomRightRect =
            new Vector2(rectangle.GlobalTransform.Position.X + rectangle.Width / 2, rectangle.GlobalTransform.Position.Y - rectangle.Height / 2);

        if (topLeftOwn.X > bottomRightRect.X) return false;  // правее

        else if (bottomRightOwn.X < topLeftRect.X) return false; // левее 

        else if (topLeftOwn.Y < bottomRightRect.Y) return false; // ниже

        else if (bottomRightOwn.Y >  topLeftRect.Y) return false; // выше


        return true;
    }
}
