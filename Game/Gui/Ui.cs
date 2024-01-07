using System.Numerics;
using ImGuiNET;
using UniversityGameProject.Input;
using UniversityGameProject.Main.Control;
using UniversityGameProject.Window;

namespace UniversityGameProject.Game.Gui;

public enum AppState
{
    Paused,
    Active,
    LevelUp,
    Restart,
}

public class Ui : VisualInstanceControl
{
    private ImGuiWindowFlags _windowFlags =
        ImGuiWindowFlags.NoDecoration |
        ImGuiWindowFlags.AlwaysAutoResize |
        ImGuiWindowFlags.NoNav |
        ImGuiWindowFlags.NoSavedSettings |
        ImGuiWindowFlags.NoFocusOnAppearing |
        ImGuiWindowFlags.NoMove;
    
    private ImFontPtr _font;

    private WindowServer _window;
    private Vector2 _pauseSize = new Vector2();

    private AppState _state = AppState.Active;
    private Player _player;

    public Ui(string name, WindowServer window, Player player) : base(name)
    {
        _window = window;
        _pauseSize = _window.WindowSize;
        _player = player;

        //_font = ImGui.GetIO().Fonts.AddFontFromFileTTF("Gui/Fonts/AnonymousPro-Regular.ttf", 30.0f);
    }

    public override void Process(float delta)
    {
        if (!_player.IsPlayerAlive())
        {
            _state = AppState.Restart;
        }
        switch (_state)
        {
            case AppState.Paused:
                PauseMenu();
                break;
            
            case AppState.Active:
                break;
            
            case AppState.LevelUp:
                break;
            
            case AppState.Restart:
                RestartMenu();
                break;
        }
    }
    
    private void RestartMenu()
    {
        var text = "Press ENTER to restart";
        ImGui.SetNextWindowBgAlpha(0.45f);
        ImGui.SetNextWindowPos(Vector2.Zero);
        ImGui.SetNextWindowSize(_window.WindowSize);

        ImGui.Begin("Text", _windowFlags);
        {
            ImGui.SetWindowFontScale(2.0f);
            var calc = ImGui.CalcTextSize(text);
            ImGui.SetCursorPos((_window.WindowSize - calc) * 0.5f);
            ImGui.Text(text);
        }
    }

    private void PauseMenu()
    {
        var text = "Game is paused";
        ImGui.SetNextWindowBgAlpha(0.45f);
        ImGui.SetNextWindowPos(Vector2.Zero);
        ImGui.SetNextWindowSize(_window.WindowSize);

        ImGui.Begin("Text", _windowFlags);
        {
            ImGui.SetWindowFontScale(2.0f);
            var calc = ImGui.CalcTextSize(text);
            ImGui.SetCursorPos((_window.WindowSize - calc) * 0.5f);
            ImGui.Text(text);
        }
    }

    public override void Input(InputEvent input)
    {
        if (InputServer!.IsActionPressed("pause"))
        {
            _state = _state == AppState.Paused ? AppState.Active : AppState.Paused;
        }
    }
}