using System.Collections;
using UnityEngine;

namespace RSR.Player
{
    public sealed class PlayerFacade : MonoBehaviour
    {
        public IPlayerSpeedMultiplyer PlayerSpeedMultiplyer { get; private set; }
        public void Construct()
        {
            PlayerSpeedMultiplyer = GetComponent<IPlayerSpeedMultiplyer>();
        }
    }
}