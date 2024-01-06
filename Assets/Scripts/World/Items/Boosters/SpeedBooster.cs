using RSR.ServicesLogic;

namespace RSR.World
{
    public sealed class SpeedBooster : Booster, IInteractable
    {
        public override BoosterType Type => BoosterType.Speed;

        private float _duration;
        private float _multiplyer;

        public void Constuct(IBoostersSettingsProvider settingsProvider)
        {
            _duration = settingsProvider.BoostersSettings.speedBoosterDuration;
            _multiplyer = settingsProvider.BoostersSettings.speedBoosterMultiplyer;
        }

        public void OnInteract()
        {

        }
    }
}