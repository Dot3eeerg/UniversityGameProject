﻿using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace UniversityGameProject.GUI;
    
public class GuiServer
{
    private ImGuiController _controller;
    private ImGuiNET.ImGuiViewport _viewport;

    private GL _gl;
    private IView _window;
    private IInputContext _input;

    public GuiServer(GL gl, IView window, IInputContext input)
    {
        _gl = gl;
        _window = window;
        _input = input;

        _controller = new ImGuiController(gl, window, input);
        _viewport = new ImGuiViewport();

    }

    public void SetupFrame(float delta)
    {
        _controller.Update(delta);
    }

    public void RenderFrame()
    {
        _controller.Render();
    } 
}
