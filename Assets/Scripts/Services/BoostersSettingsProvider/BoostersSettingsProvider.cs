using RSR.Settings;

namespace RSR.ServicesLogic
{
    public sealed class BoostersSettingsProvider : IBoostersSettingsProvider
    {
        public BoostersSettings BoostersSettings { get; }
        public BoostersSettingsProvider(BoostersSettings boostersSettings)
        {
            BoostersSettings = boostersSettings;
        }
    }
}
