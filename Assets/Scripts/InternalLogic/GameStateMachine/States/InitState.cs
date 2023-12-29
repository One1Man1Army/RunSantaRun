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
        private void BindServices()
        {
            Services.Container.AddService<IGameStateMachine>(_gameStateMachine);
            BindAssetsProvider();
            Services.Container.AddService<IInputProvider>(new InputProvider());
        }

/*        private void BindMonoProvider()
        {
            var providerObject = GameObject.Instantiate(new GameObject("MonoProvider"));
            var provider = providerObject.AddComponent<MonoProvider>();

            _services.AddService<IMonoProvider>(provider);
        }*/

        private void BindAssetsProvider()
        {
            var assetsProvider = new AssetsProvider();
            _services.AddService<IAssetsProvider>(assetsProvider);

            assetsProvider.Initialize();
        }
        #endregion
    }
}
