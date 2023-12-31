﻿using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace UniversityGameProject.Window;
    
public delegate void WindowLoaded();
public delegate void WindowStartsRender(float delta);
public delegate void WindowResized(Vector2 size);
public delegate void WindowClosed();
    
public class WindowServer
{
    private IWindow _window;
    private GL? _gl;
    private IInputContext? _input;
    
    public event WindowLoaded? OnWindowLoaded;
    public event WindowStartsRender? OnWindowStartsRender;
    public event WindowResized? OnWindowResized;
    public event WindowClosed? OnWindowClosed;

    public Vector2 WindowSize { get; private set; } = new Vector2(1920, 1080);

    public bool Running { get => !_window.IsClosing; }

    public WindowServer()
    {
        var options = WindowOptions.Default;

        options.Title = "VS clone";
        options.Size = new Vector2D<int>(1920, 1080);
        options.PreferredDepthBufferBits = 32;
        options.WindowState = WindowState.Fullscreen;


        _window = Silk.NET.Windowing.Window.Create(options);

        _window.Load += OnWindowLoad;
        _window.Resize += OnWindowResize;
        _window.Render += OnWindowRender;
        _window.Closing += OnWindowClosing;

        _window.Initialize();
    }

    public void Render()
    {
        _window.DoEvents();
        _window.DoUpdate();
        _window.DoRender();

        _window.ContinueEvents();
    }

    public void Close()
    {
        _window.Close();
    }

    public GL GetGlContext()
    {
        return _gl!;
    }

    public IInputContext GetInputContext()
    {
        return _input!;
    }

    public IView GetViewContext()
    {
        return _window!;
    }

    private void OnWindowLoad()
    {
        _gl = _window.CreateOpenGL();
        _input = _window.CreateInput();

        if (OnWindowLoaded != null)
        {
            OnWindowLoaded!.Invoke();
        }
    }

    private void OnWindowRender(double delta)
    {
        if (OnWindowStartsRender != null)
        {
            OnWindowStartsRender!.Invoke((float)delta);
        }
    }

    private void OnWindowResize(Vector2D<int> size)
    {
        var newSize = new Vector2(size.X, size.Y);
        WindowSize = newSize;

        if (OnWindowResized != null)
        {
            OnWindowResized!.Invoke(newSize);
        }

        _gl!.Viewport(size);
    }

    private void OnWindowClosing()
    {
        _window.Load -= OnWindowLoad;
        _window.Resize -= OnWindowResize;
        _window.Render -= OnWindowRender;
        _window.Closing -= OnWindowClosing;

        _window.Close();

        if (OnWindowClosed != null)
        {
            OnWindowClosed!.Invoke();
        }
    }
}
