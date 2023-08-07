using UnityEngine;
namespace RiverAttack
{
    [RequireComponent(typeof(PlayerMaster))]
    [RequireComponent(typeof(Rigidbody))]

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
            //m_PlayerMaster.EventPlayerMasterOnDestroy += ColliderOff; // desliga o collider quando destroy o player
            //m_PlayerMaster.EventPlayerMasterReSpawn += ColliderOn;   // liga o collider quando reinicia o player
            //m_GamePlayManager.EventCompletePath += ColliderOff;
        }
        void OnTriggerEnter(Collider collision)
        {
            if (!collision.GetComponentInParent<WallsMaster>() && !collision.GetComponentInParent<EnemiesMaster>() && !collision.GetComponent<BulletEnemy>()) return;
            //if(!collision.GetComponent<EffectAreaMaster>()) return;
            if (m_GamePlayManager.getGodMode) return;
            //m_GamePlayManager.CallEventPausePlayGame();
            //m_PlayerMaster.CallEventPlayerHit();
            m_PlayerMaster.CallEventPlayerMasterOnDestroy();
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterOnDestroy -= ColliderOff;
            m_PlayerMaster.EventPlayerMasterReSpawn -= ColliderOn;
            m_GamePlayManager.EventCompletePath -= ColliderOff;
        }
  #endregion

        void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_Collider = GetComponentInChildren<Collider>();
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
