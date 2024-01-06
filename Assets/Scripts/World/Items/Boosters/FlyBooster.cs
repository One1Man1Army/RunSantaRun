using RSR.ServicesLogic;

namespace RSR.World
{
    public sealed class FlyBooster : Booster, IInteractable
    {
        public override BoosterType Type => BoosterType.Fly;

        private float _duration;
        private float _height;

        public void Constuct(IBoostersSettingsProvider settingsProvider)
        {
            _duration = settingsProvider.BoostersSettings.flyBoosterDuration;
            _height = settingsProvider.BoostersSettings.flyBoosterHeight;
        }

        public void OnInteract()
        {

        }
    }
}