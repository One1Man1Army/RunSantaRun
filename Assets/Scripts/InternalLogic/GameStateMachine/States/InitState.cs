using Cysharp.Threading.Tasks;
using RSR.CommonLogic;
using RSR.Curtain;
using RSR.ServicesLogic;
using System;
using System.Threading.Tasks;
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

        public InitState(GameStateMachine gameStateMachine, Services services)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;

            BindServices();
        }

        #region State Machine Logic
        public void Enter()
        {

        }

        public void Exit()
        {

        }
        #endregion

        #region Services Bindings
        private async void BindServices()
        {
            Services.Container.AddService<IGameStateMachine>(_gameStateMachine);
            BindAssetsProvider();
            Services.Container.AddService<IInputProvider>(new InputProvider());
            await BindGameSettingsProvider();
            await BindCurtainsService();
            BindWorldBuilder();
        }

        private void BindWorldBuilder()
        {
            Services.Container.AddService<IWorldBuilder>(new WorldBuilder(
                Services.Container.GetService<IAssetsProvider>(),
                Services.Container.GetService<IInputProvider>(),
                Services.Container.GetService<IGameSettingsProvider>()));
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
