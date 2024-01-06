using DG.Tweening;
using RSR.Curtain;
using RSR.ServicesLogic;
using System;
using UnityEngine;

namespace RSR.World
{
    public sealed class WorldStarter : MonoBehaviour, IWorldStarter
    {
        public event Action OnReady;
        public event Action OnStart;
        public event Action OnRestart;

        private IInputProvider _inputProvider;
        private ICurtainsService _curtainsService;

        private bool _isReady;
        private bool _isStarted;
        private bool _isFinished;

        public void Construct(IInputProvider inputProvider, ICurtainsService curtainsService)
        {
            _inputProvider = inputProvider;
            _curtainsService = curtainsService;
        }

        public void GetReady()
        {
            _isReady = true;
            _isStarted = false;
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
                    if (CanStart())
                    {
                        StartWorld();
                        return;
                    }

                    if (CanRetart())
                    {
                        RestartWorld();
                    }
                }
            }
        }

        private void StartWorld()
        {
            _isStarted = true;
            _isFinished = false;
            OnStart?.Invoke();
            _curtainsService.HideCurtains();
        }

        private void RestartWorld()
        {
            _isStarted = false;
            _isFinished = false;
            OnRestart?.Invoke();
            _curtainsService.ShowCurtain(CurtainType.Intro);
        }

        private bool CanStart()
        {
            return _isReady && !_isStarted;
        }

        private bool CanRetart()
        {
            return _isFinished && _isStarted;
        }

        private bool IsInitialized()
        {
            return _inputProvider != null && _curtainsService != null;
        }

        public void FinishWorld()
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                _isFinished = true;
                _curtainsService.ShowCurtain(CurtainType.Outro);
            });
        }
    }
}