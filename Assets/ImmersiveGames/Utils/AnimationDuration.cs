using System.Linq;
using UnityEngine;

namespace ImmersiveGames.Utils
{
    public static class AnimationDuration
    {
        public static float GetAnimationDuration(Animator animator, string animationName)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
            {
                Debug.LogWarning("Animator or RuntimeAnimatorController is null");
                return 0f;
            }

            // Get the animation clip
            var clip = animator.runtimeAnimatorController.animationClips
                .FirstOrDefault(c => c.name.Equals(animationName));

            if (clip == null)
            {
                Debug.LogWarning($"Animation clip '{animationName}' not found");
                return 0f;
            }

            return clip.length;
        }
    }
}