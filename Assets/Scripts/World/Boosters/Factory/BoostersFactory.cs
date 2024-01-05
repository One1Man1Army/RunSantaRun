using Cysharp.Threading.Tasks;
using RSR.InternalLogic;
using RSR.Player;
using RSR.ServicesLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSR.World
{
    /// <summary>
    /// Boosters' factory.
    /// Uses objects pools to store boosters.
    /// To add new booster type you don't need to change any existing logic.
    /// All you need to do is inherit from Booster base class and IInteractable interface, add label "Booster" to you addressbales prefab and add your boosters' initialization logic to ConstructBoosterPrefab method.
    /// </summary>
    public sealed class BoostersFactory : IBoostersFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IBoostersSettingsProvider _settingsProvider;
        private readonly IPlayerSpeedMultiplyer _playerSpeedMultiplyer;

        private readonly Dictionary<BoosterType, ObjectsPool<Booster>> _boostersStorage = new();

        private readonly float _boostersSpawnHeight;

        public BoostersFactory(IAssetsProvider assetsProvider, IBoostersSettingsProvider settingsProvider, PlayerFacade player) 
        {
            _settingsProvider = settingsProvider;
            _assetsProvider = assetsProvider;
            _playerSpeedMultiplyer = player.SpeedMultiplyer;

            _boostersSpawnHeight = settingsProvider.BoostersSettings.boostersSpawnHeight;
        }

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

        #region Initialization

        //Fulfill boosters' storage with pools for each booster type from addressables "Booster" labled prefabs.
        public async UniTask Initialize()
        {
            var boosters = await _assetsProvider.LoadMultiple<GameObject>(AssetsKeys.BoostersLabel);

            foreach (var prefab in boosters) 
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
            _boostersStorage.Add(booster.Type, new ObjectsPool<Booster>(booster, 3, 10, $"{nameof(booster)} pool"));
            booster.SetPool(_boostersStorage[booster.Type]);
        }

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
        #endregion
    }
}