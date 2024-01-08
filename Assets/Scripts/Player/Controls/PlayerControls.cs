using RSR.ServicesLogic;
using RSR.World;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerControls : MonoBehaviour, IPlayerControls
    {
        public bool IsControlsEnabled { get; private set; }

        private IInputProvider _inputProvider;
        private IPlayerJump _playerJump;
        private IPlayerDeath _playerDeath;
        private IWorldStarter _worldStarter;

        public void Construct(IInputProvider inputProvider, IPlayerJump playerJump, IPlayerDeath playerDeath, IWorldStarter worldStarter)
        {
            _inputProvider = inputProvider;
            _playerJump = playerJump;
            _playerDeath = playerDeath;
            _worldStarter = worldStarter;

            _playerDeath.OnPlayerDeath += DisableControls;
            _worldStarter.OnStart += EnableControls;
        }

        private void Update()
        {
            if (IsInitialized())
            {
                if (IsControlsEnabled)
                {
                    JumpOnTap();
                }
            }
        }

        private void JumpOnTap()
        {
            if (_inputProvider.HasPlayerTapped())
                _playerJump.Jump();
        }

        private void DisableControls()
        {
            IsControlsEnabled = false;
        }

        private void EnableControls()
        {
            IsControlsEnabled = true;
        }

        private bool IsInitialized()
        {
            return _inputProvider != null && _playerJump!= null;
        }

        private void OnDestroy()
        {
            _playerDeath.OnPlayerDeath -= DisableControls;
            _worldStarter.OnStart -= EnableControls;
        }
    }
}