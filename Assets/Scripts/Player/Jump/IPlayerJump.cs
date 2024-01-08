using System;

namespace RSR.Player
{
    public interface IPlayerJump
    {
        event Action OnJump;
        event Action OnLand;
        void Fly(float duration, float height, float amplitude);
        void Jump();
    }
}