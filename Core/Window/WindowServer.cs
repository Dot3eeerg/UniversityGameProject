using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace UniversityGameProject.Core.Window;

public class WindowServer
{
    private IWindow _window;
    private WindowContext _context = new WindowContext();

    public WindowServer()
    {
        var options = WindowOptions.Default;

        options.Title = "Game";
        options.Size = new Vector2D<int>(1280, 720);

        _window = Silk.NET.Windowing.Window.Create(options);

        _window.Load += OnLoad;
        _window.Render += OnRender;
        _window.Resize += OnResize;
        _window.Closing += OnClosing;
        
        _window.Initialize();
    }

    private void OnLoad()
    {
        _context.AttachWindow(_window);
    }
    
    private void OnRender(double delta)
    {
        _context.FrameSetup();
        
        _context.Gui!.Process((float) delta);
    }

    private void OnResize(Vector2D<int> size)
    {
        _context.Gl!.Viewport(size);
        _context.Gui!.SetViewportSize(new Vector2(size.X, size.Y));
    }

    private void OnClosing()
    {
        _window.Load -= OnLoad;
        _window.Render -= OnRender;
        _window.Resize -= OnResize;
        _window.Closing -= OnClosing;
    }
}