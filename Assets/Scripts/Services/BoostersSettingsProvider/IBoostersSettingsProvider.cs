using RSR.Settings;

namespace RSR.ServicesLogic
{
    public interface IBoostersSettingsProvider : IService
    {
        public BoostersSettings BoostersSettings { get; }
    }
}