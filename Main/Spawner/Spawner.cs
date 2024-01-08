using Silk.NET.Vulkan;
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

    private int _enemyID = 0;

    private SpawnData[] _spawnRates;

    private int[] _rateTimings;

    private int _spawnRateIdx = 0;

    private long _lastSpawnTime = 0;

    private long[] _spawnTimers = { 0, 0, 0 };
    private int _numRates;

    public Spawner(Scene.Scene scene)
    {
        _scene = scene;
        _player = (Player)_scene.Root.Childs[7];

        _rateTimings = [ 0, 15, 40, 70, 105, 140, 180 ];
        int[] ghostRates = { 0, 400, 1000, 2000, 3000, 3000, 800 };
        int[] slimeRates = { 0,    0,   1500, 1000, 1500, 3000, 1600 };
        int[] giantRates = { 0,    0,   0,    0,    4000, 2000, 0 };

        _spawnRates =
        [
            new(ghostRates, EnemyType.GhostEnemy),
            new(slimeRates, EnemyType.SlimeEnemy),
            new(giantRates, EnemyType.GiantEnemy)
        ];

        _numRates = _rateTimings.Count();
    }


    public void SpawnEnemy()
    {
        long delta = _scene.TotalTime - _lastSpawnTime;
        _lastSpawnTime = _scene.TotalTime;
        bool spawned = false;

        UpgradeTimers(delta);

        GetSpawnRateIdx();

        if (!_bossSpawned && _spawnRateIdx == _numRates - 1)
        {
            SpawnBoss();
            spawned = true;
        }

        if (_scene.NumAliveEnemies < _scene.MaxAliveEnemies)
        {
            for (int i = 0; i < 3; i++)
            {
                int spawnRate = _spawnRates[i].SpawnRates[_spawnRateIdx];
                var enemyType = _spawnRates[i].Type;
                while (_spawnTimers[i] >= spawnRate && spawnRate != 0)
                {

                    SpawnEnemy(enemyType);

                    _spawnTimers[i] -= spawnRate;
                    spawned = true;

                }
            }
        }

        if (spawned)
        {
            _scene.Root.AddChild(new Ground("G", "Textures/grass1.png"), "Textures/grass1.png", ShaderType.GroundShader);
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

    private void SpawnEnemy(EnemyType type)
    {
        float randomAngle = _random.NextSingle() * 2.0f * 3.14f;
        float distX = _screenDist * (float)System.Math.Cos(randomAngle);
        float distY = _screenDist * (float)System.Math.Sin(randomAngle);
        var pos = _player.BodyData.GlobalTransform.Position + new Vector3(distX > 0.55f ? 0.55f : distX, distY > 0.55f ? 0.55f : distY, 0);

        var name = $"Enemy{_enemyID++}";

        Enemy enemy;

        switch (type)
        {
            case EnemyType.GhostEnemy:
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
    }

    private void GetSpawnRateIdx()
    {
        if (_spawnRateIdx < _spawnRates.Count() - 1)
            if (_lastSpawnTime >= _rateTimings[_spawnRateIdx + 1] * 1000)
            {
                _spawnRateIdx++;
                ResetTimers();
            }
    }

    private void UpgradeTimers(long delta)
    {
        for (int i = 0; i < _spawnTimers.Length; i++)
        {
            _spawnTimers[i] += delta;
        }
    }

    private void ResetTimers()
    {
        for (int i = 0; i < _spawnTimers.Length; i++)
        {
            _spawnTimers[i] = 0;
        }
    }

    private enum EnemyType
    {
        GhostEnemy,
        SlimeEnemy,
        GiantEnemy,
        BossEnemy
    }

    private struct SpawnData
    {
        public int[] SpawnRates;
        public EnemyType Type;

        public SpawnData(int[] spawnRates, EnemyType type)
        {
            SpawnRates = spawnRates;
            Type = type;
        }
    }
}
