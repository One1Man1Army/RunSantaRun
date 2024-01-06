using DG.Tweening;
using RSR.ServicesLogic;
using RSR.World;
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

        private bool CanJump()
        {
            return _moveDirReporter.MoveDirection.y == 0 && _moveDirReporter.MoveDirection.x > 0;
        }

        private void InitJumpSequence()
        {
            _jump = DOTween.Sequence();

            _jump.Append(transform.DOMoveY(_jumpHeight, _jumpTime / 2f).SetEase(Ease.OutQuad));
            _jump.Append(transform.DOMoveY(_groundHeight, _jumpTime / 2f).SetEase(Ease.InQuad));

            _jump.SetAutoKill(false);
        }
    }
}