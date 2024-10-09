using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems.Mines
{
    public class MineAnimation : MonoBehaviour
    {
        public string onAlert = "warning";
        public string onAlertStop = "warning_stop";
        
        private Animator _animator;
        private MineMaster _mineMaster;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _mineMaster.EventObstacleChangeSkin += SetAnimations;
            _mineMaster.EventAlertApproach += AnimationAlert;
            _mineMaster.EventAlertStop += AnimationAlertStop;
        }

        private void OnDisable()
        {
            _mineMaster.EventObstacleChangeSkin -= SetAnimations;
            _mineMaster.EventAlertApproach -= AnimationAlert;
            _mineMaster.EventAlertStop -= AnimationAlertStop;
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
        private void AnimationAlertStop()
        {
            if (_animator == null || string.IsNullOrEmpty(onAlertStop)) return;
            _animator.SetTrigger(onAlertStop);
        }
    }
}