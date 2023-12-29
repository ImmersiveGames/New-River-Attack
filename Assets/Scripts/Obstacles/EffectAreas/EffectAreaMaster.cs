using System;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class EffectAreaMaster : ObstacleMaster
    {
        float m_Timer;
        float m_TimeToAccess;
        EffectAreaScriptable m_EffectArea;

        #region Events
        public event GeneralEventHandler EventEnterAreaEffect;
        public event GeneralEventHandler EventExitAreaEffect;
  #endregion

        #region UNITYMETHODS

        void OnTriggerExit(Collider collision)
        {
            playerMaster = collision.GetComponentInParent<PlayerMaster>();
            if(playerMaster == null || !playerMaster.shouldPlayerBeReady) return;
            OnEventExitAreaEffect();
        }
        void OnTriggerStay(Collider collision)
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

        protected override void DestroyObstacle()
        {
            OnEventExitAreaEffect();
            playerMaster.inPowerUp = false;
            base.DestroyObstacle();
        }

        #region Calls
        void OnEventAreaEffect()
        {
            playerMaster.inPowerUp = true;
            EventEnterAreaEffect?.Invoke();
        }

        void OnEventExitAreaEffect()
        {
            playerMaster.inPowerUp = false;
            EventExitAreaEffect?.Invoke();
        }
  #endregion

    }
}
