using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;
namespace RiverAttack
{
    public abstract class ObstacleMoveByApproach : MonoBehaviour
    {
        [SerializeField] protected internal float radiusPlayerProximity;
        [SerializeField, Range(.1f, 5)] public float timeToCheck;
        [SerializeField, Tools.MinMaxRangeAttribute(0f, 10f)] protected internal Tools.FloatRanged randomPlayerDistanceNear;

    #region GizmoSettings
        [Header("Gizmo Settings")]
        [SerializeField]
        public DifficultyList enemyDifficultyList;
        [HideInInspector]
        public string difficultType;
        [SerializeField]
        Color gizmoColor = new Color(255, 0, 0, 150);
    #endregion

        protected ObstacleMovement obstacleMovement;
        GamePlayManager m_GamePlayManager;
        EnemiesDifficulty m_EnemiesDifficulties;
        protected float playerDistance;
        
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventResetEnemies += ResetPatrol;
            InvokeRepeating(nameof(ApproachPlayer), 0, timeToCheck);
        }

        protected virtual void SetInitialReferences()
        {
            m_GamePlayManager = GamePlayManager.instance;
            obstacleMovement = GetComponent<ObstacleMovement>();
            if (GetComponent<EnemiesDifficulty>() == null) return;
            m_EnemiesDifficulties = GetComponent<EnemiesDifficulty>();
            if (radiusPlayerProximity > 0 || randomPlayerDistanceNear.maxValue > 0)
            {
                obstacleMovement.canMove = false;
            }
        }

        protected float GetPlayerDistance()
        {
            return (m_EnemiesDifficulties.myDifficulty.multiplyPlayerDistance > 0) ? radiusPlayerProximity * m_EnemiesDifficulties.myDifficulty.multiplyPlayerDistance : radiusPlayerProximity;
        }
        void ResetPatrol()
        {
            if (radiusPlayerProximity > 0 || randomPlayerDistanceNear.maxValue > 0)
                obstacleMovement.canMove = false;
        }
        protected float rangePatrol
        {
            get { return Random.Range(randomPlayerDistanceNear.minValue, randomPlayerDistanceNear.maxValue); }
        }

        protected virtual void ApproachPlayer()
        {
            //implementar no concreto
        }
        void OnDrawGizmosSelected()
        {
            if (!(radiusPlayerProximity > 0) && !(randomPlayerDistanceNear.maxValue > 0)) return;
            float circleDistance = radiusPlayerProximity;
            if (randomPlayerDistanceNear.maxValue > 0)
                circleDistance = randomPlayerDistanceNear.maxValue;
            if (GetComponent<EnemiesDifficulty>() != null && Math.Abs(radiusPlayerProximity - circleDistance) > TOLERANCE)
            {
                var myDifficulty = GetComponent<EnemiesDifficulty>().GetDifficult(difficultType);
                circleDistance *= myDifficulty.multiplyPlayerDistance;
            }
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, circleDistance);
        }
        const double TOLERANCE = 0.10;
        void OnDisable()
        {
            m_GamePlayManager.EventResetEnemies -= ResetPatrol;
        }
    }
}
