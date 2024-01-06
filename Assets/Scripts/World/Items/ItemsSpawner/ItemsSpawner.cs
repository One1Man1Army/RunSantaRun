using RSR.Player;
using RSR.ServicesLogic;
using System.Collections.Generic;
using UnityEngine;

namespace RSR.World
{
    public sealed class ItemsSpawner : MonoBehaviour, IItemsSpawner
    {
        private IRandomService _randomService;
        private IWorldStarter _worldStarter;
        private IGameSettingsProvider _settingsProvider;
        private IBoostersFactory _boostersFactory;
        private IObstaclesFactory _obstaclesFactory;
        private IPlayerDeath _playerDeath;
        private Transform _player;

        //Inner content weights table, initialized from games' settings data, where we can set weights values.
        private readonly Dictionary<int, ItemType> _itemsRandomWeightsTable = new();

        private float _nextItemSpawnTime;
        private float _timer;
        private bool _canSpawn;

        public void Construct(IGameSettingsProvider settingsProvider, IRandomService randomService, IWorldStarter worldStarter, IBoostersFactory boostersFactory, IObstaclesFactory obstaclesFactory, PlayerFacade player)
        {
            _randomService = randomService;
            _worldStarter = worldStarter;
            _settingsProvider = settingsProvider;
            _boostersFactory = boostersFactory;
            _obstaclesFactory = obstaclesFactory;
            _player = player.transform;
            _playerDeath = player.Death;

            _worldStarter.OnStart += EnableSpawn;
            _worldStarter.OnStart += SetTimer;
            _worldStarter.OnReady += ReleaseAll;
            _playerDeath.OnPlayerDeath += DisableSpawn;

            InitRndWeightsTable();
        }

        private void Update()
        {
            if (!_canSpawn)
                return;

            _timer += Time.deltaTime;
            if (_timer > _nextItemSpawnTime)
            {
                SetTimer();
                SpawnRandomItem();
            }
        }

        private void SetTimer()
        {
            _timer = 0;

            _nextItemSpawnTime = _randomService.GetRange(
                _settingsProvider.GameSettings.spawnCooldownMin,
                _settingsProvider.GameSettings.spawnCooldownMax);
        }

        private void SpawnRandomItem()
        {
            var pos = _player.position;
            pos.x += _settingsProvider.GameSettings.spawnToPlayerOffset;

            var item = _randomService.GetWeightedRandomValue(_itemsRandomWeightsTable);

            switch (item)
            {
                case ItemType.Obstacle:
                    _obstaclesFactory.CreateRandom(pos);
                    break;
                case ItemType.Booster:
                    _boostersFactory.CreateRandom(pos);
                    break;
            }
        }

        private void InitRndWeightsTable()
        {
            for (int i = 0; i < _settingsProvider.GameSettings.itemsRandomWeights.Length; i++)
            {
                _itemsRandomWeightsTable.TryAdd(_settingsProvider.GameSettings.itemsRandomWeights[i].weight, _settingsProvider.GameSettings.itemsRandomWeights[i].item);
            }
        }

        private void ReleaseAll()
        {
            Debug.Log("Released All");
            _boostersFactory.ReleaseAll();
            _obstaclesFactory.ReleaseAll();
        }

        private void EnableSpawn()
        {
            _canSpawn = true;
        }

        private void DisableSpawn()
        {
            _canSpawn = false;
        }

        private void OnDestroy()
        {
            _worldStarter.OnStart -= EnableSpawn;
            _worldStarter.OnStart -= SetTimer;
            _worldStarter.OnReady -= ReleaseAll;
            _playerDeath.OnPlayerDeath -= DisableSpawn;
        }
    }
}