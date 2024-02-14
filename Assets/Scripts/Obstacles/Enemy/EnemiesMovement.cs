using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace RiverAttack
{
    public class EnemiesMovement : ObstacleDetectApproach
    {
        [Header("Movement Settings")]
        [SerializeField]
        private bool ignoreWalls;
        [SerializeField] private bool ignoreEnemies;
        [SerializeField] internal float moveVelocity;

        private enum Directions { None, Forward, Back, Up, Right, Down, Left, Free }
        [Header("Movement Directions")]
        [SerializeField]
        private Directions startDirection;
        protected Vector3 m_VectorDirection;
        public Vector3 freeVectorDirection;
        [Header("Movement with Animation Curve")]
        [SerializeField] internal float animationDuration;
        [SerializeField] internal AnimationCurve animationCurve;

        protected IMove m_ActualState;
        protected ObstacleMaster m_ObstacleMaster;
        protected GamePlayManager m_GamePlayManager;
        private bool m_InCollision;

        #region UNITYMETHODS

        private void Awake()
        {
            m_VectorDirection = SetDirection(startDirection);
        }

        private void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventReSpawnEnemiesMaster += ResetEnemyMovement;
        }

        private void Start()
        {
            ChangeState(new StateMoveHold(this, m_ObstacleMaster));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_ActualState is not StateMove) return;
            if (!m_GamePlayManager.shouldBePlayingGame || !m_ObstacleMaster.shouldObstacleBeReady || m_ObstacleMaster.isDestroyed || !meshRenderer.isVisible)
                return;
            if(m_InCollision || (other.GetComponentInParent<WallsMaster>() && ignoreWalls) || (other.GetComponentInParent<EnemiesMaster>() && ignoreEnemies)) 
                return;
            if (!other.GetComponentInParent<EnemiesMaster>() && !other.GetComponentInParent<WallsMaster>()) return;
            
            m_InCollision = true;
            m_VectorDirection *= -1;
            var enemiesMaster = m_ObstacleMaster as EnemiesMaster;
            if (enemiesMaster != null)
                enemiesMaster.OnEventObjectMasterFlipEnemies(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (m_ActualState is not StateMove) return;
            if (!m_GamePlayManager.shouldBePlayingGame || !m_ObstacleMaster.shouldObstacleBeReady || m_ObstacleMaster.isDestroyed || !meshRenderer.isVisible)
                return;
            if(!m_InCollision || (other.GetComponentInParent<WallsMaster>() && ignoreWalls) || (other.GetComponentInParent<EnemiesMaster>() && ignoreEnemies)) 
                return;
            if (!other.GetComponentInParent<EnemiesMaster>() && !other.GetComponentInParent<WallsMaster>()) return;
            m_InCollision = false;
        }

        private void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_ObstacleMaster.shouldObstacleBeReady || m_ObstacleMaster.isDestroyed || !meshRenderer.isVisible)
                return;
            m_ActualState.UpdateState(transform, m_VectorDirection);
        }
  #endregion
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            m_GamePlayManager = GamePlayManager.instance;
            m_ObstacleMaster = GetComponent<ObstacleMaster>();
        }
        internal bool shouldBeMoving { get { return m_ObstacleMaster.isActive && moveVelocity > 0; } }
        internal void ChangeState(IMove newState)
        {
            if (m_ActualState == newState) return;
            m_ActualState?.ExitState();

            m_ActualState = newState;
            m_ActualState?.EnterState();
        }

        private void ResetEnemyMovement()
        {
            m_VectorDirection = SetDirection(startDirection);
            m_InCollision = false;
            target = null;
            ChangeState(new StateMoveHold(this, m_ObstacleMaster));
        }

        private Vector3 SetDirection(Directions dir)
        {
            return dir switch
            {
                Directions.Up => Vector3.up,
                Directions.Right => Vector3.right,
                Directions.Down => Vector3.down,
                Directions.Left => Vector3.left,
                Directions.Back => Vector3.back,
                Directions.Forward => Vector3.forward,
                Directions.None => Vector3.zero,
                Directions.Free => freeVectorDirection,
                _ => Vector3.zero
            };
        }

        #region Gizmos

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            
            if (playerApproachRadius <= 0 && playerApproachRadiusRandom.y <= 0) return;
            float newPlayerApproachRadius = playerApproachRadiusRandom != Vector2.zero ? Random.Range(playerApproachRadiusRandom.x, playerApproachRadiusRandom.y) : playerApproachRadius;
            var enemiesMaster = GetComponent<EnemiesMaster>();
            if (!enemiesMaster.enemy && !enemiesMaster.enemy.enemiesSetDifficultyListSo) return;
            var difficult = enemiesMaster.enemy.enemiesSetDifficultyListSo.GetDifficultByEnemyDifficult(enemiesMaster.actualDifficultName);
            float realApproachRadius = newPlayerApproachRadius * difficult.multiplyPlayerDistanceRadiusToMove;
            var position = transform.position;

            // Código que será executado apenas no Editor
            if (playerApproachRadiusRandom == Vector2.zero)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawWireSphere(center: position, realApproachRadius);
            }
            if(playerApproachRadiusRandom == Vector2.zero) return;
            Gizmos.color = gizmoColor + new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, playerApproachRadiusRandom.x * difficult.multiplyPlayerDistanceRadiusToMove);
            Gizmos.color = gizmoColor - new Color(0.2f, 0.2f, 0.2f);
            Gizmos.DrawWireSphere(center: position, playerApproachRadiusRandom.y * difficult.multiplyPlayerDistanceRadiusToMove);
#endif
        }
  #endregion

  
    }
}
