﻿using Cysharp.Threading.Tasks;
using RSR.Player;
using RSR.World;
using Unity.VisualScripting;
using UnityEngine;

namespace RSR.ServicesLogic
{
    /// <summary>
    /// Represents builder design pattern.
    /// Similar to world builder, but responsible only for player.
    /// Look at WorlBuilder.cs summary if details are needed.
    /// </summary>
    public sealed class PlayerBuilder : IPlayerBuilder
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IInputProvider _inputProvider;
        private readonly IGameSettingsProvider _settingsProvider;
        private readonly IWorldStarter _worldStarter;

        private PlayerFacade _player;

        public PlayerBuilder(IAssetsProvider assetsProvider, IInputProvider inputProvider, IGameSettingsProvider settingsProvider, IWorldStarter worldStarter)
        {
            _assetsProvider = assetsProvider;
            _inputProvider = inputProvider;
            _settingsProvider = settingsProvider;
            _worldStarter = worldStarter;
        }

        public async UniTask<PlayerFacade> Build()
        {
            await CreatePlayer();

            BuildPlayerDeath();
            BuildPlayerMoveDirReporter();
            BuildPlayerJump();
            BuildPlayerControls();
            BuildPlayerInteractor();
            BuildPlayerAnimator();

            return _player;
        }
        
        public async UniTask Prewarm()
        {
            await _assetsProvider.Load<GameObject>(AssetsKeys.PlayerKey);
        }

        private async UniTask CreatePlayer()
        {
            var player = await _assetsProvider.Instantiate(AssetsKeys.PlayerKey);
            _player = player.GetOrAddComponent<PlayerFacade>();
        }

        #region Player's Components Building
        private void BuildPlayerAnimator()
        {
            var animator = _player.GetOrAddComponent<PlayerAnimator>();
            animator.Construct(_worldStarter, _player.Jump, _player.Death);
            _player.Animator = animator;
        }

        private void BuildPlayerDeath()
        {
            var death = _player.GetOrAddComponent<PlayerDeath>();
            death.Construct(_worldStarter);
            _player.Death = death;
        }

        private void BuildPlayerControls()
        {
            var controls = _player.GetOrAddComponent<PlayerControls>();
            controls.Construct(_inputProvider, _player.Jump, _player.Death, _worldStarter);
            _player.Controls = controls;
        }

        private void BuildPlayerMoveDirReporter()
        {
            var reporter = _player.GetOrAddComponent<PlayerMoveDirReporter>();
            _player.MoveDirReporter = reporter;
        }

        private void BuildPlayerJump()
        {
            var jump = _player.GetOrAddComponent<PlayerJump>();
            jump.Construct(_settingsProvider, _player.Death);
            _player.Jump = jump;
        }

        private void BuildPlayerInteractor()
        {
            var interactor = _player.GetOrAddComponent<PlayerInteractor>();
            _player.Interactor = interactor;
        }
        #endregion
    }
}