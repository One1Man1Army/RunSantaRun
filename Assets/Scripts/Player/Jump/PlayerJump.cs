using DG.Tweening;
using RSR.ServicesLogic;
using System;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerJump : MonoBehaviour, IPlayerJump
    {
        public event Action OnJump;
        public event Action OnLand;

        private IPlayerDeath _playerDeath;

        private float _jumpHeight;
        private float _jumpTime;
        private float _groundHeight;

        private Sequence _jump;
        private bool _isJumping;

        public void Construct(IGameSettingsProvider settingsProvider, IPlayerDeath playerDeath)
        {
            _jumpHeight = settingsProvider.GameSettings.playerJumpHeight;
            _jumpTime = settingsProvider.GameSettings.playerJumpTime;
            _groundHeight = transform.position.y;

            _playerDeath = playerDeath;
            _playerDeath.OnPlayerDeath += StopJump;
        }

        public void Jump()
        {
            if (_isJumping)
                return;

            if (_jump == null)
                InitJumpSequence();

            _isJumping = true;
            _jump.Rewind();
            _jump.Play();

            OnJump?.Invoke();
        }

        public void Fly(float duration, float height, float amplitude)
        {
            InitFlySequence(duration, height, amplitude);
            _isJumping = true;
            OnJump?.Invoke();
        }

        private void InitJumpSequence()
        {
            _jump = DOTween.Sequence()
                .Append(transform.DOMoveY(_jumpHeight, _jumpTime / 2f).SetEase(Ease.OutQuad))
                .Append(transform.DOMoveY(_groundHeight, _jumpTime / 2f).SetEase(Ease.InQuad))
                .SetAutoKill(false)
                .OnComplete(() =>
                {
                    if (_isJumping)
                        OnLand?.Invoke();

                    _isJumping = false;
                });
        }

        private void InitFlySequence(float duration, float height, float amplitude)
        {
            _jump?.Kill();

            _jump = DOTween.Sequence()
                .Append(transform.DOMoveY(height + amplitude, duration / 6f).SetEase(Ease.OutBack))
                .Append(transform.DOMoveY(height - amplitude, duration / 6f).SetEase(Ease.InOutBack))
                .Append(transform.DOMoveY(height + amplitude, duration / 6f).SetEase(Ease.InOutBack))
                .Append(transform.DOMoveY(height - amplitude, duration / 6f).SetEase(Ease.InOutBack))
                .Append(transform.DOMoveY(height + amplitude, duration / 6f).SetEase(Ease.InOutBack))
                .Append(transform.DOMoveY(_groundHeight, duration / 6f).SetEase(Ease.InCirc))
                .OnComplete(() => 
                { 
                    _jump = null;

                    if (_isJumping)
                        OnLand?.Invoke();

                    _isJumping = false;
                });
        }

        private void StopJump()
        {
            _jump?.Kill();
            _jump = null;
            _isJumping = false;
        }

        private void OnDestroy()
        {
            _playerDeath.OnPlayerDeath -= StopJump;
        }
    }
}