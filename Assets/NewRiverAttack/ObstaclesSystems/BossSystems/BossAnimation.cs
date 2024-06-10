using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.EnemiesSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossAnimation : MonoBehaviour
    {
        public string onEmerge = "Emerge";
        public string onSubmerge = "Submerge";
        public string onGotHit = "GotHit";
        public string onDeath = "Death";

        private GameObject smokeVFX;

        private bool _onDeath;
        private Animator _animator;
        private BossMaster _bossMaster;
        private GamePlayManager _gamePlayManager;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _bossMaster.EventObstacleChangeSkin += SetAnimations;
            _bossMaster.EventBossShowUp += AnimateEmerge;
            _gamePlayManager.EventGameRestart += ResetAnimation;
        }

        private void OnDisable()
        {
            _bossMaster.EventObstacleChangeSkin -= SetAnimations;
            _bossMaster.EventBossShowUp -= AnimateEmerge;
            _gamePlayManager.EventGameRestart -= ResetAnimation;
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            _bossMaster = GetComponent<BossMaster>();
            smokeVFX = GetComponentInChildren<BossVfxTag>()?.gameObject;
        }

        private void SetAnimations()
        {
            _animator = GetComponentInChildren<Animator>(true);
            smokeVFX = GetComponentInChildren<BossVfxTag>(true)?.gameObject;
            if (_animator == null) return;
            if(!string.IsNullOrEmpty(onDeath))
                _onDeath = _animator.GetBool(onDeath);
        }
        private void ResetAnimation()
        {
            _animator = GetComponentInChildren<Animator>(true);
            if (_animator == null) return;
            if(string.IsNullOrEmpty(onDeath))
                _animator.SetBool(onDeath, false);
        }
        
        private void AnimateEmerge()
        {
            if (_animator == null || string.IsNullOrEmpty(onEmerge)) return;
            _animator.SetTrigger(onEmerge);
        }

        private void AnimateSubmerge()
        {
            if (_animator == null || string.IsNullOrEmpty(onSubmerge)) return;
            _animator.SetTrigger(onSubmerge);
        }

        private void AnimateGotHit()
        {
            if (_animator == null || string.IsNullOrEmpty(onGotHit)) return;
            _animator.SetTrigger(onGotHit);
        }

        private void AnimateDeath(bool active)
        {
            if (_animator == null || string.IsNullOrEmpty(onDeath)) return;
            _animator.SetBool(onDeath, active);
        }

        private void SmokeBoss(Transform boss)
        {
            if (smokeVFX != null)
            {
                var smoke = Instantiate(smokeVFX, boss);
            }
        }
    }
}