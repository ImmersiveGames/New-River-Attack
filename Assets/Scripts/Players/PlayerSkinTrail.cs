using UnityEngine;
namespace RiverAttack
{
    public class PlayerSkinTrail : MonoBehaviour
    {
        private TrailRenderer[] m_TrailRenderer;
        [Range(0f, 1f)] private const float RANGE_AXIS_Y = 0.2f;

        private PlayerMaster m_PlayerMaster;

        #region UNITYMETHODS

        private void OnEnable()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_TrailRenderer = GetComponentsInChildren<TrailRenderer>();
            m_PlayerMaster.EventPlayerMasterControllerMovement += ActiveTrailsOnMovement;
        }

        private void OnDisable()
        {
            m_PlayerMaster.EventPlayerMasterControllerMovement -= ActiveTrailsOnMovement;
        }
  #endregion

  private void SetTrails(bool setting)
        {
            foreach (var t in m_TrailRenderer)
            {
                t.enabled = setting;
            }
        }

        private void ActiveTrailsOnMovement(Vector2 dir)
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
