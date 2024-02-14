using System;
using UnityEngine;

namespace RiverAttack
{
    public class PlayerAnimator : MonoBehaviour
    {
        private readonly int m_DirX = Animator.StringToHash("DirX");
        private readonly int m_DirY = Animator.StringToHash("DirY");
        private readonly int m_GotHit = Animator.StringToHash("GotHit");

        private Animator m_Animator;
        private Animator m_AnimatorSkin;
        private PlayerMaster m_PlayerMaster;

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerMasterControllerMovement += AnimationMovement;
            m_PlayerMaster.EventPlayerMasterRespawn += AnimationReset;
            m_PlayerMaster.EventPlayerMasterBossHit += AnimationHit;
            m_PlayerMaster.EventPlayerMasterUpdateSkin += UpdateAnimator;
        }

        private void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterControllerMovement -= AnimationMovement;
            m_PlayerMaster.EventPlayerMasterRespawn -= AnimationReset;
            m_PlayerMaster.EventPlayerMasterBossHit -= AnimationHit;
            m_PlayerMaster.EventPlayerMasterUpdateSkin -= UpdateAnimator;
        }
  #endregion

  private void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_Animator = GetComponent<Animator>();
        }

        private void UpdateAnimator()
        {
            if (m_AnimatorSkin)
                return;
            var skinPart = GetComponentInChildren<PlayerSkinAttach>().gameObject;
            m_AnimatorSkin = skinPart.GetComponent<Animator>();
            AnimationReset();
        }

        private void AnimationMovement(Vector2 dir)
        {
            m_Animator.SetFloat(m_DirX, dir.x);
            m_Animator.SetFloat(m_DirY, dir.y);
        }

        private void AnimationReset()
        {
            m_Animator.SetFloat(m_DirX, 0);
            m_Animator.SetFloat(m_DirY, 0);
        }

        private void AnimationHit(bool active)
        {
            m_AnimatorSkin.SetBool(m_GotHit,active);
        }
    }
}
