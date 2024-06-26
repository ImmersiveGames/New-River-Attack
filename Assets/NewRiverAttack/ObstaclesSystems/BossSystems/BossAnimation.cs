using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.BossSystems
{
    public class BossAnimation : MonoBehaviour
    {
        public string onEmerge = "Emerge";
        public string onSubmerge = "Submerge";
        public string onGotHit = "GotHit";
        public string onDeath = "Death";

        private BossVfxTag _smokeVFX;
        private BossVfxTag _splashVFX;

        private bool _onDeath;
        private Animator _animator;
        private BossMaster _bossMaster;
        private GamePlayManager _gamePlayManager;
        private GamePlayBossManager _gamePlayBossManager;

        #region Unity Methods

        private void OnEnable()
        {
            SetInitialReferences();
            _bossMaster.EventObstacleChangeSkin += SetAnimations;
            _bossMaster.EventObstacleHit += AnimateGotHit;
            _gamePlayBossManager.EventEnterBoss += AnimateEmerge;
            _gamePlayManager.EventGameRestart += ResetAnimation;
            _bossMaster.EventObstacleDeath += AnimateDeath;
        }

        private void OnDisable()
        {
            _bossMaster.EventObstacleChangeSkin -= SetAnimations;
            _bossMaster.EventObstacleHit -= AnimateGotHit;
            _gamePlayBossManager.EventEnterBoss -= AnimateEmerge;
            _gamePlayManager.EventGameRestart -= ResetAnimation;
            _bossMaster.EventObstacleDeath -= AnimateDeath;
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            _gamePlayBossManager = GamePlayBossManager.instance;
            _bossMaster = GetComponent<BossMaster>();
        }

        private void SetAnimations()
        {
            _animator = GetComponentInChildren<Animator>(true);
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

        private void AnimateGotHit(PlayerMaster playerMaster)
        {
            Debug.Log("Acertou");
            if (_animator == null || string.IsNullOrEmpty(onGotHit)) return;
            Debug.Log("Trigger");
            _animator.SetTrigger(onGotHit);
        }

        private void AnimateDeath(PlayerMaster playerMaster)
        {
            if (_animator == null || string.IsNullOrEmpty(onDeath)) return;
            _animator.SetBool(onDeath, true);
        }

        private void SmokeBoss(Transform boss)
        {
            if (_smokeVFX != null)
            {
                var smoke = Instantiate(_smokeVFX, boss);
            }
        }

        private void ActiveSplash()
        {
            SetVfxTypes(GetComponentsInChildren<BossVfxTag>(true));
            _splashVFX.gameObject.SetActive(true);
            var particleSystems = _splashVFX.GetComponentsInChildren<ParticleSystem>();
            foreach (var particle in particleSystems)
            {
                particle.Play();
            }
        }

        private void SetVfxTypes(BossVfxTag[] vfxTags)
        {
            foreach (var vfx in vfxTags)
            {
                //Debug.Log(vfx.idName);
                switch (vfx.idName)
                {
                    case "Smoke":
                        _smokeVFX = vfx;
                        break;
                    case "Splash":
                        _splashVFX = vfx;
                        break;
                }
            }
        }
    }
}