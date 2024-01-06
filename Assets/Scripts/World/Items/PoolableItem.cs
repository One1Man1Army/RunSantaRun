using DG.Tweening;
using RSR.InternalLogic;
using UnityEngine;

namespace RSR.World
{
    public abstract class PoolableItem : MonoBehaviour
    {
        private ObjectsPool<PoolableItem> _pool;
        public abstract ItemType ItemType { get; }
        private void OnEnable()
        {
            DOVirtual.DelayedCall(15f, ReleaseSelf);
        }

        public void SetPool(ObjectsPool<PoolableItem> pool)
        {
            _pool ??= pool;
        }

        protected void Release()
        {
            _pool.Release(this);
        }

        private void ReleaseSelf()
        {
            if (gameObject.activeInHierarchy)
            {
                if (_pool != null)
                    Release();
            }
        }
    }
}