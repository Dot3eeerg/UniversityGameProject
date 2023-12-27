using UniversityGameProject.Core.Viewport;
using UniversityGameProject.Core.Window;
using UniversityGameProject.Game.GUI;
using UniversityGameProject.Input;

namespace UniversityGameProject.Main.Scene;

public class Scene : MainLoop
{
    private List<Node> _nodes = new List<Node>();
    private List<Viewport> _viewports = new List<Viewport>();
    
    private WindowServer _window;
    // render server
    private InputServer _inputServer;
    private GUI _gui;
    
    public Node Root { get; init; }

    public Scene() : base()
    {
        Root = new Node(this, "Root");

        _window = new WindowServer();
        // render server
        _inputServer = new InputServer();
        //_gui = new GUI();
    }
    
    public bool IsInTree(Node child)
    {
        return false;
    }
    
    public void AddChild(Node child) { }
    
    public void LoadNode(Node child) { }
}