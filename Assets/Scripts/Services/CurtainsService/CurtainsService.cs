using RSR.CommonLogic;
using RSR.Curtain;

namespace RSR.ServicesLogic
{
    /// <summary>
    /// Curtain logic represents MVC design pattern.
    /// Curtain service is a controller.
    /// Curtains fade speed is a model stored in GameSettings.
    /// Curtain view is a view that contains animation logic.
    /// </summary>
    public sealed class CurtainsService : ICurtainsService
    {
        private readonly ICurtainsView _curtainsView;

        public CurtainsService(ICurtainsView curtainsView, IGameSettingsProvider settingsProvider)
        {
            _curtainsView = curtainsView;
            _curtainsView.SetFadeTime(settingsProvider.GameSettings.curtainsFadeTime);
        }

        public void ShowCurtain(CurtainType curtainType)
        {
            _curtainsView.ShowCurtain(curtainType);
        }
    }
}