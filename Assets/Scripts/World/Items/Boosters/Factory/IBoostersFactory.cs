using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RSR.World
{
    public interface IBoostersFactory
    {
        Booster Create(BoosterType booster, Vector3 pos);
        Booster CreateRandom(Vector3 pos);
        UniTask Initialize();
        void ReleaseAll();
    }
}