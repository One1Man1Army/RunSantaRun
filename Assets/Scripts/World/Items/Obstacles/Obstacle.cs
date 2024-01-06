using RSR.Player;
using RSR.ServicesLogic;
using UnityEngine;

namespace RSR.World
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Obstacle : PoolableItem
    {
        public override ItemType ItemType => ItemType.Obstacle;
        public abstract ObstacleType Type { get; }

        private IObstaclesSettingsProvider _settingsProvider;
        private IPlayerMoveDirReporter _playerMoveDir;
        private IPlayerDeath _playerDeath;

        public void Consturct(IObstaclesSettingsProvider settingsProvider, IPlayerDeath playerDeath, IPlayerMoveDirReporter playerMoveDir)
        {
            _settingsProvider = settingsProvider;
            _playerDeath = playerDeath;
            _playerMoveDir = playerMoveDir;
        }

        public void OnInteract()
        {
            if (_settingsProvider.ObstaclesSettings.isDestroyedByJump)
            {
                if(_playerMoveDir.MoveDirection.y < 0)
                {
                    Release();
                    return;
                }
            }

            _playerDeath.Happen();
        }
    }
}
