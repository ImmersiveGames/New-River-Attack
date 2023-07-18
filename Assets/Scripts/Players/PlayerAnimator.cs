using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
    #region Variables Private References
        private PlayerMaster m_PlayerMaster;
        private Animator m_Animator;
        static readonly int DirX = Animator.StringToHash("DirX");
        static readonly int DirY = Animator.StringToHash("DirY");
    #endregion
        /// <summary>
        /// Executa quando ativa o objeto
        /// </summary>
        private void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventControllerMovement += AnimationMovement;
        }

        /// <summary>
        /// Configura as referencias iniciais
        /// </summary>
        private void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_Animator = GetComponent<Animator>();
        }
        private void AnimationMovement(Vector2 dir)
        {
            m_Animator.SetFloat(DirX, dir.x);
            m_Animator.SetFloat(DirY, dir.y);
        }

        private void OnDisable()
        {
            m_PlayerMaster.EventControllerMovement -= AnimationMovement;
        }
    }
}
