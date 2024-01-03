using RSR.Curtain;

namespace RSR.ServicesLogic
{
    internal interface ICurtainsService : IService
    {
        void ShowCurtain(CurtainType curtainType);
    }
}