using System;
using UnityEngine;

namespace RiverAttack
{
    public class PlayerAnimator : MonoBehaviour
    {
        readonly int m_DirX = Animator.StringToHash("DirX");
        readonly int m_DirY = Animator.StringToHash("DirY");
        readonly int m_GotHit = Animator.StringToHash("GotHit");

        Animator m_Animator;
        Animator m_AnimatorSkin;
        PlayerMaster m_PlayerMaster;

        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerMasterControllerMovement += AnimationMovement;
            m_PlayerMaster.EventPlayerMasterRespawn += AnimationReset;
            m_PlayerMaster.EventPlayerMasterBossHit += AnimationHit;
            m_PlayerMaster.EventPlayerMasterUpdateSkin += UpdateAnimator;
        }
        
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterControllerMovement -= AnimationMovement;
            m_PlayerMaster.EventPlayerMasterRespawn -= AnimationReset;
            m_PlayerMaster.EventPlayerMasterBossHit -= AnimationHit;
            m_PlayerMaster.EventPlayerMasterUpdateSkin -= UpdateAnimator;
        }
  #endregion
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_Animator = GetComponent<Animator>();
        }

        void UpdateAnimator()
        {
            if (m_AnimatorSkin)
                return;
            var skinPart = GetComponentInChildren<PlayerSkinAttach>().gameObject;
            m_AnimatorSkin = skinPart.GetComponent<Animator>();
            AnimationReset();
        }
        void AnimationMovement(Vector2 dir)
        {
            m_Animator.SetFloat(m_DirX, dir.x);
            m_Animator.SetFloat(m_DirY, dir.y);
        }

        void AnimationReset()
        {
            m_Animator.SetFloat(m_DirX, 0);
            m_Animator.SetFloat(m_DirY, 0);
        }
        void AnimationHit(bool active)
        {
            m_AnimatorSkin.SetBool(m_GotHit,active);
        }
    }
}
