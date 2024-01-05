using Cysharp.Threading.Tasks;
using RSR.CommonLogic;
using RSR.Curtain;
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
            await BindGameSettingsProvider();
            await BindCurtainsService();
            BindWorldStarter();
            BindPlayerBuilder();
            BindWorldBuilder();
        }

        private static void BindInputProvider()
        {
            Services.Container.AddService<IInputProvider>(new InputProvider());
        }

        private void BindGameStateMachine()
        {
            Services.Container.AddService<IGameStateMachine>(_gameStateMachine);
        }

        private void BindWorldStarter()
        {
            var starter = UnityEngine.Object.Instantiate(new GameObject("World Starter")).AddComponent<WorldStarter>();
            starter.Construct(Services.Container.GetService<IInputProvider>(), Services.Container.GetService<ICurtainsService>());
            _services.AddService<IWorldStarter>(starter);
        }

        private void BindPlayerBuilder()
        {
            Services.Container.AddService<IPlayerBuilder>(new PlayerBuilder(
                Services.Container.GetService<IAssetsProvider>(),
                Services.Container.GetService<IInputProvider>(),
                Services.Container.GetService<IGameSettingsProvider>(),
                Services.Container.GetService<ICurtainsService>(),
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
                Services.Container.GetService<IPlayerBuilder>()));
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

        private void BindAssetsProvider()
        {
            var assetsProvider = new AssetsProvider();
            _services.AddService<IAssetsProvider>(assetsProvider);

            assetsProvider.Initialize();
        }
        #endregion
    }
}
