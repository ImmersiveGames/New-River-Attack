using UnityEngine;
namespace RiverAttack
{
    public class BossAnimator: MonoBehaviour
    {
        public string onEmerge;
        public string onSubmerge;
        public string onGotHit;
        public string onDeath;

        public GameObject smokeVFX;

        private BossMaster m_BossMaster;
        private Animator m_Animator;

        private void OnEnable()
        {
            SetInitialReferences();
            m_BossMaster.EventBossEmerge += AnimateEmerge;
            m_BossMaster.EventBossSubmerge += AnimateSubmerge;
            m_BossMaster.EventBossHit += AnimateGotHit;
            m_BossMaster.EventSmokeSpawn += SmokeBoss;
            m_BossMaster.EventBossDeath += AnimateDeath;
        }

        private void OnDisable()
        {
            m_BossMaster.EventBossEmerge -= AnimateEmerge;
            m_BossMaster.EventBossSubmerge -= AnimateSubmerge;
            m_BossMaster.EventBossHit -= AnimateGotHit;
            m_BossMaster.EventSmokeSpawn -= SmokeBoss;
            m_BossMaster.EventBossDeath -= AnimateDeath;
        }

        private void SetInitialReferences()
        {
            m_BossMaster = GetComponent<BossMaster>();
            m_Animator = GetComponentInChildren<Animator>();
        }

        private void AnimateEmerge()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onEmerge)) 
                return;
            m_Animator.SetTrigger(onEmerge);
        }

        private void AnimateSubmerge()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onSubmerge)) 
                return;
            m_Animator.SetTrigger(onSubmerge);
        }

        private void AnimateGotHit()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onGotHit)) 
                return;
            m_Animator.SetTrigger(onGotHit);
        }

        private void AnimateDeath()
        {
            if (m_Animator == null)
                m_Animator = GetComponentInChildren<Animator>();
            if (string.IsNullOrEmpty(onDeath)) 
                return;
            m_Animator.SetBool(onDeath, true);
        }

        private void SmokeBoss(Transform boss)
        {
            if (smokeVFX != null)
            {
                var smoke = Instantiate(smokeVFX, boss);
            }
        }
    }
}
