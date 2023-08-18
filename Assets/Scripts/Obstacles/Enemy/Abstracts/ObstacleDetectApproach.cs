using GD.MinMaxSlider;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace RiverAttack
{
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class ObstacleDetectApproach : MonoBehaviour
    {
        /*[Header("Start Move By Player Approach")]
        [Tooltip("If the enemy has versions with and without player approach, it is recommended to use a different Enemy SO.")]
        [SerializeField] protected internal float playerApproachRadius;
        [SerializeField, MinMaxSlider(0f,20f)] protected internal Vector2 playerApproachRadiusRandom;
        PlayerDetectApproach m_PlayerDetectApproach;
        protected MeshRenderer meshRenderer;
        protected Transform target;
        
        #region GizmoSettings
        [Header("Gizmo Settings")]
        public Color gizmoColor = new Color(255, 0, 0, 150);
        #endregion

        protected virtual void SetInitialReferences()
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            playerApproachRadius = SetPlayerApproachRadius();
        }

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
        protected bool shouldBeApproach
        {
            get
            {
                return playerApproachRadius != 0 || playerApproachRadiusRandom != Vector2.zero;
            }
        }

        float SetPlayerApproachRadius()
        {
            if (playerApproachRadiusRandom != Vector2.zero)
                playerApproachRadius = randomRangeDetect;
            return playerApproachRadius;
        }
        void OnDrawGizmosSelected()
        {
            if (playerApproachRadius <= 0 && playerApproachRadiusRandom.y <= 0) return;
            float radius = playerApproachRadius;
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(center: transform.position, radius);
        }*/
    }
}
