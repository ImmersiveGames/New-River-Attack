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

        private BossMasterOld _mBossMasterOld;
        private Animator m_Animator;

        private void OnEnable()
        {
            SetInitialReferences();
            _mBossMasterOld.EventBossEmerge += AnimateEmerge;
            _mBossMasterOld.EventBossSubmerge += AnimateSubmerge;
            _mBossMasterOld.EventBossHit += AnimateGotHit;
            _mBossMasterOld.EventSmokeSpawn += SmokeBoss;
            _mBossMasterOld.EventBossDeath += AnimateDeath;
        }

        private void OnDisable()
        {
            _mBossMasterOld.EventBossEmerge -= AnimateEmerge;
            _mBossMasterOld.EventBossSubmerge -= AnimateSubmerge;
            _mBossMasterOld.EventBossHit -= AnimateGotHit;
            _mBossMasterOld.EventSmokeSpawn -= SmokeBoss;
            _mBossMasterOld.EventBossDeath -= AnimateDeath;
        }

        private void SetInitialReferences()
        {
            _mBossMasterOld = GetComponent<BossMasterOld>();
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
