using RSR.CommonLogic;
using UnityEditor;
using UnityEngine;

namespace RSR.ServicesLogic
{
    public sealed class GameSettingsProvider : IGameSettingsProvider
    {
        public GameSettings GameSettings { get; }
        public GameSettingsProvider(GameSettings gameSettings)
        {
            GameSettings = gameSettings;
        }
    }
}