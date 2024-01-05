using RSR.World;
using UnityEngine;

namespace RSR.Player
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class PlayerInteractor : MonoBehaviour, IPlayerInteractor
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.OnInteract();
            }
        }
    }
}
