using System;
using System.Collections.Generic;

namespace RSR.InternalLogic
{
	public sealed class GameStateMachine : IGameStateMachine
	{
		private readonly Dictionary<Type, IState> _states;
		private IState _activeState;

		public GameStateMachine()
		{
			_states = new Dictionary<Type, IState>
			{
				[typeof(InitState)] = new InitState(this),
				[typeof(LoadState)] = new LoadState(this),
				[typeof(PlayState)] = new PlayState(this)
			};
		}
		
		public void Enter<T>() where T : class, IState
		{
			IState state = GoToState<T>();
			state.Enter();
		}
		
		public T GoToState<T>() where T : class, IState
		{
			_activeState?.Exit();

            var state = GetState<T>();
			_activeState = state;
			
			return state;
		}

        private T GetState<T>() where T : class, IState
        {
            return _states[typeof(T)] as T;
        }
    }
}
