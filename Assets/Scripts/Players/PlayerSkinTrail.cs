using UnityEngine;
namespace RiverAttack
{
    public class PlayerSkinTrail : MonoBehaviour
    {
        PlayerMaster m_PlayerMaster;
        GamePlayManager m_GamePlayManager;
        TrailRenderer[] m_TrailRenderer;

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            if (m_PlayerMaster)
                m_PlayerMaster.EventPlayerMasterControllerMovement += SetTrail;
            m_TrailRenderer = GetComponentsInChildren<TrailRenderer>();
            m_GamePlayManager.EventCompletePath += EnableTrails;
            SetTrails(false);
        }
        void OnDisable()
        {
            if (m_PlayerMaster)
                m_PlayerMaster.EventPlayerMasterControllerMovement -= SetTrail;
            m_GamePlayManager.EventCompletePath -= EnableTrails;
        }
  #endregion
        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponentInParent<PlayerMaster>();
            m_GamePlayManager = GamePlayManager.instance;
        }
        void SetTrail(Vector2 dir)
        {
            switch (dir.y)
            {
                case > 0:
                    SetTrails(true);
                    break;
                case <= 0:
                    SetTrails(false);
                    break;
            }
        }
        void SetTrails(bool setting)
        {
            foreach (var t in m_TrailRenderer)
            {
                t.enabled = setting;
            }
        }
        void EnableTrails()
        {
            foreach (var t in m_TrailRenderer)
            {
                t.enabled = true;
            }
        }
    }
}
