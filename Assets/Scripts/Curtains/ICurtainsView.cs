namespace RSR.Curtain
{
    public interface ICurtainsView
    {
        void ShowCurtain(CurtainType curtainType);
        void HideCurtains();
        void SetFadeTime(float time);
    }
}