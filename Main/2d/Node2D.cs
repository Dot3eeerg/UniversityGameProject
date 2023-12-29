using System.Numerics;
using ReMath = UniversityGameProject.Math;

namespace UniversityGameProject.Main._2d;

public class Node2D : Node
{
   public ReMath.Transform Transform { get; set; } = new ReMath.Transform();

   public ReMath.Transform GlobalTransform
   {
       get
       {
           var global = new ReMath.Transform();

           global.Position += Transform.Position;
           global.Scale *= Transform.Scale;
           global.Rotation = Quaternion.Concatenate(global.Rotation, Transform.Rotation);

           if (Parent is Node2D)
           {
               var parent = ((Node2D)Parent).GlobalTransform;

               global.Position = parent.Position + parent.Scale * Vector3.Transform(global.Position, parent.Rotation);
               global.Scale *= parent.Scale;
               global.Rotation = Quaternion.Concatenate(global.Rotation, parent.Rotation);
           }

           return global;
       }
   }
   
   public virtual Matrix4x4 View
   {
       get
       {
           if (Parent is Node2D)
           {
               return Transform.ViewMatrix * ((Node2D)Parent).View;
           }

           return Transform.ViewMatrix;
       }
   }

   public Node2D(string name) : base(name) { }

   public void Translate(Vector3 offset)
   {
       var newTransform = Transform;
       newTransform.Position += offset;

       Transform = newTransform;
   }

   public void Translate(float x, float y, float z)
   {
       var newTransform = Transform;
       newTransform.Position += new Vector3(x, y, z);

       Transform = newTransform;
   }
}