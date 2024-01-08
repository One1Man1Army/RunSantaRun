using Cysharp.Threading.Tasks;
using RSR.CameraLogic;
using RSR.InternalLogic;
using RSR.Player;
using RSR.World;
using System;
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
        private readonly IBoostersSettingsProvider _boosterSettingsProvider;
        private readonly IObstaclesSettingsProvider _obstaclesSettingsProvider;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ICurtainsService _curtainsService;
        private readonly IPlayerBuilder _playerBuilder;
        private readonly IWorldStarter _worldStarter;
        private readonly IRandomService _randomService;

        //All game world's units.
        private PlayerFacade _player;
        private GameObject _camera;
        private GameObject _background;
        private Transform _movingWorld;
        private ISpeedMultiplyer _speedMultiplyer;
        private IWorldMover _worldMover;
        private IBoostersFactory _boostersFactory;
        private IObstaclesFactory _obstaclesFactory;
        private IItemsSpawner _itemsSpawner;
        private IItemsReleaser _itemsReleaser;
        private ITimeMachine _timeMachine;

        public WorldBuilder(IAssetsProvider assetsProvider,
                            IInputProvider inputProvider,
                            IGameSettingsProvider gameSettingsProvider,
                            ICurtainsService curtainsService,
                            IWorldStarter worldStarter,
                            IPlayerBuilder playerBuilder,
                            IBoostersSettingsProvider boosterSettingsProvider,
                            IRandomService randomService,
                            IObstaclesSettingsProvider obstaclesSettingsProvider)
        {
            _assetsProvider = assetsProvider;
            _inputProvider = inputProvider;
            _gameSettingsProvider = gameSettingsProvider;
            _curtainsService = curtainsService;
            _worldStarter = worldStarter;
            _playerBuilder = playerBuilder;
            _boosterSettingsProvider = boosterSettingsProvider;
            _randomService = randomService;
            _obstaclesSettingsProvider = obstaclesSettingsProvider;
        }

        public async UniTask Build()
        {
            await BuildPlayer();
            await BuildCamera();
            BuildMovingWorldObject();
            BuildSpeedMultiplyer();
            BuildWorldMover();
            await BuildBoostersFactory();
            await BuildObstaclesFactory();
            BuildItemsSpawner();
            BuildItemsReleaser();
            await BuildBackground();
            BuildTimeMachine();
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
            _camera = await _assetsProvider.Instantiate(AssetsKeys.CameraKey, _gameSettingsProvider.GameSettings.cameraPosition);

            BuildCameraAspectToScreenSize();
        }

        private void BuildCameraAspectToScreenSize()
        {
            _camera.GetOrAddComponent<CameraAspectToScreenSize>();
        }
        #endregion

        #region Background Building
        private async UniTask BuildBackground()
        {
            _background = await _assetsProvider.Instantiate(AssetsKeys.BackgroundKey, _gameSettingsProvider.GameSettings.backgroundPosition);

            BuildParallaxMovers();
        }

        private void BuildParallaxMovers()
        {
            var parallaxMovers = _background.GetComponentsInChildren<ParallaxMover>();
            foreach (var parallaxMover in parallaxMovers)
            {
                parallaxMover.Construct(_worldMover);
            }
        }
        #endregion

        #region Factories Building
        private async UniTask BuildBoostersFactory()
        {
            _boostersFactory = new BoostersFactory(_assetsProvider, _boosterSettingsProvider, _randomService, _player.Jump, _speedMultiplyer);
            await _boostersFactory.Initialize();
        }

        private async UniTask BuildObstaclesFactory()
        {
            _obstaclesFactory = new ObstaclesFactory(_assetsProvider, _obstaclesSettingsProvider, _randomService, _player);
            await _obstaclesFactory.Initialize();
        }
        #endregion

        #region Items Spawner Building
        private void BuildItemsSpawner()
        {
            var spawner = new GameObject(Constants.ItemsSpawnerName).AddComponent<ItemsSpawner>();

            spawner.Construct(_gameSettingsProvider, _randomService, _worldStarter, _boostersFactory, _obstaclesFactory, _player.Death, _movingWorld);

            spawner.transform.position = new(_player.transform.position.x + _gameSettingsProvider.GameSettings.spawnToPlayerOffset,
                                                _player.transform.position.y,
                                                _player.transform.position.z);

            _itemsSpawner = spawner;
        }
        #endregion

        #region Items Releaser Building
        private void BuildItemsReleaser()
        {
            var gObj = new GameObject(Constants.ItemsReleaserName);

            gObj.AddComponent<Rigidbody2D>().isKinematic = true;
            gObj.AddComponent<BoxCollider2D>().isTrigger = true;

            gObj.transform.localScale = new Vector3(1f, 50f, 1f);

            var releaser = gObj.AddComponent<ItemsReleaser>();

            releaser.transform.position = new(_player.transform.position.x - _gameSettingsProvider.GameSettings.spawnToPlayerOffset,
                _player.transform.position.y,
                _player.transform.position.z);

            _itemsReleaser = releaser;
        }
        #endregion

        #region Moving World Building
        private void BuildMovingWorldObject()
        {
            _movingWorld = new GameObject(Constants.MovingWorldName).transform;

            BuildMovingWorldRigidbody();
        }

        private void BuildMovingWorldRigidbody()
        {
            var body = _movingWorld.AddComponent<Rigidbody2D>();
            body.isKinematic = true;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private void BuildSpeedMultiplyer()
        {
            var multiplyer = _movingWorld.AddComponent<SpeedMultiplyer>();
            multiplyer.Construct(_gameSettingsProvider, _worldStarter);
            _speedMultiplyer = multiplyer;
        }

        private void BuildWorldMover()
        {
            var mover = _movingWorld.AddComponent<WorldMover>();
            mover.Construct(_gameSettingsProvider, _speedMultiplyer, _player.Death, _worldStarter);
            _worldMover = mover;
        }
        #endregion

        #region Time Machine Building
        private void BuildTimeMachine()
        {
            var machine = new GameObject(Constants.TimeMachineName).AddComponent<TimeMachine>();
            machine.Construct(_gameSettingsProvider, _worldStarter);
            _timeMachine = machine;
        }
        #endregion
    }
}
