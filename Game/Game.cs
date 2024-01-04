using UniversityGameProject.Main.Scene;
using UniversityGameProject.Render.Material;

namespace UniversityGameProject.Game;

public class Game
{
    private Scene _scene;

    public Game()
    {
        _scene = new Scene();
        
        var weapon = new Weapon("Weapon", "Textures/swing1.png", "Textures/swing2.png");
        _scene.Root.AddChild(weapon, "Textures/swing1.png", ShaderType.TextureShader);
        
        var player = new Player("Player", "Textures/character.png", weapon);
        _scene.Root.AddChild(player, "Textures/character.png", ShaderType.TextureShader);
      
        var ground = new Ground("Ground tile", "Textures/grass1.png");
        _scene.Root.AddChild(ground, "Textures/grass1.png", ShaderType.GroundShader);

        _scene.AttachViewport(player.Camera);
        
        _scene.Run();
    }
}