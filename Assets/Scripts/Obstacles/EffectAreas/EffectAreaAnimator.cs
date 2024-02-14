using UnityEngine;
namespace RiverAttack
{
    public sealed class EffectAreaAnimator : MonoBehaviour
    {
        public string onFueling;
        private Animator m_Animator;

        private GamePlayManager m_GamePlayManager;

        private EffectAreaMaster m_EffectAreaMaster;
        // Start is called before the first frame update

        #region UNIYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventReSpawnEnemiesMaster += ResetAnimation;
            m_GamePlayManager.EventEnemiesMasterForceRespawn += ResetAnimation;
            m_GamePlayManager.EventOtherEnemiesKillPlayer += ResetAnimation;
        }

        private void OnDisable()
        {
            m_GamePlayManager.EventReSpawnEnemiesMaster -= ResetAnimation;
            m_GamePlayManager.EventEnemiesMasterForceRespawn -= ResetAnimation;
            m_GamePlayManager.EventOtherEnemiesKillPlayer -= ResetAnimation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponentInParent<PlayerMaster>()) return;
            OnFuelingAnimation(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponentInParent<PlayerMaster>()) return;
            OnFuelingAnimation(false);
        }
        #endregion

        private void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_EffectAreaMaster = GetComponent<EffectAreaMaster>();
            m_Animator = GetComponentInChildren<Animator>();
        }

        private void OnFuelingAnimation(bool activeBool)
        {
            if (!m_Animator)
            {
                m_Animator = GetComponentInChildren<Animator>();
            }
            //if (m_Animator == null || string.IsNullOrEmpty(onFueling) || !m_Animator.gameObject.activeSelf)
            if (m_Animator == null || string.IsNullOrEmpty(onFueling) || !m_Animator.gameObject.activeSelf)
                return;
            m_Animator.SetBool(onFueling, activeBool);
        }

        private void ResetAnimation()
        {
            if (!string.IsNullOrEmpty(onFueling))
                m_Animator.SetBool(onFueling, false);
        }
    }
}
