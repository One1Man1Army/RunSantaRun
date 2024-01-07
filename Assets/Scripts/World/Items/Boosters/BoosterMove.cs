using RSR.ServicesLogic;
using UnityEngine;

namespace RSR.World
{
    public sealed class BoosterMove : MonoBehaviour
    {
        private float _speed;
        private float _height;
        private float _startHeight;

        public void Construct(IBoostersSettingsProvider boostersSettingsProvider)
        {
            _startHeight = boostersSettingsProvider.BoostersSettings.boostersSpawnHeight;
            _speed = boostersSettingsProvider.BoostersSettings.boostersMoveSpeed;
            _height = boostersSettingsProvider.BoostersSettings.boostersMoveHeight;
        }

        private void Update()
        {
            float newY = Mathf.Sin(Time.time * _speed) * _height + _startHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}