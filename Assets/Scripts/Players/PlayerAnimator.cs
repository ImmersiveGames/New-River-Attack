﻿using UnityEngine;

namespace RiverAttack
{
    public class PlayerAnimator : MonoBehaviour
    {
        readonly int m_DirX = Animator.StringToHash("DirX");
        readonly int m_DirY = Animator.StringToHash("DirY");
        
        Animator m_Animator;
        PlayerMaster m_PlayerMaster;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerMasterControllerMovement += AnimationMovement;
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterControllerMovement -= AnimationMovement;
        }
  #endregion
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_Animator = GetComponent<Animator>();
        }
        void AnimationMovement(Vector2 dir)
        {
            m_Animator.SetFloat(m_DirX, dir.x);
            m_Animator.SetFloat(m_DirY, dir.y);
        }
    }
}
