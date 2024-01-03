using UnityEngine;

namespace RSR.CommonLogic
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Data/GameSettings")]
    public sealed class GameSettings : ScriptableObject
    {
        [Header("Curtain")]
        public float curtainsFadeTime = 0.33f;
    }
}