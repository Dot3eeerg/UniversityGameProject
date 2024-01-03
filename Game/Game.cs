using UniversityGameProject.Main.Scene;
using UniversityGameProject.Render.Material;

namespace UniversityGameProject.Game;

public class Game
{
    private Scene _scene;

    public Game()
    {
        _scene = new Scene();
        
        var player = new Player("Player", "Textures/character.png");
        _scene.Root.AddChild(player, "Textures/character.png", ShaderType.TextureShader);
      
        var weapon = new Weapon("Weapon", "Textures/swing1.png", "Textures/swing2.png");
        _scene.Root.AddChild(weapon, "Textures/swing1.png", ShaderType.TextureShader);

        var enemy = new Enemy("Enemy", "Texture/slime.png", player.BodyData);
        _scene.Root.AddChild(enemy, "Textures/slime.png", ShaderType.TextureShader);
        enemy.Translate(1.0f, 1.0f, 0.0f);
        
        var ground = new Ground("Ground tile", "Textures/grass1.png");
        _scene.Root.AddChild(ground, "Textures/grass1.png", ShaderType.GroundShader);
        
        _scene.AttachViewport(player.Camera);
        
        _scene.Run();
    }
}