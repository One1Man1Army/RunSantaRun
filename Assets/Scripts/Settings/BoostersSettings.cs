using UnityEngine;

namespace RSR.Settings
{
    [CreateAssetMenu(fileName = "BoostersSettings", menuName = "Settings/Boosters_Settings")]
    public sealed class BoostersSettings : ScriptableObject
    {
        [Header("SlowDownBooster")]
        public float slowDownBoosterDuration = 10f;
        public float slowDownBoosterMultiplyer = 2f;

        [Header("SpeedUpBooster")]
        public float speedUpBoosterDuration = 10f;
        public float speedUpBoosterMultiplyer = 2f;

        [Header("FlyBooster")]
        public float flyBoosterDuration = 10f;
        public float flyBoosterHeight = 10f;
    }
}