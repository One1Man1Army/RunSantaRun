using RSR.InternalLogic;
using UnityEngine;

namespace RSR.World
{
    public abstract class PoolableItem : MonoBehaviour
    {
        private ObjectsPool<PoolableItem> _pool;
        public abstract ItemType ItemType { get; }

        public void SetPool(ObjectsPool<PoolableItem> pool)
        {
            _pool = pool;
        }

        protected void Release()
        {
            _pool.Release(this);
        }
    }
}