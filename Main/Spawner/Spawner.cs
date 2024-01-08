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
        _player = (Player)_scene.Root.Childs[7];
    }

    private int _enemyID = 0;


    private SpawnData[] _spawnRates =
    {
        new (0, 500, EnemyType.HeadEnemy),
        new (15, 1000, EnemyType.SlimeEnemy),
        new (30, 2500, EnemyType.GiantEnemy),
        new (50, 1000, EnemyType.BossEnemy)
    };


    private int _spawnRateIdx = 0;

    public void SpawnEnemy()
    {
        bool spawned = false;
        getSpawnRateIdx();

        var spawnRateType = _spawnRates[_spawnRateIdx].Type;
        var _spawnRate = _spawnRates[_spawnRateIdx].SpawnRate;

        if (!_bossSpawned && _spawnRates[_spawnRateIdx].Type == EnemyType.BossEnemy)
        {
            SpawnBoss();
        }

        while (_scene.SpawnTimer > _spawnRate)
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
                    enemy = new GhostEnemy(name, _player.BodyData);
                    break;
                case EnemyType.SlimeEnemy:
                    enemy = new SlimeEnemy(name, _player.BodyData);
                    break;
                case EnemyType.GiantEnemy:
                    enemy = new GiantEnemy(name, _player.BodyData);
                    break;
                default:
                    enemy = new GhostEnemy(name, _player.BodyData);
                    break;
            }


            _scene.Root.AddChild(enemy, enemy.TexturePath, ShaderType.TextureShader);
            enemy.Translate(pos);
            _scene.NumAliveEnemies++;

            _scene.SpawnTimer -= _spawnRate;
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

    private void getSpawnRateIdx()
    {
        if (_spawnRateIdx < _spawnRates.Count() - 1)
            if (_scene.TotalTime >= _spawnRates[_spawnRateIdx + 1].Time * 1000)
            {
                _spawnRateIdx++;
            }
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
