using Cysharp.Threading.Tasks;
using RSR.ServicesLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSR.World
{
    /// <summary>
    /// Items abstract factory.
    /// Holds prefabs' loading logic.
    /// </summary>
    public abstract class ItemsFactory
    {
        protected IAssetsProvider _assetsProvider;
        protected IList<GameObject> _prefabs;

        protected async UniTask LoadPrefabs(string prefabsLabel)
        {
            try
            {
                _prefabs = await _assetsProvider.LoadMultiple<GameObject>(prefabsLabel);
            }
            catch (Exception e)
            {
                Debug.Log($"{prefabsLabel} prefabs loading failed! {e.Message}");
            }
        }

        protected abstract void InitStorage();
    }
}