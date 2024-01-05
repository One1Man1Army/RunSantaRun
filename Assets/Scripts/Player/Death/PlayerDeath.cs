using RSR.World;
using System;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerDeath : MonoBehaviour, IPlayerDeath
    {
        private IWorldStarter _worldStarter;
        public void Construct(IWorldStarter worldStarter)
        {
            _worldStarter = worldStarter;
            _worldStarter.OnReady += GetAlive;
        }

        public bool IsDead { get; private set; }

        public event Action OnPlayerDeath;

        public void Happen()
        {
            IsDead = true;
            OnPlayerDeath?.Invoke();
        }

        private void GetAlive()
        {
            IsDead = false;
        }

        private void OnDestroy()
        {
            if (_worldStarter != null)
                _worldStarter.OnReady -= GetAlive;
        }
    }
}