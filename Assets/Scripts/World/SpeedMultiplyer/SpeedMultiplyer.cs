using DG.Tweening;
using RSR.ServicesLogic;
using UnityEngine;

namespace RSR.World
{
    public sealed class SpeedMultiplyer : MonoBehaviour, ISpeedMultiplyer
    {
        public float Default { get; private set; }
        public float Current { get; private set; }

        private IGameSettingsProvider _settingsProvider;
        private IWorldStarter _worldStarter;

        private Sequence _boostTween;

        public void Construct(IGameSettingsProvider gameSettingsProvider, IWorldStarter worldStarter)
        {
            _settingsProvider = gameSettingsProvider;
            _worldStarter = worldStarter;

            _worldStarter.OnReady += ResetMultiplyer;

            Default = _settingsProvider.GameSettings.defaultSpeedMultiplyer;
            Current = Default;
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

        private void ResetMultiplyer()
        {
            _boostTween?.Kill();
            Current = Default;
        }

        private void OnDestroy()
        {
            _worldStarter.OnReady -= ResetMultiplyer;
        }
    }
}