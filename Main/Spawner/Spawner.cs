using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UniversityGameProject.Game;
using UniversityGameProject.Main._2d;
using ShaderType = UniversityGameProject.Render.Material.ShaderType;

namespace UniversityGameProject.Main;

public class Spawner
{
    private Scene.Scene _scene;

    private float _screenDist = 0.9f;

    private Random _random = new Random();

    private Player _player;

    public Spawner(Scene.Scene scene)
    {
        _scene = scene;
        _player = (Player)_scene.Root.Childs[0];
    }

    private int _enemyID = 0;

    private SpawnData[] _spawnRates =
    {
        new (0, EnemyType.HeadEnemy),
        new (10, EnemyType.SlimeEnemy),
        new (20, EnemyType.GiantEnemy),
        new (30, EnemyType.BossEnemy)
    };


    public void SpawnEnemy()
    {
        if (_scene.TotalTime / 500 >= _scene.NumAliveEnemies && _scene.NumAliveEnemies < _scene.MaxAliveEnemies)
        {
            var spawnRateIdx = getSpawnRateIdx();

            float randomAngle = _random.NextSingle() * 2.0f * 3.14f;
            var pos = _player.BodyData.GlobalTransform.Position + _screenDist * new Vector3((float)System.Math.Cos(randomAngle), (float)System.Math.Sin(randomAngle), 0);

            var name = $"Enemy{_enemyID++}";

            Enemy enemy;
            
            switch (spawnRateIdx)
            {
                case EnemyType.HeadEnemy:
                    enemy = new HeadEnemy(name, _player.BodyData);
                    break;
                case EnemyType.SlimeEnemy:
                    enemy = new SlimeEnemy(name, _player.BodyData);
                    break;
                case EnemyType.GiantEnemy:
                    enemy = new GiantEnemy(name, _player.BodyData);
                    break;
                case EnemyType.BossEnemy:
                    enemy = new BossEnemy(name, _player.BodyData);
                    break;
                default:
                    enemy = new HeadEnemy(name, _player.BodyData);
                    break;
            }



            _scene.Root.AddChild(enemy, enemy.TexturePath, ShaderType.TextureShader);
            enemy.Translate(pos);

            var ground = new Ground("Ground tile", "Textures/grass1.png");
            _scene.Root.AddChild(ground, "Textures/grass1.png", ShaderType.GroundShader);
            _scene.NumAliveEnemies++;
        }
    }

    private EnemyType getSpawnRateIdx()
    {
        for (int i = 0; i < _spawnRates.Length; i++)
        {
            if (_spawnRates[i].Time > _scene.TotalTime / 1000)
            {
                return _spawnRates[i - 1].Type;
            }
        }
        return _spawnRates[^1].Type;
    }

    private enum EnemyType
    {
        HeadEnemy,
        SlimeEnemy,
        GiantEnemy,
        BossEnemy
    }

    private struct SpawnData(int time, EnemyType type)
    {
        public int Time = time;
        public EnemyType Type = type;
    }
}
