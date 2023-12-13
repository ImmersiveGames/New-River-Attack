using UnityEngine;
namespace RiverAttack
{
    public class BossAnimator: MonoBehaviour
    {
        public string onEmerge;
        public string onSubmerge;
        public string onGotHit;
        
        BossMaster m_BossMaster;
        Animator m_Animator;

        void OnEnable()
        {
            SetInitialReferences();
            m_BossMaster.EventBossEmerge += AnimateEmerge;
            m_BossMaster.EventBossSubmerge += AnimateSubmerge;
            m_BossMaster.EventBossHit += AnimateGotHit;
        }

        void OnDisable()
        {
            m_BossMaster.EventBossEmerge -= AnimateEmerge;
            m_BossMaster.EventBossSubmerge -= AnimateSubmerge;
            m_BossMaster.EventBossHit -= AnimateGotHit;
        }

        void SetInitialReferences()
        {
            m_BossMaster = GetComponent<BossMaster>();
            m_Animator = GetComponentInChildren<Animator>();
        }

        void AnimateEmerge()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onEmerge)) 
                return;
            m_Animator.SetTrigger(onEmerge);
        }
        void AnimateSubmerge()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onSubmerge)) 
                return;
            m_Animator.SetTrigger(onSubmerge);
        }
        void AnimateGotHit()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onGotHit)) 
                return;
            m_Animator.SetTrigger(onGotHit);
        }
    }
}
