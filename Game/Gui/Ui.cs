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

    private Random _randomGen = new Random();
    private int _weaponNext = 0;
    private int _playerNext = 0;
    private int _playerNext2 = 0;
    private int _whipUpgrades = 0;

    private List<string> _upgradeText = new List<string>();
    
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
                GenerateUpgrades();
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

        ShowTimer();
    }

    private void LevelUpMenu()
    {
        var text = "Choose your upgrade";
        ImGui.SetNextWindowBgAlpha(0.45f);
        ImGui.SetNextWindowPos(Vector2.Zero);
        ImGui.SetNextWindowSize(_window.WindowSize);
        
        var calc = ImGui.CalcTextSize(text);

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
        ImGui.Begin("Weapon upgrade", _windowFlags);
        {
            ImGui.SetWindowFontScale(1.7f);
            
            ImGui.TextWrapped(_upgradeText[0]);
            
            ImGui.SetCursorPos(new Vector2(_window.WindowSize.X * 0.069f, _window.WindowSize.Y * 0.29f));
            if (ImGui.Button("Select"))
            {
                if (_weaponNext == 0)
                {
                    _whipUpgrades++;
                }
                _player.UpgradePlayer((UpgradeType) _weaponNext);
                _player.LevelUpIsHandled = true;
                _state = AppState.Active;
            }
        }
        
        ImGui.SetNextWindowBgAlpha(0.8f);
        ImGui.SetNextWindowPos(new Vector2(_window.WindowSize.X * 0.4f, _window.WindowSize.Y * 0.25f));
        ImGui.SetNextWindowSize(new Vector2(_window.WindowSize.X * 0.2f, _window.WindowSize.Y * 0.4f));
        ImGui.Begin("Player stats upgrade 1", _windowFlags);
        {
            ImGui.SetWindowFontScale(1.7f);

            ImGui.TextWrapped(_upgradeText[1]);

            
            ImGui.SetCursorPos(new Vector2(_window.WindowSize.X * 0.069f, _window.WindowSize.Y * 0.29f));
            if (ImGui.Button("Select"))
            {
                _player.UpgradePlayer((UpgradeType) _playerNext);
                _player.LevelUpIsHandled = true;
                _state = AppState.Active;
            }
        }
        
        ImGui.SetNextWindowBgAlpha(0.8f);
        ImGui.SetNextWindowPos(new Vector2(_window.WindowSize.X * 0.7f, _window.WindowSize.Y * 0.25f));
        ImGui.SetNextWindowSize(new Vector2(_window.WindowSize.X * 0.2f, _window.WindowSize.Y * 0.4f));
        ImGui.Begin("Player stats upgrade 2", _windowFlags);
        {
            ImGui.GetStyle().WindowTitleAlign = new Vector2(0.5f, 0.5f);
            ImGui.SetWindowFontScale(1.7f);
            
            ImGui.TextWrapped(_upgradeText[2]);

            
            ImGui.SetCursorPos(new Vector2(_window.WindowSize.X * 0.069f, _window.WindowSize.Y * 0.29f));
            if (ImGui.Button("Select"))
            {
                _player.UpgradePlayer((UpgradeType) _playerNext2);
                _player.LevelUpIsHandled = true;
                _state = AppState.Active;
            }
        }
    }
    
    private void GenerateUpgrades()
    {
        _upgradeText.Clear();
        _weaponNext = _randomGen.Next(0, 2);
        _playerNext = _randomGen.Next(2, 5);
        _playerNext2 = _randomGen.Next(2, 5);
        while (_playerNext == _playerNext2)
        {
            _playerNext2 = _randomGen.Next(2, 5);
        }
        UpdateUpgradeText();
    }

    private void UpdateUpgradeText()
    {
        switch (_weaponNext)
        {
            case 0:
                if (_whipUpgrades < 3)
                {
                    _upgradeText.Add("Add 1 more direction of whip");
                }
                else
                {
                    _upgradeText.Add(
                        $"Attack speed increased by 0.1 sec.: {System.Math.Round(_player.GetAttackCooldown() / 1000.0f, 1)} sec. -> {System.Math.Round(_player.GetAttackCooldown() / 1000.0f - 0.1f, 1)} sec.");
                }
                break;
                
            case 1:
                _upgradeText.Add($"Fireball damage increase by 3: {_player.GetFireballDamage()} -> {_player.GetFireballDamage() + 3}");
                break;
        }

        SwitchPlayerStats(_playerNext);
        SwitchPlayerStats(_playerNext2);
    }

    private void SwitchPlayerStats(int upgradeNumber)
    {
        switch (upgradeNumber)
        {
            case 2:
                _upgradeText.Add(
                    $"Increase HP regeneration by 0.2 HP/sec.: {_player.PlayerStats.HpRegen} HP/sec. --> {_player.PlayerStats.HpRegen + 1} HP/sec.");
                break;
            
            case 3:
                _upgradeText.Add(
                    $"Increase player movement speed by 10%%: {(int) (_player.PlayerStats.Speed * 1000 / 3)}%% --> {(int) ((_player.PlayerStats.Speed + 0.03) * 1000 / 3)}%%");
                break;
            
            case 4:
                _upgradeText.Add(
                    $"Increase player damage reduction by 7%%: {System.Math.Round(_player.PlayerStats.DamageReduction * 100, 1)}%% --> {System.Math.Round((_player.PlayerStats.DamageReduction + 0.07f) * 100, 1)}%%");
                break;
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

    private void ShowTimer()
    {
        var text = System.Math.Round((double)_player.Scene.TotalTime / 1000, 2).ToString("F2");
        ImGui.SetNextWindowBgAlpha(0f);
        ImGui.SetNextWindowPos(Vector2.Zero);
        ImGui.SetNextWindowSize(_window.WindowSize);

        ImGui.Begin("Text", _windowFlags | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoInputs);
        {
            ImGui.SetWindowFontScale(2.0f);
            var calc = ImGui.CalcTextSize(text);
            ImGui.SetCursorPos(new Vector2((_window.WindowSize.X - calc.X) * 0.5f, calc.Y));
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