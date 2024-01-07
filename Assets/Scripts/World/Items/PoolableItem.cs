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

        private Tween _selfDestroy;
        private Vector3 _defaultScale;

        private void Awake()
        {
            _defaultScale = transform.localScale;
        }
        
        protected virtual void OnEnable()
        {
            _selfDestroy = DOVirtual.DelayedCall(20f, ReleaseSelf);
            transform.localScale = _defaultScale;
        }

        public void SetPool(ObjectsPool<PoolableItem> pool)
        {
            _pool = pool;
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
                {
                    Release();
                }
            }
        }

        private void OnDisable()
        {
            _selfDestroy?.Kill();
        }
    }
}