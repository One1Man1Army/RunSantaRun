using RSR.ServicesLogic;
using RSR.World;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerControls : MonoBehaviour, IPlayerControls
    {
        public bool IsInputEnabled { get; private set; }

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

            _playerDeath.OnPlayerDeath += DisableInput;
            _worldStarter.OnStart += EnableInput;
        }

        private void Update()
        {
            if (IsInitialized())
            {
                if (IsInputEnabled)
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

        private void DisableInput()
        {
            IsInputEnabled = false;
        }

        private void EnableInput()
        {
            IsInputEnabled = true;
        }

        private bool IsInitialized()
        {
            return _inputProvider != null && _playerJump!= null;
        }

        private void OnDestroy()
        {
            if (_playerDeath != null)
                _playerDeath.OnPlayerDeath -= DisableInput;

            if (_worldStarter != null)
                _worldStarter.OnStart -= EnableInput;
        }
    }
}