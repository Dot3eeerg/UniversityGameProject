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
        ImGuiWindowFlags.NoCollapse |
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

        else if (_state != AppState.Paused)
        {
            if (!_player.LevelUpIsHandled && _state != AppState.LevelUp)
            {
                _state = AppState.LevelUp;
            }
            else if (_state != AppState.LevelUp)
            {
                _state = AppState.Active;
            }
        }

        switch (_state)
        {
            case AppState.Paused:
                PauseMenu();
                break;
            
            case AppState.Active:
                break;
            
            case AppState.LevelUp:
                LevelUpMenu();
                break;
            
            case AppState.Restart:
                RestartMenu();
                break;
        }
    }

    private void LevelUpMenu()
    {
        var text = "Choose your upgrade";
        ImGui.SetNextWindowBgAlpha(0.45f);
        ImGui.SetNextWindowPos(Vector2.Zero);
        ImGui.SetNextWindowSize(_window.WindowSize);
        
        var calc = ImGui.CalcTextSize(text);
        var kekX = _window.WindowSize.X - calc.X;
        var kekY = _window.WindowSize.Y - calc.Y;

        ImGui.Begin("Message", _windowFlags | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoInputs);
        {
            ImGui.SetWindowFontScale(2.0f);
            ImGui.SetCursorPosX((_window.WindowSize.X - calc.X) * 0.45f);
            ImGui.SetCursorPosY((_window.WindowSize.Y - calc.Y) * 0.13f);
            ImGui.Text(text);
        }

        ImGui.SetNextWindowBgAlpha(0.8f);
        ImGui.SetNextWindowPos(new Vector2(_window.WindowSize.X * 0.1f, _window.WindowSize.Y * 0.25f));
        ImGui.SetNextWindowSize(new Vector2(_window.WindowSize.X * 0.2f, _window.WindowSize.Y * 0.4f));
        ImGui.Begin("Whip upgrade", _windowFlags);
        {
            ImGui.SetWindowFontScale(2.0f);
            
            ImGui.TextWrapped("Add 1 more hit direction of whip");
            
            if (ImGui.Button("Select"))
            {
                _player.UpgradePlayer(UpgradeType.WeaponUpgrade);
                _player.LevelUpIsHandled = true;
                _state = AppState.Active;
            }
        }
        
        ImGui.SetNextWindowBgAlpha(0.8f);
        ImGui.SetNextWindowPos(new Vector2(_window.WindowSize.X * 0.4f, _window.WindowSize.Y * 0.25f));
        ImGui.SetNextWindowSize(new Vector2(_window.WindowSize.X * 0.2f, _window.WindowSize.Y * 0.4f));
        ImGui.Begin("LevelUp2", _windowFlags);
        {
            ImGui.SetWindowFontScale(2.0f);
            
            ImGui.Text("Upgrade2");
            ImGui.Text("kek");

            if (ImGui.Button("Select"))
            {
                _player.LevelUpIsHandled = true;
                _state = AppState.Active;
            }
        }
        
        ImGui.SetNextWindowBgAlpha(0.8f);
        ImGui.SetNextWindowPos(new Vector2(_window.WindowSize.X * 0.7f, _window.WindowSize.Y * 0.25f));
        ImGui.SetNextWindowSize(new Vector2(_window.WindowSize.X * 0.2f, _window.WindowSize.Y * 0.4f));
        ImGui.Begin("LevelUp3", _windowFlags);
        {
            ImGui.GetStyle().WindowTitleAlign = new Vector2(0.5f, 0.5f);
            ImGui.SetWindowFontScale(2.0f);
            
            ImGui.Text("Upgrade3");
            ImGui.Text("kek");

            if (ImGui.Button("Select"))
            {
                _player.LevelUpIsHandled = true;
                _state = AppState.Active;
            }
        }
    }
    
    private void RestartMenu()
    {
        var text = "Press ENTER to restart";
        ImGui.SetNextWindowBgAlpha(0.45f);
        ImGui.SetNextWindowPos(Vector2.Zero);
        ImGui.SetNextWindowSize(_window.WindowSize);

        ImGui.Begin("Text", _windowFlags| ImGuiWindowFlags.NoDecoration);
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

        ImGui.Begin("Text", _windowFlags| ImGuiWindowFlags.NoDecoration);
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