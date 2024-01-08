using RSR.Player;
using UnityEngine;

namespace RSR.World
{
    public sealed class WorldMover : MonoBehaviour, IWorldMover
    {
        private ISpeedMultiplyer _speedMultiplyer;
        private IPlayerDeath _playerDeath;
        private IWorldStarter _worldStarter;

        private bool _isMoving;

        public void Construct(ISpeedMultiplyer speedMultiplyer, IPlayerDeath playerDeath, IWorldStarter worldStarter)
        {
            _speedMultiplyer = speedMultiplyer;
            _playerDeath = playerDeath;
            _worldStarter = worldStarter;

            _worldStarter.OnReady += ResetPosition;
            _worldStarter.OnStart += StartMoving;
            _playerDeath.OnPlayerDeath += Stop;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (_isMoving)
            {
                transform.Translate(Vector3.left * _speedMultiplyer.Current * _speedMultiplyer.Acceleration * Time.deltaTime);
            }
        }

        private void Stop()
        {
            _isMoving = false;
        }

        private void StartMoving()
        {
            _isMoving = true;
        }

        private void ResetPosition()
        {
            transform.position = Vector3.zero;
        }

        private void OnDestroy()
        {
            if (_playerDeath != null)
                _playerDeath.OnPlayerDeath -= Stop;

            if (_worldStarter != null)
            {
                _worldStarter.OnReady -= ResetPosition;
                _worldStarter.OnStart -= StartMoving;
            }
        }
    }
}
