﻿using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;

namespace NewRiverAttack.PlayerManagers.PlayerSystems
{
    public class PlayerAnimator: MonoBehaviour
    {
        private readonly int _dirX = Animator.StringToHash("DirX");
        private readonly int _dirY = Animator.StringToHash("DirY");
        private readonly int _gotHit = Animator.StringToHash("GotHit");
        
        private Animator _animator;
        private Animator _animatorSkin;
        private PlayerMaster _playerMaster;

        #region Unity Methods
        private void OnEnable()
        {
            SetInitialReferences();
            _playerMaster.EventPlayerMasterAxisMovement += AnimationMovement;
            _playerMaster.EventPlayerMasterGetHit += AnimationBossHit;
            _playerMaster.EventPlayerMasterRespawn += AnimationReset;
            
        }

        private void Start()
        {
            UpdateSkin(null);
            _playerMaster.EventPlayerMasterChangeSkin += UpdateSkin;
        }

        private void OnDisable()
        {
            _playerMaster.EventPlayerMasterAxisMovement -= AnimationMovement;
            _playerMaster.EventPlayerMasterGetHit -= AnimationBossHit;
            _playerMaster.EventPlayerMasterRespawn -= AnimationReset;
            _playerMaster.EventPlayerMasterChangeSkin -= UpdateSkin;
        }

        #endregion
        
        private void SetInitialReferences()
        {
            _playerMaster = GetComponent<PlayerMaster>();
            _animator = GetComponent<Animator>();
        }

        private void UpdateSkin(ShopProductSkin shopProductSkin)
        {
            var children = GetComponentInChildren<SkinAttach>();
            _animatorSkin = children.GetComponent<Animator>();
        }
        
        private void AnimationMovement(Vector2 dir)
        {
            if (!_playerMaster.ObjectIsReady) return;
            if (_playerMaster.AutoPilot) dir = Vector2.zero;
            _animator.SetFloat(_dirX, dir.x);
            _animator.SetFloat(_dirY, dir.y);
        }

        private void AnimationBossHit()
        {
            AnimationHit(true);
        }
        private void AnimationHit(bool active)
        {
            if(_animatorSkin)
                _animatorSkin.SetBool(_gotHit,active);
        }
        private void AnimationReset()
        {
            _animator.SetFloat(_dirX, 0);
            _animator.SetFloat(_dirX, 0);
            if(_animatorSkin)
                _animatorSkin.SetBool(_gotHit,false);
        }
    }
}