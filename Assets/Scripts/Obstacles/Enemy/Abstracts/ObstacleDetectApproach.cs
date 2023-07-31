using GD.MinMaxSlider;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace RiverAttack
{
    public abstract class ObstacleDetectApproach : MonoBehaviour
    {
        [Header("Start Move By Player Approach")]
        [Tooltip("If the enemy has versions with and without player approach, it is recommended to use a different Enemy SO.")]
        [SerializeField] protected internal float playerApproachRadius;
        [SerializeField, Range(.1f, 5)] public float timeToCheck = 2f;
        [SerializeField, MinMaxSlider(0f,20f)] protected internal Vector2 playerApproachRadiusRandom;
        protected float startApproachRadius;
        PlayerDetectApproach m_PlayerDetectApproach;
        
        #region GizmoSettings
        [Header("Gizmo Settings")]
        public Color gizmoColor = new Color(255, 0, 0, 150);
        #endregion

        float randomRangeDetect
        {
            get { return Random.Range(playerApproachRadiusRandom.x, playerApproachRadiusRandom.y); }
        }
        protected Transform FindTarget<T>(LayerMask layer)
        {
            m_PlayerDetectApproach ??= new PlayerDetectApproach(transform.position, playerApproachRadius);
            m_PlayerDetectApproach.UpdatePatrolDistance(playerApproachRadius);
            return m_PlayerDetectApproach.TargetApproach<T>(layer);
        }
        
        protected float SetPlayerApproachRadius()
        {
            if (playerApproachRadiusRandom != Vector2.zero)
                playerApproachRadius = randomRangeDetect;
            return playerApproachRadius;
        }
        protected abstract void DifficultUpdates();
        protected abstract void HasPlayerApproach();

        void OnDrawGizmosSelected()
        {
            if (playerApproachRadius <= 0 && playerApproachRadiusRandom.y <= 0) return;
            float radius = playerApproachRadius;
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(center: transform.position, radius);
        }
    }
}
