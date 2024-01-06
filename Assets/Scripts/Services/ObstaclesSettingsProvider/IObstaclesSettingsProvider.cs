using RSR.Settings;

namespace RSR.ServicesLogic
{
    public interface IObstaclesSettingsProvider : IService
    {
        ObstaclesSettings ObstaclesSettings { get; }
    }
}