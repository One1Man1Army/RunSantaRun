using UnityEngine;

namespace RSR.Player
{
    public static class AnimatorHashKeys 
    {
        public static readonly int MoveDirXHash = Animator.StringToHash("MoveDirX");
        public static readonly int MoveDirYHash = Animator.StringToHash("MoveDirY");
        public static readonly int DieHash = Animator.StringToHash("Die");
        public static readonly int IdleHash = Animator.StringToHash("Idle");
    }
}