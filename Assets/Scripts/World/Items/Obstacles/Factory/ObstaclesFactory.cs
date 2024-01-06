using Cysharp.Threading.Tasks;
using RSR.InternalLogic;
using RSR.Player;
using RSR.ServicesLogic;
using System.Collections.Generic;
using UnityEngine;

namespace RSR.World
{
    /// <summary>
    /// Spawns obstacles at position by ObstacleType in Create method.
    /// Spawn position Y is defined in obstacles settings.
    /// Obstacles are spawned regulary, so factory uses objects pools to reuse already instantiated ones.
    /// To add new obstacle type you don't need to change any existing logic.
    /// All you need to do is inherit from Obstacle base class and add label "Obstacle" to you addressbales prefab.
    /// If you want your obstacle to be spawned at CreateRandomObstacle method, set it's weight at obstacles' settings.
    /// </summary>
    public sealed class ObstaclesFactory : ItemsFactory, IObstaclesFactory
    {
        private readonly IObstaclesSettingsProvider _settingsProvider;
        private readonly IRandomService _randomService;
        private readonly IPlayerDeath _playerDeath;

        //Inner obstacles weights table, initialized from obstacles' settings data, where we can set weights values.
        private readonly Dictionary<int, ObstacleType> _obstaclesRandomWeightsTable = new();

        //Storage with a pool for each obstacle type.
        private readonly Dictionary<ObstacleType, ObjectsPool<PoolableItem>> _obstaclesStorage = new();

        private readonly float _obstaclesSpawnHeight;

        public ObstaclesFactory(IAssetsProvider assetsProvider, IObstaclesSettingsProvider settingsProvider, IRandomService randomService, IPlayerDeath playerDeath)
        {
            _settingsProvider = settingsProvider;
            _assetsProvider = assetsProvider;
            _randomService = randomService;

            _obstaclesSpawnHeight = settingsProvider.ObstaclesSettings.obstaclesSpawnHeight;
        }

        #region Spawning

        public void Create(ObstacleType obstacle, Vector3 pos)
        {
            if (!_obstaclesStorage.ContainsKey(obstacle))
            {
                Debug.Log($"Creating a {obstacle} failed! No such obstacle in storage.");
                return;
            }

            pos.y = _obstaclesSpawnHeight;
            _obstaclesStorage[obstacle].Get(pos);
        }

        public void CreateRandom(Vector3 pos)
        {
            Create(_randomService.GetWeightedRandomValue(_obstaclesRandomWeightsTable), pos);
        }
        #endregion

        #region Initialization

        public async UniTask Initialize()
        {
            await LoadPrefabs(AssetsKeys.ObstaclesLabel);
            InitStorage();
            InitRndWeightsTable();
        }

        //Fulfills obstacles' storage with pools for each obstacle type from all addressables "Obstacle"-labled prefabs.
        private void InitStorage()
        {
            foreach (var prefab in _prefabs)
            {
                var obstacle = prefab.GetComponent<Obstacle>();

                if (_obstaclesStorage.ContainsKey(obstacle.Type))
                {
                    Debug.Log($"Failed to add obstacle! Reason: {obstacle.Type} duplicate.");
                    continue;
                }

                ConstructObstaclePrefab(obstacle);
                AddToStorage(obstacle);
            }
        }

        private void AddToStorage(Obstacle obstacle)
        {
            _obstaclesStorage.Add(obstacle.Type, new ObjectsPool<PoolableItem>(obstacle, 3, 10, $"{obstacle.Type} obstacles pool"));
            obstacle.SetPool(_obstaclesStorage[obstacle.Type]);
        }

        //Provides obstacle with necessary dependencies.
        private void ConstructObstaclePrefab(Obstacle obstacle)
        {
            obstacle.Consturct(_playerDeath);
        }

        private void InitRndWeightsTable()
        {
            int obstaclesCount = _settingsProvider.ObstaclesSettings.obstaclesRandomWeights.Length;

            for (int i = 0; i < obstaclesCount; i++)
            {
                _obstaclesRandomWeightsTable.TryAdd(_settingsProvider.ObstaclesSettings.obstaclesRandomWeights[i].weight, _settingsProvider.ObstaclesSettings.obstaclesRandomWeights[i].obstacle);
            }
        }
        #endregion
    }
}