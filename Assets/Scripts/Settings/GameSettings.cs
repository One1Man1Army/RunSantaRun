﻿using RSR.World;
using UnityEngine;

namespace RSR.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game_Settings")]
    public sealed class GameSettings : ScriptableObject
    {
        [Header("Graphics")]
        [Min(30)]
        public int frameRate = 60;
        public bool maxFrameRate;
        [Min(0)]
        public int vSync = 0;

        [Header("Time")]
        public float timeScaleAddPerFrame = 0.001f;

        [Header("World")]
        public float worldMoveSpeed = 1f;
        public float defaultSpeedMultiplyer = 1f;

        [Header("Curtain")]
        [Min(0f)]
        public float curtainsFadeTime = 0.33f;

        [Header("Camera")]
        public Vector3 cameraPosition;

        [Header("Player")]
        [Min(0.1f)]
        public float playerJumpHeight = 1f;
        [Min(0.1f)]
        public float playerJumpTime = 0.1f;

        [Header("Background")]
        public Vector3 backgroundPosition;

        [Header("Items")]
        [Min(0.1f)]
        public float spawnCooldownMin = 0.2f;
        [Min(0.1f)]
        public float spawnCooldownMax = 1.5f;
        [Min(0.25f)]
        public float spawnToPlayerOffset = 15f;
        public ItemRandomWeight[] itemsRandomWeights;
    }
}