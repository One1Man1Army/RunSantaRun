using Cysharp.Threading.Tasks;
using RSR.CameraLogic;
using RSR.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RSR.ServicesLogic
{
    /// <summary>
    /// Represents builder design pattern.
    /// Gets all necessary data, than builds game world unit by unit.
    /// Grants us control over all game world's objects instantiation and instantiation order.
    /// Provides game units with dependencies they need.
    /// </summary>
    public sealed class WorldBuilder : IWorldBuilder
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IInputProvider _inputProvider;
        private readonly IGameSettingsProvider _settingsProvider;

        private PlayerFacade _playerFacade;

        public WorldBuilder(IAssetsProvider assetsProvider, IInputProvider inputProvider, IGameSettingsProvider settingsProvider) 
        {
            _assetsProvider = assetsProvider;
            _inputProvider = inputProvider;
            _settingsProvider = settingsProvider;
        }

        public async void Build()
        {
            await BuildPlayer();
            await BuildCamera();
        }

        private async UniTask BuildPlayer()
        {
            var player = await _assetsProvider.Instantiate(AssetsKeys.PlayerKey);
            _playerFacade = player.GetComponent<PlayerFacade>();
            _playerFacade.Construct();
            //_playerFacade.playerSpeedMultiplyer.Construct
        }

        private async UniTask BuildCamera()
        {
            var camera = await _assetsProvider.Instantiate(AssetsKeys.CameraKey);
            camera.GetComponent<CameraFollow>().Construct(_playerFacade.transform, _settingsProvider);
            
        }
    }
}
