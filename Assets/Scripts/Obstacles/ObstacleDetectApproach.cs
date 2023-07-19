using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace RiverAttack
{
    public abstract class ObstacleDetectApproach : MonoBehaviour
    {
        [SerializeField]
        public float radiusPlayerProximity;
        [SerializeField]
        [Tools.MinMaxRangeAttribute(0f, 10f)]
        public Tools.FloatRanged randomPlayerDistanceNear;

    #region GizmoSettings
        [Header("Gizmo Settings")]
        [SerializeField]
        public DifficultyList enemiesDifficultyList;
        [HideInInspector]
        public string difficultType;
        [SerializeField]
        Color gizmoColor = new Color(255, 0, 0, 150);
    #endregion

        EnemiesDifficulty m_EnemiesDifficulties;
        float m_PlayerDistance;

        float randomRangeDetect
        {
            get { return Random.Range(randomPlayerDistanceNear.minValue, randomPlayerDistanceNear.maxValue); }
        }

        protected virtual void OnEnable()
        {
            SetInitialReferences();
        }
        protected virtual void SetInitialReferences()
        {
            if (GetComponent<EnemiesDifficulty>() != null)
                m_EnemiesDifficulties = GetComponent<EnemiesDifficulty>();
        }
        [SuppressMessage("ReSharper", "Unity.PreferNonAllocApi")]
        protected Collider[] ApproachPlayer(LayerMask layerMask)
        {
            //implementar no concreto
            m_PlayerDistance = GetPlayerDistance();
            if (randomPlayerDistanceNear.maxValue > 0)
                m_PlayerDistance = randomRangeDetect;
            return Physics.OverlapSphere(transform.position, m_PlayerDistance, layerMask);
        }

        float GetPlayerDistance()
        {
            return (m_EnemiesDifficulties.myDifficulty.multiplyPlayerDistance > 0) ? radiusPlayerProximity * m_EnemiesDifficulties.myDifficulty.multiplyPlayerDistance : radiusPlayerProximity;
        }
        void OnDrawGizmosSelected()
        {
            if (!(radiusPlayerProximity > 0) && !(randomPlayerDistanceNear.maxValue > 0))
                return;
            float radius = radiusPlayerProximity;
            if (randomPlayerDistanceNear.maxValue > 0)
                radius = randomPlayerDistanceNear.maxValue;
            if (GetComponent<EnemiesDifficulty>() != null && Math.Abs(radiusPlayerProximity - radius) > TOLERANCE)
            {
                var myDifficulty = GetComponent<EnemiesDifficulty>().GetDifficult(difficultType);
                radius *= myDifficulty.multiplyPlayerDistance;
            }
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.localPosition, radius);
        }
        const double TOLERANCE = 0.10;
    }
}
