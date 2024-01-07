using UnityEngine;

namespace RSR.Parallax
{
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class ParallaxMover : MonoBehaviour
    {
        [SerializeField] private float _parallaxSpeed;

        private Transform _camera;
        private Material _material;
        private float _camStartPosX;
        private float _distance;
        private float _speedMultiplyer = 0.05f;

        private void Start()
        {
            _camera = Camera.main.transform;
            _material = GetComponent<MeshRenderer>().material;
        }

        private void LateUpdate()
        {
            if (_camera != null)
            {
                MoveImage();
            }
        }

        private void MoveImage()
        {
            _distance = _camera.transform.position.x - _camStartPosX;
            transform.position = new Vector3(_camera.transform.position.x, transform.position.y, transform.position.z);
            _material.SetTextureOffset("_MainTex", new Vector2(_distance, 0) * _parallaxSpeed * _speedMultiplyer);
        }
    }
}


