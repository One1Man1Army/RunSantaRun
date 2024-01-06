using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RSR.World
{
    public interface IObstaclesFactory
    {
        void Create(ObstacleType booster, Vector3 pos);
        void CreateRandom(Vector3 pos);
        UniTask Initialize();
    }
}