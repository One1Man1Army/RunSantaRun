using RSR.InternalLogic;
using UnityEngine;
using UnityEngine.Pool;

namespace RSR.World
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Booster : MonoBehaviour
    {
        private ObjectsPool<Booster> _pool;
        public abstract BoosterType Type { get; }

        public void SetPool(ObjectsPool<Booster> pool)
        {
            _pool = pool;
        }

        protected void Release()
        {
            _pool.Release(this);
        }
    }
}
