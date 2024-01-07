using DG.Tweening;
using RSR.Curtains;
using RSR.ServicesLogic;
using System;
using UnityEngine;

namespace RSR.World
{
    public sealed class WorldStarter : MonoBehaviour, IWorldStarter
    {
        public event Action OnReady;
        public event Action OnStart;

        private IInputProvider _inputProvider;
        private ICurtainsService _curtainsService;

        private WorldState _state;

        public void Construct(IInputProvider inputProvider, ICurtainsService curtainsService)
        {
            _inputProvider = inputProvider;
            _curtainsService = curtainsService;
        }

        public void GetReady()
        {
            _state = WorldState.Ready;
            _curtainsService.ShowCurtain(CurtainType.Intro);
            OnReady?.Invoke();
        }

        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (IsInitialized())
            {
                if (_inputProvider.HasPlayerTapped())
                {
                    switch (_state)
                    {
                        case WorldState.Ready:
                            StartWorld();
                            break;

                        case WorldState.Finished:
                            RestartWorld(); 
                            break;
                    }
                }
            }
        }

        private void StartWorld()
        {
            _state  = WorldState.Started;
            OnStart?.Invoke();
            _curtainsService.HideCurtains();
        }

        private void RestartWorld()
        {
            _state = WorldState.Ready;
            OnReady?.Invoke();
            _curtainsService.ShowCurtain(CurtainType.Intro);
        }

        private bool IsInitialized()
        {
            return _inputProvider != null && _curtainsService != null;
        }

        public void FinishWorld()
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                _state = WorldState.Finished;
                _curtainsService.ShowCurtain(CurtainType.Outro);
            });
        }
    }
}