using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {/*
    #region Variables Private References
        PlayerMaster m_PlayerMaster;
        Animator m_Animator;
        static readonly int DirX = Animator.StringToHash("DirX");
        static readonly int DirY = Animator.StringToHash("DirY");
    #endregion

        #region UNITY METHODS
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
            m_Animator.SetFloat(DirX, dir.x);
            m_Animator.SetFloat(DirY, dir.y);
        }*/
    }
}
