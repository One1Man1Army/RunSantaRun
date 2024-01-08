using Cysharp.Threading.Tasks;
using RSR.InternalLogic;
using RSR.Player;
using RSR.ServicesLogic;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        #region Fields
        private readonly IBoostersSettingsProvider _settingsProvider;
        private readonly IRandomService _randomService;
        private readonly IPlayerJump _playerJump;
        private readonly ISpeedMultiplyer _speedMultiplyer;

        //Inner boosters weights table, initialized from boosters' settings data, where we can set weights values.
        private readonly Dictionary<int, BoosterType> _boostersRandomWeightsTable = new();

        //Storage with a pool for each booster type.
        private readonly Dictionary<BoosterType, ObjectsPool<PoolableItem>> _boostersStorage = new();

        private readonly float _boostersSpawnHeight;
        #endregion

        public BoostersFactory(IAssetsProvider assetsProvider, IBoostersSettingsProvider settingsProvider, IRandomService randomService, IPlayerJump playerJump, ISpeedMultiplyer speedMultiplyer) 
        {
            _settingsProvider = settingsProvider;
            _assetsProvider = assetsProvider;
            _randomService = randomService;
            _playerJump = playerJump;
            _speedMultiplyer = speedMultiplyer;

            _boostersSpawnHeight = settingsProvider.BoostersSettings.boostersSpawnHeight;
        }

        #region Spawning

        public Booster Create(BoosterType booster, Vector3 pos)
        {
            if (!_boostersStorage.ContainsKey(booster))
            {
                Debug.Log($"Creating a {booster} failed! No such booster in storage.");
                return null;
            }

            pos.y = _boostersSpawnHeight;
            var instance = _boostersStorage[booster].Get(pos) as Booster;

            if (!instance.IsConstructed) 
            {
                ConstructBooster(instance);
            }

            return instance;
        }

        //Provides booster with all necessary dependencies.
        private void ConstructBooster(Booster instance)
        {
            instance.SetPool(_boostersStorage[instance.Type]);

            BuildBoosterMove(instance);

            switch (instance.Type)
            {
                case BoosterType.Slow:
                    (instance as SlowBooster).Constuct(_settingsProvider, _speedMultiplyer);
                    break;
                case BoosterType.Speed:
                    (instance as SpeedBooster).Constuct(_settingsProvider, _speedMultiplyer);
                    break;
                case BoosterType.Fly:
                    (instance as FlyBooster).Constuct(_settingsProvider, _playerJump);
                    break;
            }
        }

        private void BuildBoosterMove(Booster instance)
        {
            var boosterMove = instance.GetOrAddComponent<BoosterMove>();
            boosterMove.Construct(_settingsProvider);
        }

        public Booster CreateRandom(Vector3 pos)
        {
            return Create(_randomService.GetWeightedRandomValue(_boostersRandomWeightsTable), pos);
        }

        public void ReleaseAll()
        {
            foreach (var pool in _boostersStorage.Values)
            {
                pool.ReleaseAll();
            }
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
        protected override void InitStorage()
        {
            if (_prefabs == null || _prefabs.Count == 0)
                return;

            foreach (var prefab in _prefabs)
            {
                var booster = prefab.GetComponent<Booster>();

                if (_boostersStorage.ContainsKey(booster.Type))
                {
                    Debug.Log($"Failed to add booster! Reason: {booster.Type} duplicate.");
                    continue;
                }

                _boostersStorage.Add(booster.Type, new ObjectsPool<PoolableItem>(booster, 3, 10, $"{booster.Type} boosters pool"));
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