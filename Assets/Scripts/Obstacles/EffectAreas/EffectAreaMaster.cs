using UnityEngine;

namespace RiverAttack
{
    public class EffectAreaMaster : ObstacleMaster
    {
        private float m_Timer;
        private float m_TimeToAccess;
        private EffectAreaScriptable m_EffectArea;

        #region Events
        public event GeneralEventHandler EventEnterAreaEffect;
        public event GeneralEventHandler EventExitAreaEffect;
  #endregion

        #region UNITYMETHODS

        private void OnTriggerExit(Collider collision)
        {
            playerMaster = collision.GetComponentInParent<PlayerMaster>();
            if(playerMaster == null || !playerMaster.shouldPlayerBeReady) return;
            OnEventExitAreaEffect();
        }

        private void OnTriggerStay(Collider collision)
        {
            playerMaster = collision.GetComponentInParent<PlayerMaster>();
            if (playerMaster == null || !playerMaster.shouldPlayerBeReady) 
                return;
            CollectThis(playerMaster);
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EffectArea = enemy as EffectAreaScriptable;
            if (m_EffectArea != null)
                m_TimeToAccess = m_EffectArea.timeToAccess;
        }

        private void CollectThis(PlayerMaster collision)
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

        protected override void DestroyObstacle()
        {
            OnEventExitAreaEffect();
            playerMaster.inEffectArea = false;
            base.DestroyObstacle();
        }

        #region Calls

        private void OnEventAreaEffect()
        {
            playerMaster.inEffectArea = true;
            EventEnterAreaEffect?.Invoke();
        }

        private void OnEventExitAreaEffect()
        {
            playerMaster.inEffectArea = false;
            EventExitAreaEffect?.Invoke();
            GameSteamManager.StoreStats();
        }
  #endregion

    }
}
