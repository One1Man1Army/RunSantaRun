using RSR.Settings;

namespace RSR.ServicesLogic
{
    public sealed class ObstaclesSettingsProvider : IObstaclesSettingsProvider
    {
        public ObstaclesSettings ObstaclesSettings { get; }
        public ObstaclesSettingsProvider(ObstaclesSettings obstaclesSettings)
        {
            ObstaclesSettings = obstaclesSettings;
        }
    }
}