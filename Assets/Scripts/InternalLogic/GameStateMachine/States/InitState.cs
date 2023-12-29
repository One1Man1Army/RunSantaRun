using System;

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

        public void Enter()
        {

        }

        public void Exit()
        {

        }

        #region Services Bindings
        private void BindServices()
        {
            BindAssetsProvider();
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
