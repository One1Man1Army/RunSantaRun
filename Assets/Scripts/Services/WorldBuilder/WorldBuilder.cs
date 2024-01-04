using Cysharp.Threading.Tasks;
using RSR.CameraLogic;
using RSR.InternalLogic;
using RSR.Player;
using UnityEngine;

namespace RSR.ServicesLogic
{
    /// <summary>
    /// Represents builder design pattern.
    /// Gets all necessary data, than builds game world unit by unit.
    /// Grants us control over all game world's objects' instantiation and instantiation order.
    /// Provides game units with dependencies they need.
    /// Instantiation is safe - if any vital component on prefab is missing, the builder adds it.
    /// Because the components are accessed by the interfaces, we can easily swap between game logic realization variants just by changing component type in builder's build methods. 
    /// </summary>
    public sealed class WorldBuilder : IWorldBuilder
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IInputProvider _inputProvider;
        private readonly IGameSettingsProvider _settingsProvider;

        private GameObject _player;
        private IPlayerSpeedMultiplyer _playerSpeedMultiplyer;
        private IPlayerMoveDirReporter _playerMoveDirReporter;
        private IPlayerJump _playerJump;
        private IPlayerControls _playerControls;
        private IPlayerDeath _playerDeath;

        private GameObject _camera;

        private GameObject _background;

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
            await BuildBackground();
        }

        #region Player Building Logic
        private async UniTask BuildPlayer()
        {
            _player = await _assetsProvider.Instantiate(AssetsKeys.PlayerKey);

            BuildPlayerSpeedMultiplyer();
            BuildPlayerMoveDirReporter();
            BuildPlayerDeath();
            BuildPlayerMove();
            BuildPlayerJump();
            BuildPlayerControls();
            BuildPlayerInteractor();
            BuildPlayerAnimator();
        }

        private void BuildPlayerAnimator()
        {
            var animator = _player.GetComponentWithAdd<PlayerAnimator>();
            animator.Construct(_playerMoveDirReporter, _playerDeath);
        }

        private void BuildPlayerDeath()
        {
            _playerDeath = _player.GetComponentWithAdd<PlayerDeath>();
        }

        private void BuildPlayerControls()
        {
            var controls = _player.GetComponentWithAdd<PlayerControls>();
            controls.Construct(_inputProvider, _playerJump, _playerDeath);
            _playerControls = controls;
        }

        private void BuildPlayerMoveDirReporter()
        {
            _playerMoveDirReporter = _player.GetComponentWithAdd<PlayerMoveDirReporter>();
        }

        private void BuildPlayerJump()
        {
            var jump = _player.GetComponentWithAdd<PlayerJump>();
            jump.Construct(_settingsProvider, _playerMoveDirReporter);
            _playerJump = jump;
        }

        private void BuildPlayerInteractor()
        {
            _player.GetComponentWithAdd<PlayerInteractor>();
        }

        private void BuildPlayerMove()
        {
            var move = _player.GetComponentWithAdd<PlayerMove>();
            move.Construct(_playerSpeedMultiplyer);
        }

        private void BuildPlayerSpeedMultiplyer()
        {
            var speedMultiplyer = _player.GetComponentWithAdd<PlayerSpeedMultiplyer>();
            speedMultiplyer.Construct(_settingsProvider, _playerDeath);
            _playerSpeedMultiplyer = speedMultiplyer;
        }
        #endregion

        #region Camera Building Logic
        private async UniTask BuildCamera()
        {
            _camera = await _assetsProvider.Instantiate(AssetsKeys.CameraKey);

            BuildCameraFollow();

        }

        private void BuildCameraFollow()
        {
            _camera.GetComponentWithAdd<CameraFollow>().Construct(_player.transform, _settingsProvider);
        }
        #endregion

        #region Background Building Logic
        private async UniTask BuildBackground()
        {
            _background = await _assetsProvider.Instantiate(AssetsKeys.BackgroundKey);
        }
        #endregion
    }
}
