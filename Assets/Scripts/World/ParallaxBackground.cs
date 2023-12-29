using UnityEngine;

namespace RSR
{
    public sealed class ParallaxBackground : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Vector2 _parallaxSpeed = Vector2.one;

        private Transform _camera;
        private Vector3 _lastCameraPosition;
        private float _textureUnitSizeX;
        private float _textureUnitSizeY;
        #endregion

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _camera = Camera.main.transform;
            _lastCameraPosition = _camera.transform.position;
            var sprite = GetComponent<SpriteRenderer>().sprite;
            var texture = sprite.texture;
            _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
            _textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
        }

        private void LateUpdate()
        {
            Move();

            if (IsOutOfScreenWidth())
            {
                BundgeLeft();
            }

            if (IsOutOfScreenHeight())
            {
                BundgeUp();
            }
        }

        private void Move()
        {
            var deltaMovement = _camera.transform.position - _lastCameraPosition;
            transform.position += new Vector3(deltaMovement.x * _parallaxSpeed.x, deltaMovement.y * _parallaxSpeed.y, 0);
            _lastCameraPosition = _camera.transform.position;
        }

        private void BundgeUp()
        {
            var offsetY = (_camera.transform.position.y - transform.position.y) % _textureUnitSizeY;
            transform.position = new Vector3(_camera.transform.position.x, transform.position.y + offsetY, transform.position.z);
        }

        private void BundgeLeft()
        {
            var offsetX = (_camera.transform.position.x - transform.position.x) % _textureUnitSizeX;
            transform.position = new Vector3(_camera.transform.position.x + offsetX, transform.position.y, transform.position.z);
        }

        private bool IsOutOfScreenWidth() =>
            Mathf.Abs(_camera.transform.position.x - transform.position.x) >= _textureUnitSizeX;

        private bool IsOutOfScreenHeight() => 
            Mathf.Abs(_camera.transform.position.y - transform.position.y) >= _textureUnitSizeY;
    }
}
