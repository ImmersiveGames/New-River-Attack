using System.Linq;
using ImmersiveGames.Utils;
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
        public string onReset = "ResetIdle";
        private BossVfxTag _splashVFX;

   
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
            _bossMaster.EventBossEmerge += AnimateEmerge;
            _bossMaster.EventBossSubmerge += AnimateSubmerge;
            _gamePlayBossManager.EventEnterBoss += AnimateEmerge;
            _gamePlayManager.EventGameRestart += ResetAnimation;
            _bossMaster.EventObstacleDeath += AnimateDeath;
            _bossMaster.EventBossResetForEnter += BossResetForEnter;
        }

        private void OnDisable()
        {
            _bossMaster.EventObstacleChangeSkin -= SetAnimations;
            _bossMaster.EventObstacleHit -= AnimateGotHit;
            _bossMaster.EventBossEmerge -= AnimateEmerge;
            _bossMaster.EventBossSubmerge -= AnimateSubmerge;
            _gamePlayBossManager.EventEnterBoss -= AnimateEmerge;
            _gamePlayManager.EventGameRestart -= ResetAnimation;
            _bossMaster.EventObstacleDeath -= AnimateDeath;
            _bossMaster.EventBossResetForEnter -= BossResetForEnter;
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.Instance;
            _gamePlayBossManager = GamePlayBossManager.instance;
            _bossMaster = GetComponent<BossMaster>();
        }
        private void BossResetForEnter()
        {
            SetAnimations();
            _animator.SetTrigger(onReset);
        }

        private void SetAnimations()
        {
            _animator = GetComponentInChildren<Animator>(true);
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
            if (_animator == null || string.IsNullOrEmpty(onGotHit)) return;
            _animator.SetTrigger(onGotHit);
        }

        private void AnimateDeath(PlayerMaster playerMaster)
        {
            if (_animator == null || string.IsNullOrEmpty(onDeath)) return;
            _animator.SetBool(onDeath, true);
        }

        public void SmokeBoss()
        {
            var tagVFX = GetComponentsInChildren<BossVfxTag>();
            var smokeVFX = (from tagName in tagVFX where tagName.idName == "Smokes" select tagName.transform).FirstOrDefault();
            if (smokeVFX == null) return;
            foreach (Transform child in smokeVFX)
            {
                if (child.gameObject.activeSelf) continue;
                child.gameObject.SetActive(true);
                break; // Para a iteração após ativar o primeiro filho
            }
        }

        public float GetSubmergeTime()
        {
            return Tools.GetAnimationDuration(_animator, onSubmerge);
        }
    }
}