using RSR.ServicesLogic;

namespace RSR.InternalLogic
{
    public sealed class LoadState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly Services _services;

        public LoadState(GameStateMachine gameStateMachine, Services services)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;
        }

        public async void Enter()
        {
            await _services.GetService<IWorldBuilder>().Build();

            _gameStateMachine.Enter<PlayState>();
        }

        public void Exit()
        {

        }
    }
}
