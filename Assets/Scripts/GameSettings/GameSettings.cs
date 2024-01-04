using UnityEngine;

namespace RSR.CommonLogic
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Data/GameSettings")]
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
        public float playerJumpHeight = 1f;
        public float playerJumpTime = 0.1f;
    }
}