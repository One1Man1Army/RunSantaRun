using RSR.Curtain;
using RSR.ServicesLogic;
using System;
using UnityEngine;

namespace RSR.World
{
    public sealed class WorldStarter : MonoBehaviour, IWorldStarter
    {
        public event Action OnStart;
        public event Action OnReady;

        private IInputProvider _inputProvider;
        private ICurtainsService _curtainsService;

        private bool _isReady;
        private bool _isStarted;

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
            StartOnTap();
        }

        private void StartOnTap()
        {
            if (IsInitialized())
            {
                if (CanStart())
                {
                    if (_inputProvider.HasPlayerTapped())
                    {
                        StartWorld();
                    }
                }
            }
        }

        private void StartWorld()
        {
            _isStarted = true;
            OnStart?.Invoke();
            _curtainsService.HideCurtains();
        }

        private bool CanStart()
        {
            return _isReady && !_isStarted;
        }

        private bool IsInitialized()
        {
            return _inputProvider != null && _curtainsService != null;
        }
    }
}