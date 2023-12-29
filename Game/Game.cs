using UniversityGameProject.Main.Scene;

namespace UniversityGameProject.Game;

public class Game
{
    private Scene _scene;

    public Game()
    {
        _scene = new Scene();
        
        _scene.AttachViewport();
        
        _scene.Run();
    }
}