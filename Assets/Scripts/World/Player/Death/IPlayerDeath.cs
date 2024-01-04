using System;

namespace RSR.Player
{
    public interface IPlayerDeath
    {
        bool IsDead { get; }
        event Action OnPlayerDeath;
        void Happen();
    }
}