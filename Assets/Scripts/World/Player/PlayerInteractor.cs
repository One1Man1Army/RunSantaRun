using RSR.World;
using UnityEngine;

namespace RSR.Player
{
    [RequireComponent(typeof(Collider))]
    public sealed class PlayerInteractor : MonoBehaviour
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
