using UnityEngine;
namespace RiverAttack
{
    public class EffectAreaAnimator : EnemiesAnimator
    {
        public string onFueling;
        // Start is called before the first frame update

        #region UNIYMETHODS
        void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponentInParent<PlayerMaster>()) return;
                OnFuelingAnimation(true);
        }
        void OnTriggerExit(Collider other)
        {
            if (!other.GetComponentInParent<PlayerMaster>()) return;
            OnFuelingAnimation(false);
        }
        #endregion

        void OnFuelingAnimation(bool activeBool)
        {
            if (!animator)
            {
                animator = GetComponentInChildren<Animator>();
            }
            if (animator == null || string.IsNullOrEmpty(onFueling) || !animator.gameObject.activeSelf)
                return;
            animator.SetBool(onFueling,activeBool );
        }

        internal override void ResetAnimation()
        {
            base.ResetAnimation();
            if(!string.IsNullOrEmpty(onFueling))
                animator.SetBool(onFueling, false);
        }
    }
}
