using System.Media;
using System.Numerics;
using System.Windows.Media;
using Silk.NET.Input;
using UniversityGameProject.Game;
using UniversityGameProject.Game.Gui;
using UniversityGameProject.GUI;
using UniversityGameProject.Input;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Main.Timer;
using UniversityGameProject.Render;
using UniversityGameProject.Render.Camera;
using UniversityGameProject.Render.Viewport;
using UniversityGameProject.Window;
using static UniversityGameProject.Game.Player;
using ShaderType = UniversityGameProject.Render.Material.ShaderType;

namespace UniversityGameProject.Main.Scene;

public class Scene : MainLoop
{
    private HashSet<Node> _nodes = new HashSet<Node>();
    private List<Viewport> _viewports = new List<Viewport>();

    private List<Node> _ground = new List<Node>();
    private List<Enemy> _enemies = new List<Enemy>();
    private List<Weapon> _colliders = new List<Weapon>();
    private HashSet<int>_hittedEnemy = new HashSet<int>();
    private Player _mainCollision;
    private Spawner _spawner;
    private Fireball _fireball;
    private HashSet<int> _fireballHitted = new HashSet<int>();
    private List<Enemy> _toDelete = new List<Enemy>();

    private Random _randomGenerator = new Random();

    private MediaPlayer _mediaPlayer = new MediaPlayer();

    private WindowServer _window;
    private RenderServer _renderServer;
    private InputServer _inputServer;
    private GuiServer _guiServer;

    private AppState _state = AppState.Active;
    
    private Timer.Timer _timer;
    public long TotalTime => _timer.Time;
    public long SpawnTimer;

    private int _numAliveEnemies = 0;
    public int NumAliveEnemies
    {
        get => _numAliveEnemies; 
        set => _numAliveEnemies = value;
    }

    public bool IsPlayerAlive { get; set; } = true;

    private bool _isPaused = false;
    private bool _isInputPaused = false;

    public int MaxAliveEnemies { get; set; } = 100;

    public Node Root { get; init; }

    public Scene() : base()
    {
        Root = new Node(this, "Root");

        _window = new WindowServer();
        _renderServer = new RenderServer(_window.GetGlContext());
        _inputServer = new InputServer(_window.GetInputContext());
        _timer = new Timer.Timer("Timer");
        _guiServer = new GuiServer(
            _window.GetGlContext(),
            _window.GetViewContext(),
            _window.GetInputContext()
        );

        _window.OnWindowStartsRender += Process;
        _inputServer.OnInputEmited += Input;

        _mediaPlayer.Open(new Uri(Path.GetFullPath("Sounds/background.wav")));
        _mediaPlayer.MediaEnded += new EventHandler(Repeat);

        Start();
    }

    private void Start()
    {
        var weaponList = new List<Weapon>();
        var weapon1 = new Weapon("Weapon", "Textures/swing2.png");
        Root.AddChild(weapon1, "Textures/swing2.png", ShaderType.TextureShader);

        var weapon2 = new Weapon("Weapon", "Textures/swing2_left.png");
        Root.AddChild(weapon2, "Textures/swing2_left.png", ShaderType.TextureShader);

        var weapon3 = new Weapon("Weapon", "Textures/swing2.png");
        Root.AddChild(weapon3, "Textures/swing2.png", ShaderType.TextureShader);

        var weapon4 = new Weapon("Weapon", "Textures/swing2_left.png");
        Root.AddChild(weapon4, "Textures/swing2_left.png", ShaderType.TextureShader);

        weaponList.Add(weapon1);
        weaponList.Add(weapon2);
        weaponList.Add(weapon3);
        weaponList.Add(weapon4);

        var player = new Player("Player", "Textures/character.png");

        var fireball = new Fireball("Fireball", "Textures/fireball.png", player.BodyData);
        Root.AddChild(fireball, "Textures/fireball.png", ShaderType.TextureShader);

        var ui = new UIElement("PlayerHPBar", "Textures/health_new.png");
        Root.AddChild(ui, "Textures/health_new.png", ShaderType.HealthBarShader);

        var xp = new UIElement("PlayerEXPBar", "Textures/xp.png");
        Root.AddChild(xp, "Textures/xp.png", ShaderType.ExpBarShader);

        player.LoadWeapons(weaponList, fireball);
        player.LoadHPBar(ui);
        player.LoadEXPBar(xp);

        Root.AddChild(player, "Textures/character.png", ShaderType.TextureShader);

        var ground = new Ground("Ground tile", "Textures/grass3.png");
        Root.AddChild(ground, "Textures/grass3.png", ShaderType.GroundShader);

        var gui = new Ui("UI", _window, player);
        Root.AddChild(gui);

        AttachViewport(player.Camera);
        _spawner = new Spawner(this);
        SpawnTimer = 0;
        _mediaPlayer.Play();
    }
   
