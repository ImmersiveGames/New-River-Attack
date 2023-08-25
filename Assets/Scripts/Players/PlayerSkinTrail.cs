using UnityEngine;
namespace RiverAttack
{
    public class PlayerSkinTrail : MonoBehaviour
    {
        TrailRenderer[] m_TrailRenderer;
        [Range(0f, 1f)] const float RANGE_AXIS_Y = 0.2f;

        PlayerMaster m_PlayerMaster;

        #region UNITYMETHODS
        void OnEnable()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_TrailRenderer = GetComponentsInChildren<TrailRenderer>();
            m_PlayerMaster.EventPlayerMasterControllerMovement += ActiveTrailsOnMovement;
        }
        void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterControllerMovement -= ActiveTrailsOnMovement;
        }
  #endregion
        void SetTrails(bool setting)
        {
            foreach (var t in m_TrailRenderer)
            {
                t.enabled = setting;
            }
        }
        void ActiveTrailsOnMovement(Vector2 dir)
        {
            switch (dir.y)
            {
                case > RANGE_AXIS_Y:
                    SetTrails(true);
                    break;
                case <= RANGE_AXIS_Y:
                    SetTrails(false);
                    break;
            }
        }
    }
}
