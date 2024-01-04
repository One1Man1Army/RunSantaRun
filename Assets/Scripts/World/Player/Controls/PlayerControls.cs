using RSR.ServicesLogic;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerControls : MonoBehaviour, IPlayerControls
    {
        public bool IsInputEnabled { get; private set; }

        private IInputProvider _inputProvider;
        private IPlayerJump _playerJump;
        private IPlayerDeath _playerDeath;

        public void Construct(IInputProvider inputProvider, IPlayerJump playerJump, IPlayerDeath playerDeath)
        {
            _inputProvider = inputProvider;
            _playerJump = playerJump;
            _playerDeath = playerDeath;

            _playerDeath.OnPlayerDeath += DisableInput;
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
        }
    }
}