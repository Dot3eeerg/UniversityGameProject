using System.Reflection.Metadata;
using UniversityGameProject.GUI;
using UniversityGameProject.Input;
using UniversityGameProject.Main._2d;
using UniversityGameProject.Render;
using UniversityGameProject.Render.Camera;
using UniversityGameProject.Render.Viewport;
using UniversityGameProject.Window;

namespace UniversityGameProject.Main.Scene;

public class Scene : MainLoop
{
    private List<Node> _nodes = new List<Node>();
    private List<Viewport> _viewports = new List<Viewport>();

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
    }

    public void Run()
    {
        while (_window.Running)
        {
            _window.Render();
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

    internal void LoadNode(Node node, string path)
    {
        foreach (var child in node.Childs)
        {
            if (IsInTree(child))
            {
                Console.WriteLine($"ERROR: The node {child.Name} is already in the Scene. It will not be added.");
                return;
            }

            child.Scene = this;
            LoadNode(child, path);
        }

        if (node is IRenderable)
        {
            _renderServer.Load((IRenderable) node, path);
        }
        
        node.AttachInputServer(_inputServer);
        _nodes.Add(node);
        
        node.Ready();
    }

    protected override void Process(float delta)
    {
        base.Process(delta);
        
        _guiServer.SetupFrame(delta);
        
        _renderServer.ChangeContextSize(_window.WindowSize);

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
        
        _guiServer.RenderFrame();
    }

    protected void Input(InputEvent input)
    {
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