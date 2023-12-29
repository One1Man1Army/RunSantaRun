using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RSR.ServicesLogic
{
    public interface IAssetsProvider : IService
    {
        UniTask<GameObject> Instantiate(string key, Vector3 pos);
        UniTask<GameObject> Instantiate(string key);
        UniTask<T> Load<T>(AssetReference prefabReference) where T : class;
        UniTask<T> Load<T>(string address) where T : class;
        void Initialize();
        void ClearCache();
    }
}
