namespace RSR.InternalLogic
{
	public interface IGameStateMachine : IService
	{
		void Enter<T>() where T : class, IState;
	}
}