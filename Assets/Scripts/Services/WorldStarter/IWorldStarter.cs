using RSR.ServicesLogic;
using System;

namespace RSR.World
{
    public interface IWorldStarter : IService
    {
        event Action OnStart;
        event Action OnReady;
        void GetReady();
    }
}