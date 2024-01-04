using RSR.ServicesLogic;
using System;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerSpeedMultiplyer : MonoBehaviour, IPlayerSpeedMultiplyer
    {
        public float Default { get; private set; }
        public float Current { get; private set; }

        private IPlayerDeath _playerDeath;
        public void Construct(IGameSettingsProvider gameSettingsProvider, IPlayerDeath playerDeath)
        {
            Default = gameSettingsProvider.GameSettings.playerSpeedMultiplyer;
            Current = Default;

            playerDeath.OnPlayerDeath += Stop;
        }

        private void Stop()
        {
            Current = 0;
        }

        public void IncreaseWithLerp(float increasedValue, float duration)
        {
            var lerpDuration = duration / 10f;
            var boostDuration = duration - lerpDuration;
            //var sq
            //DOVirtual.Float(value, increasedValue, lerpDuration, null);

/*            var forwardTween = DOTween.To(
            () => value,
            (val) => anim.SetFloat("Forward", val),
            6,
            duration)
        .SetRelative()
        .SetEase(Ease.InOutQuad);
            s.Append(forwardTween);*/
        }

        void OnDestroy()
        {
            if( _playerDeath != null )
                _playerDeath.OnPlayerDeath -= Stop;
        }
    }
}