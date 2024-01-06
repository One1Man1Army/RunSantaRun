using System.Collections.Generic;

namespace RSR.ServicesLogic
{
    public interface IRandomService : IService
    {
        T GetWeightedRandomValue<T>(Dictionary<int, T> weightsTable);
        int GetRange(int min, int max);
        float GetRange(float min, float max);
    }
}