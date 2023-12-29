namespace RSR.InternalLogic
{
    public sealed class LoadState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public LoadState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {

        }

        public void Exit()
        {

        }
    }
}
