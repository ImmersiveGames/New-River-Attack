using UnityEngine;
namespace RiverAttack
{
    public class PlayerSkinTrail : MonoBehaviour
    {
        private TrailRenderer[] m_TrailRenderer;
        [Range(0f, 1f)] private const float RANGE_AXIS_Y = 0.2f;

        private PlayerMasterOld _mPlayerMasterOld;

        #region UNITYMETHODS

        private void OnEnable()
        {
            _mPlayerMasterOld = GetComponent<PlayerMasterOld>();
            m_TrailRenderer = GetComponentsInChildren<TrailRenderer>();
            _mPlayerMasterOld.EventPlayerMasterControllerMovement += ActiveTrailsOnMovement;
        }

        private void OnDisable()
        {
            _mPlayerMasterOld.EventPlayerMasterControllerMovement -= ActiveTrailsOnMovement;
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
