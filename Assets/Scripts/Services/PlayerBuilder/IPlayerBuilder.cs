using Cysharp.Threading.Tasks;
using RSR.Player;

namespace RSR.ServicesLogic
{
    public interface IPlayerBuilder : IService
    {
        UniTask<PlayerFacade> Build();
    }
}