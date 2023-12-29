using UnityEngine;

namespace RSR.CameraLogic
{
    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Transform _player;

        private void LateUpdate()
        {
            transform.position = _player.position + _offset;
        }
    }
}
