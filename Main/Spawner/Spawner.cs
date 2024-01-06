using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
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

    private bool _bossSpawned = false;

    public Spawner(Scene.Scene scene)
    {
        _scene = scene;
        _player = (Player)_scene.Root.Childs[6];
    }

    private int _enemyID = 0;


    private SpawnData[] _spawnRates =
    {
        new (0, 500, EnemyType.HeadEnemy),
        new (10, 1000, EnemyType.SlimeEnemy),
        new (20, 2000, EnemyType.GiantEnemy),
        new (30, 500, EnemyType.BossEnemy)
    };

    private int spawnRate = 1;


    public void SpawnEnemy()
    {
        bool spawned = false;
        var spawnRateType = getSpawnRateIdx();

        if (!_bossSpawned && spawnRateType == EnemyType.BossEnemy)
        {
            SpawnBoss();
        }

        while (_scene.SpawnTimer > spawnRate)
        {
            if (_scene.NumAliveEnemies >= _scene.MaxAliveEnemies)
            {
                break;
            }

            float randomAngle = _random.NextSingle() * 2.0f * 3.14f;
            float distX = _screenDist * (float)System.Math.Cos(randomAngle);
            float distY = _screenDist * (float)System.Math.Sin(randomAngle);
            var pos = _player.BodyData.GlobalTransform.Position + new Vector3(distX > 0.55f ? 0.55f : distX, distY > 0.55f ? 0.55f : distY, 0);

            var name = $"Enemy{_enemyID++}";

            Enemy enemy;

            switch (spawnRateType)
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
                default:
                    enemy = new HeadEnemy(name, _player.BodyData);
                    break;
            }


            _scene.Root.AddChild(enemy, enemy.TexturePath, ShaderType.TextureShader);
            enemy.Translate(pos);
            _scene.NumAliveEnemies++;

            _scene.SpawnTimer -= spawnRate;
            spawned = true;
        }

        if (spawned)
        {
            var ground = new Ground("Ground tile", "Textures/grass1.png");
            _scene.Root.AddChild(ground, "Textures/grass1.png", ShaderType.GroundShader);
        }
    }

    private void SpawnBoss()
    {
        var enemy = new BossEnemy("BossEnemy", _player.BodyData);
        float randomAngle = _random.NextSingle() * 2.0f * 3.14f;
        var pos = _player.BodyData.GlobalTransform.Position + _screenDist * new Vector3((float)System.Math.Cos(randomAngle), (float)System.Math.Sin(randomAngle), 0);

        _scene.Root.AddChild(enemy, enemy.TexturePath, ShaderType.TextureShader);
        enemy.Translate(pos);
        _bossSpawned = true;
    }

    private EnemyType getSpawnRateIdx()
    {
        for (int i = 0; i < _spawnRates.Length; i++)
        {
            if (_spawnRates[i].Time > _scene.TotalTime / 1000)
            {
                spawnRate = _spawnRates[i - 1].SpawnRate;
                return _spawnRates[i - 1].Type;
            }
        }

        spawnRate = _spawnRates[^1].SpawnRate;
        return _spawnRates[^1].Type;
    }

    private enum EnemyType
    {
        HeadEnemy,
        SlimeEnemy,
        GiantEnemy,
        BossEnemy
    }

    private struct SpawnData
    {
        public int Time;
        public int SpawnRate;
        public EnemyType Type;

        public SpawnData(int time, int spawnRate, EnemyType type)
        {
            Time = time;
            SpawnRate = spawnRate;
            Type = type;
        }
    }
}
