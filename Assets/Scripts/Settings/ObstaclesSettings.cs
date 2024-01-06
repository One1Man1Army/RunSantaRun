using RSR.World;
using UnityEngine;

namespace RSR.Settings
{
    [CreateAssetMenu(fileName = "ObstaclesSettings", menuName = "Settings/Obstacles_Settings")]
    public sealed class ObstaclesSettings : ScriptableObject
    {
        public float obstaclesSpawnHeight = -0.6f;

        [Header("Obstacles Random Weights Table")]
        public ObstacleRandomWeight[] obstaclesRandomWeights;
    }
}