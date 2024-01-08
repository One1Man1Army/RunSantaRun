using DG.Tweening;
using RSR.InternalLogic;
using UnityEngine;

namespace RSR.World
{
    public abstract class PoolableItem : MonoBehaviour
    {
        public abstract ItemType ItemType { get; }
        public bool IsConstructed { get; protected set; }

        private ObjectsPool<PoolableItem> _pool;

        private Vector3 _defaultScale;

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }
        
        protected virtual void OnEnable()
        {
            transform.localScale = _defaultScale;
        }

        public void SetPool(ObjectsPool<PoolableItem> pool)
        {
            _pool = pool;
        }

        public void Release()
        {
            _pool.Release(this);
        }
    }
}