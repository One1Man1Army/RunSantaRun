using RSR.Player;
using RSR.ServicesLogic;
using UnityEngine;

namespace RSR.World
{
    public sealed class WorldMover : MonoBehaviour, IWorldMover
    {
        public float Distance { get; private set; }

        private ISpeedMultiplyer _speedMultiplyer;
        private IPlayerDeath _playerDeath;
        private IWorldStarter _worldStarter;
        private ITimeMachine _timeMachine;

        private bool _isMoving;
        private float _moveSpeed;

        public void Construct(IGameSettingsProvider gameSettingsProvider, ISpeedMultiplyer speedMultiplyer, IPlayerDeath playerDeath, IWorldStarter worldStarter, ITimeMachine timeMachine)
        {
            _speedMultiplyer = speedMultiplyer;
            _playerDeath = playerDeath;
            _worldStarter = worldStarter;
            _timeMachine = timeMachine;

            _moveSpeed = gameSettingsProvider.GameSettings.worldMoveSpeed;

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
                transform.Translate(Vector3.left * _moveSpeed * _speedMultiplyer.Current * _timeMachine.CurrentTime * Time.deltaTime);
                Distance = Mathf.Abs(transform.position.x);
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
            _worldStarter.OnReady -= ResetPosition;
            _worldStarter.OnStart -= StartMoving;
            _playerDeath.OnPlayerDeath -= Stop;
        }
    }
}
