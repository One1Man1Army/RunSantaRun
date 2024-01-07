using RSR.ServicesLogic;
using RSR.World;

namespace RSR.InternalLogic
{
	public sealed class PlayState: IState
	{
        private readonly GameStateMachine _gameStateMachine;
        private readonly Services _services;

        public PlayState(GameStateMachine gameStateMachine, Services services)
        {
            _gameStateMachine = gameStateMachine;
            _services = services;
        }

        public void Enter()
		{
            _services.GetService<IWorldStarter>().GetReady();
        }
		
		public void Exit()
		{
			
		}
	}
}