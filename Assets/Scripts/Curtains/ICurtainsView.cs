namespace RSR.Curtains
{
    public interface ICurtainsView
    {
        void ShowCurtain(CurtainType curtainType);
        void HideCurtains();
        void SetFadeTime(float time);
    }
}