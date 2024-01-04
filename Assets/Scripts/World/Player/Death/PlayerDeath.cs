using System;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerDeath : MonoBehaviour, IPlayerDeath
    {
        public bool IsDead { get; private set; }

        public event Action OnPlayerDeath;

        public void Happen()
        {
            IsDead = true;
            OnPlayerDeath?.Invoke();
        }
    }
}