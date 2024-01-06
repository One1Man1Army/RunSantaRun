using RSR.Player;
using RSR.ServicesLogic;

namespace RSR.World
{
    public sealed class SlowBooster : Booster, IInteractable
    {
        public override BoosterType Type => BoosterType.Slow;

        private IPlayerSpeedMultiplyer _playerSpeedMultiplyer;

        private float _duration;
        private float _multiplyer;

        public void Constuct(IBoostersSettingsProvider settingsProvider, IPlayerSpeedMultiplyer playerSpeedMultiplyer)
        {
            _playerSpeedMultiplyer = playerSpeedMultiplyer;

            _duration = settingsProvider.BoostersSettings.slowBoosterDuration;
            _multiplyer = settingsProvider.BoostersSettings.slowBoosterMultiplyer;
        }

        public void OnInteract()
        {
            _playerSpeedMultiplyer.Boost(_multiplyer, _duration);
            Release();
        }
    }
}
