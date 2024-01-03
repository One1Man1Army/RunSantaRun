namespace RSR.Curtain
{
    public interface ICurtainsView
    {
        void ShowCurtain(CurtainType curtainType);
        void SetFadeTime(float time);
    }
}