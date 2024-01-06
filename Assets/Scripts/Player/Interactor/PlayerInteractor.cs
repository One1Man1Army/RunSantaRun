using RSR.World;
using UnityEngine;

namespace RSR.Player
{
    [RequireComponent(typeof(Collider2D), (typeof(Rigidbody2D)))]
    public sealed class PlayerInteractor : MonoBehaviour, IPlayerInteractor
    {
        void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().isKinematic = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.OnInteract();
            }
        }
    }
}
