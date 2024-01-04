using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerMove : MonoBehaviour
    {
        private IPlayerSpeedMultiplyer _speedMultiplyer;

        public void Construct(IPlayerSpeedMultiplyer speedMultiplyer)
        {
            _speedMultiplyer = speedMultiplyer;
        }

        private void Update()
        {
            transform.Translate(Vector3.right * _speedMultiplyer.Current * Time.deltaTime);
        }
    }
}
