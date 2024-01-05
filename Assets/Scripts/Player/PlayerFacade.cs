using UnityEngine;

namespace RSR.Player
{
    /// <summary>
    /// Represents facade design pattern.
    /// Grants access to player's components.
    /// Considering this is an inetrface-like construction, properties are used.
    /// </summary>
    public sealed class PlayerFacade : MonoBehaviour
    {
        public IPlayerSpeedMultiplyer SpeedMultiplyer { get; set; }
        public IPlayerMoveDirReporter MoveDirReporter { get; set; }
        public IPlayerMove Move { get; set; }
        public IPlayerJump Jump { get; set; }
        public IPlayerControls Controls { get; set; }
        public IPlayerDeath Death { get; set; }
        public IPlayerAnimator Animator { get; set; }
        public IPlayerInteractor Interactor { get; set; }
    }
}