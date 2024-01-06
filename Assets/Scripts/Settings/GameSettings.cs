using RSR.World;
using UnityEngine;

namespace RSR.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game_Settings")]
    public sealed class GameSettings : ScriptableObject
    {
        [Header("Curtain")]
        [Min(0f)]
        public float curtainsFadeTime = 0.33f;

        [Header("Camera")]
        public Vector3 cameraOffset;

        [Header("Player")]
        [Min(0f)]
        public float playerSpeedMultiplyer = 1f;
        [Min(0.1f)]
        public float playerJumpHeight = 1f;
        [Min(0.1f)]
        public float playerJumpTime = 0.1f;

        [Header("Background")]
        public Vector3 backgroundPos;

        [Header("Items")]
        [Min(0.1f)]
        public float spawnCooldownMin = 0.5f;
        [Min(0.1f)]
        public float spawnCooldownMax = 3f;
        [Min(0.25f)]
        public float spawnToPlayerOffset = 20f;
        public ItemRandomWeight[] itemsRandomWeights;
    }
}