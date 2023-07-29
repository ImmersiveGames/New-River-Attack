using RiverAttack;
using UnityEngine;
namespace Utils
{
    public class PlayerDetectApproach
    {
        readonly Vector3 m_PositionReference;
        float m_Distance;

        public PlayerDetectApproach(Vector3 refPosition, float patrolDistance)
        {
            m_PositionReference = refPosition;
            m_Distance = patrolDistance;

        }
        public void UpdatePatrolDistance(float patrolDistance)
        {
            m_Distance = patrolDistance;
        }

        public Transform TargetApproach<T>(LayerMask targetLayer, int maxColliders = 5)
        {
            var results = new Collider[maxColliders];
            int size = Physics.OverlapSphereNonAlloc(m_PositionReference, m_Distance, results, targetLayer);
            if (size < 1) return null;
            for (int i = 0; i < size; i++)
            {
                if (typeof(T) == typeof(PlayerMaster))
                {
                    return results[i].GetComponentInParent<PlayerMaster>().transform;
                }
                if (typeof(T) == typeof(EnemiesMaster))
                {
                    return results[i].GetComponentInParent<EnemiesMaster>().transform;
                }
            }
            return null;
        }
    }
}

