using Cysharp.Threading.Tasks;
using RSR.InternalLogic;
using RSR.Player;
using RSR.ServicesLogic;
using System.Collections.Generic;
using UnityEngine;

namespace RSR.World
{
    /// <summary>
    /// Spawns boosters at position by BoosterType in Create method.
    /// Spawn position Y is defined in boosters settings.
    /// Boosters are spawned regulary, so factory uses objects pools to reuse already instantiated ones.
    /// To add new booster type you don't need to change any existing logic.
    /// All you need to do is inherit from Booster base class and IInteractable interface, add label "Booster" to you addressbales prefab and add your boosters' initialization logic to ConstructBoosterPrefab method.
    /// Add your booster's settings to BoostersSettings if needed.
    /// If you want your booster to be spawned at CreateRandomBooster method, set it's weight at boosters' settings.
    /// </summary>
    public sealed class BoostersFactory : ItemsFactory, IBoostersFactory
    {
        private readonly IBoostersSettingsProvider _settingsProvider;
        private readonly IPlayerSpeedMultiplyer _playerSpeedMultiplyer;
        private readonly IRandomService _randomService;

        //Inner boosters weights table, initialized from boosters' settings data, where we can set weights values.
        private readonly Dictionary<int, BoosterType> _boostersRandomWeightsTable = new();

        //Storage with a pool for each booster type.
        private readonly Dictionary<BoosterType, ObjectsPool<PoolableItem>> _boostersStorage = new();

        private readonly float _boostersSpawnHeight;

        public BoostersFactory(IAssetsProvider assetsProvider, IBoostersSettingsProvider settingsProvider, IRandomService randomService, PlayerFacade player) 
        {
            _settingsProvider = settingsProvider;
            _assetsProvider = assetsProvider;
            _randomService = randomService;
            _playerSpeedMultiplyer = player.SpeedMultiplyer;

            _boostersSpawnHeight = settingsProvider.BoostersSettings.boostersSpawnHeight;
        }

        #region Spawning

        public void Create(BoosterType booster, Vector3 pos)
        {
            if (!_boostersStorage.ContainsKey(booster))
            {
                Debug.Log($"Creating a {booster} failed! No such booster in storage.");
                return;
            }

            pos.y = _boostersSpawnHeight;
            _boostersStorage[booster].Get(pos);
        }

        public void CreateRandom(Vector3 pos)
        {
            Create(_randomService.GetWeightedRandomValue(_boostersRandomWeightsTable), pos);
        }
        #endregion

        #region Initialization

        public async UniTask Initialize()
        {
            await LoadPrefabs(AssetsKeys.BoostersLabel);
            InitStorage();
            InitRndWeightsTable();
        }

        //Fulfills boosters' storage with pools for each booster type from all addressables "Booster"-labled prefabs.
        private void InitStorage()
        {
            foreach (var prefab in _prefabs)
            {
                var booster = prefab.GetComponent<Booster>();

                if (_boostersStorage.ContainsKey(booster.Type))
                {
                    Debug.Log($"Failed to add booster! Reason: {booster.Type} duplicate.");
                    continue;
                }

                ConstructBoosterPrefab(booster);
                AddToStorage(booster);
            }
        }

        private void AddToStorage(Booster booster)
        {
            _boostersStorage.Add(booster.Type, new ObjectsPool<PoolableItem>(booster, 3, 10, $"{booster.Type} boosters pool"));
            booster.SetPool(_boostersStorage[booster.Type]);
        }

        //Provides booster with necessary dependencies.
        private void ConstructBoosterPrefab(Booster booster)
        {
            switch (booster.Type)
            {
                case BoosterType.Slow:
                    (booster as SlowBooster).Constuct(_settingsProvider);
                    break;
                case BoosterType.Speed:
                    (booster as SpeedBooster).Constuct(_settingsProvider);
                    break;
                case BoosterType.Fly:
                    (booster as FlyBooster).Constuct(_settingsProvider);
                    break;
            }
        }

        private void InitRndWeightsTable()
        {
            int boostersCount = _settingsProvider.BoostersSettings.boostersRandomWeights.Length;

            for (int i = 0; i < boostersCount; i++)
            {
                _boostersRandomWeightsTable.TryAdd(_settingsProvider.BoostersSettings.boostersRandomWeights[i].weight, _settingsProvider.BoostersSettings.boostersRandomWeights[i].booster);
            }
        }
        #endregion
    }
}