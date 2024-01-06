using RSR.ServicesLogic;

namespace RSR.World
{
    public sealed class SlowBooster : Booster, IInteractable
    {
        public override BoosterType Type => BoosterType.Slow;

        private float _duration;
        private float _multiplyer;

        public void Constuct(IBoostersSettingsProvider settingsProvider)
        {
            _duration = settingsProvider.BoostersSettings.slowBoosterDuration;
            _multiplyer = settingsProvider.BoostersSettings.slowBoosterMultiplyer;
        }

        public void OnInteract()
        {

        }
    }
}
