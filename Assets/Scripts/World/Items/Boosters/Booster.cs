using UnityEngine;

namespace RSR.World
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Booster : PoolableItem
    {
        public override ItemType ItemType => ItemType.Booster;
        public abstract BoosterType Type { get; }
    }
}
