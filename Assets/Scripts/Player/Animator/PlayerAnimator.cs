using RSR.World;
using UnityEngine;

namespace RSR.Player
{
    [RequireComponent(typeof(Animator))]
    public sealed class PlayerAnimator : MonoBehaviour, IPlayerAnimator
    {
        private IWorldStarter _worldStarter;
        private IPlayerJump _playerJump;
        private IPlayerDeath _playerDeath;

        private Animator _animator;

        public void Construct(IWorldStarter worldStarter, IPlayerJump playerJump, IPlayerDeath playerDeath)
        {
            _playerJump = playerJump;
            _playerDeath = playerDeath;
            _worldStarter = worldStarter;

            _playerDeath.OnPlayerDeath += PlayDie;
            _playerJump.OnJump += PlayJump;
            _playerJump.OnLand += PlayRun;
            _worldStarter.OnReady += PlayIdle;
            _worldStarter.OnStart += PlayRun;

            _animator = GetComponent<Animator>();
        }

        private void PlayDie()
        {
            _animator.Play(AnimatorHashKeys.DieHash);
        }

        private void PlayIdle()
        {
            _animator.Play(AnimatorHashKeys.IdleHash);
        }

        private void PlayRun()
        {
            _animator.Play(AnimatorHashKeys.RunHash);
        }

        private void PlayJump()
        {
            _animator.Play(AnimatorHashKeys.JumpHash);
        }

        private void OnDestroy()
        {
            _playerDeath.OnPlayerDeath -= PlayDie;
            _playerJump.OnJump -= PlayJump;
            _playerJump.OnLand -= PlayRun;
            _worldStarter.OnReady -= PlayIdle;
            _worldStarter.OnStart -= PlayRun;
        }
    }
}