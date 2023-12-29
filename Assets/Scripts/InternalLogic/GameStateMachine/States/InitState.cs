using System;

namespace RSR.InternalLogic
{
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

        private void BindServices()
        {
            
        }

        public void Enter()
        {

        }

        public void Exit()
        {

        }
    }
}
