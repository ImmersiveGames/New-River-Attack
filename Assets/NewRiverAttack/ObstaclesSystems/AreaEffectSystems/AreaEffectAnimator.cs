using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.AreaEffectSystems
{
    public class AreaEffectAnimator : MonoBehaviour
    {
        public string emergeTrigger;
        public string fuelingTrigger;
        
        private Animator _animator;
        private AreaEffectMaster _areaEffectMaster;
        private GamePlayManager _gamePlayManager;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _areaEffectMaster.EventMasterAreaEffectEnter += OnFilledUp;
            _areaEffectMaster.EventMasterAreaEffectExit += OffFilledUp;
            _areaEffectMaster.EventObstacleDeath += OffFilledUp;
            _areaEffectMaster.EventObstacleChangeSkin += SetAnimations;
            _gamePlayManager.EventGameRestart -= OffFilledUp;
        }

        private void OnDisable()
        {
            _areaEffectMaster.EventMasterAreaEffectEnter -= OnFilledUp;
            _areaEffectMaster.EventMasterAreaEffectExit -= OffFilledUp;
            _areaEffectMaster.EventObstacleDeath -= OffFilledUp;
            _areaEffectMaster.EventObstacleChangeSkin -= SetAnimations;
            _gamePlayManager.EventGameRestart -= OffFilledUp;
        }

        #endregion
        
        
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _areaEffectMaster = GetComponent<AreaEffectMaster>();
            _animator = GetComponentInChildren<Animator>();
        }
        private void SetAnimations()
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }
        
        private void OnFilledUp()
        {
            SetAnimations();
            if (_animator == null || string.IsNullOrEmpty(fuelingTrigger)) return;
            _animator.SetBool(fuelingTrigger, true);
        }
        
        private void OffFilledUp(PlayerMaster playerMaster)
        {
            OffFilledUp();
        }
        private void OffFilledUp()
        {
            SetAnimations();
            if (_animator == null || string.IsNullOrEmpty(fuelingTrigger)) return;
            _animator.SetBool(fuelingTrigger, false);
        }
    }
}