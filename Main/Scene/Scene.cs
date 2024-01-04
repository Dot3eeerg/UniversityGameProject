using UniversityGameProject.Game;
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
    private List<HashSet<int>>_hittedEnemy = new List<HashSet<int>>();
    private Player _mainCollision;
    private Enemy _enemy;
    private Spawner _spawner;

    private WindowServer _window;
    private RenderServer _renderServer;
    private InputServer _inputServer;
    private GuiServer _guiServer;
    
    private Timer.Timer _timer;
    
    private int _numAliveEnemies = 0;
    public int NumAliveEnemies
    {
        get => _numAliveEnemies; 
        set => _numAliveEnemies = value;
    }
    public int MaxAliveEnemies { get => 100; }

    public Node Root { get; init; }

    public Scene() : base()
    {
        Root = new Node(this, "Root");

        _window = new WindowServer();
        _renderServer = new RenderServer(_window.GetGlContext());
        _inputServer = new InputServer(_window.GetInputContext());
        _guiServer = new GuiServer(
            _window.GetGlContext(),
            _window.GetViewContext(),
            _window.GetInputContext()
        );

        _window.OnWindowStartsRender += Process;
        _inputServer.OnInputEmited += Input;
        _timer = new Timer.Timer("Timer");
    }
    
    public long TotalTime => _timer.Time;

    public long SpawnTimer = 0;

    public void Run()
    {
        _spawner = new Spawner(this);
        _timer.Start();
        while (_window.Running)
        {
            _spawner.SpawnEnemy();
            _window.Render();
        }
    }
    
    protected override void Process(float delta)
    {
        base.Process(delta);

        _timer.Update((long)(delta * 1000));
        
        SpawnTimer += (long)(delta * 1000);
        
        _guiServer.SetupFrame(delta);
        
        _renderServer.ChangeContextSize(_window.WindowSize);

        CheckPlayerCollision();
        CheckWeaponCollision();
        
        ApplyViewports(delta);
        
        RenderNodes(delta);
        
        _guiServer.RenderFrame();
    }
    
    

    private void CheckPlayerCollision()
    {
        for (int enemyID = 0; enemyID < _enemies.Count; enemyID++)
        {
            if (_mainCollision.Circle.CheckCollision((Circle) _enemies[enemyID].Circle))
            {
                _mainCollision.InflictDamage(_enemies[enemyID].EnemyStats.Damage);
                break;
            }
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
                    if (_hittedEnemy[weaponID].Contains(enemyID))
                    {
                        continue;
                    }
                    
                    if (_colliders[weaponID].Rectangle.CheckCollision((Circle) _enemies[enemyID].Circle))
                    {
                        Console.WriteLine("Collision inflicted");
                        _hittedEnemy[weaponID].Add(enemyID);
                        _enemies[enemyID].InflictDamage(_colliders[weaponID].WeaponStats.Damage);
                        Console.WriteLine(_enemies[enemyID].IsDead());
                        if (_enemies[enemyID].IsDead())
                        {
                            foreach (var node in _enemies[enemyID].Childs)
                            {
                                _nodes.Remove(node);
                            }

                            _nodes.Remove(_enemies[enemyID]);
                            _numAliveEnemies--;
                        }
                    }
                }
            }
            
            else if (!_colliders[weaponID].IsAttacking() && _hittedEnemy[weaponID].Count > 0)
            {
                _hittedEnemy[weaponID].Clear();
            }
        }
    }

    private void RenderNodes(float delta)
    {
        foreach (var node in _nodes)
        {
            node.Process(delta);

            if (node is IRenderable)
            {
                if (node is MeshInstance2D && node.CanRender == false)
                {
                    continue;
                }
                
                for (int viewportID = 0; viewportID < _viewports.Count; viewportID++)
                {
                    var viewport = _viewports[viewportID];
                    _renderServer.Render(viewport, (IRenderable) node);
                }
            }
        }

        foreach (var node in _ground)
        {
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

    private void ApplyViewports(float delta)
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
            _hittedEnemy.Add(new HashSet<int>());
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

        foreach (var node in _nodes)
        {
            node.Input(input);
        }
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
}