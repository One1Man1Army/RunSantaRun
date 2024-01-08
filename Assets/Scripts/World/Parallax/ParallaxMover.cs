using UnityEngine;

namespace RSR.World
{
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class ParallaxMover : MonoBehaviour
    {
        //Set in inspector for each image individually.
        [SerializeField] private float _parallaxSpeed;

        private IWorldMover _worldMover;
        private Material _material;

        public void Construct(IWorldMover worldMover)
        {
            _material = GetComponent<MeshRenderer>().material;
            _worldMover = worldMover;
        }

        private void LateUpdate()
        {
            if (_material != null)
            {
                MoveImage();
            }
        }

        private void MoveImage()
        {
            _material.SetTextureOffset("_MainTex", new Vector2(_worldMover.Distance, 0) * _parallaxSpeed * 0.05f);
        }
    }
}


