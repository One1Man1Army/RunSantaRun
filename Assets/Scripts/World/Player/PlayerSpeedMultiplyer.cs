using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace RSR.Player
{
    public  struct PlayerSpeedMultiplyer : IPlayerSpeedMultiplyer
    {
        public float Value { get; private set; }
        public void Construct()
        {

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