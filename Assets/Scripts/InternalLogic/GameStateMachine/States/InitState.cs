using Cysharp.Threading.Tasks;
using RSR.Settings;
using RSR.Curtains;
using RSR.ServicesLogic;
using RSR.World;
using System;
using UnityEngine;

namespace RSR.InternalLogic
{
    /// <summary>
    /// Initialization of internal game services.
    /// Grants us control over initialization and initialization order.
    /// Provides services with necessary dependencies.
    /// </summary>
    public sealed class InitState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly Services _services;

        public InitState(GameStateMachine gameStateMachine, Services services)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;
        }

        #region State Machine Logic
        public async void Enter()
        {
            await BindServices();

            _gameStateMachine.Enter<LoadState>();
        }

        public void Exit()
        {

        }
        #endregion

        #region Services Bindings
        private async UniTask BindServices()
        {
            BindGameStateMachine();
            BindAssetsProvider();
            BindInputProvider();
            BindRandomService();
            await BindGameSettingsProvider();
            BindFrameRateService();
            await BindBoosetersSettingsProvider();
            await BindObstaclesSettingsProvider();
            await BindCurtainsService();
            BindWorldStarter();
            BindPlayerBuilder();
            BindWorldBuilder();
        }

        private static void BindInputProvider()
        {
            Services.Container.AddService<IInputProvider>(new InputProvider());
        }

        private void BindRandomService()
        {
            Services.Container.AddService<IRandomService>(new RandomService());
        }

        private void BindGameStateMachine()
        {
            Services.Container.AddService<IGameStateMachine>(_gameStateMachine);
        }

        private void BindPlayerBuilder()
        {
           Services.Container.AddService<IPlayerBuilder>(new PlayerBuilder(
                Services.Container.GetService<IAssetsProvider>(),
                Services.Container.GetService<IInputProvider>(),
                Services.Container.GetService<IGameSettingsProvider>(),
                Services.Container.GetService<IWorldStarter>()));
        }


        private void BindWorldBuilder()
        {
            Services.Container.AddService<IWorldBuilder>(new WorldBuilder(
                Services.Container.GetService<IAssetsProvider>(),
                Services.Container.GetService<IInputProvider>(),
                Services.Container.GetService<IGameSettingsProvider>(),
                Services.Container.GetService<ICurtainsService>(),
                Services.Container.GetService<IWorldStarter>(),
                Services.Container.GetService<IPlayerBuilder>(),
                Services.Container.GetService<IBoostersSettingsProvider>(),
                Services.Container.GetService<IRandomService>(),
                Services.Container.GetService<IObstaclesSettingsProvider>()));
        }

        private async UniTask BindCurtainsService()
        {
            var curtains = await Services.Container.GetService<IAssetsProvider>().Instantiate(AssetsKeys.CurtainsKey);
            var curtainsView = curtains.GetComponent<ICurtainsView>();

            try
            {
                Services.Container.AddService<ICurtainsService>(new CurtainsService(curtainsView, Services.Container.GetService<IGameSettingsProvider>()));
            }
            catch (Exception e)
            {
                Debug.Log($"Curtains loading error! {e.Message}");
            }
            finally
            {
                Services.Container.GetService<ICurtainsService>().ShowCurtain(CurtainType.Loading);
            }
        }

        private void BindWorldStarter()
        {
            var starter = new GameObject("World Starter").AddComponent<WorldStarter>();
            starter.Construct(Services.Container.GetService<IInputProvider>(), Services.Container.GetService<ICurtainsService>());
            _services.AddService<IWorldStarter>(starter);
        }

        private async UniTask BindGameSettingsProvider()
        {
            var gameSettings = await Services.Container.GetService<IAssetsProvider>().Load<GameSettings>(AssetsKeys.GameSettingsKey);

            try
            {
                Services.Container.AddService<IGameSettingsProvider>(new GameSettingsProvider(gameSettings));
            }
            catch (Exception e) 
            {
                Debug.Log($"Game Settings loading error! {e.Message}");
            }
        }

        private void BindFrameRateService()
        {
            var frameRate = new FrameRateService();
            var settings = Services.Container.GetService<IGameSettingsProvider>();
            if (settings.GameSettings.maxFrameRate) 
            {
                frameRate.SetMaxFrameRate();
            }
            else
            {
                frameRate.SetFrameRate(settings.GameSettings.frameRate);
            }

            frameRate.SetVSync(settings.GameSettings.vSync);
            Services.Container.AddService<IFrameRateService>(frameRate);
        }

        private async UniTask BindBoosetersSettingsProvider()
        {
            var boostersSettings = await Services.Container.GetService<IAssetsProvider>().Load<BoostersSettings>(AssetsKeys.BoostersSettingsKey);

            try
            {
                Services.Container.AddService<IBoostersSettingsProvider>(new BoostersSettingsProvider(boostersSettings));
            }
            catch (Exception e)
            {
                Debug.Log($"Boosters Settings loading error! {e.Message}");
            }
        }

        private async UniTask BindObstaclesSettingsProvider()
        {
            var obstaclesSettings = await Services.Container.GetService<IAssetsProvider>().Load<ObstaclesSettings>(AssetsKeys.ObstaclesSettingsKey);

            try
            {
                Services.Container.AddService<IObstaclesSettingsProvider>(new ObstaclesSettingsProvider(obstaclesSettings));
            }
            catch (Exception e)
            {
                Debug.Log($"Obstacles Settings loading error! {e.Message}");
            }
        }

        private void BindAssetsProvider()
        {
            var assetsProvider = new AssetsProvider();
            _services.AddService<IAssetsProvider>(assetsProvider);

            assetsProvider.Initialize();
        }
        #endregion
    }
}
