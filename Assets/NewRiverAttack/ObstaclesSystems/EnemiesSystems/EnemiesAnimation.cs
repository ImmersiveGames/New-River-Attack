using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.EnemiesSystems
{
    public class EnemiesAnimation : MonoBehaviour
    {
        public string onMove = "OnMove";
        public string onFlip = "Turn";

        

        private Animator _animator;
        private EnemiesMaster _enemiesMaster;
        private GamePlayManager _gamePlayManager;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _enemiesMaster.EventObstacleChangeSkin += SetAnimations;
            _gamePlayManager.EventGameRestart += ResetAnimation;
        }

        private void OnDisable()
        {
            _enemiesMaster.EventObstacleChangeSkin -= SetAnimations;
            _gamePlayManager.EventGameRestart -= ResetAnimation;
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _enemiesMaster = GetComponent<EnemiesMaster>();
            _gamePlayManager = GamePlayManager.Instance;
        }

        private void SetAnimations()
        {
            _animator = GetComponentInChildren<Animator>(true);
        }
        private void ResetAnimation()
        {
            _animator = GetComponentInChildren<Animator>(true);
            if (_animator == null) return;
            if(string.IsNullOrEmpty(onMove))
                _animator.SetBool(onMove, false);
            if(!string.IsNullOrEmpty(onFlip))
                _animator.SetBool(onFlip, false);
        }
        
        public void AnimationMove(bool active)
        {
            if (_animator == null || string.IsNullOrEmpty(onMove)) return;
            _animator.SetBool(onMove, active);
        }
        public void AnimationFlip()
        {
            if (_animator == null || string.IsNullOrEmpty(onFlip)) return;
            _animator.SetBool(onFlip, !_animator.GetBool(onFlip));
        }
    }
}