using RSR.ServicesLogic;
using System.Collections.Generic;
using UnityEngine;

namespace RSR.World
{
    public sealed class ContentSpawner : MonoBehaviour, IContentSpawner
    {
        private IRandomService _randomService;
        private IWorldStarter _worldStarter;
        private IGameSettingsProvider _settingsProvider;
        private IBoostersFactory _boostersFactory;
        private Transform _player;

        //Inner content weights table, initialized from games' settings data, where we can set weights values.
        private readonly Dictionary<int, ContentType> _contentRandomWeightsTable = new();

        private float _nextItemSpawnTime;
        private float _timer;
        private bool _canSpawn;

        public void Construct(IGameSettingsProvider settingsProvider, IRandomService randomService, IWorldStarter worldStarter, IBoostersFactory boostersFactory, Transform player)
        {
            _randomService = randomService;
            _worldStarter = worldStarter;
            _settingsProvider = settingsProvider;
            _boostersFactory = boostersFactory;
            _player = player;

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
                _settingsProvider.GameSettings.spawnCooldownMin);
        }

        void SpawnRandomItem()
        {
            var pos = _player.position;
            pos.x += _settingsProvider.GameSettings.spawnToPlayerOffset;

            var item = _randomService.GetWeightedRandomValue(_contentRandomWeightsTable);

            switch (item)
            {
                case ContentType.Obstacle:
                    break;

                case ContentType.Booster:
                    _boostersFactory.CreateRandom(pos);
                    break;
            }
        }

        private void InitRndWeightsTable()
        {
            for (int i = 0; i < _settingsProvider.GameSettings.contentRandomWeights.Length; i++)
            {
                _contentRandomWeightsTable.TryAdd(_settingsProvider.GameSettings.contentRandomWeights[i].weight, _settingsProvider.GameSettings.contentRandomWeights[i].content);
            }
        }
    }
}