using UnityEngine;
namespace RiverAttack
{
    public class MinesAnimator : MonoBehaviour
    {
        public string onAlert;

        MineMaster m_MineMaster;
        Animator m_Animator;

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_MineMaster.EventMineAlert += AnimationAlert;

        }
        void OnDisable()
        {
            m_MineMaster.EventMineAlert -= AnimationAlert;
        }
  #endregion

        void SetInitialReferences()
        {
            m_MineMaster = GetComponent<MineMaster>();
            m_Animator = GetComponentInChildren<Animator>();
        }

        void AnimationAlert()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onAlert)) 
                return;
            if (m_Animator == null) return;
            m_Animator.SetTrigger(onAlert);
        }
        
    }
}
