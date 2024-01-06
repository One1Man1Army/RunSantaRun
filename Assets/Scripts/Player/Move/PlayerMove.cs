using RSR.World;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerMove : MonoBehaviour, IPlayerMove
    {
        private IPlayerSpeedMultiplyer _speedMultiplyer;
        private IPlayerDeath _playerDeath;
        private IWorldStarter _worldStarter;

        private bool _canMove;

        public void Construct(IPlayerSpeedMultiplyer speedMultiplyer, IPlayerDeath playerDeath, IWorldStarter worldStarter)
        {
            _speedMultiplyer = speedMultiplyer;
            _playerDeath = playerDeath;
            _worldStarter = worldStarter;

            _worldStarter.OnReady += TeleportToStart;
            _worldStarter.OnStart += Go;
            _playerDeath.OnPlayerDeath += Stop;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (_canMove)
            {
                transform.Translate(Vector3.right * _speedMultiplyer.Current * _speedMultiplyer.SpeedUp * Time.deltaTime);
            }
        }

        private void Stop()
        {
            _canMove = false;
        }

        private void Go()
        {
            _canMove = true;
        }

        private void TeleportToStart()
        {
            transform.position = Vector3.zero;
        }

        private void OnDestroy()
        {
            if (_playerDeath != null)
                _playerDeath.OnPlayerDeath -= Stop;

            if (_worldStarter != null)
            {
                _worldStarter.OnReady -= TeleportToStart;
                _worldStarter.OnStart -= Go;
            }
        }
    }
}
