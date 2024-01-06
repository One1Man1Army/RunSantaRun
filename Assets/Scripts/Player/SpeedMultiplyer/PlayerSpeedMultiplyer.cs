using DG.Tweening;
using RSR.ServicesLogic;
using RSR.World;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerSpeedMultiplyer : MonoBehaviour, IPlayerSpeedMultiplyer
    {
        public float Default { get; private set; }
        public float Current { get; private set; }
        public float SpeedUp { get; private set; }

        private IGameSettingsProvider _settingsProvider;
        private IWorldStarter _worldStarter;
        private bool _isAddingTime;
        private Sequence _boostTween;

        public void Construct(IGameSettingsProvider gameSettingsProvider, IWorldStarter worldStarter)
        {
            _settingsProvider = gameSettingsProvider;
            _worldStarter = worldStarter;

            _worldStarter.OnReady += ResetTime;
            _worldStarter.OnStart += StartTime;

            Default = _settingsProvider.GameSettings.playerSpeedMultiplyer;
            Current = Default;
            SpeedUp = 1f;
        }

        public void Boost(float multiplyer, float duration)
        {
            var lerpDuration = duration / 5f;
            var boostDuration = duration - (lerpDuration * 2);
            var startValue = Current;
            var boostedValue = Current * multiplyer;

            _boostTween?.Kill();

            _boostTween = DOTween.Sequence()
                .Append(DOVirtual.Float(startValue, boostedValue, lerpDuration, v => Current = v)).
                 AppendInterval(boostDuration)
                .Append(DOVirtual.Float(startValue, Default, lerpDuration, v => Current = v));
        }

        private void Update()
        {
            if (_isAddingTime)
            {
                SpeedUp += _settingsProvider.GameSettings.speedAddPerFrame * Time.deltaTime;
            }
        }

        private void StartTime()
        {
            _isAddingTime = true;
        }

        private void ResetTime()
        {
            _isAddingTime = false;
            _boostTween?.Kill();
            SpeedUp = 1f;
            Current = Default;
        }

        private void OnDestroy()
        {
            if (_worldStarter != null)
            {
                _worldStarter.OnReady -= ResetTime;
                _worldStarter.OnStart -= StartTime;
            }
        }
    }
}