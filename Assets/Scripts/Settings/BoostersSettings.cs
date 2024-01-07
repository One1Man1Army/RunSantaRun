using RSR.World;
using UnityEngine;

namespace RSR.Settings
{
    [CreateAssetMenu(fileName = "BoostersSettings", menuName = "Settings/Boosters_Settings")]
    public sealed class BoostersSettings : ScriptableObject
    {
        public float boostersSpawnHeight = -0.6f;
        public float boostersMoveSpeed = 1f;
        public float boostersMoveHeight = 0.2f;

        [Header("Slow Booster")]
        public float slowBoosterDuration = 10f;
        public float slowBoosterMultiplyer = 2f;

        [Header("Speed Booster")]
        public float speedBoosterDuration = 10f;
        public float speedBoosterMultiplyer = 2f;

        [Header("Fly Booster")]
        public float flyBoosterDuration = 10f;
        public float flyBoosterHeight = 10f;
        public float flyBoosterAmplitude = 1f;

        [Header("Boosters Random Weights Table")]
        public BoosterRandomWeight[] boostersRandomWeights;
    }
}