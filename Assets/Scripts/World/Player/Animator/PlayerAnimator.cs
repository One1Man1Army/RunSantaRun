using System;
using UnityEngine;

namespace RSR.Player
{
    [RequireComponent(typeof(Animator))]
    public sealed class PlayerAnimator : MonoBehaviour
    {
        private IPlayerMoveDirReporter _moveDirReporter;
        private IPlayerDeath _playerDeath;

        private Animator _animator;

        public void Construct(IPlayerMoveDirReporter moveDirReporter, IPlayerDeath playerDeath)
        {
            _moveDirReporter = moveDirReporter;
            _playerDeath = playerDeath;
            _playerDeath.OnPlayerDeath += PlayDeath;

            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (IsInitialized())
            {
                if (!_playerDeath.IsDead)
                {
                    SetAnimatorValues();
                }
            }
        }

        private void SetAnimatorValues()
        {
            _animator.SetFloat(AnimatorHashKeys.MoveDirXHash, _moveDirReporter.MoveDirection.x);
            _animator.SetFloat(AnimatorHashKeys.MoveDirYHash, _moveDirReporter.MoveDirection.y);

        }

        private void PlayDeath()
        {
            _animator.SetTrigger(AnimatorHashKeys.DieHash);
        }

        private bool IsInitialized()
        {
            return _moveDirReporter != null && _playerDeath != null && _animator != null;
        }

        private void OnDestroy()
        {
            if (_playerDeath != null)
                _playerDeath.OnPlayerDeath -= PlayDeath;
        }
    }
}