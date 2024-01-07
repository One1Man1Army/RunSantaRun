using DG.Tweening;
using RSR.Player;
using RSR.ServicesLogic;
using System;
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

        private Collider2D _collider;

        public void Consturct(IObstaclesSettingsProvider settingsProvider, IPlayerDeath playerDeath, IPlayerMoveDirReporter playerMoveDir)
        {
            _settingsProvider = settingsProvider;
            _playerDeath = playerDeath;
            _playerMoveDir = playerMoveDir;
            _collider = GetComponent<Collider2D>();

            IsConstructed = true;
        }

        public void OnInteract()
        {
            if (_settingsProvider.ObstaclesSettings.isDestroyedByJump)
            {
                if(_playerMoveDir.MoveDirection.y < 0)
                {
                    GetDead();
                    return;
                }
            }

            _playerDeath.Happen();
        }

        private void GetDead()
        {
            transform.position = new Vector3(transform.position.x, _collider.bounds.min.y, transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x, 0.1f, transform.localScale.z);
            _collider.enabled = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_collider != null)
                _collider.enabled = true;
        }
    }
}
