using Cysharp.Threading.Tasks;
using RSR.CameraLogic;
using RSR.Player;
using RSR.World;
using Unity.VisualScripting;
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
        private readonly IAssetsProvider _assetsProvider;
        private readonly IInputProvider _inputProvider;
        private readonly IGameSettingsProvider _gameSettingsProvider;
        private readonly ICurtainsService _curtainsService;
        private readonly IPlayerBuilder _playerBuilder;
        private readonly IWorldStarter _worldStarter;
        private readonly IBoostersSettingsProvider _boosterSettingsProvider;

        private PlayerFacade _player;
        private GameObject _camera;
        private GameObject _background;
        private IBoostersFactory _boostersFactory;


        public WorldBuilder(IAssetsProvider assetsProvider, IInputProvider inputProvider, IGameSettingsProvider gameSettingsProvider, ICurtainsService curtainsService, IWorldStarter worldStarter, IPlayerBuilder playerBuilder, IBoostersSettingsProvider boosterSettingsProvider)
        {
            _assetsProvider = assetsProvider;
            _inputProvider = inputProvider;
            _gameSettingsProvider = gameSettingsProvider;
            _curtainsService = curtainsService;
            _worldStarter = worldStarter;
            _playerBuilder = playerBuilder;
            _boosterSettingsProvider = boosterSettingsProvider;
        }

        public async UniTask Build()
        {
            await BuildPlayer();
            await BuildCamera();
            await BuildBackground();
            await BuildBoostersFactory();

            _worldStarter.GetReady();
        }

        public async UniTask Prewarm()
        {
            await UniTask.WhenAll(
                _assetsProvider.Load<GameObject>(AssetsKeys.CameraKey),
                _assetsProvider.Load<GameObject>(AssetsKeys.BackgroundKey));
        }

        #region Player Building
        private async UniTask BuildPlayer()
        {
            _player = await _playerBuilder.Build();
        }

        #endregion

        #region Camera Building
        private async UniTask BuildCamera()
        {
            _camera = await _assetsProvider.Instantiate(AssetsKeys.CameraKey);

            BuildCameraFollow();
        }

        private void BuildCameraFollow()
        {
            _camera.GetOrAddComponent<CameraFollow>().Construct(_player.transform, _gameSettingsProvider);
        }
        #endregion

        #region Background Building
        private async UniTask BuildBackground()
        {
            _background = await _assetsProvider.Instantiate(AssetsKeys.BackgroundKey, _gameSettingsProvider.GameSettings.backgroundPos);
        }
        #endregion

        #region Boosters Factory Building
        private async UniTask BuildBoostersFactory()
        {
            _boostersFactory = new BoostersFactory(_assetsProvider, _boosterSettingsProvider, _player);
            await _boostersFactory.Initialize();
        }
        #endregion
    }
}
