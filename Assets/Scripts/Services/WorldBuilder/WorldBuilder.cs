using Cysharp.Threading.Tasks;
using RSR.CameraLogic;
using RSR.InternalLogic;
using RSR.Player;
using RSR.World;
using UnityEngine;

namespace RSR.ServicesLogic
{
    /// <summary>
    /// Represents builder design pattern.
    /// Gets all necessary data, then builds game world unit by unit.
    /// Grants us control over all game world's objects' instantiation and instantiation order.
    /// Provides game units with dependencies they need.
    /// A game units' behaviour logic is represented as a unity-native-like component-based modular system.
    /// Due to the modularity of this system we can easily add new behaviour to units without a care about existing logic. Just add new component's bulid method to unit's bulid method.
    /// Because the components are accessed by the interfaces, we can easily swap between game logic realization variants just by changing component type in builder's build methods. 
    /// Instantiation is safe - if any vital component on prefab is missing, the builder adds it.
    /// </summary>
    public sealed class WorldBuilder : IWorldBuilder
    {
        [Header("Services")]
        private readonly IAssetsProvider _assetsProvider;
        private readonly IInputProvider _inputProvider;
        private readonly IGameSettingsProvider _settingsProvider;
        private readonly ICurtainsService _curtainsService;

        [Header("Player")]
        private GameObject _player;
        private IPlayerSpeedMultiplyer _playerSpeedMultiplyer;
        private IPlayerMoveDirReporter _playerMoveDirReporter;
        private IPlayerJump _playerJump;
        private IPlayerControls _playerControls;
        private IPlayerDeath _playerDeath;

        [Header("Units")]
        private GameObject _camera;
        private GameObject _background;
        private IWorldStarter _worldStarter;

        public WorldBuilder(IAssetsProvider assetsProvider, IInputProvider inputProvider, IGameSettingsProvider settingsProvider, ICurtainsService curtainsService) 
        {
            _assetsProvider = assetsProvider;
            _inputProvider = inputProvider;
            _settingsProvider = settingsProvider;
            _curtainsService = curtainsService;
        }

        public async UniTask Build()
        {
            BuildWorldStarter();
            await BuildPlayer();
            await BuildCamera();
            await BuildBackground();

            _worldStarter.GetReady();
        }

        #region Player Building Logic
        private async UniTask BuildPlayer()
        {
            _player = await _assetsProvider.Instantiate(AssetsKeys.PlayerKey);

            BuildPlayerDeath();
            BuildPlayerSpeedMultiplyer();
            BuildPlayerMoveDirReporter();
            BuildPlayerMove();
            BuildPlayerJump();
            BuildPlayerControls();
            BuildPlayerInteractor();
            BuildPlayerAnimator();
        }

        private void BuildPlayerAnimator()
        {
            var animator = _player.GetComponentWithAdd<PlayerAnimator>();
            animator.Construct(_playerMoveDirReporter, _playerDeath, _worldStarter);
        }

        private void BuildPlayerDeath()
        {
            var death = _player.GetComponentWithAdd<PlayerDeath>();
            death.Construct(_worldStarter);
            _playerDeath = death;
        }

        private void BuildPlayerControls()
        {
            var controls = _player.GetComponentWithAdd<PlayerControls>();
            controls.Construct(_inputProvider, _playerJump, _playerDeath, _worldStarter);
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
            move.Construct(_playerSpeedMultiplyer, _playerDeath, _worldStarter);
        }

        private void BuildPlayerSpeedMultiplyer()
        {
            var speedMultiplyer = _player.GetComponentWithAdd<PlayerSpeedMultiplyer>();
            speedMultiplyer.Construct(_settingsProvider);
            _playerSpeedMultiplyer = speedMultiplyer;
        }
        #endregion

        #region World Starter Building Logic
        private void BuildWorldStarter()
        {
            var starter = GameObject.Instantiate(new GameObject("World Starter")).AddComponent<WorldStarter>();
            starter.Construct(_inputProvider, _curtainsService);
            _worldStarter = starter;
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
            _background = await _assetsProvider.Instantiate(AssetsKeys.BackgroundKey, _settingsProvider.GameSettings.backgroundPos);
        }
        #endregion
    }
}
