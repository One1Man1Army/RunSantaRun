using RSR.World;
using System;
using UnityEngine;

namespace RSR.Player
{
    [RequireComponent(typeof(Animator))]
    public sealed class PlayerAnimator : MonoBehaviour
    {
        private IPlayerMoveDirReporter _moveDirReporter;
        private IPlayerDeath _playerDeath;
        private IWorldStarter _worldStarter;

        private Animator _animator;

        public void Construct(IPlayerMoveDirReporter moveDirReporter, IPlayerDeath playerDeath, IWorldStarter worldStarter)
        {
            _moveDirReporter = moveDirReporter;
            _playerDeath = playerDeath;
            _worldStarter = worldStarter;

            _playerDeath.OnPlayerDeath += PlayDeath;
            _worldStarter.OnReady += PlayIdle;

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


        private void PlayIdle()
        {
            _animator.SetTrigger(AnimatorHashKeys.IdleHash);
        }

        private bool IsInitialized()
        {
            return _moveDirReporter != null && _playerDeath != null && _animator != null;
        }

        private void OnDestroy()
        {
            if (_playerDeath != null)
                _playerDeath.OnPlayerDeath -= PlayDeath;

            if (_worldStarter != null)
                _worldStarter.OnReady -= PlayIdle;
        }
    }
}