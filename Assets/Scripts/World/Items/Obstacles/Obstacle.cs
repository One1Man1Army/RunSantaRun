using RSR.Player;
using UnityEngine;

namespace RSR.World
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Obstacle : PoolableItem, IInteractable
    {
        private IPlayerDeath _playerDeath;
        public void Consturct(IPlayerDeath playerDeath)
        {
            _playerDeath = playerDeath;
        }
        public override ItemType ItemType => ItemType.Obstacle;
        public abstract ObstacleType Type { get; }

        public void OnInteract()
        {
            _playerDeath.Happen();
            Release();
        }
    }
}
