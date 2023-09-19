using UnityEngine;
namespace RiverAttack
{
    public class EffectAreaMaster : ObstacleMaster
    {
        float m_Timer;
        float m_TimeToAccess;
        EffectAreaScriptable m_EffectArea;
        PlayerMaster m_PlayerMaster;

        #region Events
        public event GeneralEventHandler EventEnterAreaEffect;
        public event GeneralEventHandler EventExitAreaEffect;
  #endregion

        #region UNITYMETHODS
        void OnTriggerExit(Collider collision)
        {
            if(m_PlayerMaster == null)
                m_PlayerMaster = collision.GetComponentInParent<PlayerMaster>();
            if (!m_PlayerMaster) return;
            if (!m_PlayerMaster.shouldPlayerBeReady) return;
            m_PlayerMaster.inEffectArea = false;
            OnEventExitAreaEffect();
        }
        void OnTriggerStay(Collider collision)
        {
            if(m_PlayerMaster == null)
                m_PlayerMaster = collision.GetComponentInParent<PlayerMaster>();
            if (!m_PlayerMaster) return;
            if (!m_PlayerMaster.shouldPlayerBeReady) return;
            if (!m_PlayerMaster.inEffectArea) m_PlayerMaster.inEffectArea = true;
            CollectThis(m_PlayerMaster);
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EffectArea = enemy as EffectAreaScriptable;
            if (m_EffectArea != null)
                m_TimeToAccess = m_EffectArea.timeToAccess;
        }
        void CollectThis(PlayerMaster collision)
        {
            var player = collision.getPlayerSettings;
            if (m_Timer <= 0)
            {
                m_EffectArea.EffectAreaStart(player);
                OnEventAreaEffect();
                m_Timer = m_TimeToAccess;
            }
            m_Timer -= Time.deltaTime;
        }

        #region Calls
        void OnEventAreaEffect()
        {
            EventEnterAreaEffect?.Invoke();
        }

        void OnEventExitAreaEffect()
        {
            EventExitAreaEffect?.Invoke();
        }
  #endregion

    }
}
