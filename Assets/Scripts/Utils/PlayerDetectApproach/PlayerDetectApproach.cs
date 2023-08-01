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
            //Debug.Log("SIZE: "+ size + results[0] +" 1 "+ results[1] +" 2 "+ results[2] +" 3 "+ results[3] +" 4 "+ results[4]+" DISTANCE "+m_Distance);
            Transform target = null;
            if (size < 1) {return null;}
            foreach (var t in results)
            {
                Debug.Log("COLLIDES: "+ t +" Referencia: "+m_PositionReference+" Distancia: "+m_Distance);
                if (typeof(T) == typeof(PlayerMaster))
                {
                    target = t.GetComponentInParent<PlayerMaster>().transform;
                    break;
                }
                if (typeof(T) == typeof(EnemiesMaster))
                {
                    target = t.GetComponentInParent<EnemiesMaster>().transform;
                    break;
                }
            }
            return target;
        }
    }
}

