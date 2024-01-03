using RSR.ServicesLogic;
using UnityEngine;

namespace RSR.CameraLogic
{
    public sealed class CameraFollow : MonoBehaviour
    {
        private Transform _player;
        private Vector3 _offset;

        public void Construct(Transform player, IGameSettingsProvider settingsProvider)
        {
            _player = player;
            _offset = settingsProvider.GameSettings.cameraOffset;
        }

        private void LateUpdate()
        {
            if (IsPlayerInitialized())
                FollowPlayer();
        }

        private bool IsPlayerInitialized()
        {
            return _player != null;
        }

        private void FollowPlayer()
        {
            transform.position = _player.position + _offset;
        }
    }
}
