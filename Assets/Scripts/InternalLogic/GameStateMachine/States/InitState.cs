using Cysharp.Threading.Tasks;
using RSR.CommonLogic;
using RSR.Curtain;
using RSR.ServicesLogic;
using System;
using UnityEngine;

namespace RSR.InternalLogic
{
    /// <summary>
    /// Initialization of game services.
    /// Grants us control over initialization order.
    /// </summary>
    public sealed class InitState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly Services _services;

        private bool _isBindingComplete;


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

        private void BindWorldBuilder()
        {
            Services.Container.AddService<IWorldBuilder>(new WorldBuilder(
                Services.Container.GetService<IAssetsProvider>(),
                Services.Container.GetService<IInputProvider>(),
                Services.Container.GetService<IGameSettingsProvider>(),
                Services.Container.GetService<ICurtainsService>()));
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
