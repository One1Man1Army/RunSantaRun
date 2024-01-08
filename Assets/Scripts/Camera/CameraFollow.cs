using RSR.ServicesLogic;
using UnityEngine;

namespace RSR.CameraLogic
{
    public sealed class CameraFollow : MonoBehaviour
    {
        private Transform _player;
        private Vector3 _offset;
        private float _smoothDamp = 0.25f;

        private Vector3 _velocity;

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

        private void FollowPlayer()
        {
            Vector3 targetPosition = new(_player.position.x + _offset.x, _offset.y, _player.position.z + _offset.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothDamp);
        }

        private bool IsPlayerInitialized()
        {
            return _player != null;
        }

/*        private void FollowPlayer()
        {
            var newPos = _player.position + _offset;
            newPos.y = _offset.y;
            transform.position = newPos;
        }*/
    }
}
