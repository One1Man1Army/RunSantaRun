using RSR.CommonLogic;

namespace RSR.ServicesLogic
{
    public interface IGameSettingsProvider : IService
    {
        public GameSettings GameSettings { get; }
    }
}