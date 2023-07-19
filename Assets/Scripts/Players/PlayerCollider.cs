﻿using UnityEngine;
namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PlayerCollider : MonoBehaviour
    {
    #region Variable Private References
        PlayerMaster m_PlayerMaster;
        GamePlayManager m_GamePlayManager;
        Collider m_Collider;
    #endregion
        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerDestroy += ColliderOff; // desliga o collider quando destroy o player
            m_PlayerMaster.EventPlayerReload += ColliderOn;   // liga o collider quando reinicia o player
            m_GamePlayManager.EventCompletePath += ColliderOff;
        }
        void OnTriggerEnter(Collider collision)
        {
            if (!collision.GetComponentInParent<WallsMaster>() && !collision.GetComponentInParent<EnemiesMaster>()) return;
            if (m_GamePlayManager.getGodMode) return;
            //m_GamePlayManager.CallEventPausePlayGame();
            //m_PlayerMaster.CallEventPlayerHit();
            m_PlayerMaster.CallEventPlayerDestroy();
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerDestroy -= ColliderOff;
            m_PlayerMaster.EventPlayerReload -= ColliderOn;
            m_GamePlayManager.EventCompletePath -= ColliderOff;
        }
  #endregion

        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_Collider = GetComponent<Collider>();
        }

        void ColliderOn()
        {
            m_Collider.enabled = true;
        }
        void ColliderOff(Levels level)
        {
            ColliderOff();
        }
        void ColliderOff()
        {
            m_Collider.enabled = false;
        }
    }
}
