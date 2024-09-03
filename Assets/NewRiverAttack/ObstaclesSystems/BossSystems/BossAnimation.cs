﻿using System.Linq;
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
            _bossMaster.EventBossEmerge += AnimateEmerge;
            _bossMaster.EventBossSubmerge += AnimateSubmerge;
            _gamePlayBossManager.EventEnterBoss += AnimateEmerge;
            _gamePlayManager.EventGameRestart += ResetAnimation;
            _bossMaster.EventObstacleDeath += AnimateDeath;
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

        /*private void ActiveSplash()
        {
            SetVfxTypes(GetComponentsInChildren<BossVfxTag>(true));
            _splashVFX.gameObject.SetActive(true);
            var particleSystems = _splashVFX.GetComponentsInChildren<ParticleSystem>();
            foreach (var particle in particleSystems)
            {
                particle.Play();
            }
        }*/

        public float GetSubmergeTime()
        {
            return Tools.GetAnimationDuration(_animator, onSubmerge);
        }
    }
}