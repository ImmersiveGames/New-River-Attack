using System;
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
            if(playerMaster == null)
                playerMaster = collision.GetComponentInParent<PlayerMaster>();
            if (!playerMaster) return;
            playerMaster.inEffectArea = false;
            if (!playerMaster.shouldPlayerBeReady) return;
            OnEventExitAreaEffect();
        }
        void OnTriggerStay(Collider collision)
        {
            if(playerMaster == null)
                playerMaster = collision.GetComponentInParent<PlayerMaster>();
            if (!playerMaster || !playerMaster.shouldPlayerBeReady) return;
            playerMaster.inEffectArea = true;
            //if (!playerMaster.inEffectArea) playerMaster.inEffectArea = true;
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
            if (m_Timer <= 0 && playerMaster.inEffectArea)
            {
                m_EffectArea.EffectAreaStart(player);
                OnEventAreaEffect();
                m_Timer = m_TimeToAccess;
            }
            m_Timer -= Time.deltaTime;
        }

        protected override void DestroyObstacle()
        {
            if (playerMaster.inEffectArea)
            {
                playerMaster.inEffectArea = false;
                //OnEventExitAreaEffect();
            }
            OnEventExitAreaEffect();
            base.DestroyObstacle();
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
