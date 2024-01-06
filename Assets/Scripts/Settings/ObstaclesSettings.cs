using RSR.World;
using UnityEngine;

namespace RSR.Settings
{
    [CreateAssetMenu(fileName = "ObstaclesSettings", menuName = "Settings/Obstacles_Settings")]
    public sealed class ObstaclesSettings : ScriptableObject
    {
        public bool isDestroyedByJump;

        [Header("Spawn Height")]
        public float lowSpawnHeight = -0.2f;
        public float highSpawnHeight = 0.1f;

        [Header("Boosters Random Weights Table")]
        public ObstacleRandomWeight[] obstaclesRandomWeights;
    }
}