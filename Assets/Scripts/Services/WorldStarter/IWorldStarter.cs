using RSR.ServicesLogic;
using System;

namespace RSR.World
{
    public interface IWorldStarter : IService
    {
        event Action OnReady;
        event Action OnStart;
        void GetReady();
        void FinishWorld();
    }
}