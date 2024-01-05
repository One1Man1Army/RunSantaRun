using Cysharp.Threading.Tasks;
using RSR.ServicesLogic;
using System.Threading.Tasks;

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
            await _services.GetService<IPlayerBuilder>().Prewarm();

            await BuildWorld();

            _gameStateMachine.Enter<PlayState>();
        }

        private async UniTask BuildWorld()
        {
            var worldBuilder = _services.GetService<IWorldBuilder>();

            await worldBuilder.Prewarm();
            await worldBuilder.Build();
        }

        public void Exit()
        {

        }
    }
}
