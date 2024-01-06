using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RSR.World
{
    public interface IBoostersFactory
    {
        void Create(BoosterType booster, Vector3 pos);
        void CreateRandom(Vector3 pos);
        UniTask Initialize();
        void ReleaseAll();
    }
}