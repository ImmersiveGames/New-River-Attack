using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{

    [RequireComponent(typeof(PlayerMaster))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PlayerCollider : MonoBehaviour
    {

    #region Variable Private References
        private PlayerMaster m_PlayerMaster;
        private GamePlayManager m_GamePlayManager;
        private Collider m_Collider;
    #endregion

        /// <summary>
        /// Executa quando ativa o objeto
        /// </summary>
        /// 
        private void OnEnable()
        {
            SetInitialReferences();
            m_PlayerMaster.EventPlayerDestroy += ColliderOff; // desliga o collider quando destroy o player
            m_PlayerMaster.EventPlayerReload += ColliderOn;   // liga o collider quando reinicia o player
            m_GamePlayManager.EventCompletePath += ColliderOff;
        }
        /// <summary>
        /// Executa quando ativa o objeto
        /// </summary>
        private void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_Collider = GetComponent<Collider>();
        }
        /// <summary>
        /// Executa quando o player atinge um objeto com trigger collider
        /// </summary>
        /// <param name="collision">objeto coledido</param>
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponentInParent<EnemiesMaster>() || collision.GetComponent<WallsMaster>())
            {
                if (m_GamePlayManager.getGodMode) return;
                m_GamePlayManager.CallEventPausePlayGame();
                m_PlayerMaster.CallEventPlayerHit();
                m_PlayerMaster.CallEventPlayerDestroy();
            }
        }
        /// <summary>
        /// ativa o collider
        /// </summary>
        /// 
        private void ColliderOn()
        {
            m_Collider.enabled = true;
        }
        /// <summary>
        /// Desativa o Collider
        /// </summary>
        /// 
        private void ColliderOff(Levels level)
        {
            ColliderOff();
        }
        private void ColliderOff()
        {
            m_Collider.enabled = false;
        }
        /// <summary>
        /// Executa quando desativa o objeto
        /// </summary>
        /// 
        private void OnDisable()
        {
            m_PlayerMaster.EventPlayerDestroy -= ColliderOff;
            m_PlayerMaster.EventPlayerReload -= ColliderOn;
            m_GamePlayManager.EventCompletePath -= ColliderOff;
        }
    }
}
