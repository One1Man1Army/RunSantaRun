using DG.Tweening;
using RSR.ServicesLogic;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerJump : MonoBehaviour, IPlayerJump
    {
        private IPlayerMoveDirReporter _moveDirReporter;

        private float _jumpHeight;
        private float _jumpTime;
        private float _groundHeight;

        private Sequence _jump;

        public void Construct(IGameSettingsProvider settingsProvider, IPlayerMoveDirReporter moveDirReporter)
        {
            _moveDirReporter = moveDirReporter;

            _jumpHeight = settingsProvider.GameSettings.playerJumpHeight;
            _jumpTime = settingsProvider.GameSettings.playerJumpTime;
            _groundHeight = transform.position.y;
        }

        public void Jump()
        {
            if (!CanJump())
                return;

            if (_jump == null)
                InitJumpSequence();

            _jump.Rewind();
            _jump.Play();
        }

        public void Fly(float duration, float height, float amplitude)
        {
            InitFlySequence(duration, height, amplitude);
        }

        private bool CanJump()
        {
            return _moveDirReporter.MoveDirection.y == 0 && _moveDirReporter.MoveDirection.x > 0;
        }

        private void InitJumpSequence()
        {
            _jump = DOTween.Sequence()
                .Append(transform.DOMoveY(_jumpHeight, _jumpTime / 2f).SetEase(Ease.OutQuad))
                .Append(transform.DOMoveY(_groundHeight, _jumpTime / 2f).SetEase(Ease.InQuad))
                .SetAutoKill(false);
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
                .Append(transform.DOMoveY(_groundHeight, duration / 6f).SetEase(Ease.InElastic))
                .OnComplete(() => _jump = null);
        }
    }
}