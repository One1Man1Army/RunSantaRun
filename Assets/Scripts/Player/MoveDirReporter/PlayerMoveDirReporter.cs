using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerMoveDirReporter : MonoBehaviour, IPlayerMoveDirReporter
    {
        public Vector2 MoveDirection { get; private set; }

        private Vector2 _prevPos;
        private Vector2 _currentPos;

        private void Update()
        {
            CalcMoveDirection();
        }

        private void CalcMoveDirection()
        {
            _currentPos = transform.position;

            MoveDirection = (_currentPos - _prevPos).normalized;

            _prevPos = _currentPos;
        }
    }
}