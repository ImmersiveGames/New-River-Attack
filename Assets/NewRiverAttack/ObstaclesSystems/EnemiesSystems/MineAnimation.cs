using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class MineAnimation : MonoBehaviour
    {
        public string onAlert = "warning";
        
        private Animator _animator;
        private MineMaster _mineMaster;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _mineMaster.EventObstacleChangeSkin += SetAnimations;
            _mineMaster.EventAlertApproach += AnimationAlert;
        }

        private void OnDisable()
        {
            _mineMaster.EventObstacleChangeSkin -= SetAnimations;
            _mineMaster.EventAlertApproach -= AnimationAlert;
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _mineMaster = GetComponent<MineMaster>();
        }

        private void SetAnimations()
        {
            _animator = GetComponentInChildren<Animator>(true);
        }

        private void AnimationAlert()
        {
            if (_animator == null || string.IsNullOrEmpty(onAlert)) return;
            _animator.SetTrigger(onAlert);
        }
    }
}