using System.Collections.Generic;
using System.Linq;

namespace RSR.ServicesLogic
{
    public sealed class RandomService : IRandomService
    {
        //Random with weights calculation algorithm.
        public T GetWeightedRandomValue<T>(Dictionary<int, T> weightsTable)
        {
            int[] weights = weightsTable.Keys.ToArray();

            int randomWeight = GetRange(0, weights.Sum());

            for (int i = 0; i < weights.Length; ++i)
            {
                randomWeight -= weights[i];
                if (randomWeight < 0)
                {
                    return weightsTable[i];
                }
            }

            return weightsTable.FirstOrDefault().Value;
        }

        public int GetRange(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public float GetRange(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}