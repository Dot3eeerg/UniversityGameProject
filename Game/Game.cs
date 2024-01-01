using UniversityGameProject.Main.Scene;
using UniversityGameProject.Render.Material;

namespace UniversityGameProject.Game;

public class Game
{
    private Scene _scene;

    public Game()
    {
        _scene = new Scene();

        var player = new Player("Player", "Textures/zaika.jpg");
        _scene.Root.AddChild(player, "Textures/zaika.jpg", ShaderType.TextureShader);
        
        var ground = new Ground("Ground tile", "Textures/kek.jpg");
        _scene.Root.AddChild(ground, "Textures/kek.jpg", ShaderType.GroundShader);
        
        _scene.AttachViewport(player.Camera);
        
        _scene.Run();
    }
}