using UnityEngine;

namespace RSR.World
{
    [RequireComponent(typeof(Collider2D), (typeof(Rigidbody2D)))]
    public sealed class ItemsReleaser : MonoBehaviour, IItemsReleaser
    {
        private void Awake()
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PoolableItem>(out var item))
            {
                item.Release();
            }
        }
    }
}