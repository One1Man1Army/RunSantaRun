using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RSR.World
{
    public interface IObstaclesFactory
    {
        Obstacle Create(ObstacleType booster, Vector3 pos);
        Obstacle CreateRandom(Vector3 pos);
        UniTask Initialize();
        void ReleaseAll();
    }
}