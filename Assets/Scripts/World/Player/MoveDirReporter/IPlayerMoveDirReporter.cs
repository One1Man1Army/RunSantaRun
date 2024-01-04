using UnityEngine;

namespace RSR.Player
{
    public interface IPlayerMoveDirReporter
    {
        Vector2 MoveDirection { get; }
    }
}