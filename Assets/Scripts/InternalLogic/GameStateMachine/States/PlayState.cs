namespace RSR.InternalLogic
{
	public sealed class PlayState: IState
	{
		private readonly GameStateMachine _gameStateMachine;

		public PlayState(GameStateMachine gameStateMachine)
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