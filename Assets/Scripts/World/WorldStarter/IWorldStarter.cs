using System;

namespace RSR.World
{
    public interface IWorldStarter
    {
        event Action OnStart;
        event Action OnReady;
        void GetReady();
    }
}