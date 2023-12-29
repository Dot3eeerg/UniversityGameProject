﻿using UniversityGameProject.Main.Scene;

namespace UniversityGameProject.Game;

public class Game
{
    private Scene _scene;

    public Game()
    {
        _scene = new Scene();

        var ground = new Ground("Ground tile", "Textures/kek.jpg");
        _scene.Root.AddChild(ground, "Textures/kek.jpg");
        
        _scene.AttachViewport();
        
        _scene.Run();
    }
}