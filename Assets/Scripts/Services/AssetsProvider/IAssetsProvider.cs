using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
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
        UniTask<IList<T>> LoadMultiple<T>(string key, Action<T> callback = null) where T : class;

        void Initialize();
        void ClearCache();
    }
}
