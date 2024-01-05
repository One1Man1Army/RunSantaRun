using Cysharp.Threading.Tasks;

namespace RSR.ServicesLogic
{
    public interface IWorldBuilder : IService
    {
        UniTask Build();
        UniTask Prewarm();
    }
}