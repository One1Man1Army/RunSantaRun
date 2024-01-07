using RSR.Player;
using RSR.ServicesLogic;

namespace RSR.World
{
    public sealed class FlyBooster : Booster, IInteractable
    {
        public override BoosterType Type => BoosterType.Fly;

        private IPlayerJump _playerJump;

        private float _duration;
        private float _height;
        private float _amplitude;

        public void Constuct(IBoostersSettingsProvider settingsProvider, IPlayerJump playerJump)
        {
            _playerJump = playerJump;

            _duration = settingsProvider.BoostersSettings.flyBoosterDuration;
            _height = settingsProvider.BoostersSettings.flyBoosterHeight;
            _amplitude = settingsProvider.BoostersSettings.flyBoosterAmplitude;

            IsConstructed = true;
        }

        public void OnInteract()
        {
            _playerJump.Fly(_duration, _height, _amplitude);
            Release();
        }
    }
}