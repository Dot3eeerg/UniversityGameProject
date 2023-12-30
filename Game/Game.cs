using UniversityGameProject.Main.Scene;

namespace UniversityGameProject.Game;

public class Game
{
    private Scene _scene;

    public Game()
    {
        _scene = new Scene();

        var player = new Player("Player", "Textures/zaika.jpg");
        _scene.Root.AddChild(player, "Textures/zaika.jpg");
        
        var ground = new Ground("Ground tile", "Textures/kek.jpg");
        _scene.Root.AddChild(ground, "Textures/kek.jpg");
        
        _scene.AttachViewport(player.Camera);
        
        _scene.Run();
    }
}