    public void Run()
    {
        _timer.Start();

        while (_window.Running)
        {
            if (!_isPaused && IsPlayerAlive && !_isInputPaused)
            {
                if (_mainCollision.PlayerStats.CurrentHealth > 0)
                    _spawner.SpawnEnemy();
                else
                {
                    PlayerDied();
                }
            }
            _window.Render();
        }
    }
    
    protected override void Process(float delta)
    {
        if (!_isPaused && IsPlayerAlive && !_isInputPaused)
        {
            base.Process(delta);

            _timer.Update((long)(delta * 1000));

            SpawnTimer += (long)(delta * 1000);

            Console.WriteLine($"{(double)TotalTime / 1000}  {_mainCollision.PlayerStats.CurrentHealth}");
        }
        
        _guiServer.SetupFrame(delta);

        _renderServer.ChangeContextSize(_window.WindowSize);

        HandleLevelUp();
        if (!_isPaused && IsPlayerAlive && !_isInputPaused)
        {
            CheckPlayerCollision();
            CheckWeaponCollision();
            CheckFireballCollision();
            HandleCollidingEnemies(delta);
        }
        
        ApplyViewports();
        
        RenderNodes(delta);
        
        _guiServer.RenderFrame();
    }

    private void HandleCollidingEnemies(float delta)
    {
        for (int enemy1 = 0; enemy1 < _enemies.Count; enemy1++)
        {
            for (int enemy2 = enemy1 + 1; enemy2 < _enemies.Count; enemy2++)
            {
                if (_enemies[enemy1].Collision.CheckCollision(_enemies[enemy2].Collision))
                {
                    Vector3 kek = -Vector3.Normalize(_enemies[enemy2].GlobalTransform.Position -
                                                     _enemies[enemy1].GlobalTransform.Position);
                    _enemies[enemy1].ChangePosition(kek, delta);
                }
            }
        }
    }

    private void HandleLevelUp()
    {
        _mainCollision.CheckLevelUp();
        _inputServer.SetCursorMode(!_mainCollision.LevelUpIsHandled ? CursorMode.Normal : CursorMode.Disabled);
        if (!_isPaused)
        {
            _isPaused = !_mainCollision.LevelUpIsHandled;
        }
        else if (_mainCollision.LevelUpIsHandled)
        {
            _isPaused = false;
        }
    }

    private void CheckPlayerCollision()
    {
        for (int enemyID = 0; enemyID < _enemies.Count; enemyID++)
        {
            if (_mainCollision.Rect.CheckCollision( _enemies[enemyID].Collision))
            {
                _mainCollision.InflictDamage(_enemies[enemyID].EnemyStats.Damage);
                break;
            }
        }
    }

    private void CheckFireballCollision()
    {
        if (_fireball.IsAttacking())
        {
            if (!_fireball.DirectionPicked && _enemies.Count > 0)
            {
                _fireball.GiveDirection(_enemies[_randomGenerator.Next(0, _enemies.Count)].GlobalTransform.Position);
            }
            
            for (int enemyID = 0; enemyID < _enemies.Count; enemyID++)
            {
                if (_fireballHitted.Contains(enemyID))
                {
                    continue;
                }

                if (_fireball.Circle.CheckCollision(_enemies[enemyID].Collision))
                {
                    _fireballHitted.Add(enemyID);
                    _enemies[enemyID].InflictDamage(_fireball.WeaponStats.Damage);
                    _fireball.PiercedEnemy();

                    if (_enemies[enemyID].IsDead())
                    {
                        if (_enemies[enemyID] is BossEnemy)
                            _spawner.KillBoss();

                        foreach (var node in _enemies[enemyID].Childs)
                        {
                            _nodes.Remove(node);
                        }

                        _mainCollision.PlayerStats.CurrentExp += _enemies[enemyID].EnemyStats.Exp;
                        _nodes.Remove(_enemies[enemyID]);
                        _numAliveEnemies--;
                        _toDelete.Add(_enemies[enemyID]);
                    }
                }
            }
        }
        
        else if (!_fireball.IsAttacking() && _fireballHitted.Count > 0)
        {
            _fireballHitted.Clear();
        }

        if (_toDelete.Count > 0)
        {
            foreach (var kek in _toDelete)
            {
                _enemies.Remove(kek);
            }
            _toDelete.Clear();
        }
    }

