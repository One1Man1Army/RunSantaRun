using System;

namespace RSR.ServicesLogic
{
    public interface IInputProvider : IService
    {
        bool IsFingerDown();
    }
}
