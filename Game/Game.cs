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

        //var enemy = new SlimeEnemy("SlimeEnemy", player.BodyData);
        //_scene.Root.AddChild(enemy, enemy.TexturePath, ShaderType.TextureShader);
        //enemy.Translate(1.0f, 1.0f, 0.0f);

        //var enemy2 = new HeadEnemy("HeadEnemy", player.BodyData);
        //_scene.Root.AddChild(enemy2, enemy2.TexturePath, ShaderType.TextureShader);
        //enemy2.Translate(1.0f, 1.0f, 0.0f);

        var ground = new Ground("Ground tile", "Textures/grass1.png");
        _scene.Root.AddChild(ground, "Textures/grass1.png", ShaderType.GroundShader);
        
        _scene.AttachViewport(player.Camera);
        
        _scene.Run();
    }
}