using RSR.Player;
using RSR.ServicesLogic;

namespace RSR.World
{
    public sealed class SpeedBooster : Booster, IInteractable
    {
        public override BoosterType Type => BoosterType.Speed;

        ISpeedMultiplyer _speedMultiplyer;

        private float _duration;
        private float _multiplyer;

        public void Constuct(IBoostersSettingsProvider settingsProvider, ISpeedMultiplyer speedMultiplyer)
        {
            _speedMultiplyer = speedMultiplyer;

            _duration = settingsProvider.BoostersSettings.speedBoosterDuration;
            _multiplyer = settingsProvider.BoostersSettings.speedBoosterMultiplyer;

            IsConstructed = true;
        }

        public void OnInteract()
        {
            _speedMultiplyer.Boost(_multiplyer, _duration);
            Release();
        }
    }
}