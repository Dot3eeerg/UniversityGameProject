using System.Numerics;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace UniversityGameProject.Core.Window;

public class WindowContext
{
    internal IInputContext? Input;
    internal IKeyboard? Keyboard;
    internal IMouse? Mouse;
    internal GL? Gl;
    internal Game.GUI.GUI? Gui;

    public bool Initialized { get; private set; } = false;
    
    public WindowContext() { }

    public void AttachWindow(IWindow window)
    {
        Input = window.CreateInput();
        Gl = window.CreateOpenGL();

        Gui = new Game.GUI.GUI(Gl, window, Input);

        Keyboard = Input.Keyboards.FirstOrDefault();
        Mouse = Input.Mice.FirstOrDefault();

        Initialized = true;
    }
    
    internal void FrameSetup()
    {
        if (Initialized)
        {
            Gl!.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            Gl!.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

            Gl!.Enable(EnableCap.DepthTest);
            Gl!.Enable(EnableCap.Blend);
            Gl!.Enable(EnableCap.LineSmooth);
            Gl!.Enable(EnableCap.Multisample);
            Gl!.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }
    }

    internal Vector2 MousePosition => Mouse!.Position;
}