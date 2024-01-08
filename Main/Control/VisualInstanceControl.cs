using UniversityGameProject.GUI;

namespace UniversityGameProject.Main.Control;

public abstract class VisualInstanceControl : Control, IGuiElement
{
    public VisualInstanceControl(string name) : base(name) { }
}