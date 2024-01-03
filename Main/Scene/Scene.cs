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
    private List<Node> _nodes = new List<Node>();
    private List<Viewport> _viewports = new List<Viewport>();

    private List<Node> _ground = new List<Node>();
    private List<Enemy> _enemies = new List<Enemy>();
    private List<Rectangle> _colliders = new List<Rectangle>();
    private Player _mainCollision;
    private Enemy _enemy;

    private WindowServer _window;
    private RenderServer _renderServer;
    private InputServer _inputServer;
    private GuiServer _guiServer;


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

    private Timer.Timer _timer;

    public void Run()
    {

        _timer.Start();
        int enemies = 0;
        while (_window.Running)
        {
            SpawnEnemy(_timer.Time, ref enemies);
            _window.Render();
        }
    }

    private void SpawnEnemy(long ms, ref int enemies)
    {
        if (ms / 5000 >= enemies)
        {
            var player = (Player)Root.Childs[0];
            //_enemy = new SlimeEnemy($"SlimeEnemy{enemies}", player.BodyData);
            //this.Root.AddChild(_enemy, _enemy.TexturePath, ShaderType.TextureShader);
            //_enemy.Translate(-0.2f, 0.0f, 0.0f);
            _enemy = new HeadEnemy($"HeadEnemy{enemies++}", player.BodyData);
            this.Root.AddChild(_enemy, _enemy.TexturePath, ShaderType.TextureShader);
            _enemy.Translate(0.0f, 0.2f, 0.0f);
            _enemy = new SlimeEnemy($"SlimeEnemy{enemies++}", player.BodyData);
           this.Root.AddChild(_enemy, _enemy.TexturePath, ShaderType.TextureShader);
           _enemy.Translate(0.2f, 0.0f, 0.0f);
        }

    }
    
    protected override void Process(float delta)
    {
        base.Process(delta);

        _timer.Update((long)(delta * 1000));
        
        _guiServer.SetupFrame(delta);
        
        _renderServer.ChangeContextSize(_window.WindowSize);

        CheckPlayerCollision();
        
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

    private void RenderNodes(float delta)
    {
        for (int nodeID = 0; nodeID < _nodes.Count; nodeID++)
        {
            var node = _nodes[nodeID];
            
            node.Process(delta);

            if (node is IRenderable)
            {
                for (int viewportID = 0; viewportID < _viewports.Count; viewportID++)
                {
                    var viewport = _viewports[viewportID];
                    _renderServer.Render(viewport, (IRenderable)node);
                }
            }
        }

        for (int nodeID = 0; nodeID < _ground.Count; nodeID++)
        {
            var node = _ground[nodeID];
            
            node.Process(delta);

            if (node is IRenderable)
            {
                for (int viewportID = 0; viewportID < _viewports.Count; viewportID++)
                {
                    var viewport = _viewports[viewportID];
                    _renderServer.Render(viewport, (IRenderable)node);
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
        _ground.Add(node);
        
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
        var viewport = new Viewport(_window, camera);
        _viewports.Add(viewport);
    }
}