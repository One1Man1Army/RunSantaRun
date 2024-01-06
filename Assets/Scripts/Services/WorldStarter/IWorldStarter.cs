using RSR.ServicesLogic;
using System;

namespace RSR.World
{
    public interface IWorldStarter : IService
    {
        event Action OnReady;
        event Action OnStart;
        event Action OnRestart;
        void GetReady();
        void FinishWorld();
    }
}