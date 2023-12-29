using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RSR.ServicesLogic
{
    /// <summary>
    /// Provides us access to addressables assets loading and instantiation functionality.
    /// Prevents addressables from unnecessary memory usage by caching addressables' handles.
    /// </summary>
    public sealed class AssetsProvider : MonoBehaviour, IAssetsProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedHandles = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }

        #region Instantiation Methods
        public UniTask<GameObject> Instantiate(string key, Vector3 pos)
        {
            return Addressables.InstantiateAsync(key, pos, Quaternion.identity).ToUniTask();
        }

        public UniTask<GameObject> Instantiate(string key)
        {
            return Addressables.InstantiateAsync(key).ToUniTask();
        }
        #endregion

        #region Loading Methods
        public async UniTask<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completedHandles.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
            {
                return completedHandle.Result as T;
            }
            else
            {
                var handle = await RunAndCacheOnComplete(Addressables.LoadAssetAsync<T>(assetReference), assetReference.AssetGUID);
                return handle;
            }
        }

        public async UniTask<T> Load<T>(string key) where T : class
        {
            if (_completedHandles.TryGetValue(key, out AsyncOperationHandle completedHandle))
            {
                return completedHandle.Result as T;
            }
            else
            {
                var handle = await RunAndCacheOnComplete(Addressables.LoadAssetAsync<T>(key), key);
                return handle;
            }
        }
        #endregion

        #region Inner Methods
        private async Task<T> RunAndCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completeHandle =>
            {
                _completedHandles[cacheKey] = completeHandle;
            };

            AddHandle<T>(cacheKey, handle);

            return await handle.Task;
        }

        private void AddHandle<T>(string cacheKey, AsyncOperationHandle handle) where T : class
        {
            if (!_handles.TryGetValue(cacheKey, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[cacheKey] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }
        #endregion

        public void ClearCache()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
            {
                foreach (AsyncOperationHandle handle in resourceHandles)
                {
                    Addressables.Release(handle);
                }
            }

            _completedHandles.Clear();
            _handles.Clear();
        }
    }
}
