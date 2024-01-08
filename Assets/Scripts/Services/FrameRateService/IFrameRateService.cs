namespace RSR.ServicesLogic
{
    public interface IFrameRateService : IService
    {
        void SetFrameRate(int frameRate);
        void SetVSync(int vSynCount);
        void SetMaxFrameRate();
    }
}