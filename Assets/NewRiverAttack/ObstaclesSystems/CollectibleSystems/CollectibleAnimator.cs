using NewRiverAttack.GamePlayManagers;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.CollectibleSystems
{
    public class CollectibleAnimator: MonoBehaviour
    {
        [SerializeField] private string collectTrigger;
        [SerializeField] private string resetTrigger;
        
        private Animator _animator;
        private CollectibleMaster _collectibleMaster;
        private GamePlayManager _gamePlayManager;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _collectibleMaster.EventMasterCollectCollect += MasterCollectAnimation;
            _collectibleMaster.EventObstacleChangeSkin += SetAnimations;
            _gamePlayManager.EventGameRestart += ResetCollectableAnimation;
        }

        private void OnDisable()
        {
            _collectibleMaster.EventMasterCollectCollect -= MasterCollectAnimation;
            _collectibleMaster.EventObstacleChangeSkin -= SetAnimations;
            _gamePlayManager.EventGameRestart -= ResetCollectableAnimation;
        }

        #endregion
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _collectibleMaster = GetComponent<CollectibleMaster>();
            _animator = GetComponentInChildren<Animator>();
        }
        private void SetAnimations()
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }
        private void MasterCollectAnimation()
        {
            SetAnimations();
            if (_animator == null || string.IsNullOrEmpty(collectTrigger)) return;
            _animator.SetTrigger(collectTrigger);
        }

        private void ResetCollectableAnimation()
        {
            if (_animator == null || string.IsNullOrEmpty(collectTrigger)) return;
            _animator.SetTrigger(resetTrigger);
        }

        public bool HasAnimation => !string.IsNullOrEmpty(collectTrigger);

    }
}