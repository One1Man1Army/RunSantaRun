using RSR.Curtains;

namespace RSR.ServicesLogic
{
    public interface ICurtainsService : IService
    {
        void ShowCurtain(CurtainType curtainType);
        void HideCurtains();
    }
}