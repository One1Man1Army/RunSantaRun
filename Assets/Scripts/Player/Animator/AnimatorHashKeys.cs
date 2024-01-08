using UnityEngine;

namespace RSR.Player
{
    public static class AnimatorHashKeys 
    {
        public static readonly int IdleHash = Animator.StringToHash("Idle");
        public static readonly int RunHash = Animator.StringToHash("Run");
        public static readonly int JumpHash = Animator.StringToHash("Jump");
        public static readonly int DieHash = Animator.StringToHash("Die");
    }
}