    private void CheckWeaponCollision()
    {
        for (int weaponID = 0; weaponID < _colliders.Count; weaponID++)
        {
            if (_colliders[weaponID].IsAttacking())
            {
                for (int enemyID = 0; enemyID < _enemies.Count; enemyID++)
                {
                    if (_hittedEnemy.Contains(enemyID))
                    {
                        continue;
                    }
                    
                    if (_colliders[weaponID].Rectangle.CheckCollision(_enemies[enemyID].Collision))
                    {
                        _hittedEnemy.Add(enemyID);
                        _enemies[enemyID].InflictDamage(_colliders[weaponID].WeaponStats.Damage);
                        if (_enemies[enemyID].IsDead())
                        {
                            if (_enemies[enemyID] is BossEnemy)
                                _spawner.KillBoss();

                            foreach (var node in _enemies[enemyID].Childs)
                            {
                                _nodes.Remove(node);
                            }

                            _mainCollision.PlayerStats.CurrentExp += _enemies[enemyID].EnemyStats.Exp;
                            _nodes.Remove(_enemies[enemyID]);
                            _numAliveEnemies--;
                            _hittedEnemy.Remove(enemyID);
                            _toDelete.Add(_enemies[enemyID]);
                        }
                    }
                }
            }
            
            else if (!_colliders[weaponID].IsAttacking() && _hittedEnemy.Count > 0)
            {
                _hittedEnemy.Clear();
            }
            
            if (_toDelete.Count > 0)
            {
                foreach (var kek in _toDelete)
                {
                    _enemies.Remove(kek);
                }
                _toDelete.Clear();
            }
        }
    }

    private void RenderNodes(float delta)
    {
        foreach (var node in _nodes)
        {
            if (!_isPaused && _mainCollision.LevelUpIsHandled && !_isInputPaused)
            {
                node.Process(delta);
            }
            else if (_isPaused && node is Ui)
            {
                node.Process(delta);
            }
            else if (_isInputPaused && node is Ui)
            {
                node.Process(delta);
            }
            else if (IsPlayerAlive == false && node is Ui)
            {
                node.Process(delta);
            }
                
            if (node is IRenderable)
            {
                if (node is MeshInstance2D && node.CanRender == false)
                {
                    continue;
                }
                
                for (int viewportID = 0; viewportID < _viewports.Count; viewportID++)
                {
                    var viewport = _viewports[viewportID];
                    
                    if (node is UIElement.Body && node.Parent.Name == "PlayerHPBar")
                    {
                        _renderServer.Render(viewport, (IRenderable) node, (float) _mainCollision.PlayerStats.CurrentHealth / 100);
                    }
                    else if (node is UIElement.Body && node.Parent.Name == "PlayerEXPBar")
                    {
                        _renderServer.Render(viewport, (IRenderable)node,
                            (float)_mainCollision.PlayerStats.CurrentExp * 100 / _mainCollision.PlayerStats.ExpToLevel /
                            100);
                    }
                    else
                    {
                        _renderServer.Render(viewport, (IRenderable) node);
                    }
                }
            }
        }

        foreach (var node in _ground)
        {
            if (!_isPaused)
                node.Process(delta);

            if (node is IRenderable)
            {
                for (int viewportID = 0; viewportID < _viewports.Count; viewportID++)
                {
                    var viewport = _viewports[viewportID];
                    _renderServer.Render(viewport, (IRenderable) node);
                }
            }
        }
    }

