using UniversityGameProject.Main._2d;
using UniversityGameProject.Main.Scene;
using UniversityGameProject.Render.Material;

namespace UniversityGameProject.Game;

public class Game
{
    private Scene _scene;

    public Game()
    {
        _scene = new Scene();

        var weaponList = new List<Weapon>();

        var weapon1 = new Weapon("Weapon", "Textures/swing2.png");
        _scene.Root.AddChild(weapon1, "Textures/swing2.png", ShaderType.TextureShader);
        
        var weapon2 = new Weapon("Weapon", "Textures/swing2_left.png");
        _scene.Root.AddChild(weapon2, "Textures/swing2_left.png", ShaderType.TextureShader);
        
        var weapon3 = new Weapon("Weapon", "Textures/swing2.png");
        _scene.Root.AddChild(weapon3, "Textures/swing2.png", ShaderType.TextureShader);
        
        var weapon4 = new Weapon("Weapon", "Textures/swing2_left.png");
        _scene.Root.AddChild(weapon4, "Textures/swing2_left.png", ShaderType.TextureShader);
        
        weaponList.Add(weapon1);
        weaponList.Add(weapon2);
        weaponList.Add(weapon3);
        weaponList.Add(weapon4);
        
        var player = new Player("Player", "Textures/character.png");

        var fireball = new Fireball("Fireball", "Textures/fireball.png", player.BodyData);
        _scene.Root.AddChild(fireball, "Textures/fireball.png", ShaderType.TextureShader);
        
        var ui = new UIElement("PlayerHPBar", "Textures/health_new.png");
        _scene.Root.AddChild(ui,"Textures/health_new.png", ShaderType.HealthBarShader);

        var xp = new UIElement("PlayerEXPBar", "Textures/xp.png");
        _scene.Root.AddChild(xp,"Textures/xp.png", ShaderType.ExpBarShader);
        
        player.LoadWeapons(weaponList, fireball);
        player.LoadHPBar(ui);
        player.LoadEXPBar(xp);

        _scene.Root.AddChild(player, "Textures/character.png", ShaderType.TextureShader);
      
        var ground = new Ground("Ground tile", "Textures/grass3.png");
        _scene.Root.AddChild(ground, "Textures/grass3.png", ShaderType.GroundShader);

        _scene.AttachViewport(player.Camera);
        
        _scene.Run();
    }
}