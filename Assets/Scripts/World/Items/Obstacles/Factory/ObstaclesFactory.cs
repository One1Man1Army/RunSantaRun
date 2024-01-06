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
        #region Fields
        private readonly IObstaclesSettingsProvider _settingsProvider;
        private readonly IRandomService _randomService;
        private readonly PlayerFacade _playerFacade;

        //Inner obstacles weights table, initialized from obstacles' settings data, where we can set weights values.
        private readonly Dictionary<int, ObstacleType> _obstaclesRandomWeightsTable = new();

        //Storage with a pool for each obstacle type.
        private readonly Dictionary<ObstacleType, ObjectsPool<PoolableItem>> _obstaclesStorage = new();
        #endregion

        public ObstaclesFactory(IAssetsProvider assetsProvider, IObstaclesSettingsProvider settingsProvider, IRandomService randomService, PlayerFacade playerFacade)
        {
            _settingsProvider = settingsProvider;
            _assetsProvider = assetsProvider;
            _randomService = randomService;
            _playerFacade = playerFacade;
        }

        #region Spawning

        //Spawns obstacle and provides it with all necessary dependencies.
        public void Create(ObstacleType obstacle, Vector3 pos)
        {
            if (!_obstaclesStorage.ContainsKey(obstacle))
            {
                Debug.Log($"Creating a {obstacle} failed! No such obstacle in storage.");
                return;
            }

            switch(obstacle)
            {
                case ObstacleType.Low:
                    pos.y = _settingsProvider.ObstaclesSettings.lowSpawnHeight; 
                    break;
                case ObstacleType.High:
                    pos.y = _settingsProvider.ObstaclesSettings.highSpawnHeight;
                    break;
            }

            var instance = _obstaclesStorage[obstacle].Get(pos) as Obstacle;
            instance.Consturct(_settingsProvider, _playerFacade.Death, _playerFacade.MoveDirReporter);
            instance.SetPool(_obstaclesStorage[obstacle]);
        }

        public void CreateRandom(Vector3 pos)
        {
            Create(_randomService.GetWeightedRandomValue(_obstaclesRandomWeightsTable), pos);
        }

        public void ReleaseAll()
        {
            foreach (var pool in _obstaclesStorage.Values)
            {
                pool.ReleaseAll();
            }
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
        protected override void InitStorage()
        {
            if (_prefabs == null || _prefabs.Count == 0)
                return;

            foreach (var prefab in _prefabs)
            {
                var obstacle = prefab.GetComponent<Obstacle>();

                if (_obstaclesStorage.ContainsKey(obstacle.Type))
                {
                    Debug.Log($"Failed to add obstacle! Reason: {obstacle.Type} duplicate.");
                    continue;
                }

                _obstaclesStorage.Add(obstacle.Type, new ObjectsPool<PoolableItem>(obstacle, 3, 10, $"{obstacle.Type} obstacles pool"));
            }
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