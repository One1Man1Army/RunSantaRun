using RSR.Player;
using RSR.ServicesLogic;

namespace RSR.World
{
    public sealed class SpeedBooster : Booster, IInteractable
    {
        public override BoosterType Type => BoosterType.Speed;

        IPlayerSpeedMultiplyer _playerSpeedMultiplyer;

        private float _duration;
        private float _multiplyer;

        public void Constuct(IBoostersSettingsProvider settingsProvider, IPlayerSpeedMultiplyer playerSpeedMultiplyer)
        {
            _playerSpeedMultiplyer = playerSpeedMultiplyer;

            _duration = settingsProvider.BoostersSettings.speedBoosterDuration;
            _multiplyer = settingsProvider.BoostersSettings.speedBoosterMultiplyer;
        }

        public void OnInteract()
        {
            _playerSpeedMultiplyer.Boost(_multiplyer, _duration);
            Release();
        }
    }
}