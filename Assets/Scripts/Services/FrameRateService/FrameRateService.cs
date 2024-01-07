using UnityEngine;

namespace RSR.ServicesLogic
{
    public sealed class FrameRateService : IFrameRateService
    {
        public void SetFrameRate(int frameRate)
        {
            Application.targetFrameRate = frameRate;
        }

        public void SetVSync(int vSynCount)
        {
            QualitySettings.vSyncCount = vSynCount;
        }
    }
}