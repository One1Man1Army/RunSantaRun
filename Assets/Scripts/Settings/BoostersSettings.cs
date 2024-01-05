using UnityEngine;

namespace RSR.Settings
{
    [CreateAssetMenu(fileName = "BoostersSettings", menuName = "Settings/Boosters_Settings")]
    public sealed class BoostersSettings : ScriptableObject
    {
        public float boostersSpawnHeight = -0.6f;

        [Header("SlowBooster")]
        public float slowBoosterDuration = 10f;
        public float slowBoosterMultiplyer = 2f;

        [Header("SpeedBooster")]
        public float speedBoosterDuration = 10f;
        public float speedBoosterMultiplyer = 2f;

        [Header("FlyBooster")]
        public float flyBoosterDuration = 10f;
        public float flyBoosterHeight = 10f;
    }
}