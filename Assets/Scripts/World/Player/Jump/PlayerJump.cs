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
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.OnInteract();
            }
        }

        public void Jump()
        {
            if (!IsAbleToJump())
                return;

            if (_jump == null)
                InitJumpSequence();

            _jump.Play();
        }

        private bool IsAbleToJump()
        {
            return _moveDirReporter.MoveDirection.y == 0 && _moveDirReporter.MoveDirection.x > 0;
        }

        private void InitJumpSequence()
        {
            _jump = DOTween.Sequence();

            _jump.Append(transform.DOMoveY(_jumpHeight, _jumpTime / 2f).SetEase(Ease.OutExpo));
            _jump.Append(transform.DOMoveY(_groundHeight, _jumpTime / 2f).SetEase(Ease.InExpo));
        }
    }
}