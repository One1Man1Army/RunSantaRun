using RSR.ServicesLogic;
using RSR.World;
using System;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerSpeedMultiplyer : MonoBehaviour, IPlayerSpeedMultiplyer
    {
        public float Default { get; private set; }
        public float Current { get; private set; }

        public void Construct(IGameSettingsProvider gameSettingsProvider)
        {
            Default = gameSettingsProvider.GameSettings.playerSpeedMultiplyer;
            Current = Default;
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
    }
}