using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows.Media;
using ReMath = UniversityGameProject.Math;

namespace UniversityGameProject.Main._2d;

public class Node2D : Node
{
    private ReMath.Transform _global = new ReMath.Transform();
    private ReMath.Transform _parent = new ReMath.Transform();
    
   public ReMath.Transform Transform { get; set; } = new ReMath.Transform();

   public ReMath.Transform GlobalTransform
   {
       get
       {
           _global.Revert();
           
           _global.Position += Transform.Position;
           _global.Scale *= Transform.Scale;
           _global.Rotation = Quaternion.Concatenate(_global.Rotation, Transform.Rotation);

           if (Parent is Node2D)
           {
               _parent = ((Node2D) Parent).GlobalTransform;

               _global.Position = _parent.Position + _parent.Scale * Vector3.Transform(_global.Position, _parent.Rotation);
               _global.Scale *= _parent.Scale;
               _global.Rotation = Quaternion.Concatenate(_global.Rotation, _parent.Rotation);
           }

           return _global;
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

   public void SetTransform(Vector3 position)
   {
       var newTransform = Transform;
       newTransform.Position = position;
       Transform = newTransform;
   }
}