    private void ApplyViewports()
    {
        for (int viewportID = 0; viewportID < _viewports.Count; viewportID++)
        {
            var viewport = _viewports[viewportID];
            _renderServer.ApplyEnvironment(viewport);
        }
    }

    public bool IsInTree(Node node)
    {
        foreach (var sceneNode in _nodes)
        {
            if (node == sceneNode)
            {
                return true;
            }
        }

        return false;
    }

    internal void LoadNode(Node node, string path, ShaderType type)
    {
        if (node is Enemy)
        {
            _enemies.Add((Enemy) node);
        }

        if (node is Player)
        {
            _mainCollision = (Player) node;
        }

        if (node is Weapon)
        {
            _colliders.Add((Weapon) node);
        }

        if (node is Fireball)
        {
            _fireball = (Fireball) node;
        }
            
        foreach (var child in node.Childs)
        {
            if (IsInTree(child))
            {
                Console.WriteLine($"ERROR: The node {child.Name} is already in the Scene. It will not be added.");
                return;
            }
            
            child.Scene = this;
            LoadNode(child, path, type);
        }

        if (node is IRenderable)
        {
            _renderServer.Load((IRenderable) node, path, type);
        }
        
        node.AttachInputServer(_inputServer);
        _nodes.Add(node);
        
        node.Ready();
    }
    
    internal void LoadNode(Node node)
    {
        foreach (var child in node.Childs)
        {
            if (IsInTree(child))
            {
                Console.WriteLine($"ERROR: The node {child.Name} is already in the Scene. It will not be added.");
                return;
            }

            child.Scene = this;
            LoadNode(child);
        }

        node.AttachInputServer(_inputServer);
        _nodes.Add(node);
        
        node.Ready();
    }
    
    internal void LoadGround(Node node, string path, ShaderType type)
    {
        foreach (var child in node.Childs)
        {
            if (IsInTree(child))
            {
                Console.WriteLine($"ERROR: The node {child.Name} is already in the Scene. It will not be added.");
                return;
            }
            

            child.Scene = this;
            LoadGround(child, path, type);
        }

        if (node is IRenderable)
        {
            _renderServer.Load((IRenderable) node, path, type);
        }
        
        node.AttachInputServer(_inputServer);
        if (_ground.Count < 1) _ground.Add(node);
        
        node.Ready();
    }


    protected void Input(InputEvent input)
    {
        if (_inputServer!.IsActionPressed("exit"))
        {
            _window.Close();
            return;
        }

        if (_inputServer!.IsActionPressed("pause"))
        {
            _isInputPaused = !_isInputPaused;
            Console.WriteLine((_isInputPaused ? "" : "un") + "pause");
        }

        if (!IsPlayerAlive)
        {
            if (_inputServer!.IsActionPressed("restart"))
            {
                Restart();
            }
        }

        foreach (var node in _nodes)
        {
            node.Input(input);
        }
    }


    private void Restart()
    {
        _nodes.Clear();
        Root.Childs.Clear();
        _ground.Clear();
        _nodes.Clear();
        _enemies.Clear();
        _hittedEnemy.Clear();
        _colliders.Clear();
        _timer.Start();
        Start();
        NumAliveEnemies = 0;
        IsPlayerAlive = true;
    }

    private void PlayerDied()
    {
        _timer.Stop();

        foreach (var node in _nodes)
        {
            if (node is not Ui)
            {
                _nodes.Remove(node);
            }
        }
       
        _enemies.Clear();

        IsPlayerAlive = false;

        LoadNode(_mainCollision.BodyData, "Textures/death.png", ShaderType.TextureShader);

        _mediaPlayer.Stop();
    }

    public void AttachViewport()
    {
        var viewport = new Viewport(_window, new Camera2D("MainCamera"));
        _viewports.Add(viewport);
    }
    
    public void AttachViewport(ICamera camera)
    {
        if (_viewports.Count > 0) _viewports.Clear();
        var viewport = new Viewport(_window, camera);
        _viewports.Add(viewport);
    }

    private void Repeat(object sender, EventArgs e)
    {
        _mediaPlayer.Position = TimeSpan.Zero;
        _mediaPlayer.Play();
    }

}