using NewRiverAttack.SteamGameManagers;
using UnityEngine;

namespace RiverAttack
{
    public class EffectAreaMasterOld : ObstacleMasterOld
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
            PlayerMasterOld = collision.GetComponentInParent<PlayerMasterOld>();
            if(PlayerMasterOld == null || !PlayerMasterOld.ShouldPlayerBeReady) return;
            OnEventExitAreaEffect();
        }

        private void OnTriggerStay(Collider collision)
        {
            PlayerMasterOld = collision.GetComponentInParent<PlayerMasterOld>();
            if (PlayerMasterOld == null || !PlayerMasterOld.ShouldPlayerBeReady) 
                return;
            CollectThis(PlayerMasterOld);
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_EffectArea = enemy as EffectAreaScriptable;
            if (m_EffectArea != null)
                m_TimeToAccess = m_EffectArea.timeToAccess;
        }

        private void CollectThis(PlayerMasterOld collision)
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
            PlayerMasterOld.inEffectArea = false;
            base.DestroyObstacle();
        }

        #region Calls

        private void OnEventAreaEffect()
        {
            PlayerMasterOld.inEffectArea = true;
            EventEnterAreaEffect?.Invoke();
        }

        private void OnEventExitAreaEffect()
        {
            PlayerMasterOld.inEffectArea = false;
            EventExitAreaEffect?.Invoke();
            //SteamGameManager.StoreStats();
        }
  #endregion

    }
}
