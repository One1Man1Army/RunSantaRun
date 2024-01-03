using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerMove : MonoBehaviour
    {
        public float _speed;

        private void Update()
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
    }
}
